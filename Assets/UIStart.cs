using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStart : MonoBehaviourPunCallbacks {
    public Dropdown dropdown, mapDropdown;
    public List<string> list = new List<string>();
    public InputField field, nameField;
    public Image mapPreview;
    public void Start()
    {
        nameField.text = PhotonNetwork.NickName;
        if (nameField.text == "Player")
        {
            nameField.text = "Murder" + Random.Range(-2000, 2000).ToString();
        }
        Weapons weapons = FindObjectOfType<Weapons>();
        for (int i = 0; i < weapons.weapons.Count; i++)
        {
            list.Add(weapons.weapons[i].weapon.name);
        }
        dropdown.AddOptions(list);
    }

    public void Update()
    {
        mapPreview.sprite = mapDropdown.options[mapDropdown.value].image;
    }

    public void SaveEq()
    {
        FindObjectOfType<Weapons>().currentWeapon = dropdown.value;
    }

    public void Close_Open(GameObject gameObject)
    {
        gameObject.active = !gameObject.active;
    }

}
