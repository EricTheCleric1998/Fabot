using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    //public Scene scene = SceneManager.GetActiveScene(); //for some reason this isn't working, the game won't play when this code is executed.
    public float movementSpeed;
    public Rigidbody2D rigidBody2D;
    public int viewResolution;
    public int fieldOfView;
    public int viewDistance;
    public GameObject glueSplash;
    public bool rocketBottom = false;//is changed to true once rocket bottom is collected
    public BoxCollider2D bc;
	
	public static int playerHealthMax = 5;//player stats
	public static int playerHealth = 0;
    public static int ammo = 2;//ammo for the glue gun
    public static int maxAmmo = 3;

    private Animator animator;
    private Vector2 moveDirection;
    public Material fovMaterial;
    public bool invisible = false;


    private GameObject fabotFieldOfView;
    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    private float meshDepth = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        fabotFieldOfView = new GameObject("Fabot Field of View") as GameObject;
        MeshRenderer meshRenderer = fabotFieldOfView.AddComponent<MeshRenderer>();
        //TODO change material
        meshRenderer.sharedMaterial = fovMaterial;
        viewMeshFilter = fabotFieldOfView.AddComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "Field of View Mesh";
        viewMeshFilter.mesh = viewMesh;
        fabotFieldOfView.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Lit Background");
        glueSplash.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Background Effects");
        glueSplash.layer = 9;


    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    // FixedUpdate is called in a fix interval
    void FixedUpdate()
    {
        Move();
        Scan();
    }

    private void Scan()
    {
        float stepAngleSize = fieldOfView / viewResolution;
        List<Vector3> viewPoints = new List<Vector3>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        int layer_mask = ~LayerMask.GetMask("Enemies", "Collectibles");

        for (int i = 0; i <= viewResolution; i++)
        {

            float angle = -Angle(moveDirection) + 90;
            float newAngle = angle - fieldOfView / 2 + stepAngleSize * i;
            float newAngleRad = newAngle * Mathf.PI / 180;
            Vector3 destination = new Vector3(transform.position.x + Mathf.Cos(newAngleRad) * viewDistance, transform.position.y + Mathf.Sin(newAngleRad) * viewDistance, 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(destination.x - transform.position.x, destination.y - transform.position.y, 0), viewDistance, layer_mask);

            if (hit.collider != null)
            {
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

    //when player collides with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Contains("Whetzel")){
            attacked();
			collision.gameObject.GetComponent<WhetzelController>().die();
		}
    }
    void ProcessInputs()//player inputs
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        animator.SetFloat("lastX", moveX);
        animator.SetFloat("lastY", moveY);

        if (ammo > 0 && Input.GetMouseButtonDown(0))//places a glue splash
        {
            ammo--;
            var v3 = Input.mousePosition;
            v3.z = 0;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            v3.z = 0;

            Instantiate(glueSplash, v3, Quaternion.identity);


            RaycastHit2D hit = Physics2D.Raycast(new Vector2(v3.x, v3.y), -Vector2.up);
            if (hit.collider != null)
            {

                if (hit.collider.gameObject.name.Contains("hetzel") || hit.collider.gameObject.name.Contains("Patroller"))
                {
                    hit.collider.gameObject.GetComponent<WhetzelController>().die();

                }
            }
        }

    }
	
	private void attacked(){
		playerHealth++;
        if (ammo < maxAmmo) {
            ammo++;
        }
		Debug.Log("PLAYER HEALTH UPDATED");

        if (playerHealth == 1)//gains torso and invisibility upgrade
        {
            StaticEffect.canUse = true;
            HUDScript.invisibleDark = false;
        }

		if(playerHealth < playerHealthMax){
			//new ability
		}
		else{
            Debug.Log("Player has been fully repaired.  Restarting Level");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
	}

    void Move()
    {
        rigidBody2D.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
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
    //when player collects something
    void Collect(Collision2D collision)
    {
        
    }
}
