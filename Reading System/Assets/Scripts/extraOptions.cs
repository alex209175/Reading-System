using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraOptions : MonoBehaviour
{
    public GameObject menuContainer; //accessing extra menu

    public void openExtraMenu() {
        menuContainer.SetActive(true); //opens extra menu
    }
    public void closeExtraMenu() {
        menuContainer.SetActive(false); //closes extra menu
    }
    public void sendFeedback() {
        Process.Start("mailto:alexseaton209@gmail.com"); //opens default mailing application and begins email to me
    }
}
