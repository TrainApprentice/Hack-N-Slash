using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDecal : MonoBehaviour
{
    public ParticleSystem partSys;
    public ParticleSystem.Particle[] part;
    float[] times;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject decal;

    // Start is called before the first frame update
    void Start()
    {
        partSys = GetComponent<ParticleSystem>();
        times = new float[partSys.main.maxParticles];
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnParticleCollision(GameObject other)
    {

        int numCollisionEvents = partSys.GetCollisionEvents(other, collisionEvents);
        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            else
            {
                Vector3 pos = collisionEvents[i].intersection;
                Quaternion rot = Quaternion.LookRotation(collisionEvents[i].velocity);
                rot.Set(0,rot.y,0,1);
                if (pos.y > 0) pos.y = 0;
                //pos.y -= 0.1f;
                Instantiate(decal, pos, rot);
                //print("decal placed");
            }
            i++;
        }
    }

    void checkParticleDeath()
    {
        partSys.GetParticles(part);
        for (int i = 0; i < part.Length; ++i)
        {
            if (times[i] < part[i].remainingLifetime && part[i].remainingLifetime > 0)
            {
                // Birth
                //StartCoroutine(Play(part[i].lifetime));
                print("dead?");
            }

            times[i] = part[i].remainingLifetime;

        }
    }

}
