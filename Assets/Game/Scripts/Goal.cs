using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    //On Collision with player
    void OnTriggerEnter(Collider other)
    {
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
