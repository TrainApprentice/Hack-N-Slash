using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Dash (shift)
 * Movement
 * Power jetpack (space)
 * Roadhog hook (right-click)
 * 
 * 
 * https://www.youtube.com/watch?v=LNLVOjbrQj4
 * https://www.youtube.com/watch?v=THnivyG0Mvo
 * https://www.youtube.com/watch?v=8uD2ATIot0A
 * 
 * Dust particles still spawn in air
*/

public class PlayerController : MonoBehaviour
{
    CharacterController player;
    public Transform cam;
    private Vector3 offset = new Vector3(0f, 14f, -10f);
    private float speed = 10f;
    private float gravity = -20f;
    private float jumpImpulse = 20f;
    private Vector3 velocity = Vector3.zero;
    private float speedY = 0;
    private float v;
    private float h;
    private float iFrames = 1f;

    public float playerMaxHealth = 100;
    public float playerCurrentHealth;
    public HUDController healthBar;
    public HUDController dashCooldown;
    public HUDController hookCooldown;
    public Material poison;
    public Material ice;
    public Material baseMat;


    public GameObject spawnDashParticles;
    private float dashTimer;
    public bool invulnerable = false;
    private Renderer mat;

    //public GameObject walkParticles;

    public Rigidbody hook;
    public float hookSpeed = 20;
    private float hookTimer;

    private Vector3 intersection;
    private Vector3 diff;

    private bool isPoisoned = false;
    private bool isSlowed = false;
    private float slowTimer = 5f;
    private float poisonTimer = 3f;

    public int chainLevel = 1;
    public int deagleLevel = 1;
    public int chainAmmo = 10;
    public int deagleAmmo = 10;
    public int grenadeLevel = 1;
    public int grenadeAmount = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        mat = GetComponent<Renderer>();
        dashTimer = 4f;
        hookTimer = 6f;

        playerCurrentHealth = playerMaxHealth;
        healthBar.SetHealthBarMaxValue(playerMaxHealth);
        dashCooldown.SetDashCooldownMaxValue(3);
        hookCooldown.SetHookCooldownMaxValue(5); //placeholder number



    }

    // Update is called once per frame
    void Update()
    {
        //dash and/or jump once every three seconds, for .25 seconds
        dashTimer += Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButton("Jump")) && dashTimer > 3f) dashTimer = 0f;
        if (dashTimer < .25f)
        {
            GameObject trail = (GameObject)Instantiate(spawnDashParticles, transform.position, transform.rotation);
        }

        //hook once every five seconds
        hookTimer += Time.deltaTime;

        //update bars
        healthBar.SetHealthBarValue(playerCurrentHealth);
        dashCooldown.SetDashCooldownValue(dashTimer);
        hookCooldown.SetHookCooldownValue(hookTimer);

        if (playerCurrentHealth <= 0) SceneManager.LoadScene(2);

        //////////////////////////////////////////////////////DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.L))
        {
            chainLevel++;
            if (chainLevel > 2) chainLevel = 1;
            print(chainLevel);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            deagleLevel++;
            if (deagleLevel > 3) deagleLevel = 1;
            print(deagleLevel);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            chainAmmo += 10;
            deagleAmmo += 10;
        }

    }

    // FixedUpdate is called once per physics calculation
    void FixedUpdate()
    {
        //Status Effects

        if (isSlowed)
        {
            mat.material = ice;
            speed = 5f;
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                speed = 10f;
                isSlowed = false;
                slowTimer = 5f;
            }
        }

        if (isPoisoned)
        {
            mat.material = poison;
            poisonTimer -= Time.deltaTime;
            playerCurrentHealth -= Time.deltaTime * 6.666667f;
            if (poisonTimer <= 0)
            {
                isPoisoned = false;
                poisonTimer = 3f;
            }
        }
        if (!isPoisoned && !isSlowed) mat.material = baseMat;

        if (chainLevel > 2) chainLevel = 2;
        if (deagleLevel > 3) deagleLevel = 3;
        if (grenadeLevel > 3) grenadeLevel = 3;



        //Rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0;

        if (plane.Raycast(ray, out distance))
        {
            intersection = ray.GetPoint(distance);
            diff = intersection - transform.position;

            float angle = Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 90 - angle, 0);
        }

        //Move
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        velocity = new Vector3(h * speed, speedY, v * speed);

        //if player is on ground, they can jump and spawn particles while moving
        if (player.isGrounded)
        {
            if (Input.GetButton("Jump") && dashTimer < .25f) speedY = jumpImpulse;
            else speedY = 0;
        }

        //movement for dash is done by physics calculation, not frames
        if (dashTimer < .25f)
        {
            velocity.x *= 5;
            velocity.z *= 5;
            invulnerable = true;
        }
        else invulnerable = false;
        if (invulnerable && dashTimer >= .25f)
        {
            iFrames -= Time.deltaTime;
            if (iFrames <= 0)
            {
                invulnerable = false;
                iFrames = 1f;
            }
        }

        //hook
        if (Input.GetMouseButtonDown(1) && hookTimer > 5f)
        {
            hookTimer = 0f;
            Vector3 hookSpawn = new Vector3((diff.x / Mathf.Abs(diff.x)) * 1f, 0f, (diff.z / Mathf.Abs(diff.z)) * 1f);
            Rigidbody instantiatedProjectile = Instantiate(hook, transform.position + hookSpawn, Quaternion.identity) as Rigidbody;
            Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
        }

        speedY += (gravity * Time.deltaTime);
        player.Move(velocity * Time.deltaTime);
        cam.position = transform.position + offset;
    }



    void OnCollisionEnter(Collision other)//trigger to collision
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!invulnerable)
            {

                //Debug.Log("Not invulnerable");
                playerCurrentHealth -= other.gameObject.GetComponent<FollowAI>().damage;
                if (other.gameObject.GetComponent<FollowAI>().enemyType == 1) isSlowed = true;
                if (other.gameObject.GetComponent<FollowAI>().enemyType == 2) isPoisoned = true;
                //invulnerable = true;
                //player.Move(new Vector3(-velocity.x * Time.deltaTime * 15f, 0f, -velocity.z * Time.deltaTime * 15f));
            }
            // else Debug.Log("Invulnerable");
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle");
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            //if (other.gameObject.GetComponent<PowerupDrop>())
            //{
            print("Gottem");
            int temp = other.gameObject.GetComponent<PowerupDrop>().upgradeType;
            if (temp == 1) chainLevel++;
            if (temp == 2) deagleLevel++;
            if (temp == 3) grenadeLevel++;
            Destroy(other.gameObject);
            //}
        }
        else if (other.gameObject.CompareTag("Ammo"))
        {
            print("gOTTEM");
            //if (other.gameObject.GetComponent<AmmoDropScript>())
            //{
            int temp = other.gameObject.GetComponent<AmmoDropScript>().ammoType;
            if (temp == 1) chainAmmo += 5;
            if (temp == 2) deagleAmmo += 10;
            if (temp == 3) grenadeAmount += 5;
            Destroy(other.gameObject);
            //}
        }
    }

}
