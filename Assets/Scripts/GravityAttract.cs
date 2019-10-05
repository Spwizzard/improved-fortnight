using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttract : MonoBehaviour
{
    public float gravityConstant = 4.0f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

     void OnTriggerStay(Collider other)
    {   
        if(other.attachedRigidbody){
            if(rb == null)
            {
                return;
            }
            Vector3 direction = transform.position - other.transform.position;
            float gravity = gravityConstant * rb.mass * other.attachedRigidbody.mass / Mathf.Pow(direction.magnitude, 2);
            other.attachedRigidbody.AddForce(direction.normalized * gravity);
        }
    }
}
