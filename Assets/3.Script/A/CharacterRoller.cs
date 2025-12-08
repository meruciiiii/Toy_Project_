using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRoller : MonoBehaviour
{
    [Header("캐릭터 및 위치 설정")]
    [SerializeField] private GameObject[] characters = new GameObject[3];

    [SerializeField] private Transform position1; // 중앙
    [SerializeField] private Transform position2; // 오른쪽
    [SerializeField] private Transform position3; // 왼쪽

    [Header("이동 설정")]
    [SerializeField] private float moveDuration = 0.5f; // 이동하는데 걸리는 시간 (초)
    // 기존 moveSpeed, rotationSpeed는 제거하고 시간으로 통일합니다.

    private bool isMoving = false;
    private int movesInProgress = 0;

    private Transform[] currentPositions = new Transform[3];

    [Header("캐릭터별 UI 그룹 (순서대로 A, B, C)")]
    [SerializeField] private GameObject[] uiGroups = new GameObject[3];

    private void Start()
    {
        if (characters.Length != 3 || position1 == null || position2 == null || position3 == null || uiGroups.Length != 3)
        {
            Debug.LogError("할당이 누락되었습니다. Inspector를 확인하세요.");
            return;
        }

        // 초기 위치 설정
        characters[0].transform.position = position1.position;
        characters[0].transform.rotation = position1.rotation;
        currentPositions[0] = position1;

        characters[1].transform.position = position2.position;
        characters[1].transform.rotation = position2.rotation;
        currentPositions[1] = position2;

        characters[2].transform.position = position3.position;
        characters[2].transform.rotation = position3.rotation;
        currentPositions[2] = position3;

        SwitchUIGroupToFocusedCharacter();
    }

    public void MoveRight()
    {
        if (isMoving) return;

        isMoving = true;
        movesInProgress = 3; // 카운터 시작

        Transform nextPos1 = position3;
        Transform nextPos2 = position1;
        Transform nextPos3 = position2;

        GameObject charAtPos1 = FindCharacterAtPosition(position1);
        GameObject charAtPos2 = FindCharacterAtPosition(position2);
        GameObject charAtPos3 = FindCharacterAtPosition(position3);

        StartCoroutine(MoveCharacterTimeBased(charAtPos1, nextPos1));
        StartCoroutine(MoveCharacterTimeBased(charAtPos2, nextPos2));
        StartCoroutine(MoveCharacterTimeBased(charAtPos3, nextPos3));
    }

    public void MoveLeft()
    {
        if (isMoving) return;

        isMoving = true;
        movesInProgress = 3; // 카운터 시작

        Transform nextPos1 = position2;
        Transform nextPos2 = position3;
        Transform nextPos3 = position1;

        GameObject charAtPos1 = FindCharacterAtPosition(position1);
        GameObject charAtPos2 = FindCharacterAtPosition(position2);
        GameObject charAtPos3 = FindCharacterAtPosition(position3);

        StartCoroutine(MoveCharacterTimeBased(charAtPos1, nextPos1));
        StartCoroutine(MoveCharacterTimeBased(charAtPos2, nextPos2));
        StartCoroutine(MoveCharacterTimeBased(charAtPos3, nextPos3));
    }

    private GameObject FindCharacterAtPosition(Transform targetPos)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (currentPositions[i] == targetPos)
            {
                return characters[i];
            }
        }
        return null;
    }

    // ⭐ 수정된 코루틴: 거리 확인 방식 -> 시간 진행 방식
    private IEnumerator MoveCharacterTimeBased(GameObject character, Transform targetTransform)
    {
        if (character == null)
        {
            // 예외 처리: 캐릭터가 없으면 카운트만 줄이고 종료
            movesInProgress--;
            CheckAllFinished();
            yield break;
        }

        Vector3 startPos = character.transform.position;
        Quaternion startRot = character.transform.rotation;
        float elapsedTime = 0f;

        // moveDuration 동안 루프를 돕니다. (시간 보장)
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // t값에 SmoothStep을 적용하면 더 부드럽게 움직입니다 (선택 사항)
            t = t * t * (3f - 2f * t);

            character.transform.position = Vector3.Lerp(startPos, targetTransform.position, t);
            character.transform.rotation = Quaternion.Lerp(startRot, targetTransform.rotation, t);

            yield return null;
        }

        // 루프가 끝나면 강제로 위치를 맞춥니다.
        character.transform.position = targetTransform.position;
        character.transform.rotation = targetTransform.rotation;

        // 위치 정보 업데이트
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == character)
            {
                currentPositions[i] = targetTransform;
                break;
            }
        }

        // 이동 완료 보고
        movesInProgress--;
        CheckAllFinished();
    }

    // 모든 이동이 끝났는지 확인하는 함수
    private void CheckAllFinished()
    {
        if (movesInProgress <= 0)
        {
            movesInProgress = 0; // 안전장치
            isMoving = false; // 버튼 잠금 해제
            SwitchUIGroupToFocusedCharacter();

            Debug.Log("캐릭터 이동 완료 및 UI 교체 성공!");
        }
    }

    private void SwitchUIGroupToFocusedCharacter()
    {
        int focusedCharacterIndex = -1;

        for (int i = 0; i < characters.Length; i++)
        {
            if (currentPositions[i] == position1)
            {
                focusedCharacterIndex = i;
                break;
            }
        }

        for (int i = 0; i < uiGroups.Length; i++)
        {
            if (uiGroups[i] != null)
            {
                if (i == focusedCharacterIndex)
                    uiGroups[i].SetActive(true);
                else
                    uiGroups[i].SetActive(false);
            }
        }
    }
}