using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBullet : MonoBehaviour
{
    public int numHits = 3;

    public float movSpeed, maxChain;
    public GameObject target;
    //public EnemyManager pullEnemies;
    //public GameObject triggerSphere;


    private Vector3 bulletIntersection;
    private Vector3 bulletDiff;
    private Rigidbody bulletRB;
    //private SphereCollider chainRadius;
    private bool foundTarget = false;
    private bool isChaining;
    public bool chainBack = false;
    private int damage = 5;
    //private List<Transform> possibleTargets = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {

        maxChain = 20f;
        movSpeed = 50f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0;

        if (plane.Raycast(ray, out distance))
        {
            bulletIntersection = ray.GetPoint(distance);
            bulletDiff = bulletIntersection - transform.position;
        }

        //give bullet velocity in aiming direction
        Vector3 aim = bulletDiff;
        aim.Normalize();
        bulletRB = GetComponent<Rigidbody>();
        bulletRB.velocity = new Vector3(aim.x * movSpeed, 0, aim.z * movSpeed);
        numHits = 3;
        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isChaining)
        {
            if (target)
            {
                var dist = Vector3.Distance(transform.position, target.transform.position);

                Vector3 trajectory = CalculateTrajectoryVelocity(transform.position, target.transform.position, .2f);
                bulletRB.velocity = new Vector3(trajectory.x, 0f, trajectory.z);
                isChaining = false;

            }
        }
        //if (target) target.GetComponentInChildren<Renderer>().material.color = Color.black;
    }
    void Chain(Transform enemyStart)
    {
        isChaining = true;


        if (!foundTarget)
        {
            CheckChain(enemyStart);

        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("Hit!");

            if (!chainBack && !other.gameObject.GetComponent<Hit>())
            {
                other.gameObject.GetComponent<FollowAI>().health -= damage;
                other.gameObject.AddComponent<Hit>();
            }
            if (chainBack && other.gameObject.GetComponent<Hit>())
            {
                other.gameObject.GetComponent<FollowAI>().health -= damage;
                Destroy(other.gameObject.GetComponent<Hit>());
            }
            foundTarget = false;
            Chain(other.transform);
            if (numHits <= 0)
            {
                print("Dying");
                if (!chainBack) other.gameObject.AddComponent<LastChainEnemy>();
                if (chainBack) target = GameObject.FindGameObjectWithTag("Player");
                EndChain();
            }
        }
    }

    void EndChain()
    {
        Destroy(gameObject);
    }
    void CheckChain(Transform enemyStart)
    {
        Collider[] nextTarget = Physics.OverlapSphere(enemyStart.position, maxChain);
        if (nextTarget == null) EndChain();
        foreach (Collider col in nextTarget)
        {
            if (!chainBack)
            {
                if (!col.gameObject.GetComponent<Hit>() && col.gameObject.tag == "Enemy")
                {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    float distanceToClosestEnemy = 1000;
                    if (dist < distanceToClosestEnemy)
                    {
                        distanceToClosestEnemy = dist;
                        target = col.gameObject;
                    }
                }
            }
            if (chainBack)
            {
                if (col.gameObject.GetComponent<Hit>() && col.gameObject.tag == "Enemy")
                {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    float distanceToClosestEnemy = 1000;
                    if (dist < distanceToClosestEnemy)
                    {
                        distanceToClosestEnemy = dist;
                        target = col.gameObject;
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxChain);
    }

    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }
}
