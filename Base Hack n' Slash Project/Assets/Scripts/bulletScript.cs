using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
<<<<<<< HEAD
    public float speed = 30f;
    public Vector3 bulletIntersection;
    private Vector3 bulletDiff;
    private Rigidbody bulletRB;
    private int bulletType = 7;
    public GameObject manager;
    List<GameObject> possibleTargets;
    public GameObject target;
    private int numHit = 3;
    //private GameObject trigSphere;
    private SphereCollider closestEnemy;
=======
    private float speed = 30f;
    private Vector3 bulletIntersection;
    private Vector3 bulletDiff;
    private Rigidbody bulletRB;
>>>>>>> Justin2

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        manager = GameObject.FindGameObjectWithTag("GameController");
        closestEnemy = GetComponentInChildren<SphereCollider>();
        for (int i = 0; i < manager.GetComponent<EnemyManager>().jellies.Count; i++)
        {
            possibleTargets.Add(manager.GetComponent<EnemyManager>().jellies[i]);
        }
        
        //if (manager.GetComponent<EnemyManager>().jellies != null) manager.GetComponent<EnemyManager>().jellies.CopyTo(enemies);
        //destroy bullet 1 second after spawning it
        Destroy(gameObject, 2f);
=======
        //destroy bullet 1 second after spawning it
        Destroy(gameObject, 1f);
>>>>>>> Justin2

        //Rotation
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
<<<<<<< HEAD
        bulletRB.velocity = new Vector3(aim.x * speed, 0f, aim.z * speed);
    }
    private void Update()
    {
        for (int i = 0; i < manager.GetComponent<EnemyManager>().jellies.Count; i++)
        {
            if (!possibleTargets.Contains(manager.GetComponent<EnemyManager>().jellies[i])) possibleTargets.Add(manager.GetComponent<EnemyManager>().jellies[i]);
        }
=======
        bulletRB.velocity = new Vector3(aim.x * speed, 0, aim.z * speed);
>>>>>>> Justin2
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle");
            Destroy(gameObject);//immediately destroy bullet
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
<<<<<<< HEAD
            //Debug.Log("Hit enemy");
            //if (!enemiesHit.Contains(other.gameObject)) enemiesHit.Add(other.gameObject);
            if (bulletType == 7)
            {
                
                ChainBullet(other);
                
            }
            //push back the enemy slightly and destroy the bullet
            other.GetComponent<Rigidbody>().velocity = (bulletRB.velocity * .25f);
            //Destroy(gameObject);
        }
        
    }
    private void ChainBullet(Collider target)
    {
        if (possibleTargets.Count > 0 && numHit > 0)
        {
            possibleTargets.Remove(target.gameObject);
            Collider[] thing = Physics.OverlapSphere(transform.position, closestEnemy.radius);
            foreach (Collider col in thing)
            {
                if (!col.gameObject.GetComponent<FollowAI>().hasChained && !possibleTargets.Contains(col.gameObject) && col != null)
                {
                    //Do thing
                    float dx = col.gameObject.transform.position.x - transform.position.x;
                    float dz = col.gameObject.transform.position.z - transform.position.z;
                    Vector3 direction = new Vector3(dx * speed, 0f, dz * speed);

                    bulletRB.velocity = direction;
                    numHit--;
                }
                else closestEnemy.radius += .5f;
            }

        }
        //print(target.gameObject.GetComponent<FollowAI>().hasChained   
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closestEnemy.radius);
=======
            Debug.Log("Hit enemy");
            //push back the enemy slightly and destroy the bullet
            other.GetComponent<Rigidbody>().velocity = (bulletRB.velocity * .25f);
            Destroy(gameObject);
        }
>>>>>>> Justin2
    }
}
