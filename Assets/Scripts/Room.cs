using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{ 
    public GameObject updoor, downdoor, leftdoor, rightdoor;
    public bool uproom, downroom, leftroom, rightroom;

    public int stepToStart;
    public Text stepText;

    public int doorNumber;
    // Start is called before the first frame update
    void Start()
    {
        updoor.SetActive(uproom);
        downdoor.SetActive(downroom);
        leftdoor.SetActive(leftroom);
        rightdoor.SetActive(rightroom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpDateRoom() {
        stepToStart = (int)(Mathf.Abs(transform.position.x/66) + Mathf.Abs(transform.position.y/36));
        stepText.text = stepToStart.ToString();
        if (uproom)
            doorNumber++;
        if (downroom)
            doorNumber++;
        if (leftroom)
            doorNumber++;
        if (rightroom)
            doorNumber++;   
    }
}
