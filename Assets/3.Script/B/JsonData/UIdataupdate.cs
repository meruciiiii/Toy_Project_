using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIdataupdate : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] rankNameText;
    public TMPro.TextMeshProUGUI[] rankChractor;
    public TMPro.TextMeshProUGUI[] rankTimeText;

    //private void Awake()
    //{
    //    rankNameText = new TextMeshProUGUI[DataManager.instance.Ranking_count];
    //    rankChractor = new TextMeshProUGUI[DataManager.instance.Ranking_count];
    //    rankTimeText = new TextMeshProUGUI[DataManager.instance.Ranking_count];
    //}
    private void Start()//Enable로 하지 마라...
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

    private void OnEnable()
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
    public void DisplayRanking()
    {
        List<Data> ranklist = DataManager.instance.GetRankingList(); //리스트를 받아오고
        for (int i = 0; i < DataManager.instance.Ranking_count; i++) //랭킹 총 카운트(4위까지)
        {
            if (i<ranklist.Count)
            {
                Data curRank = ranklist[i]; //리스트 안쪽 3개 인수만큼 text 반영
                rankNameText[i].text = "Name        : " + curRank.Playername;
                rankChractor[i].text = "Chractor    : " + curRank.charactor.ToString();
                rankTimeText[i].text = "Coding score : " + curRank.cleartime.ToString();//소수점 표시 처리?
            }
            else
            {
                // 데이터가 없는 순위는 공백 처리
                rankNameText[i].text = "Name        : null";
                rankChractor[i].text = "Chractor    : null";
                rankTimeText[i].text = "Coding score : null";
            }
        }
    }
}
