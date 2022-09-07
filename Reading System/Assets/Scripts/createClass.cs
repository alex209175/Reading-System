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

    public TextMeshProUGUI classCodeText; //accessing text to display the class code
    string classCode; //stores class code

    DatabaseReference reference; //database reference

    public void NewClass() {
        validCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"; //defining valid characters for class code
        for(int i=0; i<10; i++) {
            randomCharSelector = Random.Range(0, 62); //generates random character
            classCode = classCode + validCodeChars[randomCharSelector]; //adds new character on to class code
        }
        separatedAddresses = emailInputField.text.Split("\n"); //separates email addresses by line breaks
        passwords = new string[separatedAddresses.Length]; //sets number of passwords to be generated to the number of items in the separated addresses array
        for (int i=0; i<separatedAddresses.Length; i++) {
            generatedChars = ""; //clears generated characters for each new password
            for(int a=0; a<5; a++) {
                randomCharSelector = Random.Range(0, 63); //generates random character
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

    }
}
