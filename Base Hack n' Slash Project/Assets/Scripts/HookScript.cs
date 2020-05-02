using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    //public Rigidbody projectile;
    private float speed = 20f;
    private Vector3 hookIntersection;
    private Vector3 hookDiff;
    private Rigidbody hookRB;

    // Start is called before the first frame update
    void Start()
    {
        //destroy hook 1 second after spawning
        Destroy(gameObject, 1f);

        //Rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0;

        if (plane.Raycast(ray, out distance))
        {
            hookIntersection = ray.GetPoint(distance);
            hookDiff = hookIntersection - transform.position;
        }

        //give hook velocity
        Vector3 aim = hookDiff;
        aim.Normalize();
        hookRB = GetComponent<Rigidbody>();
        hookRB.velocity = new Vector3(aim.x * speed, 0, aim.z * speed);
    }

<<<<<<< HEAD
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            GetComponent<Rigidbody>().velocity *= -.5f;
            other.GetComponent<Rigidbody>().velocity = hookRB.velocity;
=======
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            Physics.IgnoreCollision(GetComponent<Collider>(), other.gameObject.GetComponent<Collider>());
            other.gameObject.GetComponent<basicEnemyScript>().move = (GetComponent<Rigidbody>().velocity * -2f);
            GetComponent<Rigidbody>().velocity *= -2.1f;
            transform.Translate(GetComponent<Rigidbody>().velocity * .01f);
            //other.rigidbody.velocity = hookRB.velocity;
        }
        else//if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle or player");
            Destroy(gameObject);
>>>>>>> Justin2
        }
    }
}
