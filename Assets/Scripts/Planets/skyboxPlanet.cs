using System.Collections.Generic;
using UnityEngine;

public class skyboxPlanet : MonoBehaviour
{
    //script
    private skyboxTerrainFaces terrainScript;
    //skybox planet mesh
    private MeshFilter[] skyboxMeshFilters;
    private skyboxTerrainFaces[] terrainFaces;
    private int resolution = 20;
    //planet surface
    public Vector3[] surfaceVertices;

    private void Start()
    {
        surfaceVertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero};
        InitialiseSkybox();
        GenerateSkyboxMesh();
    }

    private void Update()
    {
        foreach (skyboxTerrainFaces face in terrainFaces)
        {
            face.CheckPlayerDistance(this);
        }
    }

    private void InitialiseSkybox()
    {
        //skybox planet mesh
        skyboxMeshFilters = new MeshFilter[6];
        terrainFaces = new skyboxTerrainFaces[6];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };

        //create 6 meshes for faces of skybox
        for (int i = 0; i < 6; i++)
        {
            //create face mesh
            GameObject meshObj = new GameObject("mesh" + i);
            meshObj.transform.parent = transform;
            meshObj.layer = LayerMask.NameToLayer("PlanetSkybox");

            meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            skyboxMeshFilters[i] = meshObj.AddComponent<MeshFilter>();
            skyboxMeshFilters[i].sharedMesh = new Mesh();

            terrainFaces[i] = new skyboxTerrainFaces(skyboxMeshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    private void GenerateSkyboxMesh()
    {
        foreach (skyboxTerrainFaces face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    public void InitialiseSurface(Vector3 vertex)
    {
        for (int i = 0; i < surfaceVertices.Length; i++)
        {
            if (surfaceVertices[i] == Vector3.zero)
            {
                surfaceVertices[i] = vertex;
                break;
            }
        }

        Debug.Log(surfaceVertices[0]);
        Debug.Log(surfaceVertices[1]);
        Debug.Log(surfaceVertices[2]);
    }
}
