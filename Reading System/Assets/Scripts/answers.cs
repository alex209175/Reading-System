using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class answers : MonoBehaviour
{
    public int correctAnswers = 1; //sets number of correct answers to 1 by default so that even if the score is 0, it will show that the student has attempted the task.
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
                    correctAnswers++; //increments number of correct answers
                    QandAtext.text = "Ka Pai!";
                    if(PlayerPrefs.GetInt("CurrentBook") == 1) {
                        PlayerPrefs.SetInt("Level1Score", correctAnswers);
                        Debug.Log(PlayerPrefs.GetInt("Level1Score"));
                    }
                    if(PlayerPrefs.GetInt("CurrentBook") == 2) {
                        PlayerPrefs.SetInt("Level2Score", correctAnswers);
                        Debug.Log(PlayerPrefs.GetInt("Level2Score"));
                    }
                    if(PlayerPrefs.GetInt("CurrentBook") == 3) {
                        PlayerPrefs.SetInt("Level3Score", correctAnswers);
                        Debug.Log(PlayerPrefs.GetInt("Level3Score"));
                    }
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
            SceneManager.LoadScene(2); //loads learning tree screen once questions are finished
        }
        QandAtext.text = "Round " + (questionNum + 1).ToString();
        transform.position = new Vector3(x, y, z); //reset position of character
        frozen = 1;
        StartCoroutine(wait());
    }
}
