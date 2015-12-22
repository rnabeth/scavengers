using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null; //To make the class singleton
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

	public void Awake ()
	{
	    if (instance == null)
	        instance = this;
        else if (instance != this)
            Destroy(gameObject); //So we don't accidentaly end up with 2 instances of GameManager

	    DontDestroyOnLoad(gameObject); //To keep score between scenes
        enemies = new List<Enemy>();
	    boardScript = GetComponent<BoardManager>();
	    InitGame();
	}

    //Called everytime a scene is loaded
    private void OnLevelWasLoaded(int index)
    {
        level++;

        InitGame();
    }
    
    private void InitGame()
    {
        doingSetup = true; //So the player won't be able to move while the title card is up

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay); //Will wait for 2 seconds once we have displayed our title card before turning if off

        enemies.Clear(); //The GameManager will not be reset when the level starts, so we need to clear out any enemies from the last level
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    private IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach (Enemy e in enemies)
        {
            e.MoveEnemy();
            yield return new WaitForSeconds(e.moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
