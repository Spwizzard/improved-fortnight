using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoTrail : MonoBehaviour {
    //Bit of fragile way to get current model/prefab the player uses; Refer to PlayerModelChanger script for changes.
    //Feel there should be someway to reference or get the stage/prefab being used without having to copy the code here.
    //And get around using the Find object bit of code
    private StageController gcStageController;
    private int currentStage;
    private int newStage;
    //Used to hold the models that are used for spawning.
    public GameObject[] playerEchoModels;
    private GameObject currentEchoModel;
    //Used for determining the length between the object spawning and how long it stays
    public float startSpawnCooldown = 0.5f;
    public float destroyTimer = 4f;
    private float currentSpawnCooldown;

    // Start is called before the first frame update
    void Start() {

        gcStageController = GameObject.Find("GameController").GetComponent<StageController>();
        currentStage = gcStageController.currentStage;

        currentEchoModel = playerEchoModels[0];
    }

    // Update is called once per frame
    void Update() {
        //Start of SP's Switch statement - slight tweaks to just refer to the GameObject instead of turning stuff on and off.
        newStage = gcStageController.currentStage;
        if (newStage != currentStage) {
            switch (newStage) {
                case 0:
                case 1:
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    currentEchoModel = playerEchoModels[1];
                    break;
                case 6:
                case 7:
                case 8:
                    currentEchoModel = playerEchoModels[2];
                    break;
                default:
                    break;
            }
            currentStage = newStage;
        }
        //End of SP's Switch Statement
        
        //Spawn the GameObject
        if (currentSpawnCooldown <= 0) {
            GameObject instance = (GameObject)Instantiate(currentEchoModel, transform.position, Quaternion.identity);
            //Sets the instance to have the rotation and scale of the model
            {
                instance.transform.localRotation = this.transform.localRotation;
                instance.transform.localScale = this.transform.localScale;
            }
            Destroy(instance, destroyTimer);
            currentSpawnCooldown = startSpawnCooldown;
        }
        else {
            currentSpawnCooldown -= Time.deltaTime;
        }
    }
}

