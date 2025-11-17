using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    public GameObject[] cars;


    public GameObject current_car_obj;

    private int curCarIdx;

    // Start is called before the first frame update
    void Start()
    {
        current_car_obj = Instantiate(cars[curCarIdx], transform.position, transform.rotation);
        curCarIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Destroy(current_car_obj);
            curCarIdx++;
            if (curCarIdx >= cars.Length)
            {
                curCarIdx = 0;
            }
            current_car_obj = Instantiate(cars[curCarIdx], transform.position, transform.rotation);
        }
    }
}
