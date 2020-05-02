using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDelay = 5;
    public float fadeSpeed = .0001f;
    private float counter = 0;
    private Color col;
    // Start is called before the first frame update
    void Awake()
    {
        col = GetComponent<MeshRenderer>().material.color;
        col.a = 1;
        GetComponent<MeshRenderer>().material.color = col;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= fadeDelay) 
        { 
            col.a -= (Time.deltaTime * fadeSpeed) / 100;
            if (col.a <= 0) Destroy(gameObject);
            //print(col.a);
            GetComponent<MeshRenderer>().material.color = col;
        }
    }
}
