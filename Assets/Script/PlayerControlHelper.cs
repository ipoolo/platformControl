using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlHelper : MonoBehaviour
{
    private Collisions colls;
    private Rigidbody2D rb;
    public float verticalSpeedHelper;
    private BetterJump jumpContorller;

    private Animator animator;
    private void Awake()
    {
        colls = GetComponent<Collisions>();
        rb = GetComponent<Rigidbody2D>();
        jumpContorller = GetComponent<BetterJump>();
        animator = GetComponentInChildren<Animator>();


        colls.playerListenOnHeadChannelCallBack = (fire) =>
        {
            //把重力去掉 或者 给一个向上的速度
            Debug.Log("playerListenOnHeadChannelCallBack");
            rb.velocity = new Vector2(rb.velocity.x, verticalSpeedHelper);//用固定速度是因为要做动画的话同样的位移[升高距离,速度应该一样,也更符合人翻墙的统一速度]
            //执行动画
            animator.SetTrigger("IsPropup");
            //并且关闭canDoubleJump
            jumpContorller.canDoubleJumped = false;
            Instantiate((GameObject)Resources.Load("Prefabs/JumpParticle"), transform.position, Quaternion.identity);
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
