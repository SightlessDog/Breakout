using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    [SerializeField] private int hits = 10;
    [SerializeField] private int points = 500; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        hits--;

        if (hits <= 0)
        {
            GameManager.Instance.Score += points;
            Destroy(gameObject);
        }
    }
}
