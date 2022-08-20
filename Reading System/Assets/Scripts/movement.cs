using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    /*public Rigidbody rb; //accessing the rigidbody
    public float force = 1000000f; //defining variable for size of force being applied to the character
    public answers answers;*/

    public UnityEngine.AI.NavMeshAgent agent; //accessing the NavMeshAgent component
    public GameObject[] answers; //accessing answer objects
    public answers answersScript; //accessing the answers script

    bool canAnswer = true; //variable to ensure that the loop only runs once per each question.
    Vector3 startPos;

    public int answeredQuestion; //variable to store answer that has been selected
    public bool hasReachedAnswer = false; //variable to show whether the player has stood on the answer

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; //sets the variable of the starting position to be the position of the player on the first frame
    }

    /*// Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("w")) {
            rb.AddRelativeForce(0, -force * Time.deltaTime, 0); //moving character based on key clicks
        }
        if (Input.GetKey("s")) {
            rb.AddRelativeForce(0, force * Time.deltaTime, 0);
        }
        if (Input.GetKey("a")) {
            rb.AddRelativeForce(-force * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("d")) {
            rb.AddRelativeForce(force * Time.deltaTime, 0, 0);
        }
        if (answers.frozen == 1) { //freezing character when character touches answer
            force = 0;
        }
        if (answers.frozen == 0) { //unfreezing the character when character is able to answer
            force = 1000000f;
        }
    }*/

    void Update () {
        /*
        if (answersScript.frozen == 0 && canAnswer == true) { //characters start moving when the player is unfrozen
            canAnswer = false;
            agent.SetDestination(answers[0].transform.position); //Agent moves to a random answer
        }*/
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
            answersScript.timerTextObject.SetActive(false);
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
            answersScript.timerTextObject.SetActive(false);
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
            answersScript.timerTextObject.SetActive(false);
            StartCoroutine(reachAnswer());
        }
    }
    public void Blue() {
        if (answersScript.frozen == 0 && canAnswer) {
            agent.SetDestination(answers[1].transform.position); //Player moves to blue answer
            canAnswer = false;
            answeredQuestion = 2;
            for (int i = 0; i < answersScript.answeringUIObjects.Length; i++) {
                answersScript.answeringUIObjects[i].SetActive(false);
            }
            answersScript.timerTextObject.SetActive(false);
            StartCoroutine(reachAnswer());
        }
    }

    IEnumerator reachAnswer() {
        yield return new WaitForSeconds(3);
        hasReachedAnswer = true;
        agent.Stop();
        agent.ResetPath();
    }
}
