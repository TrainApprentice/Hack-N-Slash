using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> jellies;
    //public List<GameObject> jelliesChained;
    public GameObject jelly;
    public Transform target;
    public float spawnDelay = 2f;
    

    // Start is called before the first frame update
    void Start()
    {
        
        target = PlayerManager.instance.player.transform;
        jellies = new List<GameObject>();
        jellies.Add(jelly);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < jellies.Count; i++)
        {
            if (jellies[i].GetComponent<FollowAI>().isDead)
            {
                //EnemySplit(jellies[i].GetComponent<FollowAI>().enemyLevel, jellies[i].GetComponent<FollowAI>().enemyType);
                //jellies.Remove(jellies[i]);
                //Destroy(jellies[i].gameObject);
                
            }
        }
        if (jellies.Count < 8)
        {
            spawnDelay -= Time.deltaTime;
            if (spawnDelay <= 0)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-15, 15), this.transform.position.y, Random.Range(-15, 15));
                var newJelly = Instantiate(jelly, spawnPos, Quaternion.identity);
                jellies.Add(newJelly);
                spawnDelay = 2f;
               
            }
        }
        //print(jellies.Count);
        KillAll();
    }
    private void EnemySplit(int level, int type)
    {
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
                if (level == 3 && type == 2) newJelly.name = "PoisonJelly2";
                if (level == 2 && type == 2) newJelly.name = "PoisonJelly1";
                if (level == 3 && type == 1) newJelly.name = "IceJelly2";
                if (level == 2 && type == 1) newJelly.name = "IceJelly1";

                jellies.Add(newJelly);
                newJelly.GetComponent<NavMeshAgent>().enabled = true;
                newJelly.GetComponent<PatrolAI>().enabled = true;

            }
        }

    }
    private void KillAll()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            print("KILL");
            for (int i = 0; i < jellies.Count; i++) jellies[i].GetComponent<FollowAI>().isDead = true;
        }
    }
    
}
