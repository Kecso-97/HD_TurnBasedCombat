using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public int damage;

    //public int lifetime;

    public float speed;

    public Vector3 direction;

    public Vector3 offset;

    private Rigidbody body;

    private Transform visualTransform;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        visualTransform = transform.GetChild(0).transform;
        body = GetComponent<Rigidbody>();
        body.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
    }

    
    void LateUpdate()
    {
        visualTransform.rotation = mainCamera.transform.rotation;
        
        visualTransform.position = transform.position - offset * transform.localScale.y;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CharacterStats othersStats = other.gameObject.GetComponent<CharacterStats>();
        if (othersStats != null)
        {
            othersStats.TakeDamage(damage);
        }

    }
}
