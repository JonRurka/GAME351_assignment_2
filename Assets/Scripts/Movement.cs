using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float move_speed = 10f;
    public float turn_speed = 20f;
    public float ground_offset = 1.0f;
    public float forward_span = 5.0f;
    public bool canFireLasers = false;

    public GameObject laserPrefab;
    public Transform model;

    private float hover_offset_magnitude = 0;
    private float hover_offset = 0;
    private float turn_tilt = 0;
    private Rigidbody body;

    const float BASE_MAG = (1.0f / 3.0f);

    // Start is called before the first frame update
    void Start()
    {
        hover_offset_magnitude = BASE_MAG * Random.Range(0.5f, 1.5f);
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Hint: The global static variable "Terrain.activeTerrain" 
        // may be helpful or have useful methods for user here or in
        // other scripts.
        Terrain terrain = Terrain.activeTerrain;

        Vector3 localposition = model.localPosition;
        Vector3 position = body.transform.position;

        float F = terrain.SampleHeight(position + body.transform.forward * forward_span);
        float O = terrain.SampleHeight(position);
        Vector3 p1 = new Vector3(0, O, 0);
        Vector3 p2 = new Vector3(0, F, forward_span);
        Vector3 fnormal_local = (p2 - p1).normalized;
        Vector3 forward_normal = body.transform.TransformDirection(fnormal_local);

        float invert_rotation = 1;
        float moving_mod = 1;

        // translate by 'move_speed' on Z axis each frame for as long as
        // the space bar is held down
        if (Input.GetKey(KeyCode.W))
        {
            body.transform.Translate(fnormal_local * move_speed * Time.deltaTime);
            invert_rotation = 1;
            moving_mod = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            body.transform.Translate(-fnormal_local * move_speed * Time.deltaTime);
            invert_rotation = -1;
            moving_mod = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //turn_tilt * Time.deltaTime in third parameter of transform.Rotate()
            body.transform.Rotate(0, -move_speed * Time.deltaTime * invert_rotation, 0);
            turn_tilt--;
        }
 
        if (Input.GetKey(KeyCode.D))
        {
            //turn_tilt * Time.deltaTime in third parameter of transform.Rotate()
            body.transform.Rotate(0, move_speed * Time.deltaTime * invert_rotation, 0);
            turn_tilt++;
        }


        

        hover_offset = Mathf.Sin(Time.time * 2.5f) * hover_offset_magnitude;

        // Set hover_offset_magnitude to a random value when hover_offset is close to y = 0;
        if (Mathf.Abs(hover_offset) < 0.01f)
        {
            hover_offset_magnitude = BASE_MAG * Random.Range(0.5f, 1.5f) * moving_mod;
            //Debug.LogFormat("Set hover_offset_magnitude to {0}.", hover_offset_magnitude);
        }

        localposition.y = hover_offset;
        model.localPosition = localposition;

        //float R = terrain.SampleHeight(body.transform.position + body.transform.right * 1);
        //float L = terrain.SampleHeight(body.transform.position + -body.transform.right * 1);
        
        //Vector3 normal = new Vector3(-2 * (R - L), 4, 2 * (B - T)).normalized;
        //Vector3 forward
        //Debug.DrawRay(new Vector3(position.x, terrain.transform.position.y + O + 1f, position.z), forward_normal * 5, Color.red);
        //model.up = normal;
        model.forward = forward_normal;


        // ground clip protection.
        //body.transform.position = new Vector3(position.x, Mathf.Max(terrain.transform.position.y + O + 3, position.y, 0), position.z);
        //body.transform.position = new Vector3(position.x, Mathf.Min(terrain.transform.position.y + O + 20, position.y), position.z);

        // set the game object's translation (not an increment)
        //position.y = terrain.GetPosition().y + terrain.SampleHeight(position) + ground_offset + hover_offset;
        //transform.position = position;


        //Quaternion rotation = transform.rotation;

        // Slowly sets the game object's tilt position (the Z-axis) back to zero while it does not turn (That is the intention, at least!)
        if (turn_tilt <= 0)
        {
            turn_tilt++;
        }
        else if (turn_tilt >= 0)
        {
            turn_tilt--;
        }
        //rotation.z = turn_tilt;
        //print(turn_tilt);        

        if (canFireLasers)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 spawn_loc = model.position;
                spawn_loc.y -= 1;

                spawn_loc -= model.right;
                GameObject obj1 = Instantiate(laserPrefab, spawn_loc, Quaternion.identity);

                spawn_loc += 2 * model.right;
                GameObject obj2 = Instantiate(laserPrefab, spawn_loc, Quaternion.identity);


                obj1.transform.forward = model.forward;
                obj2.transform.forward = model.forward;
            }
        }

    }
}
