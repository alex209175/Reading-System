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
    public Image progress; //accessing progress bar object

    // Start is called before the first frame update
    void Start()
    {
        pageCount = pageParent.transform.childCount; //counts number of pages
        Button btn = button.GetComponent<Button>(); //accessing button component
        Button prevBtn = prevButton.GetComponent<Button>(); //accessing button component
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
        //Debug.Log(pageNumber); //detecting clicks and turning the page
        prev.SetActive(false);
        next.SetActive(true);
    }
    void PrevPg() {
        pageNumber--; //subtracts 1 from page number
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
}
