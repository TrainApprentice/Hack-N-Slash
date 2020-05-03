using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 offset = new Vector3(0f, 7f, -5f);
    private float speed = 10f;
    private float gravity = -20f;
    private float jumpImpulse = 20f;
    private Vector3 velocity = Vector3.zero;
    private float speedY = 0;
    private float v;
    private float h;

    public int playerMaxHealth = 100;
    public int playerCurrentHealth;
    public HUDController healthBar;
    public HUDController dashCooldown;
    public HUDController hookCooldown;

    public GameObject spawnDashParticles;
    private float dashTimer;
    public bool invulnerable = false;

    //public GameObject walkParticles;

    public Rigidbody hook;
    public float hookSpeed = 20;
    private float hookTimer;

    private Vector3 intersection;
    private Vector3 diff;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
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
    }

    // FixedUpdate is called once per physics calculation
    void FixedUpdate()
    {
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
        }

        //movement for dash is done by physics calculation, not frames
        if (dashTimer < .25f)
        {
            velocity.x *= 5;
            velocity.z *= 5;
            invulnerable = true;
        }
        else invulnerable = false;

        //hook
        if (Input.GetMouseButtonDown(1) && hookTimer > 5f)
        {
            hookTimer = 0f;
            Vector3 hookSpawn = new Vector3((diff.x / Mathf.Abs(diff.x)) * .5f, 0f, (diff.z / Mathf.Abs(diff.z)) * .5f);
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
                Debug.Log("Not invulnerable");
                playerCurrentHealth -= 10;
                //player.Move(new Vector3(-velocity.x * Time.deltaTime * 15f, 0f, -velocity.z * Time.deltaTime * 15f));
            }
            else Debug.Log("Invulnerable");
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle");
        }
    }
}
