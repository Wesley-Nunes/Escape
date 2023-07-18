using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool gameOver = false;
    public GameObject player;
    public GameObject[] enemies = new GameObject[2];
    void Update()
    {
        if (!gameOver)
        {
            int hp = player.GetComponent<Player>().hp;
            if (hp == 0)
            {
                gameOver = true;
            }
            else
            {
                StartCoroutine(EnemiesAttackCheckRoutine());
            }
        }
        else
        {
            StopAllCoroutines();
            player.SetActive(false);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].SetActive(false);
            }
        }    
    }

    IEnumerator EnemiesAttackCheckRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < enemies.Length; i++)
        {
            bool playerHit = enemies[i].GetComponent<Enemy>().playerHit;
            if(playerHit)
            {
                // ABSTRACTION
                player.GetComponent<Player>().decreaseHp();
                enemies[i].GetComponent<Enemy>().resetPlayerHitState();
            }
        }
    }
}
