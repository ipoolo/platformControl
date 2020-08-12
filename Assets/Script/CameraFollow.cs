using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D playerRb;
    public Collisions colls;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        colls = player.GetComponent<Collisions>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playerPositionX = player.transform.position.x;
        float playerPositionY = player.transform.position.y;
            if(Vector2.Distance(Camera.main.transform.position,player.transform.position) > 0.1f) { 
                float cameraTargetX = 0;
                float cameraTargetY = 0;
                float aspectRatio = Screen.width / Screen.height;
                float cameraHeight = Camera.main.orthographicSize * 2;
                float cameraWidth = cameraHeight * aspectRatio;
            
            
            

                if (playerRb.velocity.x > 0)
                {
                    cameraTargetX = playerPositionX + cameraWidth * 0.25f;
                }
                else if(playerRb.velocity.x == 0)
                {
                    cameraTargetX = playerPositionX ;
                }
                if (playerRb.velocity.x < 0)
                {
                    cameraTargetX = playerPositionX - cameraWidth * 0.25f;
                }

                //if (playerRb.velocity.y > 0)
                //{
                //    cameraTargetY = playerPositionY - screenHeight * 0.25f;
                //}
                //else if (playerRb.velocity.y == 0)
                //{
                    cameraTargetY = playerPositionY ;
                //}
                //if (playerRb.velocity.y < 0)
                //{
                //    cameraTargetY = playerPositionY + screenHeight * 0.25f;
                //}

                Vector3 targetPosiotn = new Vector3(cameraTargetX,cameraTargetY,-10.0f);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosiotn, 0.015f );
            }
    }
}
