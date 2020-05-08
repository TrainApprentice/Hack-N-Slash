using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDropScript : MonoBehaviour
{
    public Material chainGun;
    public Material deagle;
    public Material grenade;

    public int ammoType = 0;
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
        if (typeRandom <= .2f) ammoType = 1;
        if (typeRandom > .2f && typeRandom <= .6f) ammoType = 2;
        if (typeRandom > .6f) ammoType = 3;
        switch (ammoType)
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
