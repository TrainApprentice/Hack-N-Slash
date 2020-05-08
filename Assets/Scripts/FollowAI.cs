using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowAI : MonoBehaviour
{
    public NavMeshAgent agent;
    Transform target;

    public Material iceMat;
    public Material poisonMat;
    public Material enemyMat;
    private Renderer thisMat;

    public GameObject jelly;
    public GameObject upgradeDrop;
    public GameObject ammoDrop;

    public int enemyLevel;
    public int enemyType = 0;
    public int damage;

    private float speed;
    private Vector3 size = new Vector3();
    private float dropChance = .9f;
    public int health;

    //private Renderer mat;
    public bool isDead = false;
    public bool hasChained = false;
    public MeshDeath killer;
    private EnemyManager enemyManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        thisMat = GetComponentInChildren<Renderer>();
        if (name == "IceJelly1")
        {
            enemyLevel = 1;
            enemyType = 1;
        }
        else if (name == "IceJelly2")
        {
            enemyLevel = 2;
            enemyType = 1;
        }
        else if (name == "IceJelly3")
        {
            enemyLevel = 3;
            enemyType = 1;
        }
        else if (name == "PoisonJelly1")
        {
            enemyLevel = 1;
            enemyType = 2;
        }
        else if (name == "PoisonJelly2")
        {
            enemyLevel = 2;
            enemyType = 2;
        }
        else if (name == "PoisonJelly3")
        {
            enemyLevel = 3;
            enemyType = 2;
        }
        else
        {
            float levelRandom = Random.Range(0, 1f);
            if (levelRandom <= .6f) enemyLevel = 1;
            if (levelRandom > .6f && levelRandom <= .9f) enemyLevel = 2;
            if (levelRandom > .9f) enemyLevel = 3;

            float typeRandom = Random.Range(0, 1f);
            if (typeRandom <= .24f) enemyType = 1;//<=.49
            else if (typeRandom <= .49f) enemyType = 2;//>.49
            else if (typeRandom <= .74f) enemyType = 3;
            else if (typeRandom > .74f) enemyType = 4;
        }


        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        killer = GetComponent<MeshDeath>();
        enemyManagerScript = gameObject.GetComponent<EnemyManager>();


        switch (enemyLevel)
        {
            case 1:
                speed = 10;
                damage = 10;
                health = 20;
                size = new Vector3(1f, 1f, 1f);
                transform.localScale = size;
                break;

            case 2:
                speed = 7;
                damage = 15;
                health = 40;
                size = new Vector3(2f, 2f, 2f);
                transform.localScale = size;
                break;

            case 3:
                speed = 3;
                damage = 25;
                health = 60;
                size = new Vector3(3f, 3f, 3f);
                transform.localScale = size;
                break;

            default:
                speed = 10;
                damage = 10;
                health = 20;
                size = new Vector3(1f, 1f, 1f);
                transform.localScale = size;
                break;
        }
        switch (enemyType)
        {
            case 1:
                thisMat.material = iceMat;
                break;
            case 2:
                thisMat.material = poisonMat;
                break;
            case 3:
                thisMat.material = enemyMat;
                gameObject.AddComponent<EnemyShoot>();
                break;
            default:
                break;

        }
        agent.speed = speed;


    }

    // Update is called once per frame
    void Update()
    {
        //if (isDead) Destroy(gameObject);
        
        if (isDead)
        {
            float randChance = Random.Range(0, 1f);
            if (randChance <= dropChance)
            {
                float typeChance = Random.Range(0, 1f);
                GameObject drop;
                if (typeChance <= .7f) drop = Instantiate(ammoDrop, transform.position, Quaternion.identity);
                if (typeChance > .7f) drop = Instantiate(upgradeDrop, transform.position, Quaternion.identity);

            }
            enemyManagerScript.EnemySplit(enemyLevel, enemyType);
        }
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance >= 10f) agent.SetDestination(target.position);
        FacePlayer();
        if (GetComponent<Hit>()) hasChained = true;
        

    }
    private void FacePlayer()
    {
        if (target != null)
        {
            Vector3 lookPoint = new Vector3(target.position.x, 0f, target.position.z);
            transform.LookAt(lookPoint);
        }
    }

    public void Damage(int dmg, Vector3 bulletPos)
    {
        health -= dmg;
        if (health <= 0)
        {
            //killer.die();
            killer.isDead = true;
            killer.die();
            isDead = true;

            Collider[] colliders = Physics.OverlapSphere(bulletPos, 2);
            foreach (Collider hit in colliders)
            {
                //bulletPos.y = 0f;
                if (hit.tag != "PlayerProjectiles")
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null && tag=="Dead") rb.AddExplosionForce(1000, bulletPos, 1, 1f);
                    NavMeshAgent agent = hit.GetComponent<NavMeshAgent>();
                    if (agent != null) agent.enabled = true;
                }
            }

            //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            //rb.AddExplosionForce(1000, bulletPos, 1, 1f);
        }
    }
}
