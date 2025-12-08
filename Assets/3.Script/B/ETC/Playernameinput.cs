using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Playernameinput : MonoBehaviour
{
    [Header("name_input")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button confirmbutton;
    [SerializeField] private GameObject Ui_update;
    [SerializeField] private GameObject cur_Ui;
    [SerializeField] private GameObject next_Ui;

    private void Start()
    {
        Ranking rankingData = DataManager.instance.LoadFromJson();
        int finalScore = GameManager.instance.FinalScore;
        if (rankingData.Listdata.Count >= DataManager.instance.Ranking_count)
        {
            Data thirdPlaceData = rankingData.Listdata[DataManager.instance.Ranking_count - 1];
            if (thirdPlaceData.Score >= finalScore) //점수가 3위랑 같거나 낮으면 쳐내
            {
                //현재 점수가 3위 점수보다 낮으므로 랭킹 진입 실패
                Debug.Log($"랭킹 진입 실패: 3위 점수({thirdPlaceData.Score}) > 현재 점수({finalScore})");

                // 랭킹 진입이 불가능하므로 이름 입력 UI를 건너뛰고 바로 다음 화면으로 전환하는 로직 추가
                cur_Ui.SetActive(false); // 이름 입력 UI 비활성화
                Ui_update.SetActive(true);//OnEnable
                next_Ui.SetActive(true);

                // 버튼 이벤트 리스너를 추가하지 않거나 버튼을 비활성화하여
                // OnConfirmButtonClick() 호출을 막음.
                if (confirmbutton != null)
                {
                    confirmbutton.interactable = false;
                }
                return; // Start 함수 종료
            }
        }
        Debug.Log($"랭킹 진입 성공");
        // 확인 버튼 클릭 이벤트 연결
        if (confirmbutton != null)
        {
            confirmbutton.onClick.AddListener(OnConfirmButtonClick);
        }
    }

    // 인풋 필드에 텍스트 입력시 플레이어 이름 변경 (실시간 저장)
    public void PlayerName(TMP_InputField input)
    {
        confirmbutton.interactable = !string.IsNullOrEmpty(input.text.Trim());//뭔가 써있으면 활성화됌
    }

    // 확인 버튼 클릭 시 호출될 메서드
    public void OnConfirmButtonClick()
    {
        string finalName = nameInput.text.Trim(); // 입력된 텍스트의 앞뒤 공백 제거

        if (string.IsNullOrEmpty(finalName))
        {
            // 이름이 비어있다면 경고 메시지를 표시하고 종료
            Debug.LogWarning("플레이어 이름을 입력해야 확정할 수 있습니다.");
            return;
        }

        // 최종 이름 확정 및 GameManager에 저장
        GameManager.instance.Playername = finalName;
        Debug.Log($"플레이어 이름이 '{finalName}'(으)로 확정");

        DataManager.instance.AddNewRanking(new Data(GameManager.instance.Playername, GameManager.instance.player_chractor, GameManager.instance.FinalScore));
        Debug.Log("랭킹 저장!");

        Ui_update.SetActive(true);//OnEnable
        cur_Ui.SetActive(false);
        next_Ui.SetActive(true);
    }
}
