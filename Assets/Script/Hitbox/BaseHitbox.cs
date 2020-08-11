using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitbox : MonoBehaviour
{
    protected Collisions colls;
    // Start is called before the first frame update
    protected void Awake()
    {
        colls = GetComponentInParent<Collisions>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("collsion")]

    public string[] masklayerStrings;
    protected bool CheckIsTrigger(Collider2D other)
    {
        bool result = false;
        foreach (string str in masklayerStrings)
        {
            if (str == LayerMask.LayerToName(other.gameObject.layer))
            {
                result = true;
                break;
            }
        }
        return result;
    }
}
