using UnityEngine;

public class skyboxTerrainFaces
{
    //mesh
    private Mesh mesh;
    public int resolution;
    private Vector3[] vertices;
    private Vector3 localUp;
    private Vector3 axisA;

    private Vector3 axisB;

    //skybox
    private Transform skyboxCamera;
    private float skyboxScale;

    //player
    private Transform player;
    private Transform playerCam;

    public skyboxTerrainFaces(Mesh mesh, int resolution, Vector3 localUp)
    {
        //mesh
        this.mesh = mesh;
        this.resolution = resolution;
        vertices = new Vector3[resolution * resolution];
        this.localUp = localUp;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
        //skybox
        skyboxCamera = GameObject.Find("SkyboxCamera").transform;
        skyboxScale = skyboxCamera.GetComponent<skyboxCamera>().skyboxScale;
        //player
        player = GameObject.Find("Player").transform;
        playerCam = player.Find("Head").Find("PlayerCam");
    }

    public void ConstructMesh()
    {
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triangleIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triangleIndex] = i;
                    triangles[triangleIndex + 1] = i + resolution + 1;
                    triangles[triangleIndex + 2] = i + resolution;
                    triangles[triangleIndex + 3] = i;
                    triangles[triangleIndex + 4] = i + 1;
                    triangles[triangleIndex + 5] = i + resolution + 1;
                    triangleIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public void CheckPlayerDistance(skyboxPlanet script)
    {
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                if (Vector3.Distance(skyboxCamera.position, vertices[i]) < (5 / skyboxScale))
                {
                    script.InitialiseSurface(vertices[i]);
                }
            }
        }
    }
}
