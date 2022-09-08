using System.Collections;
using System.Collections.Generic;
using System.Net.Mail; //library used to verify email addresses
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

    public TextMeshProUGUI initialCreateText; //accessing UI text on first screen

    string[] separatedAddresses; //array with email addresses separated
    string generatedChars; //adds random characters onto the end of password
    string[] passwords; //string for user passwords

    List<string> invalidAddresses = new List<string>(); //string to store any email addresses which are invalid
    List<string> validAddresses = new List<string>(); //string to store any email addresses which are valid
    List<string> validPasswords = new List<string>(); //string to store passwords

    string validAddressesPlayerPrefs; //string to store valid email addresses in a form accepted by Unity Player Prefs
    string validPasswordsPlayerPrefs; //string to store valid passwords addresses in a form accepted by Unity Player Prefs

    string validCodeChars; //string to store all valid characters for class code
    int randomCharSelector; //integer to select random character from string containing all valid characters

    bool hasAcceptedClassCode = false; //boolean to define when the class code has been accepted
    bool hasRunOnce = false; //boolean to determine if the main part of the code has already been run once

    bool isCreateNewClass = true; //will either create a new class or go to the next screen on load

    bool valid = true; //boolean for if email address is valid
    bool hasInvalidEmailAddresses = false; //boolean for determining if there are invalid email addresses

    public TextMeshProUGUI classCodeText; //accessing text to display the class code
    string classCode; //stores class code

    DatabaseReference reference; //database reference

    public void NewClass() {
        if (isCreateNewClass) {
            validCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"; //defining valid characters for class code
            StartCoroutine(waitForAcceptedClassCode());
        }
        if (!isCreateNewClass && !hasInvalidEmailAddresses) { //will load the learning tree scene
            SceneManager.LoadScene("LearningTree");
        }
        if (!isCreateNewClass && hasInvalidEmailAddresses) { //will reload system for inputting email addresses
            hasInvalidEmailAddresses = false; 
            hasRunOnce = false;
            hasAcceptedClassCode = false; //changes these variables to ensure the program will work correctly on next button press
            isCreateNewClass = true;

            initialCreateText.text = "Please edit the email address(es) below to ensure they are valid";
            Debug.Log("Reload");
            for (int i=0; i<invalidAddresses.Count; i++) {
                if (i == 0) {
                    emailInputField.text = invalidAddresses[i]; //will clear the input field and set it to the first value in the list on first run
                }
                else {
                    emailInputField.text = emailInputField.text + "\n" + invalidAddresses[i]; //adds next value in the list to the text
                }
            }
            for (int i=0; i<firstElements.Length; i++) {
                firstElements[i].SetActive(true); //displays first UI elements on screen
            }
            for (int i=0; i<secondElements.Length; i++) {
                secondElements[i].SetActive(false); //hides second UI elements on screen
            }
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
        if (hasAcceptedClassCode && !hasRunOnce) {
            separatedAddresses = emailInputField.text.Split("\n"); //separates email addresses by line breaks
            passwords = new string[separatedAddresses.Length]; //sets number of passwords to be generated to the number of items in the separated addresses array
            for (int i=0; i<separatedAddresses.Length; i++) {
                try
                { 
                    var emailAddress = new MailAddress(separatedAddresses[i]); //checks if each email address is valid
                    valid = true;
                }
                catch
                {
                    valid = false;
                }
                if (valid) {
                    Debug.Log(separatedAddresses[i] + " is valid");
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

                    validPasswords.Add(passwords[i]); //adds password to list
                    validAddresses.Add(separatedAddresses[i]); //adds valid email address to list
                    hasRunOnce = true; //ensures that the code does not loop
                }
                else {
                    hasInvalidEmailAddresses = true;
                    Debug.Log(separatedAddresses[i] + " is not valid");
                    invalidAddresses.Add(separatedAddresses[i]);
                    Debug.Log("Invalid Email Address");
                    classCodeText.text = "One or more email addresses are invalid or already taken";
                    hasRunOnce = true; //ensures that the code does not loop
                    DisplayInvalidEmails(); //displays the invalid email addresses
                }
            }

            for (int i=0; i<validAddresses.Count; i++) {
                validAddressesPlayerPrefs = validAddressesPlayerPrefs + "\n" + validAddresses[i]; //stores list in a string
                validPasswordsPlayerPrefs = validPasswordsPlayerPrefs + "\n" + validPasswords[i];
            }

            if (!hasInvalidEmailAddresses) {
                classCodeText.text = "Class Code: " + classCode; //sets class code text
                for (int i=0; i<validAddresses.Count; i++) {
                    userList.text = userList.text + "\n" + validAddresses[i]; //adds email addresses to UI list if no email addresses are invalid
                }
                passwordList.text = "Passwords\n" + validPasswordsPlayerPrefs; //creates UI password list
            }

            PlayerPrefs.SetString("emails", validAddressesPlayerPrefs); //saves emails locally
            PlayerPrefs.SetString("passwords", validPasswordsPlayerPrefs); //saves passwords locally
            PlayerPrefs.SetString("class", classCode); //sets up basic account details
            PlayerPrefs.SetString("username", "Teacher");
            PlayerPrefs.SetInt("Level1Score", 0);
            PlayerPrefs.SetInt("Level2Score", 0);
            PlayerPrefs.SetInt("Level3Score", 0);

            for (int i=0; i<firstElements.Length; i++) {
                firstElements[i].SetActive(false); //hides first UI elements on screen
            }
            for (int i=0; i<secondElements.Length; i++) {
                secondElements[i].SetActive(true); //displays second UI elements on screen
            }

            isCreateNewClass = false;

            Debug.Log(PlayerPrefs.GetString("emails"));
            Debug.Log(PlayerPrefs.GetString("passwords"));
        }
    }
    
    void DisplayInvalidEmails() {
        for(int i=0; i<invalidAddresses.Count; i++) {
            if (i == 0) {
                userList.text = "Users\n" + invalidAddresses[i];
                passwordList.text = "";
            }
            else {
                userList.text = userList.text + "\n" + invalidAddresses[i];
                Debug.Log(invalidAddresses[i]);
            }
        }
    }
}
