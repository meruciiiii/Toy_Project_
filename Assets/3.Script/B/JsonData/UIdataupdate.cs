using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIdataupdate : MonoBehaviour
{
    public TextMeshProUGUI[] rankNameText;
    public TextMeshProUGUI[] rankChractor;
    public TextMeshProUGUI[] rankTimeText;
    public TextMeshProUGUI[] CheckoutText;

    private void OnEnable()//Enable로 하지 마라...
    {
        if (DataManager.instance != null)
        {
            DisplayRanking();
        }
        else
        {
            Debug.LogError("DataManager 인스턴스를 찾을 수 없습니다! 랭킹 업데이트 실패.");
        }
    }

    public void DisplayRanking()//시작할때 한번 불러오고, 기록 갱신이후 메서드 사용해서 그때 랭킹 최신화.
    {
        List<Data> ranklist = DataManager.instance.LoadFromJson().Listdata; //리스트를 받아오고
        for (int i = 0; i < DataManager.instance.Ranking_count; i++) //랭킹 총 카운트(3위까지)
        {
            if (i<ranklist.Count)
            {
                Data curRank = ranklist[i]; //리스트 안쪽 3개 인수만큼 text 반영
                rankNameText[i].text = "" + curRank.Playername;
                rankChractor[i].text = "" + curRank.charactor.ToString();
                rankTimeText[i].text = "" + curRank.Score.ToString();
                int cleartime = GameManager.instance.ClearTimeMinutes;
                int hour = cleartime / 60;
                int minute = cleartime % 60;
                if (CheckoutText != null)
                {
                    CheckoutText[i].text = $"{hour:00}:{minute:00}";
                }
            }
            else
            {
                // 데이터가 없는 순위는 공백 처리
                rankNameText[i].text = "null";
                rankChractor[i].text = "null";
                rankTimeText[i].text = "null";
                CheckoutText[i].text = "null";
            }
        }
    }
}
