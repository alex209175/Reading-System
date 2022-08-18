using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using TMPro;

public class loadData : MonoBehaviour
{
    public TextMeshProUGUI Level1Text;
    public TextMeshProUGUI Level2Text;
    public TextMeshProUGUI Level3Text;

    int lvl1;
    int lvl2;

    DatabaseReference reference; //defining reference to database
    
    void Start ()
    {
        lvl1 = PlayerPrefs.GetInt("Level1Score");

        if (lvl1 > 0) {
            Level1Text.text = ("Peter Rabbit - " + ((lvl1 - 1).ToString()) + "/5"); //if the task is attempted, it will display the score
        }
        else {
            Level1Text.text = ("Peter Rabbit - not attempted"); //if not, shows not attempted
        }

        lvl2 = PlayerPrefs.GetInt("Level2Score");

        if (lvl2 > 0) {
            Level2Text.text = ("Jack and the Beanstalk - " + ((lvl2 - 1).ToString()) + "/5");
        }
        else {
            Level2Text.text = ("Jack and the Beanstalk - not attempted");
        }

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("Level1Score").ValueChanged += HandleUpdateScore;
        UpdateScore();
    }

    //Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.GetAuth(app);

    public void HandleUpdateScore(object sender, ValueChangedEventArgs args) {
        DataSnapshot snapshot = args.Snapshot;
        //Debug.Log(snapshot.Value);
    }

    public void UpdateScore () {
        //writeNewUser();
        Debug.Log("test");
        //Debug.Log(auth);
        /*User user = new User();
        user.UserName = "";
        string json = JsonUtility.ToJson(user);*/

        /*auth.CreateUserWithEmailAndPasswordAsync("alexseaton209@gmail.com", "stupidpassword").ContinueWith(task =>
        //reference.Child("User").Child(user.UserName).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) {
                Debug.LogError(task);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("success");
                /*DataSnapshot snapshot = task.Result;
                int value = int.Parse(Convert.ToString(snapshot.Value));
                value++;
                Debug.Log(value);  
                reference.Child("Level1Score").SetValueAsync(value);*/
            //}
            //Firebase.Auth.FirebaseUser newUser = task.Result;
            //Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            //newUser.DisplayName, newUser.UserId);
        //}); 
        FirebaseDatabase.DefaultInstance.GetReference("Level1Score").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //int value = int.Parse(Convert.ToString(snapshot.Value));
                //value = localValue;
                //Debug.Log(PlayerPrefs.GetInt("Level1Score"));
                //Debug.Log(value);
                reference.Child("Level1Score").SetValueAsync(lvl1);
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("Level2Score").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //int value = int.Parse(Convert.ToString(snapshot.Value));
                //value = localValue;
                //Debug.Log(PlayerPrefs.GetInt("Level1Score"));
                //Debug.Log(value);
                reference.Child("Level2Score").SetValueAsync(lvl2);
            }
        });
    }

    public void nextBook() {
        SceneManager.LoadScene("JackAndTheBeanstalk");
    }
/*
    private void writeNewUser() {
        string json = JsonUtility.ToJson("test@test.com");
        
        reference.Child("users").Child("34567").SetRawJsonValueAsync(json);
    }
*/
}

