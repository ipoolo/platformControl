using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadChannelHitbox : BaseHitbox
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("A"+other.gameObject.name);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("B" + other.gameObject.name);

        if (transform.position.y > other.gameObject.transform.position.y)
        {
            
            //头高于台阶 触发补偿向上
            colls.FireHeadChannel();
        }
    }
}
