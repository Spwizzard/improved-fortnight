using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private GameObject playerGO;
    private Rigidbody playerRb;
    private float playerMass; 

    public int currentStage = 0;
    public float stage1Mass = 2;
    public float stage2Mass = 10;
    public float stage3Mass = 15;
    public float stage4Mass = 20;
    public float stage5Mass = 25;
    public float stage6Mass = 30;
    public float stage7Mass = 30;
    public float stage8Mass = 30;
    public float stage9Mass = 30;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("PlayerStartingParticle");
        playerRb = playerGO.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        playerMass = playerRb.mass;

        if(playerMass >= stage9Mass){
            currentStage = 9;
        }
        if(playerMass >= stage8Mass){
            currentStage = 8;
        }
        else if(playerMass >= stage7Mass){
            currentStage = 7;
        }
        else if(playerMass >= stage6Mass){
            currentStage = 6;
        }
        else if(playerMass >= stage5Mass){
            currentStage = 5;
        }
        else if(playerMass >= stage4Mass){
            currentStage = 4;
        }
        else if(playerMass >= stage3Mass){
            currentStage = 3;
        }
        else if(playerMass >= stage2Mass){
            currentStage = 2;
        }
        else if(playerMass >= stage1Mass){
            currentStage = 1;
        }
        
    }
}
