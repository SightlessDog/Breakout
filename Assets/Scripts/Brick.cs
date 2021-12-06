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

    [SerializeField] private float cubeSize = 0.2f;
    [SerializeField] private int cubesInRow = 5;

    private float cubesPivotDistance;
    private Vector3 cubesPivot;

    [SerializeField] private float explosionForce = 50f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float explosionUpward = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);

        cubesPivotDistance = cubeSize * cubesInRow / 2;
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ball(Clone)" || collision.gameObject.name == "BallBonus(Clone)")
        {
            SoundManager.Instance.PlaySoundOnce(_hitBrickEffect);
            brickWidth = Player._rigidbody.GetComponent<Renderer>().bounds.size.x;
            hits--;
            // Score points
            if (hits <= 0)
            {
                GameManager.Instance.Score += points;
				Ball._speed = 20f;
                explode(gameObject);
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

    void explode(GameObject gameobject)
    {
        gameobject.SetActive(false);

        for(int x = 0; x < cubesInRow; x += 2)
        {
            for(int y = 0; y< cubesInRow; y += 2)
            {
                for(int z = 0; z < cubesInRow; z += 2)
                {
                    createPiece(x, y, z);
                }
            }
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }

    }

    void createPiece(int x, int y, int z)
    {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        piece.GetComponent<Renderer>().material = _orgMaterial;

        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
        //Physics.IgnoreCollision;
    }
}
