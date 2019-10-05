using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private bool mouseClicked;
    public Transform cameraTransform;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
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
    }
}
