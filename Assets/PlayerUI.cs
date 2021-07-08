using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public GameManager manager;
    public Text hp;
    public Text ammo;
    public Text info;
    public Text kills;
    public Text deaths;
    public GameObject escapeMenu;
	void Update () {
		if (manager.LocalPlayer != null)
        {
            hp.text = "Здоровье: " + manager.LocalPlayer.hp.ToString();
            ammo.text = FindObjectOfType<Weapons>().weapons[FindObjectOfType<Weapons>().currentWeapon].weapon.name + ": " + manager.LocalPlayer.currAmmo + "/" + manager.LocalPlayer.fullAmmo;
            info.text = PhotonNetwork.LocalPlayer.NickName + " в комнате " + PhotonNetwork.CurrentRoom.Name;
            kills.text = "Убийства: " + manager.LocalPlayer.GetComponent<Player>().kills.ToString();
            deaths.text = "Гибели: " + manager.LocalPlayer.GetComponent<Player>().deaths.ToString();
    }

        if (escapeMenu.active)
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeMenu.active = !escapeMenu.active;
            if (escapeMenu.active)
            {
                if (manager.LocalPlayer != null)
                {
                    manager.LocalPlayer.GetComponent<PlayerController>().enabled = false;
                    manager.LocalPlayer.GetComponent<Shoot>().enabled = false;
                    Cursor.visible = true;
                }
            }
            else
            {
                if (manager.LocalPlayer != null)
                {
                    manager.LocalPlayer.GetComponent<PlayerController>().enabled = true;
                    manager.LocalPlayer.GetComponent<Shoot>().enabled = true;
                    Cursor.visible = false;
                }

            }
        }
	}
}
