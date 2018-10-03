using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public enum GetMessageType
{
    LoginResult = 0,
    RegistResult,
    PlayerData,
    CreateRoomResult,
    JoinRoomResult
}

public enum SendMessageType
{
    Login = 0,
    Regist, 
    UpdatePlayerData,
    CreateRoom,
    JoinRoom
}

public class MessageControl  {

    public Action<string[]> LoginResult;
    public Action<string[]> RegistResult;
    public Action<string[]> PlayerData;
    public Action<string[]> CreateRoomResult;
    public Action<string[]> JoinRoomResult;

    public void GetMessage(byte[] buffer)
    {
        string data = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
        data.Trim();
        Debug.Log(data);
        string[] splitdata = data.Split(',');
        int oprtype;
        if (int.TryParse(splitdata[0], out oprtype))
        {
            string[] splitdata1 = new string[splitdata.Length - 1];
            Array.Copy(splitdata, 1, splitdata1, 0, splitdata.Length - 1);
            switch ((GetMessageType)oprtype)
            {
                case GetMessageType.LoginResult:
                    LoginResult(splitdata1);
                    break;
                case GetMessageType.RegistResult:
                    RegistResult(splitdata1);
                    break;
                case GetMessageType.PlayerData:
                    PlayerData(splitdata1);
                    break;
                case GetMessageType.CreateRoomResult:
                    CreateRoomResult(splitdata1);
                    break;
                case GetMessageType.JoinRoomResult:
                    JoinRoomResult(splitdata1);
                    break;
            }
        }
    }

    public void SendMessage(SocketManager socket,SendMessageType type , string data)
    {
        string buffer = (int)type + "," + data + ",";
        socket.socket.Send(Encoding.ASCII.GetBytes(buffer));
    }
}
