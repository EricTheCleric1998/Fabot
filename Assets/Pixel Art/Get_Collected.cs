using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Get_Collected : MonoBehaviour
{
    public BoxCollider2D bc;
    public GameObject player;
    private string playername = "FaBot (Main Player)";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //called when touched by Player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == playername)
        {
            player.GetComponent<PlayerController>().rocketBottom = true;
            Destroy(gameObject);
            //for playtest only, we will reload current scene
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
