using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LivesCounter : MonoBehaviour {

    public static int lives;

    Text text;

    void Awake ()
    {
        text = GetComponent<Text>();
        lives = 5;
    }
	
	void LateUpdate ()
    {
	    text.text = "Lives: " + lives;
	}
}
