using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ladder : MonoBehaviour {

    public bool triggered;


    private void Update()
    {
        if (triggered == true)
        {
            if (FindObjectOfType<GameManager>().LocalPlayer != null)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    FindObjectOfType<GameManager>().LocalPlayer.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2000 * Time.deltaTime);
                    print("Ladder");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        triggered = false;
    }

}
