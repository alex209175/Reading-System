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
        if (PlayerPrefs.GetInt("hasLoggedInAsTeacher") == 1 && PlayerPrefs.GetString("class") != null) { //if these conditions satisfied, teacher has already created class
            SceneManager.LoadScene("TeacherView");
        }
        if (PlayerPrefs.GetInt("hasLoggedInAsStudent") == 1 && PlayerPrefs.GetString("class") != null) { //just goes straight to student login
            SceneManager.LoadScene("StudentLogin");
        }

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
            PlayerPrefs.SetInt("hasLoggedInAsTeacher", 1);
            SceneManager.LoadScene("CreateClass");
        }
        else {
            PlayerPrefs.SetInt("hasLoggedInAsStudent", 1);
            SceneManager.LoadScene("StudentLogin");
        }
    }
}
