using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 30f;
    private Rigidbody bulletRB;
    private GameObject player;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //destroy bullet 1 second after spawning it
        Destroy(gameObject, 1f);
        player = GameObject.FindGameObjectWithTag("Player");

        //calculate direction for bullet
        float dx = player.transform.position.x - transform.position.x;
        float dz = player.transform.position.z - transform.position.z;
        direction = new Vector3(dx, 0f, dz);

        //give bullet velocity in aiming direction
        direction.Normalize();
        bulletRB = GetComponent<Rigidbody>();
        bulletRB.velocity = new Vector3(direction.x * speed, 0, direction.z * speed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player");
            if(!player.GetComponent<PlayerController>().invulnerable) player.GetComponent<PlayerController>().playerCurrentHealth -= 5;
            Destroy(gameObject);//immediately destroy bullet
        }
    }
}
