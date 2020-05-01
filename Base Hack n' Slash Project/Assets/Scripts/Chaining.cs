using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaining : MonoBehaviour
{
    //public GameObject bullet;
    public GameObject bullet;
    bulletScript thing;
    private SphereCollider nearestEnemy;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        bullet = GameObject.FindGameObjectWithTag("Bullet");
        speed = bullet.GetComponent<bulletScript>().speed;
        thing = GetComponentInParent<bulletScript>();
        nearestEnemy = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nearestEnemy.radius <= 20f)
        {
            nearestEnemy.radius += .5f;
        }
        else Destroy(gameObject);

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy") && !other.gameObject.GetComponent<FollowAI>().hasChained)
        {
            
            print("To chain");
            float dx = other.gameObject.transform.position.x - transform.position.x;
            float dz = other.gameObject.transform.position.z - transform.position.z;
            Vector3 direction = new Vector3(dx * speed, 0f, dz * speed);

            bullet.GetComponent<Rigidbody>().velocity = direction;
            Destroy(gameObject);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nearestEnemy.radius);
    }
}
