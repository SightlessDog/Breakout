using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBonus : MonoBehaviour
{
    [SerializeField] private int hits = 1;
    [SerializeField] private int points = 100;
    [SerializeField] private Vector3 rotator;
    [SerializeField] private Material hitMaterial;

    private Material _orgMaterial;

    private Renderer _renderer;

    public Rigidbody Prefab;

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
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        hits--;
        // Score points
        if (hits <= 0)
        {
            GameManager.Instance.Score += points;
            explode(gameObject);
            Destroy(gameObject);

            Rigidbody RigidPrefab;
            RigidPrefab = Instantiate(Prefab,
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.identity) as Rigidbody;
        }
        _renderer.sharedMaterial = hitMaterial;
        Invoke("RestoreMaterial", 0.05f);
    }

    void RestoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }

    void explode(GameObject gameobject)
    {
        gameobject.SetActive(false);

        for (int x = 0; x < cubesInRow; x += 2)
        {
            for (int y = 0; y < cubesInRow; y += 2)
            {
                for (int z = 0; z < cubesInRow; z += 2)
                {
                    createPiece(x, y, z);
                }
            }
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
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

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

