using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListScript : MonoBehaviour {


    public RoomInfo room;
    public Text nameText, playersCountText;

    private void Start()
    {
        nameText.text = room.Name;
        playersCountText.text = "" + room.PlayerCount + "/"+ room.MaxPlayers;
    }
    public void OnClickJoin()
    {
        FindObjectOfType<PUNNetworkManager>().ConnectByListRoomName(room.Name);
    }
    

}
