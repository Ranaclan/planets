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

        //loop through vertices of face
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB; //determine vertex positon
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized; //make each vertex same distance from centre, forming sphere
                vertices[i] = pointOnUnitSphere;

                //set triangles around current vertex (if vertex is not on far right or bottom edge)
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
                if (Vector3.Distance(skyboxCamera.position, vertices[i]) < (500 / skyboxScale))
                {
                    script.AddSurfaceVertex(vertices[i]);
                }
            }
        }
    }
}
