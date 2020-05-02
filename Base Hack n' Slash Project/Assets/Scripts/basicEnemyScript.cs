using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemyScript : MonoBehaviour
{
    private Rigidbody rb;
<<<<<<< HEAD
=======
    public Vector3 move = new Vector3();
    private float timer = 1f;
>>>>>>> Justin2

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per physics calc
    void FixedUpdate()
    {
<<<<<<< HEAD
        rb.velocity *= .9f;
        if(Mathf.Abs(rb.velocity.x) < .1f)
=======
        //if(this enemy's velocity is greater than it's maxVelocity i.e. got hooked)
        /*rb.velocity *= .95f;
        if(Mathf.Abs(rb.velocity.x) < .1f)//.1f is for complete stop. replace with maxVelocity
>>>>>>> Justin2
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
<<<<<<< HEAD
=======
        }*/

        //enemy got hooked. Move enemy for half of a second in direction hook came from
        rb.velocity = Vector3.zero;
        if(timer > .3f && move != Vector3.zero)
        {
            timer = 0f;
            transform.Translate(move * Time.deltaTime);
            Debug.Log(move);
        }
        else if(move != Vector3.zero)
        {
            transform.Translate(move * Time.deltaTime);
        }

        timer += Time.deltaTime;
        if(timer > .3f)
        {
            move = Vector3.zero;
>>>>>>> Justin2
        }
    }
}
