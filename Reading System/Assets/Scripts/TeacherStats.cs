using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;

public class TeacherStats : MonoBehaviour
{
    public TextMeshProUGUI emailAddressText; //text for email addresses
    public TextMeshProUGUI scoreText; //text for score
    int[] Scores; //array to store scores

    string[] UID; //array to store user IDs

    string currentUID; //current user ID

    public TMP_Dropdown dropdown; //accessing dropdown menu

    string currentLevel = "Level1Score"; //selects level to get user scores from

    // Start is called before the first frame update
    void Start()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference; //retrieve database reference

        UID = PlayerPrefs.GetString("UID").Split("\n");
        Scores = new int[UID.Length]; //sets length of array
        StartCoroutine(getUserData());
        dropdown.onValueChanged.AddListener(delegate { //adds listener to dropdown menu
            dropdownValueChanged(dropdown);
        });
    }

    IEnumerator getUserData() {
        Debug.Log("run");
        for (int i=0; i<UID.Length-1; i++) {
            bool isCompleted = false;
            currentUID = UID[i];
            FirebaseDatabase.DefaultInstance.RootReference.Child("classes").Child(PlayerPrefs.GetString("class")).Child(currentUID).GetValueAsync().ContinueWith(t => {
                if (t.IsCompleted) {
                    DataSnapshot snapshot = t.Result;
                    Scores[i] = Convert.ToInt32(snapshot.Child(currentLevel).Value); //gets score value
                    Debug.Log(Scores[i]);
                }
                isCompleted = true;
            });
            while(!isCompleted) {
                yield return null;
            }
        }
        yield return new WaitForSeconds(5); //will update scores every five seconds
        StartCoroutine(getUserData());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentLevel);
        for (int i=0; i<UID.Length-1; i++) {
            if (i == 0) {
                scoreText.text = "";
                emailAddressText.text = "";
            }
            emailAddressText.text = emailAddressText.text + PlayerPrefs.GetString("emails").Split("\n")[i] + ":\n"; //sets email address and score text
            if (Scores[i] == 0) {
                scoreText.text = scoreText.text + "not attempted\n";
            }
            else {
                scoreText.text = scoreText.text + (Scores[i] - 1).ToString() + "\n"; //it is score - 1 as 1 is added to the score to determine whether the student has attempted the task or not
            }
        }
    }
    //Sets the level and restarts the coroutine
    void dropdownValueChanged(TMP_Dropdown change) {
        currentLevel = "Level" + (change.value + 1) + "Score";
        StartCoroutine(getUserData());
    }
}
