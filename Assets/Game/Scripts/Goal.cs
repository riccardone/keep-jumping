using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Managers
{
    public class Goal : MonoBehaviour
    {
        //On Collision with player
        void OnTriggerEnter(Collider other)
        {
            EventManager.Broadcast(new GameOverEvent()); //On trigger of the goal, Broadcast gameover event
            /*
            if (other.gameObject.layer == 1) //Double check player layer
            {
                switch((SceneManager.GetActiveScene()).name)
                {
                    case "":
                        SceneManager.LoadScene("");
                        break;
                    case "":
                        SceneManager.LoadScene("");
                        break;
                    case "":
                        SceneManager.LoadScene("");
                        break;
                    //Question: Do I need to break?
                }
            }
            */
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}