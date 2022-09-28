using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bookManager : MonoBehaviour
{
    public void PeterRabbit() {
        SceneManager.LoadScene("PeterRabbit"); //loads Peter Rabbit book when clicked
    }
    public void JackBeanstalk() {
        if (PlayerPrefs.GetInt("Level1Score") > 1) { //ensures that the player has at least answered one question in the first story correctly
            SceneManager.LoadScene("JackAndTheBeanstalk"); //loads Jack and the Beanstalk book when clicked
        }
    }
    public void Maui() {
        if (PlayerPrefs.GetInt("Level1Score") > 1) { //ensures that the player has at least answered one question in the first story correctly
            SceneManager.LoadScene("TeIkaAMaui"); //loads Te Ika a Maui book when clicked
        }
    }
}
