using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    float dist;
    public float sizeControll = 0;

	void Update () {
        dist = Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2), transform.position);
        transform.position = Input.mousePosition;
        transform.localScale = new Vector2(dist / sizeControll, dist / sizeControll);
	}
}
