﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseWeapon : MonoBehaviour

{
    PlayerControl PController;
    //PlayerMovement otherController; //just a reference to the enemy testing player script

    public GameObject currentHitObject;

    public int defaultDamage;
    public int damage = 25;
    public int damageMulti;

    public float maxDistance = 100f;
    public float sphereRadius;
    public LayerMask layerMask;

    public Vector3 firePoint;
    private Vector3 direction;

    private float currentHitDistance;

    public ParticleSystem Shooty;

    public AudioClip audioClip;

    public AudioSource audioSource;
    private void Start()
    {
        PController = GetComponent<PlayerControl>();
        //otherController = GetComponentInParent<PlayerMovement>();
        defaultDamage = damage;
        

    }
   
    // Update is called once per frame
    void Update()
    {

      if(PController != null)
      {
            if (Input.GetButtonDown(PController.F1))
            {
                Shoot();
                shooty();
                audioSource.clip = audioClip;
                audioSource.Play();
            }
      }
        
    }

    void Shoot()
    {
        firePoint = transform.position;
        direction = transform.forward;
        RaycastHit hit;

        if (Physics.SphereCast(firePoint, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;

            EnemyStats target = hit.transform.GetComponent<EnemyStats>();
            if (target != null)
            {
                //Debug.Log(hit.transform.name);
                target.TakeDamage(damage);
            }
        }
    }

    void shooty()
    {
        Shooty.Play();
        StartCoroutine(StopShooty(Shooty, 0.15f));
    }

    IEnumerator StopShooty(ParticleSystem particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
        Shooty.Stop();
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

        damage = defaultDamage * damageMulti;
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
    }
}