using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool touchHeart {get; private set;} = false;
    public bool touchBonus {get; private set;} = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            if(gameObject.name.Contains("Bonus"))
            {
                touchBonus = true;
            }

            if (gameObject.name.Contains("Heart"))
            {
                touchHeart = true;
            }
        }
    }

    public void resetTouchHeartState()
    {
        touchHeart = false;
    }

    public void resetTouchBonusState()
    {
        touchBonus = false;
    }
}
