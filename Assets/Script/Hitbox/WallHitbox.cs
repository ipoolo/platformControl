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
    [Header("FootDownRay")]
    public Transform footDownRayTransform;
    public float footDownRayDistance;

    [Header("HeadPoint")]
    public Transform headPointTransform;

    public LayerMask rayMaskLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        colls.isTopAboveOverlap = RayCastCheck(climbRayTransform.position, climbRayDistance);
        colls.ispropupRayOverlap = RayCastCheck(propupRayTransform.position,propupRayDistance);
        colls.isFootRayOverlap = RayCastCheck(footRayTransform.position,footRayDistance);
        colls.isFootDownRayOverlap = RayCastCheck(footDownRayTransform.position, footDownRayDistance) && !RayCastCheckUpDown(footDownRayTransform.position, footDownRayDistance);
        Debug.DrawLine(footDownRayTransform.position, footDownRayTransform.position + new Vector3(footDownRayDistance,0),Color.red,1 );
    }

    private bool RayCastCheck(Vector2 position , float distance)
    {
        bool result;
        RaycastHit2D hit = Physics2D.Raycast(position,transform.right,distance, rayMaskLayer.value);
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

    private bool RayCastCheckUpDown(Vector2 position, float distance)
    {
        bool result;
        RaycastHit2D hit = Physics2D.Raycast(position, transform.up * -1, distance, rayMaskLayer.value);
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
