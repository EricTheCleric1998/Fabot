using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    //variables
    public Image invisibleIcon, nightVisionIcon;
    public static bool invisibleDark = true, nightVisionDark = true;
    Color darkened = new Color(1f, 1f, 1f, .4f);
    public Text ammoCounter;
    public Text health;
    public static int repairsLeft = 5;
    public static int ammo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammo = PlayerController.ammo;
        ammoCounter.text = ammo.ToString();
        repairsLeft = PlayerController.playerHealthMax = PlayerController.playerHealth;
        health.text = repairsLeft.ToString();
    }

    //called at fixed interval
    private void FixedUpdate()
    {
        if (invisibleDark == true)//for invisibility
        {
            invisibleIcon.GetComponent<Image>().color = darkened;
        }
        else
        {
            invisibleIcon.GetComponent<Image>().color = Color.white;
        }

        if (nightVisionDark == true)//for night vision
        {
            nightVisionIcon.GetComponent<Image>().color = darkened;
        }
        else
        {
            nightVisionIcon.GetComponent<Image>().color = Color.white;
        }
    }
}
