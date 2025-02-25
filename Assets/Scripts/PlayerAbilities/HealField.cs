﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealField : MonoBehaviour
{
    private float fireRate = 15f;  //the amount of time in seconds before the ability can be used again
    private float cooldown = 0f;  //used to create a timer with Time.time and firerate so the ability cannot be used all the time

    PlayerControl PController;
    //PlayerMovement otherController; //just a reference to the enemy testing player script

    public Transform playerPos;
    public GameObject healObject;
    private GameObject placedHeal;

    public float lifeTime = 5f;

    public Text timer;
    public Image clock;

    public GameObject abiltyText;
    
    // Start is called before the first frame update
    void Start()
    {
        PController = GetComponentInParent<PlayerControl>();
        PController = GetComponent<PlayerControl>();
        //otherController = GetComponentInParent<PlayerMovement>();
        playerPos = gameObject.transform;

        timer.text = cooldown.ToString("0");
        
        clock.fillAmount = cooldown / fireRate;

        //cooldown = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (PController != null)
        {
            if (cooldown <= 0f)  //if player uses fire2 and the timer is above or equal to the cooldown then it proceeds
            {
                timer.color = Color.white;
                timer.text = ("X");
                                

                if (Input.GetButtonDown(PController.F3))
                {
                    timer.color = Color.white;
                    cooldown = fireRate;  //the cooldown equals the timer which counts up, and if it has been the amount of the firerates seconds then itll Cast
                   // Debug.Log("Cooldown time is " + cooldown);

                   // Debug.Log("Fired");
                    Field();  //Field referce to void Cast() where it shoots the raycast for the ability

                }

            }
            else
            {
                timer.text = cooldown.ToString("0");
                clock.fillAmount = cooldown / fireRate;
                cooldown -= Time.deltaTime;
            }
        }

    }

    void Field()
    {

        placedHeal = Instantiate(healObject, playerPos.position, playerPos.rotation);

        abiltyText.SetActive(true); Debug.Log("set Active " + abiltyText.activeSelf);
        abiltyText.GetComponentInChildren<Text>().text = ("Heal Bot");
        abiltyText.GetComponentInChildren<Text>().color = Color.green;

        /*if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destruction();
            }
        }*/
        Destruction();
    }
        
    void Destruction()
    {
        // Debug.Log("I am being Detroyed");
        Destroy(placedHeal, lifeTime);        
    }

    public void InactivateText()
    {
        abiltyText.SetActive(false);
    }

}
