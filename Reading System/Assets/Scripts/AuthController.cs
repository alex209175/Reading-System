using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase;

public class AuthController : MonoBehaviour
{
    bool verified = false; //boolean for if the username and password have been verified
    bool classVerified = false; //boolean for if the class code has been verified
    bool hasFailed = false; //boolean for if the username or password was incorrect
    bool usernameExists = false; //boolean to ensure that the username does not already exist
    bool usernameAllowed = false; //boolean to move to the next scene if username is approved

    public TMP_InputField emailInput;
    public TMP_InputField classInput;
    public TMP_InputField passwordInput; //accessing email, class and password input fields

    public TextMeshProUGUI instructions; //accesing instructional text

    public GameObject[] firstInputs; //accessing the input fields as game objects in order to be able to show and hide them
    public GameObject usernameInputField;
    
    public void Login() {
        //SceneManager.LoadScene(0);
        /*DatabaseReference rootRef = FirebaseDatabase.getInstance().getReference("name/" + name);
        rootRef.addListenerForSingleValueEvent(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot snapshot) {
                if (snapshot.exists()) {
                    Debug.Log("exists");
                } 
                else {
                    Debug.Log("does not exist");
                }
            }
            /*
            @Override
            public void onCancelled(DatabaseError error) {
                // Failed, how to handle?

            }*/

        //});

        if (!verified) {
            FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(( task => {
                Debug.Log("test");
                if(task.IsCanceled) {
                    Debug.Log("cancelled");
                    return;
                }
                if(task.IsFaulted) {
                    hasFailed = true;
                    //instructions.text = "Class Code is incorrect. Please try again, or ask your teacher for help.";
                    Debug.Log("faulted");
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    GetErrorMessage((AuthError)e.ErrorCode);
                    return;
                }
                if(task.IsCompleted) {
                    verified = true;
                }
            }));
        }
        if (classVerified) {
                FirebaseDatabase.DefaultInstance.RootReference.Child("classes").Child(classInput.text).GetValueAsync().ContinueWith(t => {
                    if (t.IsCanceled) {
                        return;
                    }
                    if (t.IsFaulted) {
                        return;
                    }
                    DataSnapshot snapshot = t.Result;
                    /*if (snapshot.HasChild(usernameInput.text)) {
                        usernameExists = true;
                        Debug.Log("Username Already Exists");
                    }
                    else {
                        Debug.Log("Username accepted");
                        usernameAllowed = true;
                    }*/
                });
            }
    }

    /*void LoadMenu() {
        Debug.Log("Loading");
        StartCoroutine(loadLevel());
        //SceneManager.LoadScene(2);
    }*/
    
    void GetErrorMessage(AuthError errorCode) {
        instructions.text = "Email or Password is incorrect. Please try again, or ask your teacher for help.";
        string msg = "";
        msg = errorCode.ToString();
        print(msg);
    }

    void Update() {
        Debug.Log(usernameExists);
        if (hasFailed) {
            instructions.text = "Email or Password is incorrect. Please try again, or ask your teacher for help.";
        }

        if(verified && classVerified) {
            for (int i=0; i<firstInputs.Length; i++) {
                firstInputs[i].SetActive(false);
            }
            
        }

        if (usernameExists) {
            instructions.text = "Username already exists in this class.";
        }
        if (usernameAllowed) {
            //PlayerPrefs.SetString("username", usernameInput.text);
            SceneManager.LoadScene(2);
        }

        if (verified) {
            Debug.Log("verified");
            if (!classVerified) {
                FirebaseDatabase.DefaultInstance.RootReference.Child("classes").GetValueAsync().ContinueWith(t => {
                    if (t.IsCanceled) {
                        return;
                    }
                    if (t.IsFaulted) {
                        return;
                    }
                    DataSnapshot snapshot = t.Result;
                    if (snapshot.HasChild(classInput.text)) {
                        Debug.Log("success!");
                        classVerified = true;
                    }
                    else {
                        Debug.Log("failure");
                        instructions.text = "Class Code is incorrect. Please try again, or ask your teacher for help.";
                        classVerified = false;
                    }
                });
            }
        }
    }
}
