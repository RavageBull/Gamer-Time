﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamAttack : MonoBehaviour
{
    public GameObject currentHitObject;

    public ParticleSystem Shooty;

    public float defaultDamage;
    public float damage = 1f;  //sets the damage of the rays being cast
    public float damageMulti;

    public float maxDistance = 50f;  //sets the range of the ability
    public float sphereRadius;
    public LayerMask layerMask;
    public float castTime = 0.1f;

    private float fireRate = 8f;  //the amount of time in seconds before the ability can be used again
    private float cooldown = 0f;  //used to create a timer with Time.time and firerate so the ability cannot be used all the time
    private float abilityTime = 3f;  //the amount of time that the beam attack casts for

    public Vector3 firePoint;
    private Vector3 direction;

    private float currentHitDistance;

    PlayerControl PController;
    //PlayerMovement otherController; //just a reference to the enemy testing player script

    // public Slider abilitySlider;
    public Text timer;
    public Image clock;
    public GameObject damageUI;

    public GameObject abiltyText;

    public AudioClip audioClip;
    public AudioSource audioSource;

    void Start()
    {
        PController = GetComponentInParent<PlayerControl>();
        //otherController = GetComponentInParent<PlayerMovement>();
        //cooldown = fireRate;

        timer.text = cooldown.ToString("0");

        clock.fillAmount = cooldown / fireRate;

        defaultDamage = damage;


    }

    // Update is called once per frame
    void Update()
    {
        if (PController != null)
        {
            if (cooldown <= 0f)  //if player uses fire2 and the timer is above or equal to the cooldown then it proceeds
            {
                timer.color = Color.white;
                timer.text = ("B");

                if (Input.GetButtonDown(PController.F2))
                {
                    abiltyText.SetActive(true);
                    abiltyText.GetComponentInChildren<Text>().text = ("Beam Attack");
                    abiltyText.GetComponentInChildren<Text>().color = Color.red;

                    timer.color = Color.white;
                    cooldown = fireRate;  //the cooldown equals the timer which counts up, and if it has been the amount of the firerates seconds then itll Cast
                    Debug.Log("Cooldown time is " + cooldown);

                    //Debug.Log("Fired");
                    Cast();  //Cast referce to void Cast() where it shoots the raycast for the ability
                    audioSource.clip = audioClip;
                    audioSource.Play();

                }
            }
            else
            {
                timer.text = cooldown.ToString("0");
                clock.fillAmount = cooldown / fireRate;
                if (clock.fillAmount <= 0.01f)
                {
                    clock.fillAmount = 0f;
                }

                cooldown -= Time.deltaTime;
            }
        }
    }


    void Cast()
    {
        firePoint = transform.position;
        direction = transform.forward;
        RaycastHit hit;
        if (Physics.SphereCast(firePoint, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;

            Invoke("Cast", castTime);  //starts Cast again based on the public float castTime
            EnemyStats target = hit.transform.GetComponent<EnemyStats>();
            if (target != null)
            {
                // Debug.Log(hit.transform.name);
                target.TakeDamage(damage);
            }
            shooty();
        }
        StartCoroutine("WaitAndExecute");
        Invoke("StopExecution", abilityTime); //when abilityTime is reached it calls StopExecution
    }

    void shooty()
    {
        Shooty.Play();
        StartCoroutine(StopShooty(Shooty, 0.1f));
    }

    IEnumerator StopShooty(ParticleSystem particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
        Shooty.Stop();
    }

    void StopExecution()
    {
        //Debug.Log("Ability pause done");
        StopCoroutine("WaitAndExecute");  //stops StartCoroutine
        CancelInvoke("Cast");  //Stops Cast from repeating once abilityTime is met

        abiltyText.SetActive(false);
    }

    IEnumerator WaitAndExecute()
    {
        //print("Printed after wait time");
        yield return new WaitForSeconds(abilityTime);

        // StartCoroutine("WaitAndExecute");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(firePoint, firePoint + direction * currentHitDistance);
        Gizmos.DrawWireSphere(firePoint + direction * currentHitDistance, sphereRadius);
    }

    public void UpdateDamage(int cooldown)
    {
        GetComponent<PlayerStats>().pickUpActive = true;
        damageUI.SetActive(true);

        damage = defaultDamage * damageMulti;
        Debug.Log("damage updated " + damage);
        StartCoroutine(DamageCooldown(cooldown));
    }

    IEnumerator DamageCooldown(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ResetDamage();
    }

    public void ResetDamage()
    {
        damage = defaultDamage;
        damageMulti = 0;

        damageUI.SetActive(false);
        GetComponent<PlayerStats>().pickUpActive = false;
    }
}







