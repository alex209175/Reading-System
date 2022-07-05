using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody rb; //accessing the rigidbody
    public float force = 1000000f; //defining variable for size of force being applied to the character
    public answers answers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
    }
}
