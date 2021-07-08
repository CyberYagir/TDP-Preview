using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetworkMapActiovator : MonoBehaviour
{
    public List<MapClass> Maps;
    // Use this for initialization
    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            string Map = PhotonNetwork.CurrentRoom.CustomProperties[RoomConstants.Map].ToString();

            ExitGames.Client.Photon.Hashtable f = new ExitGames.Client.Photon.Hashtable();
            f.Add(RoomConstants.Map, Map);

            PhotonNetwork.CurrentRoom.SetCustomProperties(f);
            PhotonNetwork.CurrentRoom.SetPropertiesListedInLobby(new string[] { Map });
            print(PhotonNetwork.CurrentRoom.CustomProperties);
            for (int i = 0; i < Maps.Count; i++)
            {
                if (Map == Maps[i]._name)
                {
                    Maps[i].mapGO.SetActive(true);
                    print("Curr map: " + Map);
                }
            }
        }
    }
}

[System.Serializable]
public class MapClass
{
    public string _name;
    public GameObject mapGO;
}
