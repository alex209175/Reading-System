using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent; //accessing the NavMeshAgent component
    public GameObject[] answers; //accessing answer objects
    public answers answersScript; //accessing the answers script

    bool canAnswer = true; //variable to ensure that the loop only runs once per each question.
    Vector3 startPos;

    public int answeredQuestion; //variable to store answer that has been selected
    public bool hasReachedAnswer = false; //variable to show whether the player has stood on the answer

    void Start()
    {
        startPos = transform.position; //sets the variable of the starting position to be the position of the player on the first frame
    }

    void Update () {
        if (answersScript.frozen == 1) { //when the player is frozen, NPCs are now able to answer a question
            canAnswer = true;
            transform.position = startPos;
            transform.rotation = Quaternion.Euler(0, 0, 0); //resets position and rotation to beginning
        }
    }

    public void Red() {
        if (answersScript.frozen == 0 && canAnswer) {
            agent.SetDestination(answers[0].transform.position); //Player moves to red answer
            canAnswer = false;
            answeredQuestion = 1;
            for (int i = 0; i < answersScript.answeringUIObjects.Length; i++) {
                answersScript.answeringUIObjects[i].SetActive(false);
            }
            answersScript.timerObject.SetActive(false);
            StartCoroutine(reachAnswer());
        }
    }
    public void Green() {
        if (answersScript.frozen == 0 && canAnswer) {
            agent.SetDestination(answers[3].transform.position); //Player moves to green answer
            canAnswer = false;
            answeredQuestion = 4;
            for (int i = 0; i < answersScript.answeringUIObjects.Length; i++) {
                answersScript.answeringUIObjects[i].SetActive(false);
            }
            answersScript.timerObject.SetActive(false);
            StartCoroutine(reachAnswer());
        }
    }
    public void Yellow() {
        if (answersScript.frozen == 0 && canAnswer) {
            agent.SetDestination(answers[2].transform.position); //Player moves to yellow answer
            canAnswer = false;
            answeredQuestion = 3;
            for (int i = 0; i < answersScript.answeringUIObjects.Length; i++) {
                answersScript.answeringUIObjects[i].SetActive(false);
            }
            answersScript.timerObject.SetActive(false);
            StartCoroutine(reachAnswer());
        }
    }
    public void Blue() {
        if (answersScript.frozen == 0 && canAnswer) {
            agent.SetDestination(answers[1].transform.position); //Player moves to blue answer
            canAnswer = false;
            answeredQuestion = 2;
            for (int i = 0; i < answersScript.answeringUIObjects.Length; i++) { //Disables UI objects
                answersScript.answeringUIObjects[i].SetActive(false);
            }
            answersScript.timerObject.SetActive(false);
            StartCoroutine(reachAnswer());
        }
    }

    IEnumerator reachAnswer() {  //Pauses player
        yield return new WaitForSeconds(3);
        hasReachedAnswer = true;
        agent.Stop();
        agent.ResetPath();
    }
}
