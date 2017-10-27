using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float turnDelay = .1f;
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private GameObject levelImage;
	private int level=1;
	private List<Enemy> enemies;
	private bool enemiesMoving;

			

	// Use this for initialization
	void Awake (){
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static public void CallbackInitialization()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		instance.level++;
		instance.InitGame();
	}

	void InitGame () {

		enemies.Clear ();
		boardScript.SetupScene (level);

	}
		
	public void GameOver(){
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playersTurn || enemiesMoving) {
			return;
		}
		StartCoroutine (MoveEnemies ());
		
	}

	public void AddEnemyToList(Enemy script){
		enemies.Add (script);
	}

	IEnumerator MoveEnemies(){
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (turnDelay);
		}
		playersTurn = true;
		enemiesMoving = false;
	}
}
