using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public int type = 2;
    private float tick = 0;
    private float lifetime;
    private float radius;
    private float power;
    public GameObject explosion;

    void Start()
    {
        switch (type)
        {
            case 1: //stun and low damage(impact)
                
                break;

            case 2: //black hole pull and medium damage(timer)
                power = -500;
                radius = 3;
                lifetime = 4;
                break;

            case 3: //explosion(timer)
                power = 500;
                radius = 3;
                lifetime = 4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        tick += Time.deltaTime;

        switch (type)
        {
            case 1: //stun and low damage(impact)
                
                break;

            case 2: //black hole pull and medium damage(timer)

                break;

            case 3: //explosion(timer)
                if (tick >= lifetime) Explode();
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor") {
            //gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            Explode();
        }
    }

    void Explode()
    {
        Destroy(gameObject);

        Vector3 explosionPos = transform.position;

        switch (type)
        {
            case 1: //stun and low damage(impact)

                break;

            case 2: //black hole pull and medium damage(timer)
                //Succ Enemies
                Collider[] colliders3 = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders3)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null) rb.AddExplosionForce(power, explosionPos, radius + 2f, 1f);
                }
                break;

            case 3: //explosion(timer)
                //Kill Enemies
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders)
                {
                    //Rigidbody rb = hit.GetComponent<Rigidbody>();
                    MeshDeath deathScript = hit.GetComponentInParent<MeshDeath>();
                    if (deathScript != null && !deathScript.isDead)
                    {
                        deathScript.die();
                        //if (rb != null) rb.AddExplosionForce(power, explosionPos, radius, -3F);
                        deathScript.isDead = true;
                    }
                    else
                    {
                        //if (rb != null) rb.AddExplosionForce(power, explosionPos, radius, -3F);
                    }
                }

                //Launch Enemies
                Collider[] colliders2 = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders2)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null) rb.AddExplosionForce(power, explosionPos, radius + 2f, 1f);
                }
                Instantiate(explosion, transform.position, transform.rotation);
                break;
        }
        

        
        
    }
}
