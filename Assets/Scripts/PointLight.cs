using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    public GameObject ball; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ball = GameObject.Find("Ball(Clone)");
        transform.position = ball.transform.position - new Vector3(-2, -2, 7);
    }
}
