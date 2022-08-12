using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Auth;

public class AuthController : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput; //accessing username and password input fields
    
    public void Login() {
        SceneManager.LoadScene(0);
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(( task => {
            if(task.IsCanceled) {
                return;
            }
            if(task.IsFaulted) {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);
                return;
            }
            if(task.IsCompleted) {
                LoadMenu();
            }
        }));
    }

    void LoadMenu() {
        Debug.Log("Loading");
        SceneManager.LoadScene(2);
    }
    
    void GetErrorMessage(AuthError errorCode) {
        string msg = "";
        msg = errorCode.ToString();
        print(msg);
    }
}
