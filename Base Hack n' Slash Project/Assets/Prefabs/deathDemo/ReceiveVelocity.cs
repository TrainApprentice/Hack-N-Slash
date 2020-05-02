using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveVelocity : MonoBehaviour
{
    public Vector3 v;
    bool hasAppliedVelocity = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(hasAppliedVelocity && v != null)
        {
            //do nothing
        }
        else if (v != null)
        {
            if (!hasAppliedVelocity)
            {
                v = new Vector3(v.x * Random.Range(1.5f, 2.25f), v.y * Random.Range(.075f, .25f), v.z * Random.Range(1.5f, 2.25f));
                gameObject.GetComponent<Rigidbody>().velocity = v;
                hasAppliedVelocity = true;
                print("received v of " + v);
            }
        }
    }
}
