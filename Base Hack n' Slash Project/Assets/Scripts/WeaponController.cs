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

    // Start is called before the first frame update
    void Start()
    {
        selectedWeapon = 1;
        weaponWheel.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
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
                break;
            case 2:
                //Fire weapon
                GameObject chainBullet = Instantiate(bullet, player.transform.position, Quaternion.identity);
                if (!chainBullet.GetComponent<ChainBullet>()) chainBullet.AddComponent<ChainBullet>();
                break;
            case 3:
                //Fire weapon
               
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
                //ammoText.text = DeagleController.ammoCount.ToString();
                break;
            case 2:
                //ammoText.text = ChainBoomerangController.ammoCount.ToString();
                break;
            case 3:
                ammoText.text = "BoomJuice = Sideways 8";
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
    }

    public void switchChainBoomerang()
    {
        print("ChainBoomerang");
        hideWeapon();
        selectedWeapon = 2;
        displayWeapon();
        player.GetComponent<punchScript>().enabled = false;
    }

    public void switchBoomGuantlets()
    {
        print("BoomGuantlets");
        hideWeapon();
        selectedWeapon = 3;
        displayWeapon();
        player.GetComponent<punchScript>().enabled = true;
    }

}
