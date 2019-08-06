﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public enum PickupType
    {
        speed,
        maxHealth,
        damage
    }
    public PickupType myType;

    public float speedMultiValue;
    public int cooldownValue = 3;
    public static bool spawned = false;

    public float damageMultiValue = 5f;



    //public Material matofobject;
    public Color newcol;

    private void Start()
    {
        spawned = true;        
    }


    private void OnTriggerEnter(Collider col)
    {
        PlayerControl player = col.GetComponent<PlayerControl>();
        if (player != null)
        {
            
                switch (myType)
                {
                    case PickupType.speed:
                        player.GetComponent<PlayerStats>().speedMulti = speedMultiValue;
                        player.GetComponent<PlayerStats>().UpdateSpeed(cooldownValue);                                          
                        DestroyObject();
                        break;

                    case PickupType.maxHealth:
                        player.GetComponent<Health>().maxhealth += 25;
                        DestroyObject();
                        break;

                    case PickupType.damage:
                        player.GetComponent<BeamAttack>().damageMulti = damageMultiValue;
                        player.GetComponent<BeamAttack>().UpdateDamage(cooldownValue);
                        DestroyObject();
                    break;
                }
            
        }
        
    }
    
    private void DestroyObject()
    {
        Destroy(gameObject);
        spawned = false;
    }
}
