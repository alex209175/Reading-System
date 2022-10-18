using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class detectInternet : MonoBehaviour
{
    int currentScene; //gets current scene as an integer
    bool hasLoadedNoInternet = false; //detects when the no internet scene has been loaded

    void Awake() {
        DontDestroyOnLoad(this.gameObject); //ensures that code will run in any scene
    }

    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!hasLoadedNoInternet) {
                currentScene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene("noInternet"); //Loads no internet screen
                hasLoadedNoInternet = true;
            }
        }
        else if (hasLoadedNoInternet) {
            SceneManager.LoadScene(currentScene);
            hasLoadedNoInternet = false;
        }
    }
}
