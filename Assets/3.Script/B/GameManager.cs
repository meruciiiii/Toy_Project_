using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isPause;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        player_chractor = 0;//none
    }

    public Charactor player_chractor;
    public string Playername = "";
    public int FinalScore;
    public int ClearTimeMinutes;

    public void SetFinalScore(int score)
    {
        FinalScore = score;
    }

    public void SetClearTimeMinutes(int time)
    {
        ClearTimeMinutes = time;
    }

    public void ResetScore()
    {
        FinalScore = 0;
    }

    //플레이어 이름 입력및 저장 GameManager.instance.Playername = string 입력 받는거
}
