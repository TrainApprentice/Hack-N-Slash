using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemyScript : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per physics calc
    void FixedUpdate()
    {
        rb.velocity *= .9f;
        if(Mathf.Abs(rb.velocity.x) < .1f)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
        }
        if (Mathf.Abs(rb.velocity.y) < .1f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
        if (Mathf.Abs(rb.velocity.z) < .1f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
        }
    }
}
