using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public int Level1Score;
    public int Level2Score;
    public int Level3Score;

    public User(int Level1Score, int Level2Score, int Level3Score) {
        this.Level1Score = Level1Score;
        this.Level2Score = Level2Score;
        this.Level3Score = Level3Score;
    }
}
