using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    bool gameOver = false;
    int playerHp;
    int bonus = 0;
    int scorePoints;
    string nameInputField;
    [SerializeField]
    TMP_Text nameUI;
    [SerializeField]
    TMP_Text timerUI;
    [SerializeField]
    GameObject winUI;
    [SerializeField]
    GameObject[] livesUI;
    [SerializeField]
    GameObject[] bonusUI;
    [SerializeField]
    GameObject[] enemies;
    [SerializeField]
    FinishTheGame finishTheGame;
    [SerializeField]
    GameObject gameOverObject;
    [SerializeField]
    GameObject player;
    void Start()
    {
        nameUI.text = UserInfo.playerName;
    }
    void Update()
    {
        if (finishTheGame.playerWin)
        {
            WinGame();
        }
        else if (!gameOver)
        {
            StartGame();
        }
        else if (gameOver)
        {
            GameOver();                
        }        
    }   
    void WinGame()
    {
        DisableTheObjects();
        int finalScore = (int)Math.Floor((1000.0 / scorePoints * (bonus + 1)) * 1000);
        GameObject congrats = winUI.transform.GetChild(0).gameObject;
        TMP_Text congratsMessage = congrats.GetComponent<TMP_Text>();
        congratsMessage.text = "Congrats, " + UserInfo.playerName + "\nYou Win!";
        GameObject highScore = winUI.transform.GetChild(1).gameObject;
        UserInfo.Instance.SaveScores(finalScore);
        if (UserInfo.Instance.isHighScore(finalScore))
        {
            TMP_Text highScoreMessage = highScore.GetComponent<TMP_Text>();
            highScoreMessage.text = $"New Record: {finalScore}";
            highScore.SetActive(true);
        }
        else
        {
            highScore.SetActive(false);
        }

        winUI.SetActive(true);
    }
    void GameOver()
    {
        DisableTheObjects();
        gameOverObject.SetActive(true);
    }
    void DisableTheObjects()
    {
        StopAllCoroutines();
        player.SetActive(false);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(false);
        }
        for (int i = 0; i < livesUI.Length; i++)
        {
            livesUI[i].SetActive(false);
        }
        for (int i = 0; i < bonusUI.Length; i++)
        {
            bonusUI[i].SetActive(false);
        }
        timerUI.text = "";
    }
    void StartGame()
    {
        playerHp = player.GetComponent<Player>().hp;
        if (playerHp == 0)
        {
            gameOver = true;
        }
        else
        {
            Timer();
            StartCoroutine(EnemiesAttackCheckRoutine());
            StartCoroutine(CollectiblesCheckRoutine());
            RenderLivesUI();
            RenderBonusUI();
        }
    }
    void RenderBonusUI()
    {
        for (int i = 0; i < bonus; i++)
        {
            bonusUI[i].GetComponent<Image>().enabled = true;
        }
    }
    void RenderLivesUI()
    {
        for (int i = 0; i < 3; i++)
        {
            livesUI[i].GetComponent<Image>().enabled = false;
        }

        for (int i = 0; i < playerHp; i++)
        {
            livesUI[i].GetComponent<Image>().enabled = true;
        }
    }
    void Timer()
    {
        scorePoints = (int)Time.timeSinceLevelLoad;
        int seconds = scorePoints;
        int minutes = 0;
        if (scorePoints >= 60)
        {
            minutes = seconds / 60;
            seconds = seconds % 60;
        }
        timerUI.text = $"{minutes}:{seconds}";
    }
    IEnumerator EnemiesAttackCheckRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < enemies.Length; i++)
        {   
            Enemy enemyScript = enemies[i].GetComponent<Enemy>();
            bool playerHit = enemyScript.playerHit;
            if(playerHit)
            {
                // ABSTRACTION
                player.GetComponent<Player>().decreaseHp();
                enemyScript.resetPlayerHitState();
            }
        }
    }
    IEnumerator CollectiblesCheckRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible"); 

        for (int i = 0; i < collectibles.Length; i++)
        {   
            GameObject collectible = collectibles[i].gameObject;
            Collectible collectibleScript = collectible.GetComponent<Collectible>();

            if (collectible.name.Contains("Heart"))
            {
                bool touchHeart = collectibleScript.touchHeart;
                if (touchHeart)
                {
                    player.GetComponent<Player>().increaseHp();
                    collectibleScript.resetTouchHeartState();

                    if (playerHp < 3)
                    {
                        Destroy(collectible);                      
                    }
                }
            }
            if (collectible.name.Contains("Bonus"))
            {
                bool touchBonus = collectibleScript.touchBonus;
                if (touchBonus)
                {
                    bonus++;
                    collectibleScript.resetTouchBonusState();
                    Destroy(collectible);
                }
            }
        }
    }    
}
