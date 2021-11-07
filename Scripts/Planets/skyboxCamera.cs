using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class skyboxCamera : MonoBehaviour
{
    //player
    private Transform player;
    private Transform playerCam;
    //skybox
    public float skyboxScale;

    private void Start()
    {
        //player
        player = GameObject.Find("Player").transform;
        playerCam = player.Find("Head").Find("PlayerCam").transform;
    }

    private void Update()
    {
        SkyboxCamera();
    }

    private void SkyboxCamera()
    {
        //rotation
        transform.rotation = playerCam.rotation;

        //movement
        transform.localPosition = playerCam.position / skyboxScale;
    }
}
