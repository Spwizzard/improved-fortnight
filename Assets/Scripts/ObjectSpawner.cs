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

    public int numberOfActiveObjects = 100;
    public float spawnDistance = 50f;
    private float startingSpawnDistance = 50f;
    public float despawnDistance = 20f;
    private float startingDespawnDistance = 20f;

    private GameObject playerGO;
    private StageController gcStageController;
    private float playerMass; 
    private List<GameObject> activeObjectList;
    private List<GameObject> lightGasParticlePool;
    private List<GameObject> heavyGasCloudPool;
    private List<GameObject> asteroid1Pool;
    private List<GameObject> asteroid2Pool;
    private List<GameObject> asteroid3Pool;

    public int currentStage = 0; 

    private float[] stage0Chances = {0.95f, 1.0f, 0.0f, 0.0f, 0.0f};
    private float[] stage1Chances = {0.7f, 0.98f, 1.0f, 0.0f, 0.0f};
    private float[] stage2Chances = {0.2f, 0.4f, 0.95f, 1.0f, 0.0f};
    private float[] stage3Chances = {0.0f, 0.2f, 0.8f, 1.0f, 0.0f};
    private float[] stage4Chances = {0.0f, 0.1f, 0.3f, 0.8f, 1.0f};
    private float[] stage5Chances = {0.0f, 0.0f, 0.2f, 0.6f, 1.0f};
    private float[] stage6Chances = {0.0f, 0.0f, 0.0f, 0.4f, 1.0f};

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

        playerGO = GameObject.Find("PlayerStartingParticle");
        startingSpawnDistance = spawnDistance;
        startingDespawnDistance = despawnDistance;
        playerMass = playerGO.GetComponent<Rigidbody>().mass;
        gcStageController = gameObject.GetComponent<StageController>();
        
        activeObjectList = new List<GameObject>(numberOfActiveObjects);
        lightGasParticlePool = new List<GameObject>(numberOfActiveObjects);
        heavyGasCloudPool = new List<GameObject>(numberOfActiveObjects);
        asteroid1Pool = new List<GameObject>(numberOfActiveObjects);
        asteroid2Pool = new List<GameObject>(numberOfActiveObjects);
        asteroid3Pool = new List<GameObject>(numberOfActiveObjects);

        lightGasParticlePool.AddRange(spawnObjectsInPool("LightGasParticles", lightGasParticlePrefab, numberOfActiveObjects));
        heavyGasCloudPool.AddRange(spawnObjectsInPool("HeavyGasClouds", heavyGasCloudPrefab, numberOfActiveObjects));
        asteroid1Pool.AddRange(spawnObjectsInPool("Asteroids1", asteroid1Prefab, numberOfActiveObjects));
        asteroid2Pool.AddRange(spawnObjectsInPool("Asteroids2", asteroid2Prefab, numberOfActiveObjects));
        asteroid3Pool.AddRange(spawnObjectsInPool("Asteroids3", asteroid3Prefab, numberOfActiveObjects));

        activateObjects(numberOfActiveObjects);

        
    }

    List<GameObject> spawnObjectsInPool(string parentName, GameObject prefab, int num){
        List<GameObject> list = new List<GameObject>(num);
        Transform parentTransform = GameObject.Find(parentName).transform;
        for(int i = 0; i < num; i++){
            GameObject go = Instantiate(prefab, new Vector3(), Quaternion.identity);
            go.SetActive(false);
            go.transform.parent = parentTransform;
            list.Add(go);
        }
        return list;
    }

    void activateObjects(int numToActivate){
        for (int i = 0; i < numToActivate ; i++){
            activateObjectFromPool(ref getPoolUsingStage());
        }
    }

    ref List<GameObject> getPoolUsingStage(){
        float randomRoll = Random.Range(0.0f, 1.0f);
        float[] objectChances = stageChances[currentStage];
        if(randomRoll < objectChances[0])
        {
            return ref lightGasParticlePool;
        }
        else if(randomRoll < objectChances[1]){
            return ref heavyGasCloudPool;
        }
        else if(randomRoll < objectChances[2]){
            return ref asteroid1Pool;
        }
        else if(randomRoll < objectChances[3]){
            return ref asteroid2Pool;
        }
        else if(randomRoll < objectChances[4]){
            return ref asteroid3Pool;
        }
        else{
            return ref asteroid3Pool;
        }
    }

    void activateObjectFromPool(ref List<GameObject> pool){
        foreach(GameObject go in pool){
            if(!go.activeSelf){
                go.SetActive(true);
                activeObjectList.Add(go);
                go.transform.position = playerGO.transform.position + getRandomVector3(spawnDistance);
                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.AddForce(getRandomVector3(0.5f), ForceMode.VelocityChange);
                rb.AddTorque(getRandomVector3(0.5f), ForceMode.VelocityChange);
                return;
            }
        }
        Debug.LogWarning("Somehow didn't find an inactive gameobject in pool!");
        return;
    }

    void deactivateObject(ref GameObject go){
        go.SetActive(false);
        activeObjectList.Remove(go);
    }

    // Update is called once per frame
    void Update()
    {
        currentStage = gcStageController.currentStage;
    }

    void FixedUpdate()
    {
        playerMass = playerGO.GetComponent<Rigidbody>().mass;
        spawnDistance = startingSpawnDistance * (playerMass * 0.05f) + 30;
        despawnDistance = startingDespawnDistance * (playerMass * 0.05f) + 31;
        List<GameObject> deactivatedObjectsList = new List<GameObject>();
        foreach(GameObject go in activeObjectList)
        {
            if(Vector3.Distance(go.transform.position, playerGO.transform.position) > despawnDistance)
            {
               deactivatedObjectsList.Add(go);
            }
        }
        while(deactivatedObjectsList.Count > 0){
            GameObject go = deactivatedObjectsList[0];
            List<GameObject> pool = getPoolUsingStage();
            if(pool.Contains(go)){
                // this object is already in the pool we want
                // so just move it
                resetObjectPosition(ref go);
            }
            else{
                deactivateObject(ref go);
                activateObjectFromPool(ref pool);
            }
            deactivatedObjectsList.Remove(go);
        }
    }
    public void resetObject(GameObject go){
        GameObject listObject = activeObjectList.Find(x => x == go);
        if(listObject){
            List<GameObject> pool = getPoolUsingStage();
            if(pool.Contains(listObject)){
                // this object is already in the pool we want
                // so just move it
                resetObjectPosition(ref listObject);
            }
            else{
                deactivateObject(ref listObject);
                activateObjectFromPool(ref pool);
            }
        }
    }

    void resetObjectPosition(ref GameObject go){
        go.transform.position = playerGO.transform.position + getRandomVector3(spawnDistance);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = new Vector3();
        rb.AddForce(getRandomVector3(0.5f), ForceMode.VelocityChange);
        rb.AddTorque(getRandomVector3(0.5f), ForceMode.VelocityChange);
    }

    Vector3 getRandomVector3(float magnitude){

        return new Vector3(getRandomExcludingCenter(magnitude), getRandomExcludingCenter(magnitude), getRandomExcludingCenter(magnitude));
    }

    float getRandomExcludingCenter(float range){
        if(Random.Range(0.0f, 1.0f) > 0.5f){
            return Random.Range(range / 20 , range);
        }
        else{
            return -Random.Range(range / 20 , range);
        }
    }
}
