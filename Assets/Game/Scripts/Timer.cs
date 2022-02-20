using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Game.Scripts.Managers;
using UnityEngine.Events;

namespace Game.Scripts.Managers
{
    public class Timer : MonoBehaviour
    {
        private float remainingTime;
        private bool timerIsRunning = false;
        public TMP_Text OSTimer;
        [Header("Timings")]
        [Tooltip("Game time in seconds")]
        public float maxGameTime; //total game time to start countdown

        //Display Time 
        void timeDisplay()
        {
            float minute = Mathf.FloorToInt(remainingTime/60);
            float second = Mathf.Floor(remainingTime%60);
            OSTimer.text=string.Format("{0:00}:{1:00}",minute,second);
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
                    remainingTime -= Time.deltaTime;
                }
                //Do whatever is needed when time is out
                else
                {
    //              Debug.Log("Time ran out");
                    timerIsRunning = false;
                    remainingTime = 0;
    //              SceneManager.LoadScene("Time Out");
                    EventManager.Broadcast(new TimeOutEvent());
                }  
            }
        }
    }
}
