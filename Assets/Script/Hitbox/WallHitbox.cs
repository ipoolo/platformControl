using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHitbox : BaseHitbox
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //预留3个镭射检测

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("a");
        if (CheckIsTrigger(other))
        {
            colls.setIsOnWall(true);
            //镭射检测
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (CheckIsTrigger(other))
        {
            //镭射检测
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (CheckIsTrigger(other))
        {
            colls.setIsOnWall(false);
        }
    }
}
