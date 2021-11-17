using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -33, 33), transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.PlayerInput)
        {
            MouseMovement();
        }
        else
        {
            KeyboardMovement();
        }
    }

    void MouseMovement()
    {
        _rigidbody.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, -17, 0));
    }

    void KeyboardMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rigidbody.MovePosition(new Vector3(transform.position.x - 0.5f, -17, 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _rigidbody.MovePosition(new Vector3(transform.position.x + 0.5f, -17, 0));
        }
        else
        {
            _rigidbody.MovePosition(new Vector3(transform.position.x, -17, 0));
        }
    }
}
