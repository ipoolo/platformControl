using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControlHelper : MonoBehaviour
{
    [Header("climb")]
    float climbOffsetHeight = 1.81f;
    float climbOffsetX = 0.45f;
    float climbLockTime = 0.24f;
    float PropUP = 1.1f;

    private Collisions colls;
    private Rigidbody2D rb;
    public float verticalHelperSpeed;
    private BetterJump jumpContorller;
    private Move movement;
    private WallHitbox wallhitbox;

    private Animator animator;
    public float jumpfalldownHelpTime = 1.0f;//补偿时间
    private float jumpfalldownHelpTimer;//补偿计时器
    private bool isJumpfalldownHelpTimerRun = false;
    [HideInInspector]
    public bool canJumpByHelpter = false;

    public bool canPropUpPass;
    public bool canClimbPass;
    public bool canCompensationArrivePass;

    private void Awake()
    {
        colls = GetComponent<Collisions>();
        rb = GetComponent<Rigidbody2D>();
        jumpContorller = GetComponent<BetterJump>();
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<Move>();
        wallhitbox = GetComponentInChildren<WallHitbox>();

        colls.playerListenOnHeadChannelCallBack = (fire) =>
        {
            if (rb.velocity.y > 0) {
                //把重力去掉 或者 给一个向上的速度
                Debug.Log("playerListenOnHeadChannelCallBack");
                rb.velocity = new Vector2(rb.velocity.x, verticalHelperSpeed);//用固定速度是因为要做动画的话同样的位移[升高距离,速度应该一样,也更符合人翻墙的统一速度]
                //执行动画
                animator.SetTrigger("IsPropup");
                //并且关闭canDoubleJump
                jumpContorller.canDoubleJumped = false;
                Instantiate((GameObject)Resources.Load("Prefabs/JumpParticle"), transform.position, Quaternion.identity);
            }
        };
    }
    // Start is called before the first frame update
    void Start()
    {


    }

    public void StartCounterJumpfalldownHelpTimer()
    {
        isJumpfalldownHelpTimerRun = true;
        jumpfalldownHelpTimer = 0;
        canJumpByHelpter = true;
    }

    public void StopCounterJumpfalldownHelpTimer()
    {
        isJumpfalldownHelpTimerRun = false;
        jumpfalldownHelpTimer = 0;
        canJumpByHelpter = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (isJumpfalldownHelpTimerRun)
        {
            jumpfalldownHelpTimer += Time.deltaTime;
            if (jumpfalldownHelpTimer > jumpfalldownHelpTime)
            {
                isJumpfalldownHelpTimerRun = false;
                canJumpByHelpter = false;
            }
        }


        bool forwarKeyHold = false;
        Vector2 ray2ClimbWallXDirection;
        if (Vector2.Dot(transform.right, Vector2.right) > 0)
        {
            forwarKeyHold = Input.GetKey(KeyCode.D);
        }
        else
        {
            forwarKeyHold = Input.GetKey(KeyCode.A);
        }
        ray2ClimbWallXDirection = transform.right;


        bool canClimbPassKeyHold = forwarKeyHold || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
        //同向或者按着方向键
        if (canClimbPass && canClimbPassKeyHold && !isPropUpRuning)
        {

              if (!isClimbPassRunning)
            {
                RaycastHit2D hit =  Physics2D.Raycast(wallhitbox.headPointTransform.position, ray2ClimbWallXDirection, wallhitbox.climbRayDistance, wallhitbox.rayMaskLayer);
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, ray2ClimbWallXDirection, wallhitbox.climbRayDistance, wallhitbox.rayMaskLayer);
                if (!hit.collider)
                {
                    hit = hit2;
                }
                Debug.Log("ClimbPass");
                    isClimbPassRunning = true;
                    rb.velocity = ray2ClimbWallXDirection.normalized * 0.001f;
                    float tmpXoffset = (hit.distance + climbOffsetX) * ray2ClimbWallXDirection.normalized.x;
                    transform.position += new Vector3(tmpXoffset, climbOffsetHeight);
                    animator.SetTrigger("IsClimb");
                    movement.LockXYControl(climbLockTime);
                    StopCoroutine(ClimbEnd(climbLockTime));
                    StartCoroutine(ClimbEnd(climbLockTime));
                //}
            }
            //垂直 4/60秒内提高到1unity高度 锁定无重力
            return;
        }

        if (canPropUpPass && forwarKeyHold && !isClimbPassRunning)
        {
            if (!isPropUpRuning)
            {
                Debug.Log("PropUp");
                isPropUpRuning = true;
                targetPropUpPostionY = transform.position.y + PropUP;
                stepPropUp = PropUP / 4.0f;
                animator.SetTrigger("IsPropup");
                StopCoroutine(PropUp());
                StartCoroutine(PropUp());
            }
            //垂直 4/60秒内提高到1unity高度 锁定无重力
            return;
        }



    }

    IEnumerator ClimbEnd(float time)
    {
        yield return new WaitForSeconds(time);
        isClimbPassRunning = false;
    }

    private bool isClimbPassRunning = false;

    private float targetPropUpPostionY;
    private float stepPropUp;
    private bool isPropUpRuning = false;
    IEnumerator PropUp()
    {

        rb.gravityScale = 0.0f;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        transform.position += new Vector3(0, stepPropUp);
        while (isPropUpRuning)
        {
            yield return new WaitForEndOfFrame();
            transform.position += new Vector3(0, stepPropUp);
            if (transform.position.y >= targetPropUpPostionY)
            {
                isPropUpRuning = false;
                rb.gravityScale = 1.0f;
            }
        }

    }
}


    //下方逻辑被放弃
    // 逻辑与porpUp剥离 现在会执行完propUp之后立刻跟一个arrivePass
    //if (canCompensationArrivePass && forwarKeyHold && !colls.getIsOnFloor() && rb.velocity.y < -0.1f && !isPropUpRuning)
    ////右侧有,下方没,右侧上面2个没,速度朝下,按住按钮,不在地面上
    //{
    //    if (!isCompensationArriveRuning)
    //    {
    //        isCompensationArriveRuning = true;
    //        float offsetX = 0.55f;
    //        if (Vector2.Dot(transform.right, Vector2.right) < 0)
    //        {
    //            offsetX *= -1;
    //        }
    //        targetCompensationArrivePostionX = transform.position.x + offsetX;
    //        stepCompensationArrive = offsetX / 4.0f;
    //        StopCoroutine(CompensationArrive());
    //        StartCoroutine(CompensationArrive());
    //    }
    //    //垂直 4/60秒内提高到1unity高度 锁定无重力
    //    return;
    //}

    //private float targetCompensationArrivePostionX;
    //private float stepCompensationArrive;
    //private bool isCompensationArriveRuning = false;
    //IEnumerator CompensationArrive()
    //{
    //    Debug.Log("CompensationArrive");
    //    rb.gravityScale = 0.0f;
    //    rb.velocity = new Vector2(rb.velocity.x, 0);
    //    transform.position += new Vector3(stepCompensationArrive,0.1f );
    //    while (isCompensationArriveRuning)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        transform.position += new Vector3(stepCompensationArrive, 0 );
    //        if (transform.position.x >= targetCompensationArrivePostionX)
    //        {
    //            isCompensationArriveRuning = false;
    //            rb.gravityScale = 1.0f;
    //        }
    //    }

    //}

    

