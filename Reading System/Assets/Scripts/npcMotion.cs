using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcMotion : MonoBehaviour
{
    public NavMeshAgent agent; //accessing the NavMeshAgent component
    public GameObject[] answers; //accessing answer objects
    public answers answersScript; //accessing the answers script

    bool canAnswer = true; //variable to ensure that the loop only runs once per each question.
    Vector3 startPos;

    int randAnswer;

    void Start()
    {
        startPos = transform.position; //sets the variable of the starting position to be the position of the player on the first frame
    }

    // Update is called once per frame
    void Update()
    {
        if (answersScript.frozen == 0 && canAnswer == true) { //characters start moving when the player is unfrozen
            canAnswer = false;
            randAnswer = Random.Range(0, 3); //generates a random answer to move towards
            agent.SetDestination(answers[randAnswer].transform.position); //Agent moves to a random answer
        }
        if (answersScript.frozen == 1) { //when the player is frozen, NPCs are now able to answer a question
            canAnswer = true;
            transform.position = startPos;
            transform.rotation = Quaternion.Euler(0, 0, 0); //resets position and rotation to beginning
        }
    }
}
