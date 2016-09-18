using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public static void doUpdate () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTimeScale();
        }
	}

   public static bool getPaused()
    {
        return isPaused;
    }

    static void ToggleTimeScale()
    {
        isPaused = !isPaused;
        if (isPaused)
        { 
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }
}
