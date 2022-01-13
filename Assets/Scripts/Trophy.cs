using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!GameBehavior.gameOver && collision.tag == "Player")
        {
            GameBehavior.win = true;
            gameObject.SetActive(false);
        }
    }
}
