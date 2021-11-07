using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetSkybox : MonoBehaviour
{
    //player
    private Transform player;
    private Transform playerCam;
    //ground
    private Transform ground;
    private MeshRenderer groundRenderer;
    private bool renderGround = true;
    private float maxHeight = 1000f;
    //skybox
    private Transform skybox;
    private float skyboxScale = 100;
    private Transform skyboxPlanet;
    private Transform skyboxCamera;


    private void Start()
    {
        //player
        player = GameObject.Find("Player").transform;
        playerCam = player.Find("Head").Find("PlayerCam");
        //ground
        ground = transform.Find("Ground");
        groundRenderer = ground.GetComponent<MeshRenderer>();
        //skybox
        skybox = transform.Find("Skybox");
        skyboxPlanet = skybox.Find("SkyboxPlanet");
        skyboxCamera = skybox.Find("SkyboxCamera");
    }

    private void Update()
    {
        GroundRenderCheck();
        SkyboxCamera();
    }

    private void GroundRenderCheck()
    {
        if (renderGround && player.position.y > maxHeight)
        {
            //disable ground when player high enough into the sky
            GroundRenderToggle(false);
        }
        else if (!renderGround && player.position.y <= maxHeight)
        {
            GroundRenderToggle(true);
        }
    }

    private void GroundRenderToggle(bool render)
    {
        groundRenderer.enabled = render;
        renderGround = render;
    }

    private void SkyboxCamera()
    {
        //rotation
        skyboxCamera.rotation = playerCam.rotation;

        //movement
        skyboxCamera.localPosition = playerCam.position / skyboxScale;
    }
}
