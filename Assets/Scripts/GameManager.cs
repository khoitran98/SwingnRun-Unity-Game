using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static event GameDelegate Dead;
    public static GameManager Instance;
    public GameObject StartMenu;
    public GameObject GameOver;
    public GameObject CountDownPage;
    public Text Score;
    private int xPos; // variable to calculate the score
    enum PageState {
        None,
        Start,
        GameOver,
        Countdown
    }
    int scoreGain = 0; // score gained from travelled distance
    int scoreLost = 0; // score lost from mace collision
    int score = 0; // overall score
    int scoreBonus = 0; // score gained from coin
    void Awake() 
    {
        Instance = this;
        xPos = (int)transform.position.x; // get the starting position of player
    }
    void OnEnable() 
    {
        SetPageState(PageState.Start);
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        Controller.OnPlayerDied += OnPlayerDied;
        Saw.OnPlayerDied += OnPlayerDied;
        Water.OnPlayerDied += OnPlayerDied;
        Mace.OnPlayerLoseScore += OnPlayerLoseScore;
        Coin.OnPlayerScored += OnPlayerScored;
    }
    void OnDisable() 
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        Controller.OnPlayerDied -= OnPlayerDied;
        Coin.OnPlayerScored -= OnPlayerScored;
        Water.OnPlayerDied -= OnPlayerDied;
        Saw.OnPlayerDied -= OnPlayerDied;
        Mace.OnPlayerLoseScore -= OnPlayerLoseScore;
    }
    void OnCountdownFinished() 
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
    }
    void OnPlayerDied() 
    {
        Dead(); // event sent to rope system
        int savedScore = PlayerPrefs.GetInt("HighScore"); // getting saved high score from a special saving place
        if (score > savedScore) {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
        Time.timeScale = 0; // temporarily pause game
    }
    void OnPlayerScored() 
    {
        scoreBonus += 10;
    }
    void OnPlayerLoseScore() 
    {
        scoreLost -= 30;
    }
    void SetPageState (PageState state) {
        switch (state) {
            case PageState.None:
                StartMenu.SetActive(false);
                GameOver.SetActive(false);
                CountDownPage.SetActive(false);
                break;
            case PageState.Start:
                StartMenu.SetActive(true);
                GameOver.SetActive(false);
                CountDownPage.SetActive(false);
                break;
            case PageState.GameOver:
                StartMenu.SetActive(false);
                GameOver.SetActive(true);
                CountDownPage.SetActive(false);
                break;
            case PageState.Countdown:
                StartMenu.SetActive(false);
                GameOver.SetActive(false);
                CountDownPage.SetActive(true);
                break;
        }
    }
    public void ConfirmGameOver() 
    {
        //when replay
        OnGameOverConfirmed(); // event sent to tap controller
        score = scoreGain = scoreLost = scoreBonus = 0; // reset the score
        SetPageState(PageState.Start);
    }
    public void StartGame() 
    { // when play
        SetPageState(PageState.Countdown);
    }
    void Update()
    {
        scoreGain = (int)transform.position.x - xPos; // using travelled distance as score
        score = scoreGain + scoreLost + scoreBonus;
        if (score < 0)
        {
            scoreGain = scoreLost = scoreBonus = 0;
            xPos = (int)transform.position.x;
        }
        Score.text = (score).ToString();
    }
}
