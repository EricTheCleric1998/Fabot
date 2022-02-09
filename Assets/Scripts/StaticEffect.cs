using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEffect : MonoBehaviour
{

    private static string MATERIAL_INTENSITY_PARAM = "_Intensity";

    public float maxIntensityDuration = 2f;
    public float intensityDurationDecay = 0.5f;

    private float maxIntensityElapsed;
    private float intensityDecayElapsed;

    public static bool canUse = false, tempCanUse = false;
    protected GameObject fabot;

    private Material material;

    private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        fabot = GameObject.Find("FaBot (Main Player)");

        this.maxIntensityElapsed = maxIntensityDuration;
        this.intensityDecayElapsed = intensityDurationDecay;
    }

    // Update is called once per frame
    void Update()
    {
        float intensity;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canUse == true)
            {
                canUse = false;
                HUDScript.invisibleDark = true;
                canUse = false;
                this.maxIntensityElapsed = 0f;
                this.intensityDecayElapsed = 0f;
                collider.enabled = false;
                tempCanUse = true;
                fabot.GetComponent<PlayerController>().invisible = true;
            }
        }

        if (this.maxIntensityElapsed < maxIntensityDuration)
        {
            this.maxIntensityElapsed += Time.deltaTime;
            intensity = 1f;
        }

        else if(this.intensityDecayElapsed < intensityDurationDecay)
        {
            this.intensityDecayElapsed += Time.deltaTime;
            intensity = Mathf.Lerp(1f, 0f, this.intensityDecayElapsed / intensityDurationDecay);
        }

        else
        {
            intensity = 0f;
            collider.enabled = true;
            fabot.GetComponent<PlayerController>().invisible = false;
            if (tempCanUse)
            {
                HUDScript.invisibleDark = false;
                tempCanUse = false;
                canUse = true;
            }
        }

        this.material.SetFloat(MATERIAL_INTENSITY_PARAM, intensity);
    }

    private void Awake()
    {
        this.material = this.GetComponent<SpriteRenderer>().material;
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnDestroy()
    {
        Destroy(this.material);
    }
}
