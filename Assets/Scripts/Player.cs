using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float velocity = 10.0f;
    // ENCAPSULATION
    public int hp {get; private set;} = 3;
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float zMove = verticalInput * Time.deltaTime * velocity;
        float xMove = horizontalInput * Time.deltaTime * velocity;
        transform.Translate(xMove, 0, zMove);
    }

    public void decreaseHp()
    {
        hp--;
    }

    public void increaseHp()
    {
        if (hp < 3)
        {
            hp++;            
        }
    }
}
