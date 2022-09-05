using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeCount : MonoBehaviour
{
    int seconds = 0; //defines seconds variable.
    int waitPeriodSeconds = 0;
    bool canCount = true;
    bool countWaitPeriod = false;
    string currentScene;

    bool displayWaitOnSceneLoad = false;

    bool hasRunOnce = false;

    public GameObject waitScreenElements;
    public GameObject[] Buttons;
    public GameObject blankScreen;

    public static TimeCount instance;

    void Awake () 
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this)
        {
            Destroy (gameObject);
        }
    
        DontDestroyOnLoad (gameObject);
    }

    void Start()
    {
        //DontDestroyOnLoad(gameObject);

        blankScreen = GameObject.Find("BlankScreen");
        waitScreenElements = GameObject.Find("Wait");
        Buttons[0] = GameObject.Find("PeterRabbit");
        Buttons[1] = GameObject.Find("JackBeanstalk");
        Buttons[2] = GameObject.Find("Maui");

        StartCoroutine(Count());
    }

    void Update() {
        Debug.Log("Count Wait Period = " + countWaitPeriod);
        /*Debug.Log("HasRunOnce = " + hasRunOnce);
        Debug.Log("Display Wait on Scene Load = " + displayWaitOnSceneLoad);*/

        if (SceneManager.GetActiveScene().name != "LearningTree") {
            hasRunOnce = false;
        }
        if (seconds > 19 && canCount && SceneManager.GetActiveScene().name != "LearningTree") { 
            displayWaitOnSceneLoad = true;
            //DisplayWaitScreen();
            canCount = false;
            seconds = 0;
            //StartCoroutine(Count());
        }
        /*if (seconds > 10 && !canCount && !displayWaitOnSceneLoad) {
            Debug.Log("10 Seconds Later");
            canCount = true;
            seconds = 0;
            StartCoroutine(Count());
        }*/
        if (displayWaitOnSceneLoad && SceneManager.GetActiveScene().name == "LearningTree" && !hasRunOnce) {
            //Debug.Log("HasRunOnce = " + hasRunOnce);
            Debug.Log("First Condition True");
            hasRunOnce = true;
            blankScreen = GameObject.Find("BlankScreen");
            waitScreenElements = GameObject.Find("Wait");
            Buttons[0] = GameObject.Find("PeterRabbit");
            Buttons[1] = GameObject.Find("JackBeanstalk");
            Buttons[2] = GameObject.Find("Maui");
            Debug.Log("Display");
            blankScreen.SetActive(false);
            displayWaitOnSceneLoad = false;
            waitScreenElements.SetActive(true);
            for(int i=0; i<Buttons.Length; i++) {
                Buttons[i].SetActive(false);
            }
            waitPeriodSeconds = 0;
            countWaitPeriod = true;
            StartCoroutine(Count());
        }
        if (!displayWaitOnSceneLoad && SceneManager.GetActiveScene().name == "LearningTree" && !hasRunOnce) {
            //Debug.Log("HasRunOnce = " + hasRunOnce);
            hasRunOnce = true;
            blankScreen = GameObject.Find("BlankScreen");
            waitScreenElements = GameObject.Find("Wait");
            Buttons[0] = GameObject.Find("PeterRabbit");
            Buttons[1] = GameObject.Find("JackBeanstalk");
            Buttons[2] = GameObject.Find("Maui");
            blankScreen.SetActive(false);
            waitScreenElements.SetActive(false);
            for(int i=0; i<Buttons.Length; i++) {
                Buttons[i].SetActive(true);
            }
        }
        if (waitPeriodSeconds > 9) {
            countWaitPeriod = false;
            /*blankScreen = GameObject.Find("BlankScreen");
            waitScreenElements = GameObject.Find("Wait");
            Buttons[0] = GameObject.Find("PeterRabbit");
            Buttons[1] = GameObject.Find("JackBeanstalk");
            Buttons[2] = GameObject.Find("Maui");*/
            waitScreenElements.SetActive(false);
            blankScreen.SetActive(false);
            for(int i=0; i<Buttons.Length; i++) {
                Buttons[i].SetActive(true);
            }
        }
    }

    IEnumerator Count() {
        while (canCount) {
            yield return new WaitForSeconds(1); //waits 1 second, increments seconds variable
            seconds++;
            //Debug.Log(seconds);
        }
        while (countWaitPeriod) {
            yield return new WaitForSeconds(1);
            waitPeriodSeconds++;
        }
        /*while (!canCount) {
            yield return new WaitForSeconds(1);
            seconds++;
            //Debug.Log(seconds);
        }*/
    }
}
