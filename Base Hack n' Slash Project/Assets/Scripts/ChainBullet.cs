using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBullet : MonoBehaviour
{
    public int numHits = 3;

    public float movSpeed, distance;
    public Transform target;
    public EnemyManager pullEnemies;

    private Vector3 bulletIntersection;
    private Vector3 bulletDiff;
    private Rigidbody bulletRB;
    private SphereCollider chainRadius;
    private bool foundTarget = false;
    private bool isChaining;
    private List<Transform> possibleTargets = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        pullEnemies = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
        for(int i = 0; i < pullEnemies.jellies.Count; i++)
        {
            possibleTargets.Add(pullEnemies.jellies[i].transform);
        }
        print(possibleTargets.Count);
        chainRadius = GetComponentInChildren<SphereCollider>();
        
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
        distance = chainRadius.radius;
        //print(numHits);
        if (isChaining)
        {
            if (target)
            {
                //if (possibleTargets.Contains(target)) possibleTargets.Remove(target);
                var dist = Vector3.Distance(transform.position, target.position);

                Vector3 trajectory = CalculateTrajectoryVelocity(transform.position, target.transform.position, .2f);
                bulletRB.velocity = new Vector3(trajectory.x, 0f, trajectory.z);

                //Rotate Bullet
                /*
                var lookPos = target.position - transform.position;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                */
                //Move Bullet
                //transform.Translate(Vector3.forward * movSpeed * Time.deltaTime);


                /*
                if (dist < distance)
                {
                    bulletRB.velocity = Vector3.zero;
                    movSpeed = 0f;
                    Chain();
                } */
                
            }
        }
    }
    void Chain(Transform enemyStart)
    {
        isChaining = true;
        if (possibleTargets.Count > 0)
        {
            if (possibleTargets.Contains(target)) numHits--;
            possibleTargets.Remove(target);
            
            if (numHits <= 0)
            {
                EndChain();
            }
        }
        Collider[] nextTarget = Physics.OverlapSphere(enemyStart.position, distance);
        if (!foundTarget)
        {
            foreach (Collider col in nextTarget)
            {
                if (col.gameObject.GetComponent<FollowAI>() != null)
                {
                    if (possibleTargets.Contains(col.gameObject.transform) && !foundTarget)
                    {
                        CheckChain();
                    }
                    else
                    {
                        target = col.gameObject.transform;
                        foundTarget = true;
                        
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && possibleTargets.Contains(other.transform))
        {
            print("Hit!");
            possibleTargets.Remove(other.transform);
            foundTarget = false;
            //target = other.transform;
            Chain(other.transform);
        }
       
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && !possibleTargets.Contains(other.transform) && target != other.transform)
        {
            possibleTargets.Add(other.transform);
        }
    } */
    void EndChain()
    {
        Destroy(gameObject);
    }
    void CheckChain()
    {
        print(distance);
        if (!foundTarget && distance <= 20f)
        {
            chainRadius.radius += .5f;
            distance = chainRadius.radius;
            movSpeed = 0f;
        }
        else
        {
            movSpeed = 50f;
            chainRadius.radius = .1f;
            distance = chainRadius.radius;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }

    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }
}
