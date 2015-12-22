using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public float turnDelay = .1f;
    public static GameManager instance = null; //To make the class singleton
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;

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

    private void InitGame()
    {
        enemies.Clear(); //The GameManager will not be reset when the level starts, so we need to clear out any enemies from the last level
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playersTurn || enemiesMoving)
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
