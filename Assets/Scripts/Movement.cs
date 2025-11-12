using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float move_speed = 10f;
    public float turn_speed = 20f;
    public float ground_offset = 1.0f;

    private float hover_offset_magnitude = 0;
    private float hover_offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        hover_offset_magnitude = Random.Range(-0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Hint: The global static variable "Terrain.activeTerrain" 
        // may be helpful or have useful methods for user here or in
        // other scripts.
        Terrain terrain = Terrain.activeTerrain;

        float invert_rotation = 1;

        // translate by 'move_speed' on Z axis each frame for as long as
        // the space bar is held down
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, move_speed * Time.deltaTime);
            invert_rotation = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -move_speed * Time.deltaTime);
            invert_rotation = -1;
        }
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -move_speed * Time.deltaTime * invert_rotation, 0);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, move_speed * Time.deltaTime * invert_rotation, 0);


        Vector3 position = transform.position;

        float base_mag = (1.0f / 3.0f) + hover_offset_magnitude;
        hover_offset = Mathf.Sin(Time.time * 2.5f) * base_mag;

        // Set hover_offset_magnitude to a random value when hover_offset is close to y = 0;
        if (Mathf.Abs(hover_offset) < 0.01f)
        {
            hover_offset_magnitude = Random.Range(-0.2f, 0.2f);
            //Debug.LogFormat("Set hover_offset_magnitude to {0}.", hover_offset_magnitude);
        }

        // set the game object's translation (not an increment)
        position.y = terrain.GetPosition().y + terrain.SampleHeight(position) + ground_offset + hover_offset;
        transform.position = position;

        

    }
}
