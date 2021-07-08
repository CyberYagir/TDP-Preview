using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun, IPunObservable
{
    public int damage;
    public float speed;
    public string player;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(damage);
            stream.SendNext(player);
        }
        else
        {
            damage = (int)stream.ReceiveNext();
            player = (string)stream.ReceiveNext();
        }
    }
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime, Space.Self);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.layer == 0)
            {
                if (collision.GetComponent<Player>() != null)
                {
                    collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage,player);
                }
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
    }
}
