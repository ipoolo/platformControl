using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Collisions colls;

    public float moveSpeed;
    public float topDownOffset = 0.5f;
    public float sildeMoveScale ;

    private float sildeTimer = 0.0f;
    private bool isCounterSildeTime = false;

    private SpriteRenderer sr;

    public Color trailColor;
    public Color fadeColor;
    public float fadeTime;

    public float wallSildeSpeed;

    public float shadowStepTime;

    public bool isXOutControl;
    public bool isXYOutControl;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        colls = GetComponent<Collisions>();
        sr = GetComponentInChildren<SpriteRenderer>();
        isXOutControl = false;
        isXYOutControl = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        UpdateAnimatorInfo();
        if (isCounterSildeTime)
        {
            sildeTimer += Time.deltaTime;
            if(sildeTimer > shadowStepTime)
            {
                sildeTimer = 0;
                SpwanSilderShadow();
            }
        }
    }

    private void CheckInput()
    {
        CheckMove();
        
        if (colls.getIsOnFloor()) {
            animator.SetBool("IsWallSilde", false);
            CheckSlide();
        }
        else 
        {
            ExitShadowEffect();
            if (colls.getIsOnWall())
            {
                CheckOnWallSlide();
            }
            else
            {
                animator.SetBool("IsWallSilde", false); 
            }
        }
        CheckDown();
        CheckFaceTo();
    }

    private void CheckMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        //rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveX * moveSpeed, rb.velocity.y), 0.8f);
        if (!isXOutControl && !isXYOutControl) {
            
            //rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveX * moveSpeed, rb.velocity.y), Time.deltaTime * 10);
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
        }


    }

    private void CheckFaceTo()
    {
        if(!isXYOutControl )
        {
            if (rb.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (rb.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0f);
            }
        }


    }
    
    private void UpdateAnimatorInfo()
    {
       
        animator.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("SpeedY", rb.velocity.y);
    }

    private void CheckSlide()
    {
        if (Input.GetKey(KeyCode.LeftControl)&& Mathf.Abs(rb.velocity.x) > 1)
        {
            if (colls.getIsOnFloor()) { 
                //按住滑行
                rb.velocity = new Vector2(rb.velocity.x * sildeMoveScale, rb.velocity.y);
                //执行动画
                animator.SetBool("IsSilde", true);
                isCounterSildeTime = true;
            }

        }
        else if(Input.GetKeyUp(KeyCode.LeftControl)|| colls.getIsOnFloor() == false || Mathf.Abs(rb.velocity.x) < 1)
        {
            ExitShadowEffect();
        }
    }

    public void ExitShadowEffect()
    {
        //松开滑行
        //退出动画
        animator.SetBool("IsSilde", false);
        isCounterSildeTime = false;
        sildeTimer = 0.0f;
    }

    public void SpwanSilderShadow()
    {
        Sequence s = DOTween.Sequence();
        //创建一个阴影
        GameObject gb = (GameObject)Resources.Load("Prefabs/Shadow");
        Transform currentshadow = Instantiate(gb).transform;
        s.AppendCallback(() => currentshadow.position = transform.position);
        if(Vector2.Dot(transform.right,Vector2.right) >0 )
        {
            s.AppendCallback(() => currentshadow.GetComponent<SpriteRenderer>().flipX = false);
        }
        else
        {
            s.AppendCallback(() => currentshadow.GetComponent<SpriteRenderer>().flipX = true);
        }
        
        s.AppendCallback(() => currentshadow.GetComponent<SpriteRenderer>().sprite = sr.sprite);
        s.Append(currentshadow.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
        s.AppendCallback(() => FadeSprite(currentshadow));
    }

    private void FadeSprite(Transform shadownTransform)
    {
        shadownTransform.GetComponent<SpriteRenderer>().material.DOKill();
        shadownTransform.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

    public void CheckOnWallSlide()
    {
        if (colls.getIsOnWall())
        {
            //动画切换到滑墙
            animator.SetBool("IsWallSilde", true);
            //粒子效果TODO

            //设置Y移速为固定值
            if (!isXOutControl) { 
                rb.velocity = new Vector2(rb.velocity.x, -1 * wallSildeSpeed);
            }
        }
        else
        {
            animator.SetBool("IsWallSilde", false);
            
        }

    }

    public void LockXControl()
    {
        isXOutControl = true;
        StopCoroutine(UnlockXControlWithTime(0.3f));
        StartCoroutine(UnlockXControlWithTime(0.3f));
    }

    IEnumerator UnlockXControlWithTime(float time)
    {
        yield return new WaitForSeconds(time);
        isXOutControl = false;
    }

    public void LockXYControl(float time)
    {
        isXYOutControl = true;
        StopCoroutine(UnlockXYControlWithTime(time));
        StartCoroutine(UnlockXYControlWithTime(time));
    }

    IEnumerator UnlockXYControlWithTime(float time)
    {
        yield return new WaitForSeconds(time);
        isXYOutControl = false;
    }

    private string[] oneWayPaltformMaskLayers = { "OneWayPaltform" };
    public void CheckDown()
    {
        if (colls.getIsOnFloor()&& Input.GetKeyDown(KeyCode.S))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2, LayerMask.GetMask(oneWayPaltformMaskLayers));
            if (hit.collider)
            {
                //执行动画
                animator.SetTrigger("IsJump");
                //向上跳跃
                rb.velocity = new Vector2(rb.velocity.x, 1.0f);
                //切换图层
                gameObject.layer = LayerMask.NameToLayer("PlayerOneWayPaltformDown");
                //start切换回来
                StartCoroutine(Back2PlayerLayer());
            }
        }
    }

    IEnumerator Back2PlayerLayer()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }


}
