using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    public GameObject astronaut;
    // Start is called before the first frame update
    void Start()
    {
        astronaut.transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
