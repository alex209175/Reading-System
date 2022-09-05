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

    private string userID; //unique id for each user

    private string username;
    private string email;


    DatabaseReference reference; //defining reference to database
    
    void Start ()
    {
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

