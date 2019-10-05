using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private bool mouseClicked;
    private float squareMaxVelocity;
    private Transform cameraTransform;
    public float maxVelocity = 3f;
    public float gravityConstant = 9.8f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        squareMaxVelocity = Mathf.Pow(maxVelocity, 2);

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseClicked = Input.GetButton("Fire1");
    }

    void FixedUpdate()
    {
        if(mouseClicked){
            Vector3 direction = new Vector3(transform.position.x - cameraTransform.position.x,
                                            transform.position.y - cameraTransform.position.y,
                                            transform.position.z - cameraTransform.position.z);
            rb.AddForce(direction);
        }  

        if(rb.velocity.sqrMagnitude > squareMaxVelocity){
            rb.AddForce(-rb.velocity.normalized * (rb.velocity.magnitude - maxVelocity));
        }
    }
     void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody){
            Vector3 direction = transform.position - other.transform.position;
            float gravity = gravityConstant * rb.mass * other.attachedRigidbody.mass / Mathf.Pow(direction.magnitude, 2);
            other.attachedRigidbody.AddForce(direction.normalized * gravity);
        }
    }
}
