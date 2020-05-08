using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public int selectedWeapon;
    // 1 = Deagle
    // 2 = Chain Boomerang
    // 3 = Boom Guantlets

    public GameObject weaponWheel;

    public Text ammoText;

    public GameObject bullet;
    public GameObject player;
    public EnemyManager evilManager;
    private PlayerController playerRef;



    // Start is called before the first frame update
    void Start()
    {
        selectedWeapon = 1;
        weaponWheel.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        playerRef = player.GetComponent<PlayerController>();
        evilManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        displayAmmo();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switchDeagle();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchChainBoomerang();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            switchBoomGuantlets();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0.1f;
            //activate Weapon wheel
            weaponWheel.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Time.timeScale = 1f;
            //deactivate Weapon Wheel
            weaponWheel.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            fireWeapon();
        }
        if(Input.GetMouseButton(0) && playerRef.deagleLevel >= 3)
        {
            fireWeapon();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponReload();
        }


    }

    void displayWeapon()
    {
        switch (selectedWeapon)
        {
            case 1:
                //Display Weapon
                break;
            case 2:
                //display weapon
                break;
            case 3:
                //display weapon
                break;
            default:
                print("Something went horribly wrong");
                break;
        }
    }

    void hideWeapon()
    {
        switch (selectedWeapon)
        {
            case 1:
                //Hide Weapon
                break;
            case 2:
                //Hide weapon
                break;
            case 3:
                //Hide weapon
                break;
            default:
                print("Something went horribly wrong");
                break;
        }
    }

    void fireWeapon()
    {
        switch (selectedWeapon)
        {
            case 1:
                //Fire Weapon
                if (playerRef.deagleAmmo > 0)
                {
                    GameObject baseBullet;
                    switch(playerRef.deagleLevel)
                    {
                        case 1:
                            baseBullet = Instantiate(bullet, player.transform.position, Quaternion.identity);
                            if (!baseBullet.GetComponent<DeagleShot>()) baseBullet.AddComponent<DeagleShot>();
                            playerRef.deagleAmmo--;
                            break;
                        case 2:
                            baseBullet = Instantiate(bullet, player.transform.position, Quaternion.identity);
                            if (!baseBullet.GetComponent<DeagleShot>()) baseBullet.AddComponent<DeagleShot>();
                            baseBullet.GetComponent<DeagleShot>().isPiercing = true;
                            playerRef.deagleAmmo--;
                            break;
                        case 3:
                            baseBullet = Instantiate(bullet, player.transform.position, Quaternion.identity);
                            if (!baseBullet.GetComponent<DeagleShot>()) baseBullet.AddComponent<DeagleShot>();
                            baseBullet.GetComponent<DeagleShot>().isPiercing = true;
                            playerRef.deagleAmmo--;
                            break;
                        default:
                            break;
                    }
                }
                break;
            case 2:
                //Fire weapon
                if (playerRef.chainAmmo > 0)
                {
                    if (playerRef.chainLevel == 1)
                    {
                        GameObject chainBullet = Instantiate(bullet, player.transform.position, Quaternion.identity);
                        if (!chainBullet.GetComponent<BoomerangShot>()) chainBullet.AddComponent<BoomerangShot>();
                        playerRef.chainAmmo--;
                    }
                    if (playerRef.chainLevel == 2)
                    {
                        GameObject chainBullet = Instantiate(bullet, player.transform.position, Quaternion.identity);
                        if (!chainBullet.GetComponent<ChainBullet>()) chainBullet.AddComponent<ChainBullet>();
                        chainBullet.GetComponent<ChainBullet>().chainBack = false;
                        playerRef.chainAmmo--;
                    }

                }
                break;
            case 3:
                //Boom Gauntlets punch in another script
                break;
            default:
                print("Something went horribly wrong");
                break;
        }
    }

    void displayAmmo()
    {
        switch (selectedWeapon)
        {
            case 1:
                ammoText.text = "Deagle: " + playerRef.deagleAmmo.ToString();
                break;
            case 2:
                ammoText.text = "Chain Gun: " + playerRef.chainAmmo.ToString();
                break;
            case 3:
                ammoText.text = "BoomJuice = Sideways 8";
                break;
            default:
                print("Something went horribly wrong");
                break;
        }
    }
    void weaponReload()
    {
        switch (selectedWeapon)
        {
            case 1:
                break;
            case 2:
                if (playerRef.chainLevel == 1) BoomerangBack();
                if (playerRef.chainLevel == 2) ChainBack();
                break;
            case 3:
                break;
            default:
                print("Something went horribly wrong");
                break;
        }
    }

    public void switchDeagle()
    {
        print("Deagle");
        hideWeapon();
        selectedWeapon = 1;
        displayWeapon();
        player.GetComponent<punchScript>().enabled = false;
        //GameObject.FindGameObjectWithTag("Wave").GetComponent<MeshCollider>().enabled = false;
    }

    public void switchChainBoomerang()
    {
        print("ChainBoomerang");
        hideWeapon();
        selectedWeapon = 2;
        displayWeapon();
        player.GetComponent<punchScript>().enabled = false;
        //GameObject.FindGameObjectWithTag("Wave").GetComponent<MeshCollider>().enabled = false;
    }

    public void switchBoomGuantlets()
    {
        print("BoomGuantlets");
        hideWeapon();
        selectedWeapon = 3;
        displayWeapon();
        player.GetComponent<punchScript>().enabled = true;
        //GameObject.FindGameObjectWithTag("Wave").GetComponent<MeshCollider>().enabled = true;
        
    }

    private void BoomerangBack()
    {
        foreach (GameObject thing in evilManager.jellies)
        {
            if (thing.GetComponent<BoomerangStuck>())
            {

                GameObject boomBullet = Instantiate(bullet, thing.transform.position, Quaternion.identity);
                if (!boomBullet.GetComponent<BoomerangShot>()) boomBullet.AddComponent<BoomerangShot>();
                boomBullet.GetComponent<BoomerangShot>().target = player.transform;
                boomBullet.GetComponent<BoomerangShot>().bounceBack = true;
                playerRef.chainAmmo++;
                Destroy(thing.GetComponent<BoomerangStuck>());
            }
        }
    }
    private void ChainBack()
    {
        foreach (GameObject thing in evilManager.jellies)
        {
            if (thing.GetComponent<LastChainEnemy>())
            {
                GameObject boomBullet = Instantiate(bullet, thing.transform.position, Quaternion.identity);
                if (!boomBullet.GetComponent<ChainBullet>()) boomBullet.AddComponent<ChainBullet>();
                boomBullet.GetComponent<ChainBullet>().chainBack = true;
                playerRef.chainAmmo++;
                Destroy(thing.GetComponent<LastChainEnemy>());
            }
        }
    }
}
