using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int menu = 0, tetris = 1, snake = 2;
    [SerializeField] private bool tetr, snak;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject gameChoose;

    [SerializeField] private Tetris tet;
    [SerializeField] private Snake sna;

    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject lvl;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject pauseBtn;

    private int level = -1;
    private bool change = true;

    public void Play()
    {
        main.SetActive(!main.activeSelf);
        gameChoose.SetActive(!gameChoose.activeSelf);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Tetris()
    {
        SceneManager.LoadScene(tetris);
    }
    public void Snake()
    {
        SceneManager.LoadScene(snake);
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }

    public void Pause()
    {
        if(!change)
        {
            bool game = false;
            if (tetr)
            {
                tet.game = !tet.game;
                game = tet.game;
            }
            else if (snak)
            {
                sna.game = !sna.game;
                game = sna.game;
            }
            if (!game)
            {
                pause.SetActive(true);
            }
            else
            {
                pause.SetActive(false);
            }
        }
    }

    public void ChangeLVL(int lvl)
    {
        if(lvl != 0) level = lvl;
        else
        {
            if (tetr)
            {
                tet.SetSpeed(level == -1? 1:level);
                tet.game = true;
            }

            else if (snak)
            {
                sna.SetSpeed(level == -1? 6: level);
                sna.game = true;
            }
            this.lvl.SetActive(false);
            pauseBtn.SetActive(true);
            change = false;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(menu);
    }
    
}
