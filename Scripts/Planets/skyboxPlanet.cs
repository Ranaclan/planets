using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private MeshFilter surfaceMeshFilter;
    private Mesh surfaceMesh;
    public Vector3[] surfaceVertices;
    private int maxSurfaceVertices = 6;
    private bool surface = false;
    private int renderVertices = 0;
    public Material surfaceMat;
    //skybox
    private Transform skyboxCam;
    private skyboxCamera camScript;
    private float skyboxScale;

    private void Start()
    {
        surfaceVertices = new Vector3[maxSurfaceVertices];
        for (int i = 0; i < maxSurfaceVertices; i++)
        {
            surfaceVertices[i] = Vector3.zero;
        }

        skyboxCam = GameObject.Find("SkyboxCamera").transform;
        camScript = skyboxCam.GetComponent<skyboxCamera>();
        skyboxScale = camScript.skyboxScale;

        InitialiseSkybox();
        GenerateSkyboxMesh();
    }

    private void Update()
    {
        foreach (skyboxTerrainFaces face in terrainFaces)
        {
            face.CheckPlayerDistance(this);
        }

        SurfaceMesh();
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
            GameObject meshObject = new GameObject("FaceMesh" + i);
            meshObject.transform.parent = transform;
            meshObject.layer = LayerMask.NameToLayer("PlanetSkybox");

            meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            skyboxMeshFilters[i] = meshObject.AddComponent<MeshFilter>();
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

    public void AddSurfaceVertex(Vector3 vertex)
    {
        //add vertices on skybox planet when skybox camera close enough
        if (renderVertices < maxSurfaceVertices)
        {
            surfaceVertices[renderVertices] = vertex * skyboxScale;
            renderVertices++;
        }
    }

    private void SurfaceMesh()
    {
        if (renderVertices % 3 == 0 && !surface)
        {
            Debug.Log("a");
            //construct mesh of surface when player close enough
            GameObject surfaceObject = new GameObject("Surface");
            surfaceObject.transform.parent = transform;
            surfaceMeshFilter = surfaceObject.AddComponent<MeshFilter>();
            surfaceObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(surfaceMat);
            MeshCollider surfaceCollider = surfaceObject.AddComponent<MeshCollider>();
            surfaceObject.layer = LayerMask.NameToLayer("Ground");
            surfaceMeshFilter.sharedMesh = new Mesh();
            surfaceMesh = surfaceMeshFilter.sharedMesh;
            surfaceMesh.name = "SurfaceMesh";

            //create triangles
            int[] triangles = new int[renderVertices];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = i;
            }

            surfaceMesh.Clear();
            surfaceMesh.vertices = surfaceVertices;
            surfaceMesh.triangles = triangles;
            surfaceMesh.RecalculateNormals();
            surfaceCollider.sharedMesh = surfaceMesh;

            surface = true;
            renderVertices = 0;
        }
    }
}
