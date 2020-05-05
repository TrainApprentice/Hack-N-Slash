using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkParticleScript : MonoBehaviour
{
    private GameObject player;
    private GameObject particles;
    private float v, h;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particles = GameObject.FindGameObjectWithTag("WalkParticle");
    }

    // Update is called once per physics calculation
    void FixedUpdate()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        if ((v != 0 || h != 0) && player.GetComponent<CharacterController>().isGrounded)
        {
            var emission = particles.GetComponent<ParticleSystem>().emission;
            emission.rateOverDistance = 3;

            float a = Mathf.Pow(v, 2f);
            float b = Mathf.Pow(h, 2f);
            float c = Mathf.Pow(a + b, .5f);
            float sin = Mathf.Asin(h / c) * Mathf.Rad2Deg;
            if (v < 0) transform.rotation = Quaternion.Euler(0f, -sin, 0f);
            else transform.rotation = Quaternion.Euler(0f, sin + 180f, 0f);
        }
        else
        {
            var emission = particles.GetComponent<ParticleSystem>().emission;
            emission.rateOverDistance = 0;
        }
    }
}
