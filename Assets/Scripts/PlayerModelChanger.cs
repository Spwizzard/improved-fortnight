using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelChanger : MonoBehaviour
{
    public GameObject startingParticleModel;
    public GameObject asteroid1Model;
    public GameObject asteroid3Model;
    private StageController gcStageController;
    private int currentStage;


    // Start is called before the first frame update
    void Start()
    {
        gcStageController = GameObject.Find("GameController").GetComponent<StageController>();
        currentStage = gcStageController.currentStage;
        startingParticleModel.SetActive(true);
        asteroid1Model.SetActive(false);
        asteroid3Model.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int newStage = gcStageController.currentStage;
        if (newStage != currentStage){
            switch(newStage){
                case 0:
                case 1:
                    break;
                case 2:
                case 3:
                case 4:
                    startingParticleModel.SetActive(false);
                    asteroid1Model.SetActive(true);
                    break;
                case 5:
                    asteroid1Model.SetActive(false);
                    asteroid3Model.SetActive(true);
                    break;
                default:
                    break;
            }
            currentStage = newStage;
        }
    }
}
