using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public float moveSpeed;
    public float topDownOffset = 0.5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMove();
        CheckFaceTo();
        UpdateAnimatorInfo();
    }
    private void CheckMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        //rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveX * moveSpeed, rb.velocity.y), 0.8f);
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

    }

    private void CheckFaceTo()
    {
        if(rb.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rb.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0f);
        }
    }

    private void UpdateAnimatorInfo()
    {
       
        animator.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("SpeedY", rb.velocity.y);
    }
    
}
