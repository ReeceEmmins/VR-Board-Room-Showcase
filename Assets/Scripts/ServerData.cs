using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerData{
    private static ServerData instance;

    public static ServerData Instance
    {
        get
        {
            if (instance == null)
                instance = new ServerData();
            return instance;
        }
    }

    public SocketManager socketmanager;

    public int playerID;

    public float gpsn = 0;

    public float gpse = 0;

}
