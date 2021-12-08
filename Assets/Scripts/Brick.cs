using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int hits = 1;
    [SerializeField] private int points = 100;

    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float resizeWidth = 1f;

    [SerializeField] private Vector3 rotator;
    [SerializeField] private Material hitMaterial;
    private Material _orgMaterial;
    public AudioClip _hitBrickEffect;

    private Renderer _renderer;
    private Vector3 scaleChange;
    private float brickWidth;
    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ball(Clone)")
        {
            SoundManager.Instance.PlayOnce(_hitBrickEffect);
            brickWidth = Player._rigidbody.GetComponent<Renderer>().bounds.size.x;
            hits--;
            // Score points
            if (hits <= 0)
            {
                GameManager.Instance.Score += points;
				Ball._speed = 20f;
                Destroy(gameObject);
            }

            if (Ball._speed <= 50f)
            {
                Ball._speed *= acceleration;
			}

            if (Ball._speed >= 20f && acceleration == 1)
            {
                Ball._speed = 20f;
			}

                scaleChange = new Vector3(resizeWidth, 1f, 1f);
                Player._rigidbody.transform.localScale = scaleChange;

            _renderer.sharedMaterial = hitMaterial;
            Invoke("RestoreMaterial", 0.05f);
        }
    }

    void RestoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }
}
