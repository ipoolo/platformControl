using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHitbox : BaseHitbox
{
    // Start is called before the first frame update



    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other_"+ other.gameObject.name);
        if (CheckIsTrigger(other))
        {
            colls.setIsOnFloor(true);
            if(other.gameObject.layer == LayerMask.NameToLayer("Wall")) {
                colls.footOnState = footOn.footOnWall;
            }else if (other.gameObject.layer == LayerMask.NameToLayer("OneWayPaltform"))
            {
                colls.footOnState = footOn.footOnOneWayPlatform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (CheckIsTrigger(other))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down,0.01f, LayerMask.GetMask(masklayerStrings));
            if (!hit.collider) { 
                colls.setIsOnFloor(false);
                colls.footOnState = footOn.none;
            }
        }
    }


}
