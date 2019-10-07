using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbOthers : MonoBehaviour
{
    public float eatMagnitude = 0.1f;
    private float startingEatMagnitude = 0.1f;
    private ObjectSpawner gcObjectSpawner;
    private Rigidbody rb;
    private float startingMass;

    //Clips needed for appropriate sounds effects
    public AudioClip absorbNoise;
    // Start is called before the first frame update
    void Start()
    {
        gcObjectSpawner = GameObject.Find("GameController").GetComponent<ObjectSpawner>();
        rb = gameObject.GetComponent<Rigidbody>();
        startingEatMagnitude = eatMagnitude;
        startingMass = rb.mass;
    }

    void FixedUpdate(){
        //Update the object's scale based on mass
        transform.localScale = new Vector3(1,1,1) * ((rb.mass/startingMass * 1.0f) + 1); 
        eatMagnitude = startingEatMagnitude * ((rb.mass * 0.5f) + 1);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody && (other.tag == "Object") || (other.tag == "BackgroundObject")){
            Vector3 direction = transform.position - other.transform.position;
            if(direction.magnitude < eatMagnitude && rb.mass >= 1.1f * other.attachedRigidbody.mass){
                //Play Absorb Noise
                SoundManager.instance.PlaySingle(absorbNoise);
                rb.mass += other.attachedRigidbody.mass;
                if(other.tag == "Object"){
                    gcObjectSpawner.resetObject(other.gameObject);
                }
                else if(other.tag == "BackgroundObject"){
                    other.gameObject.SetActive(false);
                }
                return;
            }
        }

    }
}
