using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDrop : MonoBehaviour
{
    public Material chainGun;
    public Material deagle;
    public Material grenade;

    public int upgradeType = 0;
    // Upgrade Types:
    // Chain Gun: 1
    // Deagle: 2
    // Grenades (?): 3
    private Renderer thisMat;
    private float despawnTimer = 7f;

    // Start is called before the first frame update
    void Start()
    {
        thisMat = GetComponent<Renderer>();
        float typeRandom = Random.Range(0, 1f);
        if (typeRandom <= .33f) upgradeType = 1;
        if (typeRandom > .33f && typeRandom <= .66f) upgradeType = 2;
        if (typeRandom > .66f) upgradeType = 3;
        switch(upgradeType)
        {
            case 1:
                thisMat.material = chainGun;
                break;
            case 2:
                thisMat.material = deagle;
                break;
            case 3:
                thisMat.material = grenade;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0) Destroy(gameObject);
            
    }
    
}
