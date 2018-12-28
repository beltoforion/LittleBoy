using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotionController : MonoBehaviour
{
    public float _force;

    private Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        tag = "projectile";
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "initiator")
            return;

        _force = 0;
        _rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(new Vector3(0, 0, _force));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
