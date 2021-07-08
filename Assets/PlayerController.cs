using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public enum moveMode { Walk, Sit, Сrawl };
    public GameObject[] hands;
    public float speed;
    public moveMode mode = moveMode.Walk;
    public Animator animator;
    public Rigidbody2D rigidbody;
    public float jumpForce = 10;
    public float groundDist = 1;
    public bool grounded = true;
    public GameObject camera;
    Vector3 lookAtTargetPos;
    Vector2 screen;
    public bool active = true;
    public Vector2 normalHandScale;
    float degree = 90;
    float reloadZ1 = 250, reloadZ2 = -70;
    public float walkUpDist = 1.6f, sitUpDist;
    public bool canWalk;
    public bool canSit;

    private void Start()
    {
        camera.transform.parent = null;
        normalHandScale = hands[0].transform.localScale;
    }

    void Update()
    {
        if (active)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 1.5f, 0), Vector3.up, Mathf.Infinity);
            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position - new Vector3(0, 1.5f, 0), hit.point);
                if (hit.distance <= walkUpDist)
                {
                    canWalk = false;
                }
                else
                {
                    canWalk = true;
                }
                if (hit.distance <= sitUpDist)
                {
                    canSit = false;
                }
                else
                {
                    canSit = true;
                }
            }
            screen = new Vector3(Screen.width / 2, Screen.height / 2);
            RotMonitor();
            GroundCheck();
            HandsMonitor();
            if (grounded)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (canSit)
                    {
                        mode = moveMode.Sit;
                    }
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    mode = moveMode.Сrawl;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (canWalk)
                    {
                        mode = moveMode.Walk;
                    }
                }   
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (canWalk)
                    {
                        print("Jump");
                        rigidbody.velocity = new Vector2(0, jumpForce);
                        mode = moveMode.Walk;
                    }
                }
                ModesMonitor();

            }
            else
            {
                if ((Input.GetKey(KeyCode.A)) && !Input.GetKey(KeyCode.D))
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
                }
                if ((Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.A))
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
                }
                animator.Play("Jump");
            }

        }
        else
        {
            transform.gameObject.layer = 0;
            HandMonitorClient();
            if (GetComponent<Player>().inputs.Grounded)
            {
                ModesMonitorClient();
            }
            else
            {
                animator.Play("Jump");
            }
            
        }
    }

    void RotMonitor()
    {
        if (Input.mousePosition.x < screen.x)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].transform.localScale = new Vector2(hands[i].transform.localScale.x, -normalHandScale.y);
            }
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].transform.localScale = new Vector2(hands[i].transform.localScale.x, normalHandScale.y);
            }
        }
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, groundDist);
        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.point);
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void ModesMonitorClient()
    {
        animator.Play(GetComponent<Player>().inputs.Animation);
    }

    void ModesMonitor()
    {
        if (mode == moveMode.Walk)
        {
            if ((Input.GetKey(KeyCode.A)) && !Input.GetKey(KeyCode.D))
            {
                animator.Play("Move");
                transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            }
            if ((Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.A))
            {
                animator.Play("Move");
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            }
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                animator.Play("Stay");
            }
        }
        if (mode == moveMode.Sit)
        {
            if ((Input.GetKey(KeyCode.A)) && !Input.GetKey(KeyCode.D))
            {
                animator.Play("SitMove");
                transform.Translate(Vector3.left * speed / 2 * Time.deltaTime, Space.World);
            }
            if ((Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.A))
            {
                animator.Play("SitMove");
                transform.Translate(Vector3.right * speed / 2 * Time.deltaTime, Space.World);
            }
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                animator.Play("SitStay");
            }
        }
        if (mode == moveMode.Сrawl)
        {
            if ((Input.GetKey(KeyCode.A)) && !Input.GetKey(KeyCode.D))
            {
                animator.Play("СrawlMove");
                transform.Translate(Vector3.left * speed / 2 * Time.deltaTime, Space.World);
            }
            if ((Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.A))
            {
                animator.Play("СrawlMove");
                transform.Translate(Vector3.right * speed / 2 * Time.deltaTime, Space.World);
            }
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                animator.Play("СrawlStay");
            }
        }
    }

    void HandMonitorClient()
    {
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].transform.rotation = GetComponent<Player>().inputs.HandsRot;
            hands[i].transform.localScale = GetComponent<Player>().inputs.HandsSize;
        }
    }

    void HandsMonitor()
    {
        if (GetComponent<Player>().reload == false)
        {
            for (int i = 0; i < hands.Length; i++)
            {
                Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector3 lookAt = mouseScreenPosition;

                float AngleRad = Mathf.Atan2(lookAt.y - hands[i].transform.position.y, lookAt.x - hands[i].transform.position.x);

                float AngleDeg = (180 / Mathf.PI) * AngleRad;

                hands[i].transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
            }
        }
        else
        {
            for (int i = 0; i < hands.Length; i++)
            {
                if (Input.mousePosition.x < screen.x)
                {
                    hands[i].transform.eulerAngles = new Vector3(0, 0, reloadZ1);
                }
                else
                {
                    hands[i].transform.eulerAngles = new Vector3(0, 0, reloadZ2);
                }
            }
        }
    }
}
