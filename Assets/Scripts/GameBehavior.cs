using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject winScreen;

    public static bool gameOver;
    public static bool win;

    public static GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (gameOver)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
        if (win)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResetLevel()
    {
        gameOver = false;
        gameOverScreen.SetActive(false);
        win = false;
        winScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
