using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWait : MonoBehaviour
{
    public GameObject waitScreenText;
    public GameObject[] learningTreeElements; //accessing learning tree and wait text

    void Start()
    {
        for (int i=0; i<learningTreeElements.Length; i++) {
            learningTreeElements[i].SetActive(false); //Displays the wait text
        }
        waitScreenText.SetActive(true);
        StartCoroutine(WaitTwentySeconds()); //waits for twenty seconds
    }
    IEnumerator WaitTwentySeconds() {
        for (int i=0; i<20; i++) {
            yield return new WaitForSeconds(1);
        }
        for (int i=0; i<learningTreeElements.Length; i++) { //displays the learning tree
            learningTreeElements[i].SetActive(true);
        }
        waitScreenText.SetActive(false);
    }
}