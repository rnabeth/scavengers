using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //To make the class singleton
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private int level = 3;

	public void Awake ()
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

    public void GameOver()
    {
        enabled = false;
    }
}
