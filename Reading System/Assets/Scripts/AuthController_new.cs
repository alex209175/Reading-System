using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase;

public class AuthController_new : MonoBehaviour
{
    //Email, password and code inputs
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField codeInput;

    //Instructional text
    public TextMeshProUGUI instructions;

    bool hasFailed = false; //determines whether task has failed
    bool verified = false; //determines whether task has succeeded

    bool classVerified = false; //determines whether the class has been verified
    bool classVerificationFailed = false; //determines whether class verification failed

    string UID; //unique user id accessed from Firebase Auth

    //Button press activates
    public void Login() {
        if (!verified) {
            hasFailed = false; //sets to false by default
            classVerificationFailed = false;
            FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    hasFailed = true;
                    return;
                }
                Firebase.Auth.FirebaseUser user = task.Result;
                if (task.IsCompleted) {
                    verified = true; //task has succeeded
                    UID = user.UserId; //sets UID
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        }
    }

    void Update() {
        if (hasFailed) {
            instructions.text = "Email or password is incorrect. Please try again, or ask your teacher for help.";
        }
        if (verified && !classVerified && !classVerificationFailed) {
            FirebaseDatabase.DefaultInstance.RootReference.Child("classes").Child(codeInput.text).GetValueAsync().ContinueWith(t => {
                if (t.IsFaulted) {
                    Debug.Log("failure");
                    classVerificationFailed = true;
                }
                DataSnapshot snapshot = t.Result;
                if (snapshot.HasChild(UID)) {
                    Debug.Log("success!");
                    classVerified = true;
                }
                else {
                    Debug.Log("failure");
                    classVerificationFailed = true;
                }
            });
        }
        if (classVerificationFailed) {
            instructions.text = "Class Code is incorrect. Please try again, or ask your teacher for help.";
        }

        if (classVerified) {
            PlayerPrefs.SetString("username", UID); //sets UID as username
            PlayerPrefs.SetString("class", codeInput.text); //saves class code
            SceneManager.LoadScene("LearningTree"); //loads the learning tree scene
        }
    }
}
