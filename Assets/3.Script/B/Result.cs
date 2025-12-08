using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public TMPro.TextMeshProUGUI bonus;
    public TMPro.TextMeshProUGUI basescore;
    public TMPro.TextMeshProUGUI result;
    public TMPro.TextMeshProUGUI checkoutTime;

    private void Start()//Enable로 하지 마라...
    {
        DisplayScore();
    }

    public void DisplayScore()//시작할때 한번 불러오고, 기록 갱신이후 메서드 사용해서 그때 랭킹 최신화.
    {
        bonus.text = "" + PlayerPrefs.GetInt("bouns");
        basescore.text = "" + PlayerPrefs.GetInt("baseScore");
        result.text = "" + GameManager.instance.FinalScore;
        int cleartime = GameManager.instance.ClearTimeMinutes;
        int hour = cleartime / 60;
        int minute = cleartime % 60;
        if (checkoutTime != null)
        {
            checkoutTime.text = $"{hour:00}:{minute:00}";
        }
    }
}
