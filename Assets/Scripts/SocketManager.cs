using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class SocketManager{
    private static string ip = "172.26.163.161";

    private static int port = 6666;

    public Socket socket;

    public MessageControl message;

    public byte[] data;

    public int offset;


    /// <summary>
    /// 初始化客户端连接
    /// </summary>
    public void Init()
    {
        data = new byte[1024];
        message = new MessageControl();
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(IPAddress.Parse(ip), port);
        socket.BeginReceive (data , 0, data.Length , SocketFlags.None , ReceiveCallBack, socket);
    }
    
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (socket == null || socket.Connected == false) return;
            message.GetMessage(data);
            data = new byte[data.Length];
            socket.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveCallBack, socket);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

   
}
