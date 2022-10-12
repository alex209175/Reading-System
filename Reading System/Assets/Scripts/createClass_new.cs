using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class createClass_new : MonoBehaviour
{
    DatabaseReference reference; //defining reference to database
    
    //First Elements
    public TMP_InputField emailInputField; //accesssing the email input field
    public TextMeshProUGUI mainText; //accessing the main text element

    //Second Elements
    public TextMeshProUGUI classCodeText; //accessing the class code text
    public TextMeshProUGUI userList; //accessing the text to display the user list
    public TextMeshProUGUI passwordList; //accessing the text to display the password list

    public GameObject[] firstElements; //will show and hide the first and second UI elements as necessary
    public GameObject[] secondElements;

    public GameObject Button; //accessing next button

    List<string> emailAddresses = new List<string>(); //stores email addresses
    List<string> invalidAddresses = new List<string>(); //stores all invalid addresses

    List<string> userUID = new List<string>();

    string emailListPlayerPrefs = ""; //stores email, password and uid in form which can be used by player prefs
    string passwordListPlayerPrefs = "";
    string UIDListPlayerPrefs = "";

    int numTestedAddresses = 0; //will count the number of processed email addresses
    int prevNum = 0; //prev num is numTestedAddresses from previous loop in Update()

    string validCharacters; //stores all valid characters which can be used to form the class code, as well as user passwords
    int randNumGenerator; //will generate a random number per each character in class code or user password

    string classCode; //stores the generated class code

    bool verifiedCode = false;

    bool classCodeExists = false; //will regenerate class code if it exists

    bool hasGeneratedUserAccounts = false; //ensures that the code doesn't loop
    bool isInvalid = false;

    bool hasRunAuthDisplayOnce = false; //ensures that auth display code does not loop

    int numUsersInDB = 0; //num users added to database
    bool canLoadNextScene = false;

    List<string> userPassword = new List<string>(); //creates user password
    string generatedChars; //generated chars on end of password

    void Start () {
        validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"; //defining the valid characters
        reference = FirebaseDatabase.DefaultInstance.RootReference; //database reference
    }

    void Update () {
        if (numTestedAddresses > prevNum) {
            if (!isInvalid) {
                emailListPlayerPrefs = emailListPlayerPrefs + emailAddresses[numTestedAddresses - 1] + "\n"; //adds valid email address, password and UID to player prefs string
                UIDListPlayerPrefs = UIDListPlayerPrefs + userUID[numTestedAddresses - 1] + "\n";
                passwordListPlayerPrefs = passwordListPlayerPrefs + userPassword[numTestedAddresses - 1] + "\n";
            }
            prevNum++;
        }

        if (isInvalid) {
            Debug.Log(emailAddresses[numTestedAddresses - 1] + " is invalid"); //adds invalid email addresses to list
            invalidAddresses.Add(emailAddresses[numTestedAddresses - 1]);
            isInvalid = false; //ensures code does not loop
        }

        if (classCodeExists) {
            verifyCode(); //attempts to reverify the code
        }

        if (!classCodeExists && verifiedCode && !hasGeneratedUserAccounts) {
            for (int i=0; i<emailInputField.text.Split("\n").Length; i++) {
                if (invalidAddresses.Count > 0) { //clears previous email addresses
                    emailAddresses.Clear();
                    invalidAddresses.Clear();
                }
                if (!string.IsNullOrWhiteSpace(emailInputField.text.Split("\n")[i])) {
                    emailAddresses.Add(emailInputField.text.Split("\n")[i]); //adds email address that is not blank to the list
                }
            }
            
            for (int i=0; i<emailAddresses.Count; i++) {
                //string[] userPassword = new string[emailAddresses.Count];

                for (int a=0; a<5; a++) {
                    randNumGenerator = Random.Range(0, validCharacters.Length - 1);
                    if (a == 0) {
                        generatedChars = "";
                    }
                    generatedChars = generatedChars + validCharacters[randNumGenerator];
                }
                userPassword.Add(emailAddresses[i].Substring(0, 5) + generatedChars); //adds first 5 letters in email address to start of password
                Debug.Log(userPassword[i]);
                hasGeneratedUserAccounts = true; //ensures that the code does not loop

                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailAddresses[i], userPassword[i]).ContinueWith (task => {
                    if (task.IsCanceled) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        numTestedAddresses++; //email address is invalid
                        isInvalid = true;
                        Debug.Log(numTestedAddresses);
                        return;
                    }

                    numTestedAddresses++;
                    i++;

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("Firebase user created successfully: {0} ({1})", //task successful
                        newUser.DisplayName, newUser.UserId);

                    userUID.Add(newUser.UserId); //stores user ID and email address separated by a space
                });
            }
        }

        if (numUsersInDB == emailAddresses.Count) { //enables the button
            Button.SetActive(true);
        }

        //AUTH DISPLAY
        if (numTestedAddresses == emailAddresses.Count && verifiedCode && !hasRunAuthDisplayOnce) {
            if (invalidAddresses.Count == 0) { //runs if there are no invalid addresses
                Debug.Log("No invalid addresses");

                PlayerPrefs.SetString("emails", emailListPlayerPrefs); //saves email addresses locally
                PlayerPrefs.SetString("UID", UIDListPlayerPrefs); //saves UIDs locally
                Debug.Log(PlayerPrefs.GetString("UID"));
                PlayerPrefs.SetString("passwords", passwordListPlayerPrefs); //saves passwords locally
                PlayerPrefs.SetString("class", classCode); //stores class code

                userList.text = "Users\n" + emailListPlayerPrefs; //Displays email addresses and passwords
                passwordList.text = "Passwords\n" + passwordListPlayerPrefs;
                mainText.text = "Class Code: " + classCode; //sets class code text

                for (int i=0; i<firstElements.Length; i++) {
                    firstElements[i].SetActive(false); //hides the first elements
                }
                for (int i=0; i<secondElements.Length; i++) {
                    secondElements[i].SetActive(true); //shows the second elements
                }

                canLoadNextScene = true; //can now load the next scene on next button click

                CreateUsersInDatabase(); //creates class in the database
            }
            else {
                for (int i=0; i<invalidAddresses.Count; i++) {
                    if (i == 0) {
                        emailInputField.text = ""; //clears text on first run
                    }
                    emailInputField.text = emailInputField.text + invalidAddresses[i] + "\n"; //adds invalid addresses on new line in email input field
                    mainText.text = "Please review the email addresses below";
                }
            }
            hasRunAuthDisplayOnce = true;
        }
    }

    //ADDING USERS TO DATABASE
    public void CreateUsersInDatabase() {
        for (int i=0; i<emailAddresses.Count; i++) {
            User newUser = new User(0, 0, 0); //creates an empty user
            string json = JsonUtility.ToJson(newUser);
            reference.Child("classes").Child(classCode).Child(userUID[i]).SetRawJsonValueAsync(json);
            numUsersInDB++;
        }
    }

    public void createClassWithEmailAddresses() { //Button click will activate
        if (canLoadNextScene) {
            SceneManager.LoadScene(5); //Loads the teacher statistics scene if button click happening for second time
        }
        else {
            if (invalidAddresses.Count > 0) {
                hasGeneratedUserAccounts = false; //ensures that the code can run again
                hasRunAuthDisplayOnce = false;
                numTestedAddresses = 0;
            }
            if (!string.IsNullOrWhiteSpace(emailInputField.text)) { //ensures that the text is not empty
                verifyCode();
            }
            else {
                mainText.text = "Please add at least one student email address to the class";
            }
        }
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