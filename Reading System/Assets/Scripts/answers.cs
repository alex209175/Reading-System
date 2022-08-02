using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class answers : MonoBehaviour
{
    public int frozen = 1; //disable ability for player to move
    int answeredNum = 0; //variable for the answer that the user selects

    public TextMeshProUGUI timerText; //accessing the timer object's text

    public GameObject answerText1; //accessing the text objects for the answers
    public GameObject answerText2;
    public GameObject answerText3;
    public GameObject answerText4;

    public GameObject timerTextObject; //accessing the timer object
    int time = 10; //variable for amount of time player has left to answer the question

    float x; //defining variables for storing the x, y, and z values
    float y;
    float z;

    int questionNum = 0; //current question that the player is on

    bool freezeNPCs = true;

    string[] questions = { //Array which stores the questions
        "Peter, who was very naughty, ran straight away to Mr. McGregor's garden, and squeezed under the ____?", 
        "First he ate some lettuces and some French beans; and then he ate some _____?",
        "Who is Peter running from?",
        "Where did Mr McGregor check for Peter under?",
        "But Flopsy, Mopsy, and Cotton-tail had bread and milk and ______ for supper."
    };

    string[][] storedAnswers = //Arrar which stores the answers
    { 
        new string[] {
            "Ground",
            "Gate",
            "Wall",
            "Fence"
        },
        new string[] {
            "Potatoes",
            "Peas",
            "Radishes",
            "Rhubarb"
        },
        new string[] {
            "Mrs Macandrew",
            "Mr Gilbert",
            "Mr McGilbert",
            "Mr McGregor"
        },
        new string[] {
            "A Boot",
            "A Flower Pot",
            "A Can",
            "A Sieve"
        },
        new string[] {
            "Blackberries",
            "Blueberries",
            "Grapes",
            "Red Currents"
        }
    };

    int[] correctAnswer = {2, 3, 4, 2, 1}; //Array which stores the correct answer

    public TextMeshProUGUI QandAtext; //accessing the text that displays the questions and storedAnswers

    public Texture correct; //accessing the green texture for the answer selection object
    public Texture incorrect; //accessing the red texture for the answer selection object
    public Texture neutral; //accessing the blue text for the answer selection object
    Renderer Renderer; //accessing the renderer for the answer selection object
    bool usedEnter = false;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x; //accessing starting position
        y = transform.position.y;
        z = transform.position.z;
        
        QandAtext.text = "Round " + (questionNum + 1).ToString();
        timerTextObject.SetActive(false);
        frozen = 1;
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(answeredNum);
        timerText.text = time.ToString(); //updating timer text
        if(time == 0 || Input.GetKeyDown("space") && !usedEnter) {
            usedEnter = true;
            frozen = 1; //freeze the character
            if (answeredNum != 0) {
                if (answeredNum == correctAnswer[questionNum]) {
                    QandAtext.text = "Ka Pai!";
                    Renderer.material.mainTexture = correct; //Player gets correct answer, colour changes to green, and correct text appears
                }
                else {
                    QandAtext.text = "The correct answer is " + storedAnswers[questionNum][(correctAnswer[questionNum]) - 1];
                    Renderer.material.mainTexture = incorrect; //Player gets incorrect answer, colour changes to red, and incorrect text appears
                }
            }
            else {
                QandAtext.text = "The correct answer is Gate";
            }
            StartCoroutine(nextQuestion());
        }
    }
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.name == "answer1") {
            answeredNum = 1;
            Renderer = collider.gameObject.GetComponent<Renderer>();
        }
        if (collider.gameObject.name == "answer2") {
            answeredNum = 2;
            Renderer = collider.gameObject.GetComponent<Renderer>();
        }
        if (collider.gameObject.name == "answer3") {
            answeredNum = 3;
            Renderer = collider.gameObject.GetComponent<Renderer>();
        }
        if (collider.gameObject.name == "answer4") {
            answeredNum = 4;
            Renderer = collider.gameObject.GetComponent<Renderer>();
        }
    }
    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("answer")) {
            answeredNum = 0;
        }
    }
    IEnumerator timer() {
        while (time != 0 && !usedEnter) {
            yield return new WaitForSeconds(1); //decreasing the time variable unless it is equal to zero
            time--;
        }
    }
    IEnumerator wait() {
        yield return new WaitForSeconds(2); //wait for two seconds
        freezeNPCs = false;
        usedEnter = false;
        answerText1.GetComponent<TextMeshPro>().text = storedAnswers[questionNum][0];
        answerText2.GetComponent<TextMeshPro>().text = storedAnswers[questionNum][1];
        answerText3.GetComponent<TextMeshPro>().text = storedAnswers[questionNum][2];
        answerText4.GetComponent<TextMeshPro>().text = storedAnswers[questionNum][3];
        frozen = 0; 
        QandAtext.text = questions[questionNum]; //accessing the question
        timerTextObject.SetActive(true);
        StartCoroutine(timer());
    }
    IEnumerator nextQuestion() {
        timerTextObject.SetActive(false);
        time = 10; //resetting the time
        yield return new WaitForSeconds(2); //wait for two seconds
        questionNum++; //incrementing the question number by 1
        Renderer.material.mainTexture = neutral;
        if (questionNum + 1 > questions.Length) {
            SceneManager.LoadScene(0); //loads menu screen once questions are finished
        }
        QandAtext.text = "Round " + (questionNum + 1).ToString();
        transform.position = new Vector3(x, y, z); //reset position of character
        frozen = 1;
        StartCoroutine(wait());
    }
}
