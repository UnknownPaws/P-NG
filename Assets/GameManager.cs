using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] leftNums, rightNums;

    [SerializeField]
    private GameObject matchOverScreen;

    [SerializeField]
    private TextMeshProUGUI matchOverText;

    [SerializeField]
    private GameObject gamePauseScreen;

    private int leftScore, rightScore;

    private bool matchOver;

    private bool gamePause;

    private int roundState = 3; // | 0-Ongoing | 1-Left Won | 2-Right Won | 3-Match Paused | 4-Game Paused |

    #region Constants
    public const int ONGOING_MATCH = 0;
    public const int LEFT_WON = 1;
    public const int RIGHT_WON = 2;
    public const int INTERVAL = 3;
    #endregion


    private void Update()
    {
        if (roundState == LEFT_WON || roundState == RIGHT_WON)
        {
            string winner = "";

            if (roundState == LEFT_WON)
            {
                addScore(true);
                winner = "LEFT";
                
            }
            else if (roundState == RIGHT_WON)
            {
                addScore(false);
                winner = "RIGHT";

            }

            setRoundState(INTERVAL);

            if (matchOver)
            {
                matchOverText.text = "MATCH OVER\n" + winner + " WON";
                matchOverScreen.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gamePause)
        {
            pause();
        }
    }


    #region Non-Unity Methods

    public void addScore(bool addLeft)
    {
        if (!matchOver)
        {
            if (addLeft)
            {
                leftScore++;
                leftNums[leftScore - 1].SetActive(false);
                leftNums[leftScore].SetActive(true);

                if (leftScore == 10)
                {
                    matchOver = true;
                }
            }

            else
            {
                rightScore++;
                rightNums[rightScore - 1].SetActive(false);
                rightNums[rightScore].SetActive(true);

                if (rightScore == 10)
                {
                    matchOver = true;
                }
            }
        }
    }

    public int getRoundState()
    {
        return roundState;
    }

    public void setRoundState(int roundState)
    {
        this.roundState = roundState;
    }

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void exitToMenu()
    {
        unpause();
        SceneManager.LoadScene("TitleScene");
    }

    public void pause()
    {
        Time.timeScale = 0;
        gamePause = true;
        gamePauseScreen.SetActive(true);
        AudioListener.pause = true;
    }

    public void unpause()
    {
        Time.timeScale = 1;
        gamePause = false;
        gamePauseScreen.SetActive(false);
        AudioListener.pause = false;
    }

    public bool getPauseState()
    {
        return gamePause;
    }

    #endregion
}
