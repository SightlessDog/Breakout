using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleLight : MonoBehaviour
{
    public GameObject paddle; 
    // Start is called before the first frame update
    void Start()
    {
        paddle = GameObject.Find("Player(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = paddle.transform.position + new Vector3(1, 1, -9);
    }
}