using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayeDataUpdate : MonoBehaviour
{
    public int playerid;

    private Tweener positionTweener;

    private Tweener rotationTweener;

    private string ipadress;
    
    private void Start()
    {
        if (GetComponent<PlayerController>() != null)
        {
            playerid = ServerData.Instance.playerID;
            InvokeRepeating("UpdataPlayerData", 0, 0.1f);
        }
    }
    private void UpdataPlayerData()
    {
        if (ipadress == null) { return; }
        string data = transform.position.x.ToString("f2") + "," + transform.position.y.ToString("f2") + "," + transform.position.z.ToString("f2") + "," +
             transform.localEulerAngles.x.ToString("f2") + "," + transform.localEulerAngles.y.ToString("f2") + "," + transform.localEulerAngles.z.ToString("f2"); 
        UIManager.Instance.socketmanager.message.SendMessage(UIManager.Instance.socketmanager, SendMessageType.UpdatePlayerData, data);
    }

    public void UpdataData(Vector3 position, Vector3 rotation ,float gpsn ,float gpse)
    {
        if (positionTweener != null)
            positionTweener.Kill();
        if (rotationTweener != null)
            rotationTweener.Kill();
        positionTweener = transform.DOMove(position, 0.1f);
        rotationTweener = transform.DORotate(rotation, 0.1f);
        transform.Find("gps").GetComponent<TextMesh>().text = "latitude:" + gpsn + "\n" + "longitude:" + gpse;
    }

    
}
