using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHitbox : BaseHitbox
{
    [Header("ClimbRay")]
    public Transform climbRayTransform;
    public float climbRayDistance;
    [Header("propupRay")]
    public Transform propupRayTransform;
    public float propupRayDistance;
    [Header("FootRay")]
    public Transform footRayTransform;
    public float footRayDistance;

    public LayerMask rayMaskLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        colls.isClimbRayOverlap = RayCastCheck(climbRayTransform.position, climbRayDistance);
        colls.ispropupRayOverlap = RayCastCheck(propupRayTransform.position,propupRayDistance);
        colls.isFootRayOverlap = RayCastCheck(footRayTransform.position,footRayDistance); 

    }

    private bool RayCastCheck(Vector2 position , float distance)
    {
        bool result;
        RaycastHit2D hit = Physics2D.Raycast(position,Vector2.right,distance, rayMaskLayer.value);
        if (hit.collider)
        {
            //有
            result = true;
        }
        else
        {
            //没有
            result = false;
        }

        return result;
    }
    //预留3个镭射检测

    private void OnTriggerEnter2D(Collider2D other)
    {
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
