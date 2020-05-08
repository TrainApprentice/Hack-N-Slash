using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> jellies;
    public GameObject jelly;
    public Transform target;
    public float spawnDelay = 2f;
    public float splitRad = 3f;
    

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
    }
    public void RemoveEnemy(GameObject enemy)
    {
        jellies.Remove(enemy);
    }
    public void EnemySplit(int level, int type)
    {
        print("Split");
        if (level > 1)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 spawnPos = new Vector3(this.transform.position.x + Random.Range(-1 * splitRad, splitRad), this.transform.position.y, this.transform.position.z + Random.Range(-5, 5));
                float distance = Vector3.Distance(spawnPos, target.position);
                while (distance < 5f)
                {
                    spawnPos = new Vector3(this.transform.position.x + Random.Range(-1 * splitRad, splitRad), this.transform.position.y, this.transform.position.z + Random.Range(-5, 5));
                }
                var newJelly = Instantiate(jelly, spawnPos, Quaternion.identity);
                if (level == 3 && type == 2) newJelly.name = "PoisonJelly2";
                if (level == 2 && type == 2) newJelly.name = "PoisonJelly1";
                if (level == 3 && type == 1) newJelly.name = "IceJelly2";
                if (level == 2 && type == 1) newJelly.name = "IceJelly1";

                jellies.Add(newJelly);
                newJelly.GetComponent<NavMeshAgent>().enabled = true;
                newJelly.GetComponent<FollowAI>().enabled = true;

            }
        }

    }
    
}
