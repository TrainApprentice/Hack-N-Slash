using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangShot : MonoBehaviour
{
    public float speed = 50f;
    public Vector3 bulletIntersection;
    private Vector3 bulletDiff;
    private Rigidbody bulletRB;
    public Transform target;
    public bool bounceBack = false;
    public int damage = 5;

    // Start is called before the first frame update
    void Start()
    {

        //destroy bullet 2 seconds after spawning it
        Destroy(gameObject, 2f);

        //Rotation
        Ray ray = new Ray();
        if (!bounceBack) ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (bounceBack) ray = Camera.main.ScreenPointToRay(GameObject.FindGameObjectWithTag("Player").transform.position);

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

        bulletRB.velocity = new Vector3(aim.x * speed, 0f, aim.z * speed);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!bounceBack)
            {
                Debug.Log("Hit obstacle");
                Destroy(gameObject);//immediately destroy bullet
            }
            
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<FollowAI>().Damage(damage, transform.position);
            if (!bounceBack)
            {
                Debug.Log("Hit enemy");
                //push back the enemy slightly and destroy the bullet and deal damage
                other.gameObject.GetComponent<Rigidbody>().velocity = (bulletRB.velocity * .25f);
                other.gameObject.AddComponent<BoomerangStuck>();
                Destroy(gameObject);
            }
            
           
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (bounceBack) Destroy(gameObject);
        }


    }
}
