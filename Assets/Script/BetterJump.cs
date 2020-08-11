using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BetterJump : MonoBehaviour
{
    private float fallGScale = 2.5f;
    private float jumpGScale = 2.0f;
    private float doubleJumpScale = 1.3f;

    private Animator animator;
    private Rigidbody2D rb;
    private Collisions colls;
    private PlayerControlHelper pch;

    [Range (0,100)]
    public float speedJumpY;

    public bool canDoubleJumped = true;
    public bool isFall = false;

    private Vector2 footPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colls = GetComponentInParent<Collisions>();
        animator = GetComponentInChildren<Animator>();
        pch = GetComponent<PlayerControlHelper>();
        colls.playerListenOnFloorCallBack = (isOnFloor) => {
            //if (isOnFloor == true && Mathf.Abs(rb.velocity.y) < 0.1f )
            //只有变化才能进入
            if (isOnFloor == true )
            {
                //到达地面
                canDoubleJumped = true;
            }
            else
            {
                //离开地面 
                if(rb.velocity.y < 0)
                {
                    //下坠
                    pch.StartCounterJumpfalldownHelpTimer();
                    animator.SetBool("IsSilde", false);
                }
            }
        };
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        //if (rb.velocity.y > 0.1f && (!Input.GetButton("Jump")))
        //{
        //    //向上,且没有按 v减速
        //    isFall = false;
        //    rb.velocity += Vector2.up * Physics2D.gravity.y * jumpGScale * Time.deltaTime;
        //}else 
        
        if(rb.velocity.y <= 0.1f)
        {
            isFall = true;
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallGScale * Time.deltaTime;
        }
        CheckPress();
    }

    private void CheckPress()
    {
        if (Input.GetButtonDown("Jump")|| Input.GetKeyDown(KeyCode.J))
        {
            DoJump();
        }
    }

    private void DoJump()    
    {
        if (colls.getIsOnFloor() == true || pch.canJumpByHelpter)
        {
            SimpleJump();

        }
        else if (isFall && canDoubleJumped)
        {

            DoubleJump();
        }
    }

    private void SimpleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, speedJumpY);
        animator.SetTrigger("IsJump");
        pch.StopCounterJumpfalldownHelpTimer();
    }

    private void DoubleJump()
    {
        //doubleJumpScale
        rb.velocity = new Vector2(rb.velocity.x, speedJumpY * doubleJumpScale);
        canDoubleJumped = false;
        animator.SetTrigger("IsDoubleJump");
        footPosition = GetComponentInChildren<FloorHitbox>().transform.position;
        Instantiate((GameObject)Resources.Load("Prefabs/JumpParticle"), footPosition,Quaternion.identity);
    }


}
