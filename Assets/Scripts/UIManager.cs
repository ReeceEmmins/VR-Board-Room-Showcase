using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [HideInInspector]
    public SocketManager socketmanager;

    [HideInInspector]
    public string accountdata;
    [HideInInspector]
    public string passworddata;

    private bool isCreate;

    public InputField account;

    public InputField password;

    public InputField roomId;

    public GameObject waitImage;

    public GameObject LoginPanel;

    public GameObject mainPanel;

    public GameObject choosePanel;

    public GameObject roomidPanel;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        if(ServerData.Instance.socketmanager == null)
        {
            DontDestroyOnLoad(gameObject);
            socketmanager = new SocketManager();
            ServerData.Instance.socketmanager = socketmanager;
            socketmanager.Init();
            socketmanager.message.LoginResult += LoginCallback;
            socketmanager.message.RegistResult += RegistCallback;
            socketmanager.message.JoinRoomResult += JoinRoomCallback;
            socketmanager.message.CreateRoomResult += CreateRoomCallback;
            socketmanager.message.PlayerData += UpdataPlayer;
            StartCoroutine("StartGPS");
        }
    }
    IEnumerator StartGPS()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            print("没有打开GPS");
            yield return new WaitForSeconds(20);
            StartCoroutine("StartGPS");
        }

        // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用  
        Input.location.Start(10.0f, 10.0f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)  
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            print("没有获取到GPS数据");
            yield return new WaitForSeconds(20);
            StartCoroutine("StartGPS");
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("打开位置访问服务");
            yield return new WaitForSeconds(20);
            StartCoroutine("StartGPS");
        }
        else
        {
            ServerData.Instance.gpse = Input.location.lastData.longitude;
            ServerData.Instance.gpsn = Input.location.lastData.latitude;
        }
    }

    // Update is called once per frame
    void Update () {
        if (isLogin)
        {
            isLogin = false;
            waitImage.SetActive(false);
            int result;
            if (int.TryParse(returndata[0], out result))
            {
                if (result == 1)
                {
                    LoginPanel.SetActive(false);
                    mainPanel.SetActive(true);
                }
                else
                {
                    print("登陆出错,请重新登陆");
                }
            }
            else
            {
                print("登陆出错,请重新登陆");
            }
        }
        else if (isRegist)
        {
            isRegist = false;
            waitImage.SetActive(false);
            int result;
            if (int.TryParse(returndata[0], out result))
            {
                if (result == 1)
                {
                    LoginPanel.SetActive(false);
                    mainPanel.SetActive(true);
                }
                else
                {
                    print("账号已被占用");
                }
            }
            else
            {
                print("账号已被占用");
            }
        }
        else if (isCreateRoom)
        {
            isCreateRoom = false;
            waitImage.SetActive(false);
            int result;
            if (int.TryParse(returndata[0], out result))
            {
                if (result == 1)
                {
                    choosePanel.SetActive(true);
                    roomidPanel.SetActive(false);
                    mainPanel.SetActive(false);
                    ServerData.Instance.playerID = 0;
                    SceneManager.LoadScene("VRBoardRoom");
                    ///加载场景
                }
                else
                {
                    print("房间id已经被占用");
                }
            }
            else
            {
                print("房间id已经被占用");
            }
        }
        else if (isJoinRoom)
        {
            isJoinRoom = false;
            waitImage.SetActive(false);
            int result;
            if (int.TryParse(returndata[0], out result))
            {
                if (result == 1)
                {
                    choosePanel.SetActive(true);
                    roomidPanel.SetActive(false);
                    mainPanel.SetActive(false);
                    print(returndata[1]);
                    ServerData.Instance.playerID = int.Parse(returndata[1]);
                    SceneManager.LoadScene("VRBoardRoom");
                    ///加载场景
                }
                else
                {
                    print("房间id不存在");
                }
            }
            else
            {
                print("房间id不存在");
            }
        }
        else if (isUpdata)
        {
            isUpdata = false;
            Dictionary<int, Vector3> posdata = new Dictionary<int, Vector3>();
            Dictionary<int, Vector3> rotdata = new Dictionary<int, Vector3>();
            Dictionary<int, float> gpsn = new Dictionary<int, float>();
            Dictionary<int, float> gpse = new Dictionary<int, float>();
            for (int i = 0; i < returndata.Length-9; i += 9)
            {
                posdata.Add(int.Parse(returndata[i]), new Vector3(float.Parse(returndata[i+1]), float.Parse(returndata[i+2]), float.Parse(returndata[i+3])));
                rotdata.Add(int.Parse(returndata[i]), new Vector3(float.Parse(returndata[i+4]), float.Parse(returndata[i+5]), float.Parse(returndata[i+6])));
                gpsn.Add(int.Parse(returndata[i]), float.Parse(returndata[i + 7]));
                gpse.Add(int.Parse(returndata[i]), float.Parse(returndata[i + 8]));
            }
            GameObject[] allPlayer = GameObject.FindGameObjectsWithTag("Player");
            if (allPlayer.Length == 1)
            {
                allPlayer[0].GetComponent<PlayeDataUpdate>().playerid = ServerData.Instance.playerID;
            }
            else
            {
                for (int i = 0; i < allPlayer.Length; i++)
                {
                    PlayeDataUpdate playerdata = allPlayer[i].GetComponent<PlayeDataUpdate>();
                    if (posdata.ContainsKey(playerdata.playerid))
                    {
                        if (playerdata.playerid != ServerData.Instance.playerID)
                        {
                            playerdata.UpdataData(posdata[playerdata.playerid], rotdata[playerdata.playerid] , gpsn[playerdata.playerid] , gpse[playerdata.playerid]);
                        }
                        posdata.Remove(playerdata.playerid);
                        rotdata.Remove(playerdata.playerid);
                    }
                    else
                    {
                        Destroy(playerdata.gameObject);
                    }
                }
            }
            foreach (var m in posdata)
            {
                GameObject player = (GameObject)Instantiate(Resources.Load("Player"));
                PlayeDataUpdate playerdata = player.GetComponent<PlayeDataUpdate>();
                playerdata.playerid = m.Key;
                playerdata.UpdataData(posdata[m.Key], rotdata[m.Key] , gpsn[m.Key] , gpse[m.Key]);
            }
        }
    }

    public void Login()
    {
        if (account.text != "" && password.text != "")
        {
            accountdata = account.text;
            passworddata = password.text;
            string data = accountdata + "," + passworddata;
            socketmanager.message.SendMessage(socketmanager, SendMessageType.Login, data);
            waitImage.SetActive(true);
        }
    }

    public void Regist()
    {
        if (account.text != "" && password.text != "")
        {
            accountdata = account.text;
            passworddata = password.text;
            string data = accountdata + "," + passworddata;
            socketmanager.message.SendMessage(socketmanager, SendMessageType.Regist, data);
            waitImage.SetActive(true);
        }
    }

    public void JoinOrCreateRoom()
    {
        //if(ServerData.Instance.gpse == 0 && ServerData.Instance.gpsn == 0)
        //{
        //    print("GPS数据没有获取到");
        //    return;
        //}
        if (roomId.text != "")
        {
            waitImage.SetActive(true);
            if (isCreate)
            {
                socketmanager.message.SendMessage(socketmanager, SendMessageType.CreateRoom, roomId.text + "," + ServerData.Instance.gpsn + "," + ServerData.Instance.gpse);
            }
            else
            {
                socketmanager.message.SendMessage(socketmanager, SendMessageType.JoinRoom, roomId.text + "," + ServerData.Instance.gpsn + "," + ServerData.Instance.gpse);
            }
        }
    }

    public void OpenRoomIDPanel(bool isCreate)
    {
        this.isCreate = isCreate;
        roomidPanel.SetActive(true);
        choosePanel.SetActive(false);
    }


    private bool isLogin;
    private bool isRegist;
    private bool isCreateRoom;
    private bool isJoinRoom;
    private bool isUpdata;
    private string[] returndata;
    private void LoginCallback(string[] data)
    {
        returndata = data;
        isLogin = true;
    }

    private void RegistCallback(string[] data)
    {
        returndata = data;
        isRegist = true;
    }

    private void CreateRoomCallback(string[] data)
    {
        returndata = data;
        isCreateRoom = true;
    }

    private void JoinRoomCallback(string[] data)
    {
        returndata = data;
        isJoinRoom = true;
    }

    private void UpdataPlayer(string[] data)
    {
        returndata = data;
        isUpdata = true;
    }
    private void OnApplicationQuit()
    {
        socketmanager.socket.Close();
    }
}
