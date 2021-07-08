using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPun, IPunObservable {

    public Inputs inputs;
    public int hp = 100;
    public int damage = 0;
    public int weaponID = 0;
    public float fireRate = 0f;
    public int ammo = 0;
    public int currAmmo;
    public int fullAmmo = 0;
    public int kills;
    public int deaths;
    public bool reload = false;
    bool startWrite = false;
    public struct Inputs
    {
        public Quaternion HandsRot;
        public Vector3 HandsSize;
        public string Animation;
        public bool Grounded;
    }

    public void Start()
    {
        transform.name = PhotonNetwork.NickName;
    }

    private void Awake()
    {
        if (!photonView.IsMine && GetComponent<PlayerController>()!= null)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            Destroy(GetComponent<PlayerController>().camera.gameObject);
            GetComponent<PlayerController>().active = false;
        }
        else
        {
            Weapons weapons = FindObjectOfType<Weapons>();
            weaponID = weapons.currentWeapon;
            damage = weapons.weapons[weaponID].damage;
            fireRate = weapons.weapons[weaponID].fireRate;
            ammo = weapons.weapons[weaponID].ammo;
            currAmmo = ammo;
            fullAmmo = weapons.weapons[weaponID].outAmmo;
            Instantiate(weapons.weapons[weaponID].weapon, GetComponent<Shoot>().WeaponHolder.transform);
        }
    }

    public void Update()
    {
        if (hp <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void TakeDamage(int dmg, string actorName)
    {
        hp -= dmg;

        if(hp <= 0)
        {
            Player player = GameObject.Find(actorName).GetComponent<Player>();
            player.photonView.RPC("AddKill", RpcTarget.All);
            FindObjectOfType<GameManager>().LocalPlayer.deaths++;
            Dead();
        }
    }
    [PunRPC]
    public void AddKill()
    {
        kills++;
    }
    public static void RefreshInstance(ref Player player, Player playerPrefab)
    {
        var pos = Vector2.zero;
        var rot = Quaternion.identity;
        if (player != null)
        {
            pos = player.transform.position;
            rot = player.transform.rotation;
            PhotonNetwork.Destroy(player.gameObject);
        }
        player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name,pos,rot).GetComponent<Player>();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GetComponent<PlayerController>().hands[0].transform.rotation);
            stream.SendNext(GetComponent<PlayerController>().hands[0].transform.localScale);

            stream.SendNext(weaponID);
            stream.SendNext(GetComponent<PlayerController>().grounded);
            stream.SendNext(hp);
            stream.SendNext(damage);
            stream.SendNext(fireRate);
            stream.SendNext(reload);
            stream.SendNext(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);
            stream.SendNext(kills);
            stream.SendNext(deaths);
        }
        else
        {
            inputs.HandsRot = (Quaternion)stream.ReceiveNext();
            inputs.HandsSize = (Vector3)stream.ReceiveNext();

            weaponID = (int)stream.ReceiveNext();
            inputs.Grounded = (bool)stream.ReceiveNext();
            hp = (int)stream.ReceiveNext();
            damage = (int)stream.ReceiveNext();
            fireRate = (float)stream.ReceiveNext();
            reload = (bool)stream.ReceiveNext();
            if (!startWrite)
            {
                Weapons weapons = FindObjectOfType<Weapons>();
                Instantiate(weapons.weapons[weaponID].weapon, GetComponent<Shoot>().WeaponHolder.transform);
                startWrite = true;
            }
            inputs.Animation = (string)stream.ReceiveNext();
            kills = (int)stream.ReceiveNext();
            deaths = (int)stream.ReceiveNext();
        }
    }
}
