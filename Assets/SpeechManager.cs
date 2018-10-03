using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class SpeechManager : MonoBehaviour
{
	private string filePath;
	private int m_DeviceCount;
	private int sFrequency = 8000;
	public AudioSource m_AudioSource;
	public delegate void OnComple (byte[] bytes);
	public static event OnComple OnCompleing;
    private List<string> m_sqlContents;    //总数据
    private List<string> m_resultsContents; //搜索结果
    public Text m_text;

    void Start()
	{
		Init ();
	}

	//初始化检测
	private void Init()
	{
		string[] ms = Microphone.devices;
		m_DeviceCount = ms.Length;
		if (ms.Length == 0) 
		{
			Debug.LogError ("请检测麦克风是否插好");
		}
	}


    #region 录音
    float audioLenght = 0;
    int samplingRate = 8000;
    //更新录音情况
	public void UpdateRecord()
	{
        if (m_DeviceCount != 0)
        {
            Debug.Log("Start Recording Voice");
            Microphone.End(null);
            m_AudioSource.Stop();
            m_AudioSource.loop = false;
            m_AudioSource.mute = true;
            m_AudioSource.clip = Microphone.Start(null, false, 10, sFrequency);
            while (!(Microphone.GetPosition(null) > 0))
            {
            }
        }
    }

	public void Stop()
	{
		if (!Microphone.IsRecording(null))
		{
			return;
		}
		Debug.Log("------------Stop Recording Voice----------");
        int lastP = Microphone.GetPosition(null);
        if (Microphone.IsRecording(null))
        {
            audioLenght =((float)lastP / (float)sFrequency)*0.1f;
        }
		Microphone.End(null);	
		m_AudioSource.Stop();

        float byteNum = (float)GetClipData().Length / 1024.0f;
        Debug.Log(@"The Recording Size is " + byteNum+@" KB");
	}

    //播放录音
    public void PlayRecord()
    {
        if (Microphone.IsRecording(null))
        {
            return;
        }
        if (m_AudioSource.clip == null)
        {
            return;
        }
        m_AudioSource.mute = false;
        m_AudioSource.loop = false;
        m_AudioSource.Play();
        Debug.Log("Play Recording Voice");
    }

    public void Recognition()
    {
        if (OnCompleing != null) OnCompleing(GetClipData());
    }

    //获取音频数据
    public byte[] GetClipData()
	{
        if (m_AudioSource.clip == null)
        {
            Debug.Log("音频数据为空的");
            return null;
        }
        int lenght = (int)(m_AudioSource.clip.samples * audioLenght);
        float[] samples = new float[lenght];

        m_AudioSource.clip.GetData(samples, 0);


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

    private Byte[] speech_Byte;
    bool SaveWav(string filename, AudioClip clip)
    {
        try
        {
            Debug.Log("Record Ok :" + filename);

            Directory.CreateDirectory(Path.GetDirectoryName(filename));

            using (FileStream fileStream = CreateEmpty(filename))
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
        int lenght = (int)(clip.samples * audioLenght);
        //Debug.Log("xx"+lenght);
        float[] samples = new float[lenght];

        clip.GetData(samples, 0);  //获取数据

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
        int lenght = (int)(clip.samples * audioLenght);
        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = lenght;

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
    #endregion
}