using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static float _speed = 20f;
    private Rigidbody _rigidBody;
    private Vector3 _velocity;
    private Renderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
    }

    void Launch()
    {
        _rigidBody.velocity = Vector3.up * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // We force the ball to have always the same speed that we want
        _rigidBody.velocity = _rigidBody.velocity.normalized * _speed;
        // We store the velocity before the collision happens and gets absorbed
        _velocity = _rigidBody.velocity;
        if (!_renderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(gameObject);
        }
    }

    // Collision : da wo die Collision genau betrachtet
    private void OnCollisionEnter(Collision collision)
    {
        // We use the _velocity that we stored so we guarantee that the ball will get bounced
        _rigidBody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }
}
