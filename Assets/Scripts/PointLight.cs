using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if (ball == null)
        {
            ball = GameObject.Find("Ball(Clone)");
        }
        if (ball != null)
        {
            transform.position = ball.transform.position - new Vector3(-2, -2, 7);
        }
    }
}
