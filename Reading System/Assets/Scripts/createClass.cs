using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class createClass : MonoBehaviour
{
    public TMP_InputField emailInputField; //accessing input field for entering student email addresses
    public GameObject[] firstElements; //accessing first UI elements
    public GameObject[] secondElements; //accessing second UI elements

    public TextMeshProUGUI userList; //accessing UI list to display user email addresses
    public TextMeshProUGUI passwordList; //accessing UI list to display user passwords

    string[] separatedAddresses; //array with email addresses separated
    string generatedChars; //adds random characters onto the end of password
    string[] passwords; //string for user passwords

    string validCodeChars; //string to store all valid characters for class code
    int randomCharSelector; //integer to select random character from string containing all valid characters

    bool hasAcceptedClassCode = false; //boolean to define when the class code has been accepted
    bool hasRunOnce = false; //boolean to determine if the main part of the code has already been run once

    bool isCreateNewClass = true; //will either create a new class or go to the next screen on load

    public TextMeshProUGUI classCodeText; //accessing text to display the class code
    string classCode; //stores class code

    DatabaseReference reference; //database reference

    public void NewClass() {
        if (isCreateNewClass) {
            validCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"; //defining valid characters for class code
            StartCoroutine(waitForAcceptedClassCode());
        }
        else {
            SceneManager.LoadScene("LearningTree");
        }
    }

    IEnumerator waitForAcceptedClassCode() {
        while (!hasAcceptedClassCode) {
            for(int i=0; i<10; i++) {
            randomCharSelector = Random.Range(0, 62); //generates random character
            classCode = classCode + validCodeChars[randomCharSelector]; //adds new character on to class code
            }
            FirebaseDatabase.DefaultInstance.RootReference.Child("classes").GetValueAsync().ContinueWith(t => {
                if (t.IsCanceled) {
                    return;
                }
                if (t.IsFaulted) {
                    return;
                }
                DataSnapshot snapshot = t.Result;
                if (snapshot.HasChild(classCode)) {                    //will regenerate class code if already exists, otherwise it will move on
                    Debug.Log("Class Code Already Exists");
                }
                else {
                    Debug.Log("Class Code accepted");           //class code accepted, so it will continue on
                    hasAcceptedClassCode = true; 
                }
            });
            yield return new WaitForSeconds(1);
        }
    }

    void Update() {
        /*if (canGenerateClassCode) { //generates class code when required
            for(int i=0; i<10; i++) {
                randomCharSelector = Random.Range(0, 62); //generates random character
                classCode = classCode + validCodeChars[randomCharSelector]; //adds new character on to class code
            }
            FirebaseDatabase.DefaultInstance.RootReference.Child("classes").GetValueAsync().ContinueWith(t => {
                if (t.IsCanceled) {
                    return;
                }
                if (t.IsFaulted) {
                    return;
                }
                DataSnapshot snapshot = t.Result;
                if (snapshot.HasChild(classCode)) {
                    canGenerateClassCode = true;                     //will regenerate class code if already exists, otherwise it will move on
                    Debug.Log("Class Code Already Exists");
                }
                else {
                    Debug.Log("Class Code accepted");           //class code accepted, so it will continue on
                    canGenerateClassCode = false;
                    hasAcceptedClassCode = true; 
                }
            });
        }*/
        if (hasAcceptedClassCode && !hasRunOnce) {
            separatedAddresses = emailInputField.text.Split("\n"); //separates email addresses by line breaks
            passwords = new string[separatedAddresses.Length]; //sets number of passwords to be generated to the number of items in the separated addresses array
            for (int i=0; i<separatedAddresses.Length; i++) {
                generatedChars = ""; //clears generated characters for each new password
                for(int a=0; a<5; a++) {
                    randomCharSelector = Random.Range(0, validCodeChars.Length); //generates random character
                    generatedChars = generatedChars + validCodeChars[randomCharSelector]; //adds new character on to generated password
                }
                passwords[i] = separatedAddresses[i].Substring(0, 5) + generatedChars; //gets first 5 characters of email address to use as start of password and adds random characters at end
                Debug.Log(passwords[i]);
                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(separatedAddresses[i], passwords[i]).ContinueWith (task => { //cycles through email address list and creates user with generated password
                    if (task.IsCanceled) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled."); //task is cancelled
                        return;
                    }
                    if (task.IsFaulted) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception); //task faulted
                        return;
                    }

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("Firebase user created successfully: {0} ({1})", //task successful
                        newUser.DisplayName, newUser.UserId);
                });
            }
            for (int i=0; i<firstElements.Length; i++) {
                firstElements[i].SetActive(false); //hides first UI elements on screen
            }
            for (int i=0; i<secondElements.Length; i++) {
                secondElements[i].SetActive(true); //displays second UI elements on screen
            }

            for (int i=0; i<separatedAddresses.Length; i++) {
                userList.text = userList.text + "\n" + separatedAddresses[i]; //adds email addresses and passwords to UI list
                passwordList.text = passwordList.text + "\n" + passwords[i];
            }
            classCodeText.text = "Class Code: " + classCode; //sets class code text
            hasRunOnce = true; //ensures that the code does not loop

            PlayerPrefs.SetString("emails", emailInputField.text); //saves emails locally
            Debug.Log(PlayerPrefs.GetString("emails"));
            PlayerPrefs.SetString("passwords", passwordList.text); //saves passwords locally
            PlayerPrefs.SetString("class", classCode); //sets up basic account details
            PlayerPrefs.SetString("username", "Teacher");
            PlayerPrefs.SetInt("Level1Score", 0);
            PlayerPrefs.SetInt("Level2Score", 0);
            PlayerPrefs.SetInt("Level3Score", 0);

            isCreateNewClass = false;
            /*User newUser = new User(0, 0, 0);
            string json = JsonUtility.ToJson(newUser); //creates a teacher account in the database

            reference.Child("classes").Child(classCode).Child("Teacher").SetRawJsonValueAsync(json);*/
        }
    }
}
