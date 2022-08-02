using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userSelect : MonoBehaviour
{
    string URL = "https://localhost/readingsystem/userSelect.php";
    public string [] usersData;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        WWW users = new WWW (URL);
        yield return users;
        string usersDataString = users.text;
        usersData = usersDataString.Split(';');
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
