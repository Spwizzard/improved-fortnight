using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject lightGasParticlePrefab;

    public int numberOfParticles = 100;
    public int minX = -20;
    public int maxX = 20;
    public int minY = -20;
    public int maxY = 20;
    public int minZ = -20;
    public int maxZ = 20;

    private GameObject parentGO;
    private ArrayList objectList;

    // Start is called before the first frame update
    void Start()
    {
        objectList = new ArrayList(numberOfParticles);
        parentGO = GameObject.Find("Objects");
        for(int i = 0; i < numberOfParticles; i++){
            Vector3 pos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            GameObject go = Instantiate(lightGasParticlePrefab, pos, Quaternion.identity);
            go.transform.parent = parentGO.transform;
            objectList.Add(go);

            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), ForceMode.Impulse);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
