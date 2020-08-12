using DG.Tweening;
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
    private Move movement;

    [Range(0, 10)]
    public float wallJumpSpeed;

    [Range (0,10)]
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
        movement = GetComponent<Move>();
        colls.playerListenOnFloorCallBack = (isOnFloor) => {
            //if (isOnFloor == true && Mathf.Abs(rb.velocity.y) < 0.1f )
            //只有变化才能进入
            if (isOnFloor == true )
            {
                //到达地面
                canDoubleJumped = true;
                movement.isXOutControl = false;
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
        colls.playerListenOnWallCallBack = (isOnWall) => {
            //只有变化才能进入
            if (isOnWall == true)
            {
                //到达墙壁
                canDoubleJumped = true;
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
        if (!movement.isXYOutControl) { 
            if (rb.velocity.y <= - 0.1f)
            {
                isFall = true;
                rb.velocity += Vector2.up * Physics2D.gravity.y * fallGScale * Time.deltaTime;
            }
            CheckPress();
        }
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
        if (colls.getIsOnFloor() == true || pch.canJumpByHelpter || colls.getIsOnWall())
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
        if (!colls.getIsOnWall()) { 
            rb.velocity = new Vector2(rb.velocity.x, speedJumpY);
        }
        else
        {
            rb.velocity = new Vector2(transform.right.normalized.x * -1 * wallJumpSpeed, speedJumpY);
            movement.LockXControl();
            movement.SpwanSilderShadow();
        }
        
        animator.SetTrigger("IsJump");
        pch.StopCounterJumpfalldownHelpTimer();
        
    }

    private void DoubleJump()
    {
        //doubleJumpScale
        if (!colls.getIsOnWall())
        {
            rb.velocity = new Vector2(rb.velocity.x, speedJumpY * doubleJumpScale);
        }
        else
        {
            rb.velocity = new Vector2(transform.right.normalized.x * -1 * wallJumpSpeed, speedJumpY * doubleJumpScale);
        }
        canDoubleJumped = false;
        movement.SpwanSilderShadow();
        animator.SetTrigger("IsDoubleJump");
        footPosition = GetComponentInChildren<FloorHitbox>().transform.position;
        Instantiate((GameObject)Resources.Load("Prefabs/JumpParticle"), footPosition + Vector2.up.normalized * 0.25f, Quaternion.identity);
        
    }




}
