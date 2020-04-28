using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private float speed = 30f;
    private Vector3 bulletIntersection;
    private Vector3 bulletDiff;
    private Rigidbody bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        //destroy bullet 1 second after spawning it
        Destroy(gameObject, 1f);

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
        bulletRB.velocity = new Vector3(aim.x * speed, 0, aim.z * speed);
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
            Debug.Log("Hit enemy");
            //push back the enemy slightly and destroy the bullet
            other.GetComponent<Rigidbody>().velocity = (bulletRB.velocity * .25f);
            Destroy(gameObject);
        }
    }
}
