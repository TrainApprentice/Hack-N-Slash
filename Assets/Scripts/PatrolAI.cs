using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour
{
    public NavMeshAgent agent;
    Transform target;
    List<Transform> moveSpots;
    List<GameObject> jellies;
    public GameObject jelly;
    private int randomSpot;
    private float waitTime;
    private float spawnDelay = 5f;
    public float startWaitTime;
    public float lookRadius = 4f;
    public int enemyLevel;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        if (name == "Jelly1") enemyLevel = 1;
        else if (name == "Jelly2") enemyLevel = 2;
        else if (name == "Jelly3") enemyLevel = 3;
        else
        {
            float levelRandom = Random.Range(0, 1);
            if (levelRandom <= .6f) enemyLevel = 1;
            if (levelRandom > .6f && levelRandom <= .9f) enemyLevel = 2;
            if (levelRandom > .9f) enemyLevel = 3;
        }
        
        
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        
        moveSpots = new List<Transform>();
        jellies = new List<GameObject>();
        jellies.Add(jelly);

        waitTime = startWaitTime;
        for (int i = 1; i < 5; i++)
        {
            GameObject movePoint = GameObject.Find("moveSpot" + i);
            moveSpots.Add(movePoint.transform);
        }
        
        randomSpot = Random.Range(0, moveSpots.ToArray().Length);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) Destroy(gameObject);
        if (jellies.Count <= 8)
        {
            spawnDelay -= Time.deltaTime;
            if (spawnDelay <= 0)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-15, 15), this.transform.position.y, Random.Range(-15, 15));
                var newJelly = Instantiate(jelly, spawnPos, Quaternion.identity);
                jellies.Add(newJelly);
                spawnDelay = 5f;
            }
        }
        if (Input.GetKeyDown("r"))
        {
            EnemySplit(enemyLevel);
        }
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            waitTime = startWaitTime;
        }
        else
        {
            if (waitTime <= 0)
            {
                agent.SetDestination(moveSpots[randomSpot].transform.position);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 3f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.ToArray().Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    
   
    private void EnemySplit(int level)
    {
        isDead = true;
        if (level > 1)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 spawnPos = new Vector3(this.transform.position.x + Random.Range(-5, 5), this.transform.position.y, this.transform.position.z + Random.Range(-5, 5));
                float distance = Vector3.Distance(spawnPos, target.position);
                while (distance < 5f)
                {
                    spawnPos = new Vector3(this.transform.position.x + Random.Range(-5, 5), this.transform.position.y, this.transform.position.z + Random.Range(-5, 5));
                }
                var newJelly = Instantiate(jelly, spawnPos, Quaternion.identity);
                if (level == 3) newJelly.name = "Jelly2";
                if (level == 2) newJelly.name = "Jelly1";

                jellies.Add(newJelly);
                newJelly.GetComponent<NavMeshAgent>().enabled = true;
                newJelly.GetComponent<PatrolAI>().enabled = true;
                
            }
        }
            
    }
    
}
