using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    public GameObject BulletPrefab;

    /*public float ShootingRate = 0.25f;
    private float shootCooldown;*/

    // Start is called before the first frame update
    void Start()
    {
        //shootCooldown = ShootingRate;

        GazeManager.Instance.subscribedComponents.Add(this);
    }

    void OnSelect()
    {
        if (GazeManager.Instance.FocusedObject.tag != "DontShootWhenTaped")
        {
            Shoot();
        }
    }

    void Shoot()
    {
        /*if (shootCooldown < Time.deltaTime)
        {*/
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        Vector3 SpawnPosition = (gazeDirection * 0.3f) + headPosition;

        //just spawn the bullet (the bullet script on the bullet will take care of getting it up to speed)
        GameObject bullet = Instantiate(BulletPrefab, SpawnPosition, Camera.main.transform.rotation);

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
