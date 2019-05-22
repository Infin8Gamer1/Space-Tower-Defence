using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float DestroyTime = 10.0f;

    public float Speed = 50.0f;

    [Range(0,300)]
    public int Damage = 100;

    public GameObject BulletCollisionParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this, DestroyTime);
        StartCoroutine(Delete());

        //Assuming that the bullet is put in the correct orentation apon getting spawned
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed);
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(DestroyTime);

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "HomeBase")
        {
            Health otherHealth = collision.gameObject.GetComponent<Health>();

            //remove health from enemy
            if (otherHealth != null)
            {
                otherHealth.RemoveHealth(Damage);
            }
        }

        //spawn particle system
        if (BulletCollisionParticleSystem != null)
        {
            Instantiate(BulletCollisionParticleSystem, transform.position, Quaternion.identity);
        }

        //destroy this bullet
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
