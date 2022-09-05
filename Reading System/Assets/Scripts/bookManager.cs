using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bookManager : MonoBehaviour
{
    public TimeCount timeCountScript; //accessing time count script

    public void PeterRabbit() {
        timeCountScript.waitScreenElements.SetActive(true);
        timeCountScript.blankScreen.SetActive(true);
        SceneManager.LoadScene("PeterRabbit"); //loads Peter Rabbit book when clicked
    }
    public void JackBeanstalk() {
        timeCountScript.waitScreenElements.SetActive(true);
        timeCountScript.blankScreen.SetActive(true);
        SceneManager.LoadScene("JackAndTheBeanstalk"); //loads Jack and the Beanstalk book when clicked
    }
    public void Maui() {
        timeCountScript.waitScreenElements.SetActive(true);
        timeCountScript.blankScreen.SetActive(true);
        SceneManager.LoadScene("TeIkaAMaui"); //loads Te Ika a Maui book when clicked
    }
}
