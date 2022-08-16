using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string name;
    public int level;
    
    public User(string name, int level) {
        this.name = name;
        this.level = level;
    }
}
