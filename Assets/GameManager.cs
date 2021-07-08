using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks {

    public Player playerPrefab;

    public Player LocalPlayer;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Start");
            return;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
        if (LocalPlayer == null)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void Disconnect()
    {
        if (LocalPlayer != null)
        {
            LocalPlayer.Dead();
        }
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("Manager"));
        Cursor.visible = true;
        SceneManager.LoadScene("Start");
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(6);
        Destroy(Camera.main.gameObject);
        Player.RefreshInstance(ref LocalPlayer, playerPrefab);
    }

    private void Start()
    {
        Player.RefreshInstance(ref LocalPlayer, playerPrefab);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Player.RefreshInstance(ref LocalPlayer, playerPrefab);
    }
}
