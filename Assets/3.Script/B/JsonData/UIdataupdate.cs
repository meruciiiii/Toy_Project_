using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIdataupdate : MonoBehaviour
{
    public Text[] rankNameText = new Text[DataManager.instance.Ranking_count];
    public Text[] rankChractor = new Text[DataManager.instance.Ranking_count];
    public Text[] rankTimeText = new Text[DataManager.instance.Ranking_count];

    private void OnEnable()
    {
        // 랭킹 화면이 활성화될 때마다 최신 데이터로 업데이트, 게임 오브젝트 활성화 비활성화마다 반영
        DisplayRanking();
    }
    public void DisplayRanking()
    {
        List<Data> ranklist = DataManager.instance.GetRankingList(); //리스트를 받아오고
        for (int i = 0; i < DataManager.instance.Ranking_count; i++) //랭킹 총 카운트(4위까지)
        {
            if (i<ranklist.Count)
            {
                Data curRank = ranklist[i]; //리스트 안쪽 3개 인수만큼 text 반영
                rankNameText[i].text = "Name     : " + curRank.Playername;
                rankChractor[i].text = "Chractor : " + curRank.charactor.ToString();
                rankTimeText[i].text = "퇴실시간 : " + curRank.cleartime.ToString();//소수점 표시 처리?
            }
            else
            {
                // 데이터가 없는 순위는 공백 처리
                rankNameText[i].text = "Name     : null";
                rankChractor[i].text = "Chractor : null";
                rankTimeText[i].text = "퇴실시간 : null";
            }
        }
    }
}
