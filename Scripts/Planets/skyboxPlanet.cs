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
        surfaceVertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero};

        skyboxCam = transform.Find("SkyboxCamera").transform;
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
            GameObject meshObject = new GameObject("mesh" + i);
            meshObject.transform.parent = transform;
            meshObject.layer = LayerMask.NameToLayer("PlanetSkybox");

            meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(surfaceMat);
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
            for (int i = 0; i < surfaceVertices.Length; i++)
            {
                if (surfaceVertices[i] == Vector3.zero)
                {
                    surfaceVertices[i] = vertex * skyboxScale;
                    renderVertices++;
                    break;
                }
            }
        }
    }

    private void SurfaceMesh()
    {
        if (renderVertices == 3 && !surface)
        {
            //construct mesh of surface when player close enough
            GameObject surfaceObject = new GameObject("surface");
            surfaceObject.transform.parent = transform;
            surfaceMeshFilter = surfaceObject.AddComponent<MeshFilter>();
            surfaceObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(surfaceMat);
            MeshCollider surfaceCollider = surfaceObject.AddComponent<MeshCollider>();
            surfaceObject.layer = LayerMask.NameToLayer("Ground");
            surfaceMeshFilter.sharedMesh = new Mesh();
            surfaceMesh = surfaceMeshFilter.sharedMesh;
            surfaceMesh.name = "SurfaceMesh";
            surfaceCollider.sharedMesh = surfaceMesh;

            surfaceMesh.Clear();
            surfaceMesh.vertices = surfaceVertices;
            surfaceMesh.triangles = new int[3] {0, 1, 2};
            surfaceMesh.RecalculateNormals();

            surface = true;
            renderVertices = 0;
        }
    }
}
