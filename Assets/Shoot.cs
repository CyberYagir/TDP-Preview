using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    float coolDown = 0;
    public GameObject ayes;
    public GameObject WeaponHolder;
    Player player;
    void Update()
    {
        coolDown -= Time.deltaTime;
        if (GetComponent<PlayerController>().active == true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                player = GetComponent<Player>();
                if (player.reload == false)
                {
                    if (player.currAmmo > 0)
                    {
                        if (coolDown > 0)
                        {
                            return;
                        }
                        Bullet bullet = PhotonNetwork.Instantiate("BulletTrail", ayes.transform.position, ayes.transform.rotation).GetComponent<Bullet>();
                        bullet.damage = GetComponent<Player>().damage;
                        bullet.player  = PhotonNetwork.NickName;
                        coolDown = GetComponent<Player>().fireRate;
                        player.currAmmo -= 1;
                    }
                }
            }
            if (Input.GetKey(KeyCode.R))
            {
                if (player.currAmmo < player.ammo)
                {
                    player.reload = true;
                    StartCoroutine(ReloadWait());
                }
            }
        }
    }
    IEnumerator ReloadWait()
    {
        yield return new WaitForSeconds(4);
        player.reload = false;
        if (player.fullAmmo > 0)
        {
            for (int i = 0; i < player.ammo; i++)
            {
                if (player.ammo > player.currAmmo)
                {
                    if (player.fullAmmo > 0)
                    {
                        player.currAmmo++;
                        player.fullAmmo--;
                    }
                }
            }
        }
    }
}
