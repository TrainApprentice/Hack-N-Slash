using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxScript : MonoBehaviour
{   
    [SerializeField] private Texture2D Combo1;
    [SerializeField] private Texture2D Combo2;
    private Material mat;



    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (punchScript.comboNumber == 1)
        {
            mat.mainTexture = Combo1;
        }
        if (punchScript.comboNumber == 2)
        {
            mat.mainTexture = Combo2;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle");

        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");


        }
    }
}
