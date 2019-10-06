using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject lightGasParticlePrefab;
    public GameObject heavyGasCloudPrefab;
    public GameObject asteroid1Prefab;
    public GameObject asteroid2Prefab;
    public GameObject asteroid3Prefab;

    public int numberOfParticles = 100;
    public float spawnDistance = 50f;
    private float startingSpawnDistance = 50f;
    public float despawnDistance = 20f;
    private float startingDespawnDistance = 20f;
    public float upgradeChance = 0.1f;

    private GameObject parentGO;
    private GameObject playerGO;
    private StageController gcStageController;
    private float playerMass; 
    private List<GameObject> objectList;

    public int currentStage = 0; 

    private float[] stage0Chances = {0.95f, 1.0f, 0.0f};
    private float[] stage1Chances = {0.0f, 0.9f, 1.0f};
    private float[] stage2Chances = {0.0f, 0.1f, 1.0f};
    private float[] stage3Chances = {0.0f, 0.00f, 1.0f};
    private float[] stage4Chances = {0.0f, 0.0f, 1.0f};
    private float[] stage5Chances = {0.0f, 0.0f, 1.0f};
    private float[] stage6Chances = {0.0f, 0.0f, 1.0f};

    private List<float[]> stageChances;

    // Start is called before the first frame update
    void Start()
    {
        stageChances = new List<float[]>();
        stageChances.Add(stage0Chances);
        stageChances.Add(stage1Chances);
        stageChances.Add(stage2Chances);
        stageChances.Add(stage3Chances);
        stageChances.Add(stage4Chances);
        stageChances.Add(stage5Chances);
        stageChances.Add(stage6Chances);

        objectList = new List<GameObject>(numberOfParticles);
        playerGO = GameObject.Find("PlayerStartingParticle");
        parentGO = GameObject.Find("Objects");

        startingSpawnDistance = spawnDistance;
        startingDespawnDistance = despawnDistance;
        playerMass = playerGO.GetComponent<Rigidbody>().mass;
        gcStageController = gameObject.GetComponent<StageController>();

        for(int i = 0; i < numberOfParticles; i++){
            Vector3 pos = getRandomVector3(spawnDistance);
            GameObject go = spawnObjectByStage(pos);
            objectList.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentStage = gcStageController.currentStage;
    }

    void FixedUpdate()
    {
        playerMass = playerGO.GetComponent<Rigidbody>().mass;
        spawnDistance = startingSpawnDistance * (playerMass * 0.1f) + 30;
        despawnDistance = startingDespawnDistance * (playerMass * 0.1f) + 31;
        List<GameObject> resetObjectList = new List<GameObject>();
        foreach(GameObject go in objectList)
        {
            if(Vector3.Distance(go.transform.position, playerGO.transform.position) > despawnDistance)
            {
                resetObjectList.Add(go);
            }
        }
        while(resetObjectList.Count > 0){
            GameObject go = resetObjectList[0];
            resetObjectList.RemoveAt(0);
            resetObject(go);
        }
        
    }

    private GameObject spawnObjectByStage(Vector3 pos)
    {
        float upgradeRoll = Random.Range(0.0f, 1.0f);
        float[] upgradeChances = stageChances[currentStage];
        GameObject go;
        if(upgradeRoll < upgradeChances[0])
        {
            go = spawnLightGasParticle(pos);
        }
        else if(upgradeRoll < upgradeChances[1]){
            go = spawnHeavyGasCloud(pos);
        }
        else if(upgradeRoll < upgradeChances[2]){
            go = spawnAsteroid(pos);
        }
        else{
            go = spawnAsteroid(pos);
        }

        return go;
    }

    private GameObject spawnLightGasParticle(Vector3 pos){
        GameObject go;
        go = Instantiate(lightGasParticlePrefab, pos, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = new Vector3();
        rb.AddForce(getRandomVector3(0.25f), ForceMode.VelocityChange);
        rb.AddTorque(getRandomVector3(0.5f), ForceMode.VelocityChange);
        go.transform.parent = parentGO.transform;
        return go;
    }

    private GameObject spawnHeavyGasCloud(Vector3 pos){
        GameObject go;
        go = Instantiate(heavyGasCloudPrefab, pos, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = new Vector3();
        rb.AddForce(getRandomVector3(1.0f), ForceMode.VelocityChange);
        rb.AddTorque(getRandomVector3(1.0f), ForceMode.VelocityChange);
        go.transform.parent = parentGO.transform;
        return go;
    }

    private GameObject spawnAsteroid(Vector3 pos){
        GameObject go;

        float upgradeRoll = Random.Range(0.0f, 1.0f);
        if(upgradeRoll < 0.33){
            go = Instantiate(asteroid1Prefab, pos, Quaternion.identity);
        }
        else if(upgradeRoll < 0.66){
            go = Instantiate(asteroid2Prefab, pos, Quaternion.identity);
        }
        else{
            go = Instantiate(asteroid3Prefab, pos, Quaternion.identity);
        }
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = new Vector3();
        rb.AddForce(getRandomVector3(10.0f), ForceMode.VelocityChange);
        rb.AddTorque(getRandomVector3(0.5f), ForceMode.VelocityChange);
        go.transform.parent = parentGO.transform;
        return go;
    }

    public void resetObject(GameObject go){
        GameObject listObject = objectList.Find(x => x == go);
        if(listObject){
            objectList.Remove(listObject);
            Destroy(listObject);
            objectList.Add(spawnObjectByStage(playerGO.transform.position + getRandomVector3(spawnDistance)));
        }
    }

    void moveObjectIntoRange(GameObject go)
    {
        // just move the object
        go.transform.position = playerGO.transform.position + getRandomVector3(spawnDistance);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = new Vector3();
        rb.AddForce(getRandomVector3(0.25f), ForceMode.Impulse);
        rb.AddTorque(getRandomVector3(0.5f), ForceMode.Impulse);
        
    }

    Vector3 getRandomVector3(float magnitude){
        return new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude));
    }
}
