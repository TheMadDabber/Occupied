using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{

    Rigidbody myRigidBody;
    public float upForce, forwardForce;

    public int rangedDamage = 3;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        GrenadeThrow();
    }

    private void GrenadeThrow()
    {
        myRigidBody.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
        myRigidBody.AddForce(transform.up * upForce, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthSystem>().TakeDamage(rangedDamage);
            Destroy(gameObject);
        }
    }
}
