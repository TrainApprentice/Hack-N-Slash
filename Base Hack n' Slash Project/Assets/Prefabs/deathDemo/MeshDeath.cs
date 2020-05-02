using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeath : MonoBehaviour
{
    public GameObject destroyedVersion;
    public GameObject blood;
    private Vector3 bloodpos;
    public bool isDead = false;
    Rigidbody deadRB;

    public void Update()
    {
        if (isDead)
        {
            
            
        }
    }

    public void die()
    {
        Destroy(gameObject);
        bloodpos = transform.position;
        //bloodpos.y += 1;
        Instantiate(blood, bloodpos, transform.rotation);
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        //SendVelocity sendVelScript = destroyedVersion.GetComponent<SendVelocity>();
        //Vector3 targetVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        //sendVelScript.velocityToSend = targetVelocity;
        //blood.GetComponent<Rigidbody>().velocity += targetVelocity * 100;
    }

    private void OnMouseDown()
    {
        isDead = true;
    }
    private void killEnemy()
    {
        if (isDead)
        {
            
        }
    }
}
