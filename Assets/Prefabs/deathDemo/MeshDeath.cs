using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeath : MonoBehaviour
{
    public GameObject destroyedVersion1;
    public GameObject destroyedVersion2;
    public GameObject destroyedVersion3;
    public GameObject blood;
    private Vector3 bloodpos;
    public bool isDead = false;
    private Material mat;
    public Material bloodMat;
    Rigidbody deadRB;
    public EnemyManager evilManager;
    public FollowAI enemyScript;

    public GameObject ammoDrop;
    public GameObject upgradeDrop;
    private float dropChance = .2f;

    public void Start()
    {
        evilManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
        enemyScript = gameObject.GetComponent<FollowAI>();
    }

    public void Update()
    {
        if (isDead)
        {
            
            
        }
    }

    public void die()
    {
        CheckDrops();
        //evilManager.EnemySplit(enemyScript.enemyLevel, enemyScript.enemyType);
        evilManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
        bloodpos = transform.position;
        //bloodpos.y += 1;
        Instantiate(blood, bloodpos, Quaternion.identity);
        
        FollowAI getScript = gameObject.GetComponent<FollowAI>();

        switch (getScript.enemyType)
        {
            case 1:
                mat = getScript.iceMat;
                break;
            case 2:
                mat = getScript.poisonMat;
                break;
        }




        mat = bloodMat;   //Mesh death color correction




        switch (getScript.enemyLevel)
        {
            case 1:
                Instantiate(destroyedVersion1, transform.position, transform.rotation);
                Renderer[] renderers = destroyedVersion1.GetComponentsInChildren<Renderer>();
                foreach (Renderer render in renderers)
                {
                    render.material = mat;
                }
                break;
            case 2:
                Instantiate(destroyedVersion2, transform.position, transform.rotation);
                Renderer[] renderers2 = destroyedVersion2.GetComponentsInChildren<Renderer>();
                foreach (Renderer render in renderers2)
                {
                    render.material = mat;
                }
                break;
            case 3:
                Instantiate(destroyedVersion3, transform.position, transform.rotation);
                Renderer[] renderers3 = destroyedVersion3.GetComponentsInChildren<Renderer>();
                foreach (Renderer render in renderers3)
                {
                    render.material = mat;
                }
                break;
        }

        
    }

    //private void OnMouseDown()
    //{
    //    isDead = true;
    //}
    private void killEnemy()
    {
        if (isDead)
        {
            
        }
    }
    private void CheckDrops()
    {
        float randChance = Random.Range(0, 1f);
        if (randChance < dropChance)
        {
            GameObject drop;
            float typeChance = Random.Range(0, 1f);
            if (typeChance <= .65f) drop = Instantiate(ammoDrop, transform.position, Quaternion.identity);
            if (typeChance > .65f) Instantiate(upgradeDrop, transform.position, Quaternion.identity);
        }
    }
}
