using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    private Text massText;
    private float currentMass;
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        massText = gameObject.GetComponentInChildren<Text>();
        playerRb = GameObject.Find("PlayerStartingParticle").GetComponent<Rigidbody>();
        currentMass = playerRb.mass;

        massText.text = "Mass: " + currentMass.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        currentMass = playerRb.mass;
        massText.text = "Mass: " + currentMass.ToString();
    }
}
