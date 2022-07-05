using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class nextPage : MonoBehaviour
{
    public Button button; //next button object
    public Button prevButton; //previous button object
    int pageNumber; //page number count
    public GameObject pageParent; //accessing parent object of pages
    int pageCount; //detecting how many pages in book
    GameObject prev;
    GameObject next;
    public AudioClip[] audioTexts; //accessing the audio files
    public AudioSource audioSource;
    public Image progress; //accessing progress bar object
    
    public Texture volOn; //accessing the volume on texture
    public Texture volOff; //accessing the volume off texture
    public RawImage volumeImage; //accessing the RawImage which displays the volume textures
    public Button volumeButton; //Button for turning the volume on and off
    bool volIsOn = true; //boolean to detect whether the volume is on or not

    // Start is called before the first frame update
    void Start()
    {
        pageCount = pageParent.transform.childCount; //counts number of pages
        Button btn = button.GetComponent<Button>(); //accessing button component
        Button prevBtn = prevButton.GetComponent<Button>(); //accessing button component

        Button volButton = volumeButton.GetComponent<Button>(); //accessing the button component of the volume button
        volButton.onClick.AddListener(changeVol);
        prevBtn.onClick.AddListener(PrevPg);
        btn.onClick.AddListener(NextPg);
        pageNumber = 1;
        prev = pageParent.transform.GetChild(pageNumber - 1).gameObject;
        next = pageParent.transform.GetChild(pageNumber).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(pageNumber < 1){
            pageNumber++;
        }
        if(pageNumber > pageCount - 1){
            pageNumber--;
            SceneManager.LoadScene(1); //Loads game scene
        }

        prev = pageParent.transform.GetChild(pageNumber - 1).gameObject;
        next = pageParent.transform.GetChild(pageNumber).gameObject;
        
        progress.fillAmount = (float)pageNumber / (float)pageCount; //displaying page progress
        //Debug.Log(pageNumber);
    }
    
    void NextPg() {
        pageNumber++;
        if(pageNumber < pageCount - 1 && volIsOn) { //ensures that the audio is not trying to be accessed on the last screen, where there is no audio available
            audioSource.clip = audioTexts[pageNumber - 2]; //selects the audio clip, and plays the audio when the next page button is clicked
            audioSource.Play();
        }
        else {
            audioSource.Stop();
        }
        //Debug.Log(pageNumber); //detecting clicks and turning the page
        prev.SetActive(false);
        next.SetActive(true);
    }
    void PrevPg() {
        pageNumber--; //subtracts 1 from page number
        if(pageNumber > 1) { //ensures that the audio is not trying to be accessed on the first screen, where there is no audio available
            audioSource.clip = audioTexts[pageNumber - 2]; //selects the audio clip, and plays the audio when the next page button is clicked
            audioSource.Play();
        }
        else {
            audioSource.Stop();
        }
        if(pageNumber < 1){
            pageNumber++;
        }
        if(pageNumber > pageCount){
            pageNumber--;
        }
        prev = pageParent.transform.GetChild(pageNumber - 1).gameObject;
        next = pageParent.transform.GetChild(pageNumber).gameObject; //copied from above, as sometimes does not update in time
        prev.SetActive(true);
        next.SetActive(false); //enabling and disabling respective pages
    }
    void changeVol() {
        if(volIsOn) {
            volumeImage.texture = volOff; //displays volume off texture if button pressed and volume is on
            volIsOn = false;
            audioSource.Stop();
        }
        else {
            volumeImage.texture = volOn; //displays volume on texture if button pressed and volume is off
            volIsOn = true;
            audioSource.clip = audioTexts[pageNumber - 2]; //selects the audio clip, and plays the audio when the next page button is clicked
            audioSource.Play();
        }
    }
}
