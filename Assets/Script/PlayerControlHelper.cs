using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerControlHelper : MonoBehaviour
{
    private Collisions colls;
    private Rigidbody2D rb;
    public float verticalHelperSpeed;
    private BetterJump jumpContorller;

    private Animator animator;
    public float jumpfalldownHelpTime = 1.0f;//补偿时间
    private float jumpfalldownHelpTimer;//补偿计时器
    private bool isJumpfalldownHelpTimerRun = false;
    [HideInInspector]
    public bool canJumpByHelpter = false;

    public bool canPropUpPass;
    public bool canClimbPass;

    private void Awake()
    {
        colls = GetComponent<Collisions>();
        rb = GetComponent<Rigidbody2D>();
        jumpContorller = GetComponent<BetterJump>();
        animator = GetComponentInChildren<Animator>();


        colls.playerListenOnHeadChannelCallBack = (fire) =>
        {
            if(rb.velocity.y > 0) { 
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
            if(jumpfalldownHelpTimer > jumpfalldownHelpTime)
            {
                isJumpfalldownHelpTimerRun = false;
                canJumpByHelpter = false;
            }
        }
        bool forwarKeyHold = false;
        if (Vector2.Dot(transform.right, Vector2.right) > 0)
        {
            forwarKeyHold = Input.GetKey(KeyCode.D);
        }
        else
        {
            forwarKeyHold = Input.GetKey(KeyCode.A);
        }

        if (canPropUpPass && forwarKeyHold)
        {
            if (!isPropUpRuning)
            {
                isPropUpRuning = true;
                targetPostionY = transform.position.y + 1.0f;
                stepPropUp = 1 / 4.0f;
                animator.SetTrigger("IsPropup");
                StopCoroutine(PropUp());
                StartCoroutine(PropUp());
            }
            //垂直 4/60秒内提高到1unity高度 锁定无重力
            
        }
    }
    private float targetPostionY;
    private float stepPropUp;
    private bool isPropUpRuning = false;
    IEnumerator PropUp()
    {
        Debug.Log("C");
        rb.gravityScale = 0.0f;
        transform.position += new Vector3(0, stepPropUp);
        while (isPropUpRuning)
        {
            yield return new WaitForEndOfFrame();
            transform.position += new Vector3(0, stepPropUp);
            if(transform.position.y >= targetPostionY)
            {
                isPropUpRuning = false;
                rb.gravityScale = 1.0f;
            }
        }

    }

}
