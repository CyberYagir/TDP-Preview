using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCamera : MonoBehaviour {

    public GameObject player;
    public float speed;

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10), new Vector3(player.transform.position.x, player.transform.position.y, -10), speed);
        }
    }
}
