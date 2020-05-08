using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grenade : MonoBehaviour
{

    public int type = 2;
    private float tick = 0;
    private float lifetime;
    private float radius;
    private float power;
    public GameObject explosion;
    public GameObject blackhole;
    public GameObject stunEffect;
    private bool isExploding = false;
    private Vector3 explosionPos2;
    private bool hasKilled = false;
    public Material regular;
    public Material bright;
    public Material dark;
    public float bhelevation = 0.5f;

    void Start()
    {
        switch (type)//DO NOT CHANGE VALUES WITHOUT ASKING SAM
        {
            case 1: //stun and low damage(impact)
                gameObject.GetComponent<Renderer>().material = bright;
                radius = 3;
                lifetime = 3;
                break;

            case 2: //black hole pull and medium damage(impact)
                gameObject.GetComponent<Renderer>().material = dark;
                power = -1000; //-500
                radius = 6;
                lifetime = 4;
                break;

            case 3: //explosion(timer)
                gameObject.GetComponent<Renderer>().material = regular;
                power = 500;
                radius = 3;
                lifetime = 1f;
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
                if (isExploding)
                {
                    Collider[] colliders9 = Physics.OverlapSphere(explosionPos2, radius);
                    foreach (Collider hit in colliders9)
                    {
                        if (hit.tag == "Enemy")
                        {
                            NavMeshAgent agent = hit.GetComponent<NavMeshAgent>();
                            if (agent != null) agent.enabled = false;
                            Rigidbody rb = hit.GetComponent<Rigidbody>();
                            Vector3 none = new Vector3(0, 0, 0);
                            if (rb != null) rb.velocity = new Vector3(0, 0, 0);
                            if (tick > 2)
                            {
                                Destroy(gameObject);
                                if (agent != null) agent.enabled = false;
                            }
                        }

                    }

                    

                }
                break;

            case 2: //black hole pull and medium damage(impact)
                if (isExploding)
                {
                    power += 1600*Time.deltaTime; //380
                    Collider[] colliders4 = Physics.OverlapSphere(explosionPos2, radius);
                    foreach (Collider hit in colliders4)
                    {
                        if (hit.tag != "PlayerProjectiles")
                        {
                            NavMeshAgent agent = hit.GetComponent<NavMeshAgent>();
                            if (agent != null) agent.enabled = false;
                            Rigidbody rb = hit.GetComponent<Rigidbody>();
                            if (rb != null) rb.velocity = new Vector3(0, 0, 0);
                            if (rb != null) rb.AddExplosionForce(power, explosionPos2, radius, 1f);
                            if (agent != null) agent.enabled = true;
                        }
                    }
                    if (power >= 0 && !hasKilled)
                    {
                       
                        Collider[] colliders6 = Physics.OverlapSphere(explosionPos2, radius-2);
                        foreach (Collider hit in colliders6)
                        {
                            if (hit.tag != "PlayerProjectiles")
                            {
                                hasKilled = true;
                                MeshDeath deathScript = hit.GetComponentInParent<MeshDeath>();
                                if (deathScript != null && !deathScript.isDead)
                                {
                                    deathScript.die();
                                    deathScript.isDead = true;
                                }
                                else
                                {
                                    //do nothing
                                }
                            }
                        }
                    }else if(power >= 0) {
                        Destroy(gameObject);
                        Collider[] colliders5 = Physics.OverlapSphere(explosionPos2, radius-1);
                        foreach (Collider hit in colliders5)
                        {
                            if (hit.tag != "PlayerProjectiles")
                            {
                                Rigidbody rb = hit.GetComponent<Rigidbody>();
                                power = 750;
                                if (rb != null) rb.AddExplosionForce(power, explosionPos2, radius, -0.5f);
                                NavMeshAgent agent = hit.GetComponent<NavMeshAgent>();
                                if (agent != null) agent.enabled = true;
                            }
                        }
                    }
                }
                break;

            case 3: //explosion(timer)
                if (tick >= lifetime && !hasKilled) Explode();
                if (hasKilled)
                {
                    if (tick > .01)
                    {
                        //Launch Enemies
                        //power = 1000;
                        Collider[] colliders2 = Physics.OverlapSphere(explosionPos2, radius);
                        foreach (Collider hit in colliders2)
                        {
                            if (hit.tag != "PlayerProjectiles")
                            {
                                Rigidbody rb = hit.GetComponent<Rigidbody>();
                                if (rb != null) rb.AddExplosionForce(power, explosionPos2, radius + 2f, 1f);
                            }
                        }
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor" && type!=3) {
            //gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            Explode();
        }
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;

        if (type == 4) Destroy(gameObject);
        else
        {
            isExploding = true;
            explosionPos2 = explosionPos;
            this.GetComponent<Collider>().enabled = false;
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.detectCollisions = false;
            this.GetComponent<Renderer>().enabled = false;
        }
        
        switch (type)
        {
            case 1: //stun and low damage(impact)
                tick = 0;
                explosionPos2 = explosionPos;
                Instantiate(stunEffect, transform.position, Quaternion.identity);
                isExploding = true;
                break;

            case 2: //black hole pull and medium damage(timer)
                //Succ Enemies
                Collider[] colliders3 = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders3)
                {
                    if (hit.tag != "PlayerProjectiles")
                    {
                        NavMeshAgent agent = hit.GetComponent<NavMeshAgent>();
                        if (agent != null) agent.enabled = false;
                        Rigidbody rb = hit.GetComponent<Rigidbody>();
                        if (rb != null) rb.AddExplosionForce(power, explosionPos, radius + 2f, 1f);
                        if (agent != null) agent.enabled = true;
                    }
                }
                //Quaternion angles = transform.rotation;
                Vector3 posPlusY = transform.position;
                posPlusY.y += bhelevation;
                Instantiate(blackhole, posPlusY, Quaternion.identity);
                explosionPos2 = explosionPos;
                tick = 0;
                break;

            case 3: //explosion(timer)
                //Kill Enemies
                hasKilled = true;
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
                explosionPos2 = explosionPos;
                tick = 0;
                Instantiate(explosion, transform.position, transform.rotation);
                break;
        }
        

        
        
    }
}
