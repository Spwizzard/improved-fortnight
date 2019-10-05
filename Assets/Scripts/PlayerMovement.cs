using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private bool mouseClicked;
    private float squareMaxVelocity;
    public Transform cameraTransform;
    public float maxVelocity = 3f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        squareMaxVelocity = Mathf.Sqrt(maxVelocity);

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetButtonDown("Fire1")){
           mouseClicked = true;
       }    
    }

    void FixedUpdate()
    {
        if(mouseClicked){
            mouseClicked = false;
            Vector3 direction = new Vector3(transform.position.x - cameraTransform.position.x,
                                            transform.position.y - cameraTransform.position.y,
                                            transform.position.z - cameraTransform.position.z);
            rb.AddForce(direction, ForceMode.Impulse);
        }

        
        if(rb.velocity.sqrMagnitude > squareMaxVelocity){
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}
