using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Transform playerTransform;

    public float despawnDistance;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("PlayerStartingParticle").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) > despawnDistance)
        {
            transform.position = playerTransform.position;
            //Destroy(gameObject);
        }
    }
}
