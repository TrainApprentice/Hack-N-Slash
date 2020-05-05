using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchScript : MonoBehaviour
{
    public GameObject wave;
    int waveFrames = 0;
    public static int comboNumber = 0;
    public bool isPunching = false;
    public bool isCountingDown = false;
    public float comboTimer = 300;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0))
        {
            
            if (waveFrames == 0)
            {
                waveFrames = 30;
                isPunching = true;
                isCountingDown = true;
                print(comboNumber);
                if (comboTimer > 0)
                {
                    if (comboNumber == 1)
                    {
                        comboNumber = 2;
                        comboTimer = 300;
                        print("Should be Combo 2");
                    }
                }
                if (comboNumber == 0)
                {
                    comboNumber = 1;
                    comboTimer = 300;
                    isCountingDown = true;
                    print("Should be Combo 1");
                }

            }
        }
        if (isCountingDown)
        {
            comboTimer--;
            //print (comboTimer);
        }
        if(comboTimer <= 0)
        {
            comboTimer = 0;
            isCountingDown = false;
            comboNumber = 0;
        }
        if (isPunching)
        {
            wave.SetActive (true);
            waveFrames--;
            //print(waveFrames);
        }
        if (waveFrames <= 0)
        {
            waveFrames = 0;
            wave.SetActive (false);
            isPunching = false;
        }

    
    }
}
