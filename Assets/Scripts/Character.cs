using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private int rotationSpeed = 100;
    private Animator anim;
    private bool touchingWithPaddle = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var rotationX = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;

        if (_rigidbody.velocity.y < 0)
        {
            anim.Play(Animator.StringToHash(("Falling")));
        }
        else
        {
            if (touchingWithPaddle)
            {
                anim.Play(Animator.StringToHash("Idle"));
            }
            else
            {
                anim.Play(Animator.StringToHash("Standing"));
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // TODO this name needs to be changed 
        Debug.Log("Touching with " + collider.name);
        if (collider.name == "Player(Clone)")
        {
            touchingWithPaddle = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.name == "Player(Clone)")
        {
            touchingWithPaddle = false;
        }
    }
}
