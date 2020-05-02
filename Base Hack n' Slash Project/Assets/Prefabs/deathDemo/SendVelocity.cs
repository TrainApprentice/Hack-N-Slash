using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendVelocity : MonoBehaviour
{
    public bool hasSentVelocity = false;
    public Vector3 velocityToSend;
    private Component[] receivingScripts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSentVelocity && velocityToSend != null)
        {
            //do nothing
        }
        else if (velocityToSend != null)
        {
            if (!hasSentVelocity)
            {
                receivingScripts = gameObject.GetComponentsInChildren<ReceiveVelocity>();
                foreach (ReceiveVelocity rscript in receivingScripts)
                {
                    rscript.v = velocityToSend;
                }
                hasSentVelocity = true;
                print("sent");
            }
        }
    }
}
