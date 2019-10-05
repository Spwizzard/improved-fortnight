using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject lightGasParticlePrefab;

    public int numberOfParticles = 100;
    public float spawnDistance = 50f;
    public float despawnDistance = 20f;

    private GameObject parentGO;
    private GameObject playerGO;
    private List<GameObject> objectList;

    // Start is called before the first frame update
    void Start()
    {
        objectList = new List<GameObject>(numberOfParticles);
        playerGO = GameObject.Find("PlayerStartingParticle");
        parentGO = GameObject.Find("Objects");
        for(int i = 0; i < numberOfParticles; i++){
            Vector3 pos = getRandomVector3(spawnDistance);
            GameObject go = Instantiate(lightGasParticlePrefab, pos, Quaternion.identity);
            go.transform.parent = parentGO.transform;
            objectList.Add(go);

            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(getRandomVector3(0.25f), ForceMode.Impulse);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        foreach(GameObject go in objectList)
        {
            if(Vector3.Distance(go.transform.position, playerGO.transform.position) > despawnDistance)
            {
                respawnObject(go);
                //Destroy(gameObject);
            }
        }
        
    }

    public void resetObject(GameObject go){
        GameObject listObject = objectList.Find(x => x == go);
        if(listObject){
            respawnObject(listObject);
        }
    }

    void respawnObject(GameObject go)
    {
        go.transform.position = playerGO.transform.position + getRandomVector3(spawnDistance);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = getRandomVector3(0.25f);
    }

    Vector3 getRandomVector3(float magnitude){
        return new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude));
    }
}
