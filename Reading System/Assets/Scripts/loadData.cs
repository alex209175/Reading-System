using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using TMPro;

public class loadData : MonoBehaviour
{
    public TextMeshProUGUI Level1Text;
    public TextMeshProUGUI Level2Text;
    public TextMeshProUGUI Level3Text;

    public Button PeterRabbit;
    public Button JackBeanstalk;
    public Button Maui;

    public Sprite PeterRabbitGold;
    public Sprite PeterRabbitBronze;
    public Sprite PeterRabbitSilver;

    public Sprite JackBeanstalkGold;
    public Sprite JackBeanstalkBronze;
    public Sprite JackBeanstalkSilver;

    public Sprite MauiGold;
    public Sprite MauiBronze;
    public Sprite MauiSilver;

    //public Image image;

    int lvl1;
    int lvl2;
    int lvl3;

    private string username;
    private string email;

    bool isCompleted = false; //if true, task is successful

    //Scores accessed from database
    int Level1ScoreDB;
    int Level2ScoreDB;
    int Level3ScoreDB;


    DatabaseReference reference; //defining reference to database
    
    void Start ()
    {
        username = PlayerPrefs.GetString("username");
        email = PlayerPrefs.GetString("email");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(getUserData());
    }

    private void SetUserData() {
        //If the score in the database is greater, will set local score to that value
        //Means that if the user performed worse, it would not set their score to the worse value, or if user is logged in on a new device
        if (Level1ScoreDB > PlayerPrefs.GetInt("Level1Score")) {
            PlayerPrefs.SetInt("Level1Score", Level1ScoreDB);
        }
        if (Level2ScoreDB > PlayerPrefs.GetInt("Level2Score")) {
            PlayerPrefs.SetInt("Level2Score", Level2ScoreDB);
        }
        if (Level3ScoreDB > PlayerPrefs.GetInt("Level3Score")) {
            PlayerPrefs.SetInt("Level3Score", Level3ScoreDB);
        }

        //Sets colours of icons accordingly
        lvl1 = PlayerPrefs.GetInt("Level1Score");

        if (4 > lvl1 && lvl1 > 0) {
            PeterRabbit.GetComponent<Image>().sprite = PeterRabbitBronze;
        }
        if (lvl1 == 4) {
            PeterRabbit.GetComponent<Image>().sprite = PeterRabbitSilver;
        }
        if (lvl1 > 4) {
            PeterRabbit.GetComponent<Image>().sprite = PeterRabbitGold;
        }

        lvl2 = PlayerPrefs.GetInt("Level2Score");

        if (4 > lvl2 && lvl2 > 0) {
            JackBeanstalk.GetComponent<Image>().sprite = JackBeanstalkBronze;
        }
        if (lvl2 == 4) {
            JackBeanstalk.GetComponent<Image>().sprite = JackBeanstalkSilver;
        }
        if (lvl2 > 4) {
            JackBeanstalk.GetComponent<Image>().sprite = JackBeanstalkGold;
        }
        if (lvl2 < 1 && lvl1 < 1) {
            JackBeanstalk.GetComponent<Image>().color = new Color32(0, 0, 0, 150); // Decreases opacity
        }

        lvl3 = PlayerPrefs.GetInt("Level3Score");

        if (4 > lvl3 && lvl3 > 0) {
            Maui.GetComponent<Image>().sprite = MauiBronze;
        }
        if (lvl3 == 4) {
            Maui.GetComponent<Image>().sprite = MauiSilver;
        }
        if (lvl3 > 4) {
            Maui.GetComponent<Image>().sprite = MauiGold;
        }
        if (lvl3 < 1 && lvl1 < 1) {
            Maui.GetComponent<Image>().color = new Color32(0, 0, 0, 150); // Decreases opacity
        }

        User newUser = new User(PlayerPrefs.GetInt("Level1Score"), PlayerPrefs.GetInt("Level2Score"), PlayerPrefs.GetInt("Level3Score"));
        string json = JsonUtility.ToJson(newUser);

        Debug.Log(username);
        reference.Child("classes").Child(PlayerPrefs.GetString("class")).Child(username).SetRawJsonValueAsync(json);
    }

    IEnumerator getUserData() {
        FirebaseDatabase.DefaultInstance.RootReference.Child("classes").Child(PlayerPrefs.GetString("class")).Child(username).GetValueAsync().ContinueWith(t => {
            if (t.IsCompleted) {
                DataSnapshot snapshot = t.Result;
                Level1ScoreDB = Convert.ToInt32(snapshot.Child("Level1Score").Value); //gets scores from database
                Level2ScoreDB = Convert.ToInt32(snapshot.Child("Level2Score").Value);
                Level3ScoreDB = Convert.ToInt32(snapshot.Child("Level3Score").Value);
            }
            isCompleted = true;
        });
        while(!isCompleted) {
            yield return null;
        }
        if (isCompleted) {
            SetUserData();
        }
    }
}

