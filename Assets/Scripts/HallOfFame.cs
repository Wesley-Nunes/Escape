using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HallOfFame : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;
    void Start()
    {
        if (UserInfo.scores != null && UserInfo.scores.Capacity > 0)
        {
            int scoreCapacity = UserInfo.scores.Capacity;
            if (scoreCapacity > 3)
            {
                scoreCapacity = 3;
            }
            for (int i = 0; i < scoreCapacity; i++)
            {
                TMP_Text playerTextField = players[i].GetComponent<TMP_Text>();
                playerTextField.text = $"{UserInfo.scores[i].Name}: {UserInfo.scores[i].Points}";
            }
        }
    }
}
