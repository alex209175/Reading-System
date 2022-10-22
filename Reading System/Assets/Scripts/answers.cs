using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class answers : MonoBehaviour
{
    public movement movementScript; //accessing movement script

    public int correctAnswers = 1; //sets number of correct answers to 1 by default so that even if the score is 0, it will show that the student has attempted the task.
    public int frozen = 1; //disable ability for player to move
    //int answeredNum = 0; //variable for the answer that the user selects

    public Image Timer; //accessing the timer

    public GameObject timerObject; //accessing the timer object in order to be able to disable it

    public TextMeshProUGUI answerText1; //accessing the text objects for the answers
    public TextMeshProUGUI answerText2;
    public TextMeshProUGUI answerText3;
    public TextMeshProUGUI answerText4;

    public GameObject[] answeringUIObjects; //accessing the ui objects to be able to disable them

    float time = 0; //variable for amount of time player has left to answer the question

    float x; //defining variables for storing the x, y, and z values
    float y;
    float z;

    int questionNum = 0; //current question that the player is on

    bool freezeNPCs = true;
    
    string[] questions = new string[] { //Array which stores the questions
        "Peter, who was very naughty, ran straight away to Mr. McGregor's garden, and squeezed under the ____?", 
        "First he ate some lettuces and some French beans; and then he ate some _____?",
        "Who is Peter running from?",
        "Where did Mr McGregor check for Peter under?",
        "But Flopsy, Mopsy, and Cotton-tail had bread and milk and ______ for supper."
    };

    string[][] storedAnswers = new string[][]//Array which stores the answers
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
        for (int i = 0; i < answeringUIObjects.Length; i++) {
                answeringUIObjects[i].SetActive(false);
        }
        if (PlayerPrefs.GetInt("CurrentBook") == 2) {
            questions[0] = "What is the name of the cow in the story?"; //changes questions if on Jack and the Beanstalk
            questions[1] = "The man had a large, curly moustache and was wearing a sparkly yellow ______";
            questions[2] = "What showed Jack that the beanstalk had grown overnight?";
            questions[3] = "Fee! Fi! Fo! Fum! I smell the blood of an _________?";
            questions[4] = "Which two items did Jack steal from the giant?";

            storedAnswers[0][0] = "Derrick"; //Displays answers
            storedAnswers[0][1] = "Daisy";
            storedAnswers[0][2] = "Donald";
            storedAnswers[0][3] = "Daesy";

            storedAnswers[1][0] = "Coat";
            storedAnswers[1][1] = "Jacket";
            storedAnswers[1][2] = "Balaclava";
            storedAnswers[1][3] = "Cloak";

            storedAnswers[2][0] = "The smell";
            storedAnswers[2][1] = "A shadow";
            storedAnswers[2][2] = "A stomping noise";
            storedAnswers[2][3] = "The cow";

            storedAnswers[3][0] = "Egyptianman";
            storedAnswers[3][1] = "Englishman";
            storedAnswers[3][2] = "Irishman";
            storedAnswers[3][3] = "Americanman";

            storedAnswers[4][0] = "A harp and a hen";
            storedAnswers[4][1] = "A harp and a fox";
            storedAnswers[4][2] = "A piano and an egg";
            storedAnswers[4][3] = "A piano and a hen";

            correctAnswer[0] = 2; //defining correct answers for quiz
            correctAnswer[1] = 4;
            correctAnswer[2] = 2;
            correctAnswer[3] = 2;
            correctAnswer[4] = 1; 
        }

        if (PlayerPrefs.GetInt("CurrentBook") == 3) {
            questions[0] = "Who was Māui raised by?"; //changes questions if on Te Ika A Maui
            questions[1] = "How many sons did Tangaroa have?";
            questions[2] = "What else is the main character Māui called in this story?";
            questions[3] = "What do you think 'elated' means?";
            questions[4] = "Where would you go in New Zealand if you wanted to stand on Māui's hook?";

            storedAnswers[0][0] = "Taranga"; //Displays answers
            storedAnswers[0][1] = "Tangaroa";
            storedAnswers[0][2] = "Tama-nui-ki-te Rangi";
            storedAnswers[0][3] = "Māui-i-roto";

            storedAnswers[1][0] = "4";
            storedAnswers[1][1] = "None";
            storedAnswers[1][2] = "5";
            storedAnswers[1][3] = "7";

            storedAnswers[2][0] = "Māui-potiki";
            storedAnswers[2][1] = "Māui-i-mua";
            storedAnswers[2][2] = "Māui-i-pai";
            storedAnswers[2][3] = "Māui the Trickster";

            storedAnswers[3][0] = "Furious";
            storedAnswers[3][1] = "Excited";
            storedAnswers[3][2] = "Sad";
            storedAnswers[3][3] = "Very happy";

            storedAnswers[4][0] = "Cape Kidnappers";
            storedAnswers[4][1] = "Stewart Island";
            storedAnswers[4][2] = "Cape Reinga";
            storedAnswers[4][3] = "Whangārei";

            correctAnswer[0] = 3; //defining correct answers for quiz
            correctAnswer[1] = 3;
            correctAnswer[2] = 1;
            correctAnswer[3] = 4;
            correctAnswer[4] = 1; 
        }

        x = transform.position.x; //accessing starting position
        y = transform.position.y;
        z = transform.position.z;
        
        QandAtext.text = "Round " + (questionNum + 1).ToString();
        movementScript.hasReachedAnswer = false;
        timerObject.SetActive(false);
        frozen = 1;
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(movementScript.answeredQuestion);
        Timer.fillAmount = time / 10f;
        //Debug.Log(Timer.fillAmount);
        //timerText.text = time.ToString(); //updating timer text
        if(movementScript.hasReachedAnswer) {
            movementScript.hasReachedAnswer = false;
            usedEnter = true;
            frozen = 1; //freeze the character
            if (movementScript.answeredQuestion != 0) {
                if (movementScript.answeredQuestion == correctAnswer[questionNum]) {
                    correctAnswers++; //increments number of correct answers
                    QandAtext.text = "Ka Pai!";
                    if(PlayerPrefs.GetInt("CurrentBook") == 1 && correctAnswers > PlayerPrefs.GetInt("Level1Score")) {
                        PlayerPrefs.SetInt("Level1Score", correctAnswers);
                        Debug.Log(PlayerPrefs.GetInt("Level1Score"));
                    }
                    if(PlayerPrefs.GetInt("CurrentBook") == 2 && correctAnswers > PlayerPrefs.GetInt("Level2Score")) {
                        PlayerPrefs.SetInt("Level2Score", correctAnswers);
                        Debug.Log(PlayerPrefs.GetInt("Level2Score"));
                    }
                    if(PlayerPrefs.GetInt("CurrentBook") == 3 && correctAnswers > PlayerPrefs.GetInt("Level3Score")) {
                        PlayerPrefs.SetInt("Level3Score", correctAnswers);
                        Debug.Log(PlayerPrefs.GetInt("Level3Score"));
                    }
                    //Renderer.material.mainTexture = correct; //Player gets correct answer, colour changes to green, and correct text appears
                }
                else {
                    QandAtext.text = "The correct answer is " + storedAnswers[questionNum][(correctAnswer[questionNum]) - 1];
                    //Renderer.material.mainTexture = incorrect; //Player gets incorrect answer, colour changes to red, and incorrect text appears
                }
            }
            else {
                QandAtext.text = "The correct answer is " + storedAnswers[questionNum][(correctAnswer[questionNum]) - 1];
            }
            StartCoroutine(nextQuestion());
        }
        if (!movementScript.hasReachedAnswer && time == 10) {
                frozen = 1;
                QandAtext.text = "The correct answer is " + storedAnswers[questionNum][(correctAnswer[questionNum]) - 1];
                StartCoroutine(nextQuestion());
        }
    }
    /*void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.name == "answer1") {
            answeredNum = 1;
        }
        if (collider.gameObject.name == "answer2") {
            answeredNum = 2;
        }
        if (collider.gameObject.name == "answer3") {
            answeredNum = 3;
        }
        if (collider.gameObject.name == "answer4") {
            answeredNum = 4;
        }
    }
    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("answer")) {
            answeredNum = 0;
        }
    }*/
    IEnumerator timer() {
        while (time != 10 && !usedEnter) {
            yield return new WaitForSeconds(0.025f); //increasing the time variable unless it is equal to ten
            time = time + 0.025f;
        }
    }
    IEnumerator wait() {
        yield return new WaitForSeconds(2); //wait for two seconds
        freezeNPCs = false;
        usedEnter = false;
        answerText1.text = storedAnswers[questionNum][0];
        answerText2.text = storedAnswers[questionNum][1];
        answerText3.text = storedAnswers[questionNum][2];
        answerText4.text = storedAnswers[questionNum][3];
        frozen = 0; 
        QandAtext.text = questions[questionNum]; //accessing the question
        timerObject.SetActive(true);
        for (int i = 0; i < answeringUIObjects.Length; i++) {
                answeringUIObjects[i].SetActive(true);
        }
        StartCoroutine(timer());
    }
    IEnumerator nextQuestion() {
        timerObject.SetActive(false);
        time = 0; //resetting the time
        yield return new WaitForSeconds(2); //wait for two seconds
        questionNum++; //incrementing the question number by 1
        if (questionNum + 1 > questions.Length) {
            SceneManager.LoadScene("Learning Tree"); //loads learning tree screen once questions are finished
        }
        QandAtext.text = "Round " + (questionNum + 1).ToString();
        transform.position = new Vector3(x, y, z); //reset position of character
        frozen = 1;
        StartCoroutine(wait());
    }
}
