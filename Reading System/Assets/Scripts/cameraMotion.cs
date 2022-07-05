using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMotion : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(-37.3f, 9.53f, player.transform.position.z / 2 - 14.96074f); //moves the camera to follow the player
    }
}
