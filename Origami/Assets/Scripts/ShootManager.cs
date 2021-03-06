﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootManager : MonoBehaviour
{
    public GameObject BulletPrefab;

    public AudioClip ShootSound;

    public bool Enabled;

    /*public float ShootingRate = 0.25f;
    private float shootCooldown;*/

    // Start is called before the first frame update
    void Start()
    {
        //shootCooldown = ShootingRate;

        GazeManager.Instance.subscribedComponents.Add(this);

        Enabled = false;
    }

    void OnSelect()
    {
        if (Enabled)
        {
            if (GazeManager.Instance.FocusedObject == null || GazeManager.Instance.FocusedObject.tag != "DontShootWhenTaped")
            {
                Shoot();
            }
        }
        
    }

    void Shoot()
    {
        /*if (shootCooldown < Time.deltaTime)
        {*/
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        Vector3 SpawnPosition = (gazeDirection * 0.6f) + headPosition;

        //just spawn the bullet (the bullet script on the bullet will take care of getting it up to speed)
        GameObject bullet = Instantiate(BulletPrefab, SpawnPosition, Camera.main.transform.rotation);

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = ShootSound;

        audioSource.Play();


            /*// Reset cooldown
            shootCooldown = ShootingRate;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }*/
    }
}
