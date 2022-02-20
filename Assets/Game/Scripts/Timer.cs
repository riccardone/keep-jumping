using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float remainingTime;
    private bool timerIsRunning = false;
    public TMP_Text OSTimer;
    public TMP_Text Description;
    [Header("Timings")]
    [Tooltip("Game time in seconds")]
    public float maxGameTime; //total game time to start countdown

    //Display Time 
    void timeDisplay()
    {
        float minute = Mathf.FloorToInt(remainingTime/60);
        float second = Mathf.Floor(remainingTime%60);
        if((minute==0)&&(second<=5))
        {
            OSTimer.color=new Color32(255,0,0,255);
            Description.color = new Color32(255,0,0,255);
            OSTimer.text=string.Format("{0:00}:{1:00}",minute,second);
        }
        else
        {
            OSTimer.color=new Color32(255,255,255,255);
            Description.color = new Color32(255,255,255,255);
            OSTimer.text=string.Format("{0:00}:{1:00}",minute,second);
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        remainingTime = maxGameTime;
        timerIsRunning = true;        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            //update timer if time not finished
            if(remainingTime > 0)
            {
                timeDisplay();
                remainingTime -= Time.deltaTime;
            }
            //Do whatever is needed when time is out
            else
            {
                //Debug.Log("Time ran out");
                timerIsRunning = false;
                remainingTime = 0;
                SceneManager.LoadScene("LoseScene");
            }  
        }
    }
}
