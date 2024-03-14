using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpVelocity;
    public float fallSpeed;
    public float fallDownSpeed;
    public Rigidbody rb { private set; get; }
    private float speed;

    private void Awake()
    {
        speed = Const.SPEED_ORI;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MyGameManager.Instance.CurGameState != MyGameManager.GameState.playing)
        {
            return;
        }

        // Player chuyển động

        // check xem death chưa
        if (CheckDeath())
        {
            App.Trace("death", App.DebugColor.yellow);
            MyGameManager.Instance.Death();
        }

        // chuyển động theo chiều z
        transform.Translate(0, 0, speed * Time.deltaTime);
        // Nhảy
        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Lấy cử chỉ chạm đầu tiên

            // Kiểm tra xem cử chỉ chạm là cử chỉ bắt đầu
            if (touch.phase == TouchPhase.Began)
            {
                // Thực hiện hành động nhảy
                Jump();
            }
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            rb.AddForce(Vector3.down * fallDownSpeed);
        }

        if (MyGameManager.Instance.eviController.SpawnedObs != null)
        {
            for (int i = 0; i < MyGameManager.Instance.eviController.SpawnedObs.Count; i++)
            {
                if (transform.position.z > MyGameManager.Instance.eviController.SpawnedObs[i].gameObject.transform.position.z + 10f && MyGameManager.Instance.eviController.SpawnedObs[i].gameObject.activeInHierarchy)
                {
                    MyGameManager.Instance.eviController.SpawnedObs[i].gameObject.SetActive(false);
                    MyGameManager.Instance.eviController.SpanwnNextObs();
                    break;
                }
            }
        }
        MyGameManager.Instance.UpdateScore((long)transform.position.z);
    }

    //nhảy
    void Jump()
    {

        if (isGround())
        {
            MyGameManager.Instance.audioSource.PlayOneShot(MyGameManager.Instance.audioClips[Random.Range(1,4)]);
            App.Trace("Player start jumping",App.DebugColor.red);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.velocity = Vector3.up * jumpVelocity;

            MyGameManager.Instance.UpdateJumpMission();
        }
        
    }

    // check để nhảy 
    bool isGround()
    {
        return transform.position.y < -0.8f && transform.position.y > -1.1f;
    }

    bool CheckDeath()
    {
        return transform.position.y < Const.DEATH_POS_Y;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "obs")
        {
            App.Trace("Trạm");
            MyGameManager.Instance.playerBeforeDeathPos = collision.transform.position;
        }
    }



}
