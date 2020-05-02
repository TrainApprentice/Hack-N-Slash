using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    private Camera cam;
    public GameObject ret;
    public GameObject retVert;
    public Vector3 retPoint;

    public Rigidbody grenade;
    public LayerMask layer;
    public float flightTime = 0.5f;

    public LineRenderer lineVisual;
    public int lineSegment = 10;

    public Vector3 originOffset = new Vector3(0,1,0);
    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        lineVisual.positionCount = lineSegment;

        Physics.IgnoreCollision(grenade.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;
        origin += originOffset;
        handleInput();
    }

    void handleInput()
    {
        if (Input.GetKey("q"))
        {
            print("q key is down");
            placeReticle();
            VisualizeLine(getInitialVelocity());
        }
        if (Input.GetKeyUp("q"))
        {
            print("q key came up");
            throwGrenade(getInitialVelocity());
            ret.SetActive(false);
            retVert.SetActive(false);
            lineVisual.enabled = false;
        }
    }

    void placeReticle()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            //GameObject objectHit = hit.transform.gameObject;

            // Do something with the object that was hit by the raycast.
            if (hit.transform.tag == "Floor")
            {
                retVert.SetActive(false);
                ret.SetActive(true);
                ret.transform.position = hit.point;
            }
            else if (hit.transform.tag == "Wall")
            {
                ret.SetActive(false);
                retVert.SetActive(true);
                retVert.transform.position = hit.point;
                //print(hit.normal);
                retVert.transform.rotation = Quaternion.Euler(90, 90 * hit.normal.x, 0); //<--- This here doesn't work

            }
            retPoint = hit.point;
            
        }
    }

    void throwGrenade(Vector3 velInit)
    {
        Rigidbody rb = Instantiate(grenade, origin, Quaternion.identity);
        rb.velocity = velInit;
    }

    void VisualizeLine(Vector3 Vo)
    {
        if (!lineVisual.enabled) lineVisual.enabled = true;
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalcPosInTime(Vo, i/(float)lineSegment);
            lineVisual.SetPosition(i, pos);
        }
    }

    Vector3 CalcVel(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 dist = target - origin;
        Vector3 distXZ = dist;
        distXZ.y = 0f;

        //create a float that represents distance
        float Sy = dist.y;
        float Sxz = distXZ.magnitude;

        //calculate velocity
        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        //create and return result
        Vector3 result = distXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;

    }

    Vector3 CalcPosInTime(Vector3 Vo, float time)
    {
        Vector3 Vxz = Vo;
        Vxz.y = 0f;

        Vector3 result = origin + Vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (Vo.y * time) + origin.y;

        result.y = sY;
        return result;
    }

    Vector3 getInitialVelocity()
    {
        Vector3 Vo = CalcVel(retPoint, origin, flightTime);
        return Vo;
    }
}
