using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private int rotationSpeed = 100;
    private Animator anim;
    public bool touchingWithPaddle = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var rotationX = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;

        if (_rigidBody.velocity.y < 0)
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

    void FixedUpdate(){
         if (_rigidBody.position.y <= -20)
        {
            GameManager.Instance.CharacterAlive = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // TODO this name needs to be changed 
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
