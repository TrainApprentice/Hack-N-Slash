using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private GameObject player;
    private Vector3 direction;
    private float shootTimer = 0f;
    private Rigidbody EnemyBulletRB;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //EnemyBulletRB = GameObject.FindGameObjectWithTag("EnemyBullet").GetComponent<Rigidbody>();
        EnemyBulletRB = Resources.Load<Rigidbody>("EnemyBulletSphere 1");
    }

    // FixedUpdate is called once per physics calculation
    void FixedUpdate()
    {
        if(shootTimer > 3f)
        {
            shootTimer = 0f;
            //Debug.Log("Enemy shot");
            //calculate direction for bullet
            float dx = player.transform.position.x - transform.position.x;
            float dz = player.transform.position.z - transform.position.z;
            direction = new Vector3(dx, 0f, dz);

            //shoot
            Vector3 enemyBulletSpawn = new Vector3((dx / Mathf.Abs(dx)) * .5f, 0f, (dz / Mathf.Abs(dz)) * .5f);
            Rigidbody instantiatedProjectile = Instantiate(EnemyBulletRB, transform.position + enemyBulletSpawn, Quaternion.identity) as Rigidbody;
        }

        shootTimer += Time.deltaTime;
    }
}
