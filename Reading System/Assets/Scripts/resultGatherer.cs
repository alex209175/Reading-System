using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase;

public class resultGatherer : MonoBehaviour
{
    public TextMeshProUGUI studentLevel1Score; //text to display player score

    bool hasAccessedData = false; //displays correct text once data is accessed
    int playerScore; //stores player score

    // Start is called before the first frame update
    void Start()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("classes").Child(PlayerPrefs.GetString("class")).GetValueAsync().ContinueWith(t => {
            DataSnapshot snapshot = t.Result;
            playerScore = Convert.ToInt32(snapshot.Child("alex209").Child("Level1Score").Value);
            hasAccessedData = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAccessedData) {
            studentLevel1Score.text = playerScore.ToString();
        }
    }
}
