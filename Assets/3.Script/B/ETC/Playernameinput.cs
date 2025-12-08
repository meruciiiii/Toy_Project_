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

    private void Start()
    {
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
    }
}
