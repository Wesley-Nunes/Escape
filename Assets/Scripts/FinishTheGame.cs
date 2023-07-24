using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTheGame : MonoBehaviour
{
    public bool playerWin {get; private set;}
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerWin = true;
        }
    }
}
