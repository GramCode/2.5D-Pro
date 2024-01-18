using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game Manager is NULL");

            return _instance;
        }
    }

    private bool _gameOver = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (_gameOver == true && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _gameOver = true;
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }
}
