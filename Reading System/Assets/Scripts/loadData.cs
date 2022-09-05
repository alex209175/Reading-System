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

    public RawImage PeterRabbit;
    public RawImage JackBeanstalk;
    public RawImage Maui;

    public Texture PeterRabbitGold;
    public Texture PeterRabbitBronze;
    public Texture PeterRabbitSilver;

    public Texture JackBeanstalkGold;
    public Texture JackBeanstalkBronze;
    public Texture JackBeanstalkSilver;

    public Texture MauiGold;
    public Texture MauiBronze;
    public Texture MauiSilver;



    int lvl1;
    int lvl2;
    int lvl3;

    private string userID; //unique id for each user

    private string username;
    private string email;


    DatabaseReference reference; //defining reference to database
    
    void Start ()
    {
        lvl1 = PlayerPrefs.GetInt("Level1Score");

        /*if (4 > lvl1 > 0) {
            PeterRabbit.texture = PeterRabbitBronze;
        }
        if (lvl1 == 4) {
            PeterRabbit.texture = PeterRabbitSilver;
        }
        if (lvl1 > 4) {
            PeterRabbit.texture = PeterRabbitGold;
        }*/

        lvl2 = PlayerPrefs.GetInt("Level2Score");

        if (lvl2 > 0) {
            Level2Text.text = ("Jack and the Beanstalk - " + ((lvl2 - 1).ToString()) + "/5");
        }
        else {
            Level2Text.text = ("Jack and the Beanstalk - not attempted");
        }

        lvl3 = PlayerPrefs.GetInt("Level3Score");

        if (lvl3 > 0) {
            Level3Text.text = ("Te Ika a Māui - " + ((lvl3 - 1).ToString()) + "/5");
        }
        else {
            Level3Text.text = ("Te Ika a Māui - not attempted");
        }
        username = PlayerPrefs.GetString("username");
        email = PlayerPrefs.GetString("email");
        userID = SystemInfo.deviceUniqueIdentifier; //creates a unique user id
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        CreateUser();
        //FirebaseDatabase.DefaultInstance.GetReference("Level1Score").ValueChanged += HandleUpdateScore;
        //UpdateScore();
    }

    private void CreateUser() {
        User newUser = new User(PlayerPrefs.GetInt("Level1Score"), PlayerPrefs.GetInt("Level2Score"), PlayerPrefs.GetInt("Level3Score"));
        string json = JsonUtility.ToJson(newUser);
        //Debug.Log(json);
        //Debug.Log(PlayerPrefs.GetString("username"));
        Debug.Log(username);
        reference.Child("users").Child(PlayerPrefs.GetString("class")).Child(username).SetRawJsonValueAsync(json);
        //reference.Child("users").Child("exampleSchool").Child(PlayerPrefs.GetString("username")).SetRawJsonValueAsync(json);
    }

/*
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
        /*FirebaseDatabase.DefaultInstance.GetReference("Level1Score").GetValueAsync().ContinueWith(task => {
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
        FirebaseDatabase.DefaultInstance.GetReference("Level3Score").GetValueAsync().ContinueWith(task => {
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
                reference.Child("Level3Score").SetValueAsync(lvl3);
            }
        });
    }

    public void nextBook() {
        SceneManager.LoadScene("JackAndTheBeanstalk");
    }

    */
/*
    private void writeNewUser() {
        string json = JsonUtility.ToJson("test@test.com");
        
        reference.Child("users").Child("34567").SetRawJsonValueAsync(json);
    }
*/
}

