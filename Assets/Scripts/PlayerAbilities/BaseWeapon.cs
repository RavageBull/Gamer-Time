﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseWeapon : MonoBehaviour

{
    PlayerControl PController;
    //PlayerMovement otherController; //just a reference to the enemy testing player script

    public int damage = 25;
    public float maxDistance = 100f;
    public Transform firePoint;

    private void Start()
    {
        PController = GetComponent<PlayerControl>();
        //otherController = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Moves Fowards based on speed
        //  GetComponent<Rigidbody>().velocity = transform.forward * speed;

      if(PController != null)
        {
            if (Input.GetButtonDown(PController.F1))
            {
                Shoot();
            }
        }
        
    }

    void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, firePoint.forward);        
        Debug.DrawLine(firePoint.position,(firePoint.forward * maxDistance), Color.red, 5f);

        if (Physics.Raycast(ray, out hit, maxDistance))        
        {
            EnemyStats target = hit.transform.GetComponent<EnemyStats>();
            if (target != null)
            {
                Debug.Log(hit.transform.name);
                target.TakeDamage(damage);
            }
            //reduce enemy health

        }


        // Destroy(this.gameObject);
        //destroy this bullet
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    //if not player or projectile
    //    if (other.tag != "Player" && other.tag != "Projectile")
    //    {
    //        if (other.tag == "Enemy")

    //        {
    //            RaycastHit hit;
    //            Ray ray = new Ray(transform.position, transform.forward);
    //            if (Physics.Raycast(ray, out hit, maxDistance)) ;

    //            hit.transform.GetComponent<HealthScript>().RemoveHealth(enemyDamage);
    //            //reduce enemy health

    //            Destroy(this.gameObject);
    //            //destroy this bullet
    //        }
    //    }
    //}
}

