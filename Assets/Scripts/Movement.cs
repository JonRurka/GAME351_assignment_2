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
    private float turn_tilt = 0;

    const float BASE_MAG = (1.0f / 3.0f);

    // NEW: Controls how smooth the tilt/rotation is
    public float tilt_smooth = 8f;

    // NEW: Raycast distance for terrain detection
    public float raycast_distance = 5f;

    // NEW: Two points used to detect terrain slope
    public Transform frontPoint;
    public Transform backPoint;

    // Start is called before the first frame update
    void Start()
    {
        hover_offset_magnitude = BASE_MAG * Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
void Update()
    {
        Terrain terrain = Terrain.activeTerrain;

        float invert_rotation = 1;
        float moving_mod = 1;

        // Movement controls (unchanged)
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, move_speed * Time.deltaTime);
            invert_rotation = 1;
            moving_mod = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -move_speed * Time.deltaTime);
            invert_rotation = -1;
            moving_mod = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -move_speed * Time.deltaTime * invert_rotation, 0);
            turn_tilt--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, move_speed * Time.deltaTime * invert_rotation, 0);
            turn_tilt++;
        }

        // Hover height logic
        Vector3 position = transform.position;

        hover_offset = Mathf.Sin(Time.time * 2.5f) * hover_offset_magnitude;

        if (Mathf.Abs(hover_offset) < 0.01f)
        {
            hover_offset_magnitude = BASE_MAG * Random.Range(0.5f, 1.5f) * moving_mod;
        }

        position.y = terrain.GetPosition().y + terrain.SampleHeight(position) + ground_offset + hover_offset;
        transform.position = position;

        // Reset tilt when not turning
        if (turn_tilt <= 0)
        {
            turn_tilt++;
        }
        else if (turn_tilt >= 0)
        {
            turn_tilt--;
        }

        RaycastHit frontHit;
        RaycastHit backHit;

        // Shoot a ray downward from the front and back of the hovercraft
        bool frontValid = Physics.Raycast(frontPoint.position, Vector3.down, out frontHit, raycast_distance);
        bool backValid = Physics.Raycast(backPoint.position, Vector3.down, out backHit, raycast_distance);

        if (frontValid && backValid)
        {
            // Find the uphill/downhill direction by comparing the two hit points
            Vector3 terrainForward = frontHit.point - backHit.point;

            // Use cross product to get a normal that matches the ground slope
            Vector3 terrainNormal = Vector3.Cross(transform.right, terrainForward).normalized;

            // Build a rotation that faces forward but tilts with the terrain
            Quaternion targetRot = Quaternion.LookRotation(terrainForward, terrainNormal);

            // Smoothly rotate the hovercraft toward the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * tilt_smooth);
        }

    }
}
