﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum footOn
{
    none,
    footOnWall,
    footOnOneWayPlatform
}

public class Collisions : MonoBehaviour
{
    [Header("Wall")]
    [SerializeField]private bool isOnWall;
    public Action<bool> playerListenOnWallCallBack;

    [Header("Floor")]
    [SerializeField] private bool isOnFloor;
    public Action<bool> playerListenOnFloorCallBack;

    [Header("HeadChannel")]
    [SerializeField] private bool isOnHeadChannel;
    public Action<bool> playerListenOnHeadChannelCallBack;

    public bool isTopAboveOverlap;
    public bool ispropupRayOverlap;
    public bool isFootRayOverlap;
    public bool isFootDownRayOverlap;

    public PlayerControlHelper pch;



    private Animator animator;

    public footOn footOnState;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        pch = GetComponent<PlayerControlHelper>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimatorInfo();
        UpdateHelperStateInfo();
    }

    public void UpdateHelperStateInfo()
    {
        //if (!isFootRayOverlap && !ispropupRayOverlap && !isTopAboveOverlap)
        //{
        //    pch.canPropUpPass = false;
        //    pch.canClimbPass = false;

        //}
        //else if (isFootRayOverlap && !ispropupRayOverlap && !isTopAboveOverlap)
        //{
        //    //越过
        //    pch.canPropUpPass = true;
        //    pch.canClimbPass = false;
        //}
        //else if (isFootRayOverlap && ispropupRayOverlap && !isTopAboveOverlap)
        //{
        //    //攀爬
        //    pch.canPropUpPass = false;
        //    pch.canClimbPass = true;
        //    return;
        //}
        //else if (isFootRayOverlap && ispropupRayOverlap && isTopAboveOverlap)
        //{
        //    pch.canPropUpPass = false;
        //    pch.canClimbPass = false;
        //}
        //else if (!isFootRayOverlap && !ispropupRayOverlap && isTopAboveOverlap)
        //{
        //    pch.canPropUpPass = false;
        //    pch.canClimbPass = true;
        //}

        if (isTopAboveOverlap)
        {
            //头顶有墙,全部不能
            pch.canPropUpPass = false;
            pch.canClimbPass = false;
            return;
        }
        else if (ispropupRayOverlap)
        {
            pch.canPropUpPass = false;
            pch.canClimbPass = true;
            return;
        }
        else if (isFootRayOverlap)
        {
            pch.canPropUpPass = true;
            pch.canClimbPass = false;
            return;
        }
        else
        {
            pch.canPropUpPass = false;
            pch.canClimbPass = false;
        }




    }


    public void setIsOnWall(bool _isOnWall)
    {
        if (isOnWall != _isOnWall) { 
            isOnWall = _isOnWall;
            playerListenOnWallCallBack(_isOnWall);
        }
    }

    public bool getIsOnWall()
    {
        return isOnWall;
    }

    public void setIsOnFloor(bool _isOnFloor)
    {
        if(isOnFloor != _isOnFloor)
        {
            isOnFloor = _isOnFloor;
            playerListenOnFloorCallBack(_isOnFloor);
        }
    }

    public bool getIsOnFloor()
    {
        return isOnFloor;
    }

    public void setIsOnHeadChannel(bool _isOnHeadChannel)
    {
        if (isOnHeadChannel != _isOnHeadChannel)
        {
            isOnHeadChannel = _isOnHeadChannel;

        }
    }

    public bool getIsOnHeadChannel()
    {
        return isOnHeadChannel;
    }

    public void FireHeadChannel()
    {
        Debug.Log("FireHeadChannel");
        playerListenOnHeadChannelCallBack(true);
    }

    private void UpdateAnimatorInfo()
    {

        animator.SetBool("IsOnFloor", isOnFloor);
    }
}
