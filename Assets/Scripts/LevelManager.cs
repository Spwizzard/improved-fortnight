using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown("escape") && SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene("MainMenu");
        }

        else {
            Application.Quit();
        }
    }

    public void loadGameLevel() {
        SceneManager.LoadScene("GameScene");
    }
}
