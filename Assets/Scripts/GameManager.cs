using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //To make the class singleton
    public BoardManager boardScript;

    private int level = 4;

	void Awake ()
	{
	    if (instance == null)
	        instance = this;
        else if (instance != this)
            Destroy(gameObject); //So we don't accidentaly end up with 2 instances of GameManager

	    DontDestroyOnLoad(gameObject); //To keep score between scenes
	    boardScript = GetComponent<BoardManager>();
	    InitGame();
	}

    private void InitGame()
    {
        boardScript.SetupScene(level);
    }

    // Update is called once per frame
    /*
    void Update ()
    {
	
	}
    */
}
