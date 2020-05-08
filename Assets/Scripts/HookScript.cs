using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    private float speed = 20f;
    private Vector3 hookIntersection;
    private Vector3 hookDiff;
    private Rigidbody hookRB;
    private float lifeTime = 0f;

    private Vector3 StartPos;
    private Vector3 EndPos;
    private float alpha = 2f; // percentage from start to end
    private GameObject enemy;
    private GameObject player;
    private GameObject[] hookParticleArray = new GameObject[20];
    private GameObject baseHookParticle;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        //Rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0;

        if (plane.Raycast(ray, out distance))
        {
            hookIntersection = ray.GetPoint(distance);
            hookDiff = hookIntersection - transform.position;
        }

        player = GameObject.FindGameObjectWithTag("Player");

        //give hook velocity
        Vector3 aim = hookDiff;
        aim.Normalize();
        hookRB = GetComponent<Rigidbody>();
        hookRB.velocity = new Vector3(aim.x * speed, 0, aim.z * speed);

        if (aim.z >= 0f)
        {
            angle = Vector3.Angle(hookRB.velocity, new Vector3(1f, 0f, 0f));
            transform.rotation = Quaternion.Euler(90f, 0f, angle + 115f);
        }
        else
        {
            angle = Vector3.Angle(hookRB.velocity, new Vector3(-1f, 0f, 0f));
            transform.rotation = Quaternion.Euler(90f, 0f, angle + 295f);
        }

        /* -20
         * (90f, 0f, -20f) south west
         * (90f, 0f, 25f) south
         * (90f, 0f, 70f) south east
         * (90f, 0f, 115f) east
         * (90f, 0f, 160f)
         * (90f, 0f, 205f) north
         * (90f, 0f, 250f)
         * (90f, 0f, 295f) west
         * (90f, 0f, 340f)
         */

        //make the hook particles
        baseHookParticle = GameObject.FindGameObjectWithTag("HookParticle");
        for (int i = 0; i < hookParticleArray.Length; ++i)
        {
            hookParticleArray[i] = (GameObject)Instantiate(baseHookParticle, transform);
        }

    }

    void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = player.transform.position;
        for (int i = 0; i < hookParticleArray.Length; ++i)
        {
            //lerp all the particles
            hookParticleArray[i].transform.position = Vector3.Lerp(end, start, ((float)i / (float)hookParticleArray.Length));
        }
    }

    void FixedUpdate()
    {
        /*
         * linearly interpolate enemy to player in a given time. LERP over time
         * cache enemy original position and alpha=0 so lerp doesn't override each update
         * alpha += dt
         * enemy position = lerp(transform.postition, destination transform.position, alpha) alpha 0 = start, alpha 1 = destination
         * at alpha = 1, stop and reset alpha, break out of lerp loop/update
         */
        if (alpha == 0f)
        {
            StartPos = enemy.transform.position;
            EndPos = player.transform.position;
            alpha += Time.deltaTime;
        }
        else if (alpha < .8f)
        {
            //enemy position
            EndPos = player.transform.position;
            enemy.transform.position = Vector3.Lerp(StartPos, EndPos, alpha);
            //hook position
            transform.position = Vector3.Lerp(StartPos, EndPos, alpha + (Time.deltaTime * 4f));
            alpha += Time.deltaTime;
        }
        else if (lifeTime > 1f)
        {
            //destroy hook and particles
            Debug.Log("Destroy");
            Destroy(gameObject);
            for (int i = hookParticleArray.Length - 1; i >= 0; --i)
            {
                Destroy(hookParticleArray[i]);
            }
        }
        lifeTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)//lerp
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            Physics.IgnoreCollision(GetComponent<Collider>(), other.gameObject.GetComponent<Collider>());
            transform.Translate(GetComponent<Rigidbody>().velocity * -.01f);
            alpha = 0f;
            enemy = other.gameObject;
        }
        else//if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle or player");
            Destroy(gameObject);
        }
    }
}
