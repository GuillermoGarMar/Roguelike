using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

	public int wallDamage =1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;
	public bool d;
	public Text foodText;

	private Animator animator;
	private int food;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator>();
		food = GameManager.instance.playerFoodPoints;
		base.Start ();
		foodText.text = "Food: " + food;
	}

	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playersTurn) return;
		int horizontal = 0;
		int vertical = 0;
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal == -1) {
			animator.SetBool ("Izq", true);
		}else if (horizontal == 1 || horizontal ==0) {
			animator.SetBool ("Izq", false);
		}

		if (horizontal != 0)
			vertical = 0;

		if (horizontal != 0 || vertical != 0)
			AttemptMove<Wall> (horizontal, vertical);
	}

	protected override void AttemptMove <T> (int xDir,int yDir){
		food--;
		foodText.text = "Food: " + food;
		base.AttemptMove <T> (xDir, yDir);
		RaycastHit2D hit;
		CHeckIfGameOver ();
		GameManager.instance.playersTurn = false;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}else if(other.tag=="Food"){
			food += pointsPerFood;
			foodText.text = "Food: " + food +" + " + pointsPerFood;
			other.gameObject.SetActive (false);
		}else if(other.tag=="Soda"){
			food += pointsPerSoda;
			foodText.text = "Food: " + food +" + " + pointsPerSoda;
			other.gameObject.SetActive (false);
		}
	}

	protected override void OnCantMove <T> (T component){
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("playerChop");
	}

	private void Restart(){
		SceneManager.LoadScene (0);
	}

	public void LoseFood (int loss){
		animator.SetTrigger ("playerHit");
		food -= loss;
		foodText.text = "Food: " + food +" - " + loss;
		CHeckIfGameOver ();
	}

	private void GameOver (bool b){
		animator.SetBool ("Dead", b);
	}

	private void CHeckIfGameOver(){
		if (food <= 0) {
			d = true;
			GameOver (d);
			GameManager.instance.GameOver ();
		}
	}
}
