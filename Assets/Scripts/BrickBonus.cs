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
}
