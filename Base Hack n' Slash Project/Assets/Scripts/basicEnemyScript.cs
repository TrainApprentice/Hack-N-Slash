using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemyScript : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 move = new Vector3();
    private float timer = 1f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per physics calc
    void FixedUpdate()
    {
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

        }
    }
}
