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
    public float eatMagnitude = 0.1f;
    private float startingEatMagnitude = 0.1f;
    public float playerForceScalar = 2.0f;

    public ObjectSpawner gcObjectSpawner;
    public float currentMass;


    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        squareMaxVelocity = Mathf.Pow(maxVelocity, 2);

        rb = gameObject.GetComponent<Rigidbody>();
        currentMass = rb.mass;

        startingEatMagnitude = eatMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        mouse1Clicked = Input.GetButton("Fire1");
        mouse2Clicked = Input.GetButton("Fire2");
    }

    void FixedUpdate()
    {
        //Update the player's current mass
        rb.mass = currentMass;
        transform.localScale = new Vector3(1,1,1) * ((currentMass * 1.0f) + 1); 
        eatMagnitude = startingEatMagnitude * ((currentMass * 1.0f) + 1);

        //Move if either mouse is clicked
        if(mouse1Clicked || mouse2Clicked){
            Vector3 direction = new Vector3(transform.position.x - cameraTransform.position.x,
                                            transform.position.y - cameraTransform.position.y,
                                            transform.position.z - cameraTransform.position.z);
            if(mouse2Clicked){
                direction *= -1;
            }
            rb.AddForce(direction * playerForceScalar * currentMass);
        }  

        //cap velocity
        if(rb.velocity.sqrMagnitude > squareMaxVelocity){
            rb.AddForce(-rb.velocity.normalized * (rb.velocity.magnitude - maxVelocity));
        }
    }
     void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody){

            Vector3 direction = transform.position - other.transform.position;
            if(direction.magnitude < eatMagnitude){
                currentMass += other.attachedRigidbody.mass;
                gcObjectSpawner.resetObject(other.gameObject);
                Debug.Log(currentMass);
                return;
            }
        }
    }
}
