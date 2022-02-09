using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhetzelSpawner : MonoBehaviour
{
    protected GameObject fabot;
    private float nextActionTime = 0.0f;
    public float period = 5f;
    public float minSpawnRadius = 3f;
    public float maxSpawnRadius = 3f;
    public GameObject whetzel;

    // Start is called before the first frame update
    void Start()
    {
        fabot = GameObject.Find("FaBot (Main Player)");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            float x = fabot.transform.position.x + Random.Range(minSpawnRadius, maxSpawnRadius) * Mathf.Pow(-1, (int)Random.Range(0, 2));
            float y = fabot.transform.position.y + Random.Range(minSpawnRadius, maxSpawnRadius) * Mathf.Pow(-1, (int)Random.Range(0, 2));

            nextActionTime += period;
            Instantiate(whetzel, new Vector3(x, y, 0), Quaternion.identity);

        }
    }
}
