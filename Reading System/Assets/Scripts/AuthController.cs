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
    bool verified = false;
    public TMP_InputField emailInput;
    public TMP_InputField classInput;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput; //accessing username, email, class and password input fields
    
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
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(( task => {
            Debug.Log("test");
            if(task.IsCanceled) {
                Debug.Log("cancelled");
                return;
            }
            if(task.IsFaulted) {
                Debug.Log("faulted");
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);
                return;
            }
            if(task.IsCompleted) {
                verified = true;
                /*PlayerPrefs.SetString("email", emailInput.text); //saves email locally
                Debug.Log(PlayerPrefs.GetString("email"));*/
                //StartCoroutine(loadLevel());
            }
        }));
    }

    /*void LoadMenu() {
        Debug.Log("Loading");
        StartCoroutine(loadLevel());
        //SceneManager.LoadScene(2);
    }*/
    
    void GetErrorMessage(AuthError errorCode) {
        string msg = "";
        msg = errorCode.ToString();
        print(msg);
    }

    void Update() {
        if(verified) {
            PlayerPrefs.SetString("username", usernameInput.text);
            PlayerPrefs.SetString("email", emailInput.text);
            PlayerPrefs.SetString("class", classInput.text);
            SceneManager.LoadScene(2);
        }
    }
}
