using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PUNNetworkManager : MonoBehaviourPunCallbacks, ILobbyCallbacks {

    public Button btnConnectMaster;
    public GameObject btnJoinRoom;
    public bool tryToConnectMaster = false, tryToConnectRoom = false;
    public GameObject roomListingPrefab;
    public GameObject roomHolder;
    public UIStart start;


    private void Start()
    {
        
        DontDestroyOnLoad(gameObject);
        ConnectToMaster();
    }

    private void Update()
    {
        if (Screen.width != 800 || Screen.height != 480)
        {
            Screen.SetResolution(800, 480, false);
        }
        if (btnConnectMaster != null)
        {
            PhotonNetwork.NickName = FindObjectOfType<UIStart>().nameField.text;
            btnConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !tryToConnectMaster);
            btnJoinRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !tryToConnectMaster && !tryToConnectRoom);
        }
    }
    public void ConnectToMaster() {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = "Player";
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        tryToConnectMaster = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        tryToConnectMaster = false;
        tryToConnectRoom = false;
        Debug.Log(cause);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        tryToConnectMaster = false;
        Debug.Log("Connected");
    }
    public void ConnectToRandomRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        tryToConnectRoom = true;
        PhotonNetwork.JoinRandomRoom();
    }
    public void ConnectByName()
    {
        PhotonNetwork.JoinRoom(FindObjectOfType<UIStart>().field.text);
    }
    public void ConnectByListRoomName(string _name)
    {
        PhotonNetwork.JoinRoom(_name);
    }
    //Create
    public void CreateByName()
    {
        JoinLobbyOnClick();

        RoomOptions options = new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 10};
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        options.CustomRoomProperties.Add(RoomConstants.Map, start.mapDropdown.options[start.mapDropdown.value].text);

        string[] inLobby = new string[] { start.mapDropdown.options[start.mapDropdown.value].text };

        options.CustomRoomPropertiesForLobby = inLobby;

        if (start.field.text.Trim() == "")
        {
            start.field.text = "FP" + UnityEngine.Random.Range(-999, 9999);
        }
        PhotonNetwork.CreateRoom(start.field.text + " - " + options.CustomRoomProperties[RoomConstants.Map], options);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        tryToConnectRoom = false;
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | In room: " + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("Network");
    }
    //Create
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        RoomOptions options = new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 10 };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();

        string[] inLobby = new string[] { start.mapDropdown.options[start.mapDropdown.value].text };

        options.CustomRoomPropertiesForLobby = inLobby;

        float randMap = UnityEngine.Random.Range(-2, 2);
        if (randMap > 0)
        {
            options.CustomRoomProperties.Add(RoomConstants.Map, "Snowy Rock");
        }
        else
        {
            options.CustomRoomProperties.Add(RoomConstants.Map, "Space Base");
        }
        PhotonNetwork.CreateRoom("FP" + UnityEngine.Random.Range(-999, 9999) + "[" + options.CustomRoomProperties[RoomConstants.Map] + "]", options);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        tryToConnectRoom = false;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        ClearRooms();
        foreach (RoomInfo room in roomList )
        {
            ListRoom(room);
        }
    }

    private void ListRoom(RoomInfo r)
    {
        if (r.IsOpen == true && r.IsVisible)
        {
            GameObject room = Instantiate(roomListingPrefab, roomHolder.transform);
            RoomListScript rl = room.GetComponent<RoomListScript>();
            rl.room = r;
        }
    }
    private void ClearRooms()
    {
        while(roomHolder.transform.childCount != 0)
        {
            Destroy(roomHolder.transform.GetChild(0).gameObject);
        }
    }
    public void JoinLobbyOnClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
}
