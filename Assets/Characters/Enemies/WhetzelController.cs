using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WhetzelController : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rigidBody2D;
    public int viewResolution;
    public int fieldOfView ;
    public int viewDistance;
    public Material fovMaterial;

    protected GameObject fabot;
    protected Vector2 moveDirection;
    protected bool followingFabot = false;

    private GameObject whetzelFieldOfView;
    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    private float meshDepth = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        fabot = GameObject.Find("FaBot (Main Player)");


        whetzelFieldOfView = new GameObject("Whetzel Field of View") as GameObject;
        whetzelFieldOfView.layer = 8;
        //whetzelFieldOfView.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Background Effects");

        MeshRenderer meshRenderer = whetzelFieldOfView.AddComponent<MeshRenderer>();
        //TODO change material
        meshRenderer.sharedMaterial = fovMaterial;
        viewMeshFilter = whetzelFieldOfView.AddComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "Field of View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    // Update is called once per frame
    protected void Update()
    {

    }


    protected void FixedUpdate()
    {
        ScanForFabot();
        if (fabot.GetComponent<PlayerController>().invisible)
            followingFabot = false;
    }

    public void die()
    {
        movementSpeed = 0;
        Destroy(whetzelFieldOfView);
        Destroy(gameObject);
    }


    private void ScanForFabot()
    {
        float stepAngleSize = fieldOfView / viewResolution;
        List<Vector3> viewPoints = new List<Vector3>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        for (int i = 0; i <= viewResolution; i++)
        {

            float angle = -Angle(moveDirection) + 90;
            float newAngle = angle - fieldOfView / 2 + stepAngleSize * i;
            float newAngleRad = newAngle * Mathf.PI / 180;
            Vector3 destination = new Vector3(transform.position.x + Mathf.Cos(newAngleRad) * viewDistance, transform.position.y + Mathf.Sin(newAngleRad) * viewDistance, 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(destination.x - transform.position.x, destination.y - transform.position.y, 0), viewDistance);

            if (hit.collider != null)
            {
                if (hit.collider == fabot.GetComponent<BoxCollider2D>())
                    followingFabot = true;
                viewPoints.Add(new Vector3(hit.point.x, hit.point.y, meshDepth));
            }
            else
            {
                viewPoints.Add(new Vector3(destination.x, destination.y, meshDepth));
            }
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = new Vector3(transform.position.x, transform.position.y, meshDepth);
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = viewPoints[i];

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.Optimize();
        viewMesh.RecalculateNormals();
        viewMesh.RecalculateBounds();
    }

    private float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }
}
