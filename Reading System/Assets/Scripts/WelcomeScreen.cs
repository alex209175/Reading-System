using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WelcomeScreen : MonoBehaviour
{
    public TMP_Dropdown dropdown; //accessing dropdown menu
    bool isTeacher = false; //determines whether user is a teacher or a student

    void Start() {
        dropdown.onValueChanged.AddListener(delegate { //adds listener to dropdown menu
            dropdownValueChanged(dropdown);
        });
    }

    void dropdownValueChanged(TMP_Dropdown change) {
        if (change.value == 1) {
            isTeacher = true;
        }
        else {
            isTeacher = false;
        }
    }

    public void buttonClick() {
        //Determines whether to open class creator or student login
        if (isTeacher) {
            SceneManager.LoadScene("CreateClass");
        }
        else {
            SceneManager.LoadScene("StudentLogin");
        }
    }
}
