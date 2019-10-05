using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private bool mouse1Clicked;
    private bool mouse2Clicked;
    private float squareMaxVelocity;
    private Transform cameraTransform;
    public float maxVelocity = 3f;
    public float gravityConstant = 9.8f;
    public float eatMagnitude = 0.1f;
    public float playerForceScalar = 2.0f;

    public ObjectSpawner gcObjectSpawner;


    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        squareMaxVelocity = Mathf.Pow(maxVelocity, 2);

        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        mouse1Clicked = Input.GetButton("Fire1");
        mouse2Clicked = Input.GetButton("Fire2");
    }

    void FixedUpdate()
    {
        if(mouse1Clicked || mouse2Clicked){
            Vector3 direction = new Vector3(transform.position.x - cameraTransform.position.x,
                                            transform.position.y - cameraTransform.position.y,
                                            transform.position.z - cameraTransform.position.z);
            if(mouse2Clicked){
                direction *= -1;
            }
            rb.AddForce(direction * playerForceScalar);
        }  

        if(rb.velocity.sqrMagnitude > squareMaxVelocity){
            rb.AddForce(-rb.velocity.normalized * (rb.velocity.magnitude - maxVelocity));
        }
    }
     void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody){

            Vector3 direction = transform.position - other.transform.position;
            if(direction.magnitude < eatMagnitude){
                gcObjectSpawner.resetObject(other.gameObject);
                //increment points?
                return;
            }

            //float gravity = gravityConstant * rb.mass * other.attachedRigidbody.mass / Mathf.Pow(direction.magnitude, 2);
            //other.attachedRigidbody.AddForce(direction.normalized * gravity);
        }
    }
}
