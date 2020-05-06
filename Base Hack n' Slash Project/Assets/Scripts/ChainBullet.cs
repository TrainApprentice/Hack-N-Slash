using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBullet : MonoBehaviour
{
    public int numHits = 3;

    public float movSpeed, maxChain;
    public GameObject target;
    public EnemyManager pullEnemies;
    public GameObject triggerSphere;

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
        //triggerSphere = GameObject.FindGameObjectWithTag("EditorOnly");
        pullEnemies = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
        for(int i = 0; i < pullEnemies.jellies.Count; i++)
        {
            possibleTargets.Add(pullEnemies.jellies[i].transform);
        }
        //print(possibleTargets.Count);
        //chainRadius = GetComponentInChildren<SphereCollider>();
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
        
        //distance = chainRadius.radius;
        //print(numHits);
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
        if (target) target.GetComponentInChildren<Renderer>().material.color = Color.black;
        

    }
    void Chain(Transform enemyStart)
    {
        isChaining = true;
        if (possibleTargets.Count > 0)
        {
            if (target != null)
            {
                if (possibleTargets.Contains(target.transform)) numHits--;
                possibleTargets.Remove(target.transform);
            }
            
            if (numHits <= 0)
            {
                EndChain();
            }
        }
        
        if (!foundTarget)
        {
            CheckChain(enemyStart);
            //print("Thing");
            
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "Enemy" && possibleTargets.Contains(other.transform))
        {
            print("Hit!");
            possibleTargets.Remove(other.gameObject.transform);
            other.gameObject.AddComponent<Hit>();
            foundTarget = false;
            Chain(other.transform);
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
        //print(distance);
        

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
