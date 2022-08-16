using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class loadData : MonoBehaviour
{
    DatabaseReference reference; //defining reference to database

    void Start ()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("Level1Score").ValueChanged += HandleUpdateScore;
    }

    public void HandleUpdateScore(object sender, ValueChangedEventArgs args) {
        DataSnapshot snapshot = args.Snapshot;
        //Debug.Log(snapshot.Value);
    }

    public void UpdateScore () {
        Debug.Log("test");
        FirebaseDatabase.DefaultInstance.GetReference("Level1Score").GetValueAsync().ContinueWith(task=> {
            if (task.IsFaulted) {
                Debug.LogError(task);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int value = int.Parse(Convert.ToString(snapshot.Value));
                value++;
                Debug.Log(value);
                reference.Child("Level1Score").SetValueAsync(value);
            }
        });
    }
}
