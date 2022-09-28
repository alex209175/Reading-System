//IDEA: USE USERNAME UID INSTEAD OF USERNAME IN DATABASE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class createClass_new : MonoBehaviour
{
    public TMP_InputField emailInputField; //accesssing the email input field

    public GameObject[] firstElements; //will show and hide the first and second UI elements as necessary
    public GameObject[] secondElements;

    string[] emailAddresses; //stores email addresses in the array
    int numTestedAddresses = 0; //will count the number of processed email addresses

    string validCharacters; //stores all valid characters which can be used to form the class code, as well as user passwords
    int randNumGenerator; //will generate a random number per each character in class code or user password

    string classCode; //stores the generated class code

    bool verifiedCode = false;

    bool hasRunOnce = false; //ensures that code doesn't loop

    bool classCodeExists = false; //will regenerate class code if it exists

    bool hasGeneratedUserAccounts = false; //ensures that the code doesn't loop

    bool addressIsFaulted = false; //detects whether the email address is invalid

    string savedAddress = ""; //saves the email address to show if invalid

    List<string> invalidAddresses = new List<string>(); //stores the invalid email addresses to display

    string[] userPassword; //creates user password

    void Start () {
        validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"; //defining the valid characters
    }

    void Update () {
        if (classCodeExists) {
            verifyCode(); //attempts to reverify the code
        }
        if (!classCodeExists && verifiedCode && !hasGeneratedUserAccounts) {
            emailAddresses = emailInputField.text.Split("\n"); //separates email addresses by line ending
            for (int i=0; i<emailAddresses.Length; i++) {
                string[] userPassword = new string[emailAddresses.Length];
                /*if (i == 0 && userPassword[i] != null) {
                    userPassword[i] = ""; //clears user password before regeneration
                }*/
                userPassword[i] = emailAddresses[i].Substring(0, 5); //adds first 5 letters in email address to start of password
                for (int a=0; a<5; a++) {
                    randNumGenerator = Random.Range(0, validCharacters.Length - 1);
                    userPassword[i] = userPassword[i] + validCharacters[randNumGenerator]; //adds random characters to end of password
                }
                Debug.Log(userPassword[i]);
                hasGeneratedUserAccounts = true; //ensures that the code does not loop
                savedAddress = emailAddresses[i]; //stores the address and displays it if the email address is invalid
                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailAddresses[i], userPassword[i]).ContinueWith (task => {
                    if (task.IsCanceled) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        addressIsFaulted = true; //email address is invalid
                        numTestedAddresses++;
                        return;
                    }

                    numTestedAddresses++;

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("Firebase user created successfully: {0} ({1})", //task successful
                        newUser.DisplayName, newUser.UserId);
                });
            }
            /*for (int i=0; i<secondElements.Length; i++) { //displays second elements and hides first elements
                secondElements[i].SetActive(true);
            }
            for (int i=0; i<firstElements.Length; i++) {
                firstElements[i].SetActive(false);
            }*/
        }
        if (addressIsFaulted) {
            Debug.Log("Email address " + savedAddress + " is invalid");
            invalidAddresses.Add(savedAddress); //saves the invalid email address to the list
            addressIsFaulted = false; //ensures code does not loop
        }

        if (numTestedAddresses == emailInputField.text.Split("\n").Length) {
            if (invalidAddresses.Count > 0) {
                for (int i=0; i<invalidAddresses.Count; i++) {
                    if (i == 0) {
                        emailInputField.text = ""; //clears email input field for first loop
                    }
                    else {
                        emailInputField.text = emailInputField.text + invalidAddresses[i] + "\n"; //adds items to email input field
                    }
                }
            }
        }
    }

    public void createClassWithEmailAddresses() { //Button click will activate
        verifyCode();
    }

    void verifyCode() {
        classCodeExists = false; //ensures that the coroutine does not loop
        for (int i=0; i<10; i++) {
            if (i == 0) {
                classCode = ""; //clears class code to ensure it is not being generated again
            }
            randNumGenerator = Random.Range(0, validCharacters.Length - 1); //will generate a random character in the string per each character in generated string
            classCode = classCode + validCharacters[randNumGenerator]; //adds newly generated character to class code
        }
        Debug.Log(classCode);
        FirebaseDatabase.DefaultInstance.RootReference.Child("classes").GetValueAsync().ContinueWith(t => {
            DataSnapshot snapshot = t.Result;
            if (snapshot.HasChild(classCode)) {                    //will regenerate class code if already exists, otherwise it will move on
                Debug.Log("Class Code Already Exists");
                classCodeExists = true;
                return;
            }
            else {
                Debug.Log("Class Code accepted");           //class code accepted, so it will continue on
                verifiedCode = true;
            }
        });
    }
}