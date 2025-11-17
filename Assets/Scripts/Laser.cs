using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed *  Time.deltaTime);

        if (transform.position.magnitude > 1000)
        {
            Destroy(gameObject);
        }
    }
}
