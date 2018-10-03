using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System;

public class SavWav : MonoBehaviour
{
    private float user_time = 0.0f;
    private bool b_flag = false;
    private int sFrequency = 8000;
    private int deviceCount;
    private string filePath;
    public AudioSource audio;
    public Text timer;
    public static byte[] bytes;
    void Start()
    {
        filePath = Application.streamingAssetsPath + "/";
        string[] ms = Microphone.devices;
        deviceCount = ms.Length;
        if (deviceCount == 0)
        {
            Debug.Log("没有发现麦克风");
        }
        else
        {
            Debug.Log(string.Format("发现{0}个麦克风",deviceCount));
        }
    }

    //开始录音
    public void StartRecord()
    {
        user_time = 0.0f;
        timer.text = "00:00：00";
        b_flag = true;
        audio.Stop();
        audio.loop = false;
        audio.mute = true;
        audio.clip = Microphone.Start(null, false, 5, sFrequency);
        while (!(Microphone.GetPosition(null) > 0))
        {
        }
        //audio.Play();
        Debug.Log("Start Recording Voice");//开始录音
        
    }

    //停止录音
    public void StopRecord()
    {
        b_flag = false;
        if (!Microphone.IsRecording(null))
        {
            return;
        }
        Microphone.End(null);
        audio.Stop();
        Debug.Log("Stop Recording Voice");
    }

    //数据储存
    public void PrintRecord()
    {
        b_flag = false;
        if (Microphone.IsRecording(null))
        {
            return;
        }
        DateTime date = DateTime.Now;
        string filename = "test.wav";
       // string filename = string.Format("w_{0}年{1}月{2}日{3}时{4}分{5}秒.wav", date.Year,date.Month,date.Day,date.Hour,date.Minute,date.Second);
        SaveWav(filename,audio.clip);
        bytes = GetClipData();
    }

    //播放录音
    public void PlayRecord()
    {
        b_flag = false;
        if (Microphone.IsRecording(null))
        {
            return;
        }
        if (audio.clip == null)
        {
            return;
        }
        audio.mute = false;
        audio.loop = false;
        audio.Play();
        Debug.Log("Play Recording Voice");
    }

    //获取音频数据
    public byte[] GetClipData()
    {
        if (audio.clip == null)
        {
            Debug.Log("音频数据为空的");
            return null;
        }

        float[] samples = new float[audio.clip.samples];

        audio.clip.GetData(samples, 0);


        byte[] outData = new byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            short temshort = (short)(samples[i] * rescaleFactor);

            byte[] temdata = System.BitConverter.GetBytes(temshort);

            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];
        }
        if (outData == null || outData.Length <= 0)
        {
            Debug.Log("GetClipData intData is null");
            return null;
        }
        return outData;
    }

    public Byte[] speech_Byte;
    bool SaveWav(string filename, AudioClip clip)
    {
        try
        {
            if (!filename.ToLower().EndsWith(".wav"))
            {
                filename += ".wav";
            }

            filePath = filePath + filename;

            Debug.Log("Record Ok :" + filePath);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (FileStream fileStream = CreateEmpty(filePath))
            {
                ConvertAndWrite(fileStream, clip);

            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("error : " + ex);
            return false;
        }

    }

    FileStream CreateEmpty(string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++)
        {
            fileStream.WriteByte(emptyByte);
        }
        return fileStream;
    }
    void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {
        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];

        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767; //to convert float to Int16  

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);

            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        speech_Byte = bytesData;

        fileStream.Write(bytesData, 0, bytesData.Length);

        WriteHeader(fileStream, clip);
    }
    void WriteHeader(FileStream fileStream, AudioClip clip)
    {

        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2    
        fileStream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * 2 * channels);
        fileStream.Write(subChunk2, 0, 4);

        fileStream.Close();
        Debug.Log(" OK ");
    }

}

