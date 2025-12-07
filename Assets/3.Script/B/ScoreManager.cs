using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 점수를 UI Text에 표시하려면 필요

public class ScoreManager : MonoBehaviour
{
    [Header("점수 설정")]
    public int score = 0;            // 현재 점수
    public float scorePerSecond = 10;  // 1초당 증가할 점수
    private float scoreAccumulator = 0f; // 미세한 소수점 점수를 누적할 변수 (float) Time.deltaTime 이놈이 float임 
    private Coroutine scoreCoroutine;   // 코루틴 참조를 저장할 변수

    [Header("UI 연결 ")]//현재 미작업
    public Text scoreText; // 점수를 표시할 UI Text 컴포넌트

    private bool isRunning = false;

    // 점수 증가 코루틴 시작/정지
    public void ToggleScore()
    {
        if (!isRunning)
        {
            // 코루틴이 실행 중이 아닐 때 시작
            scoreCoroutine = StartCoroutine(ScoreIncreaseRoutine());
            Debug.Log("점수 증가 시작");
        }
        else
        {
            // 코루틴이 실행 중일 때 정지
            StopCoroutine(scoreCoroutine);
            isRunning = false;
            Debug.Log("점수 증가 정지");

            SaveScoreToPlayerPrefs();
            Debug.Log("점수 증가 정지 및 저장 완료");
        }
        
    }

    // Update() 대신 코루틴을 사용하여 점수를 올립니다.
    private IEnumerator ScoreIncreaseRoutine()
    {
        isRunning = true;

        // isRunning이 true인 동안 계속 반복합니다.
        while (isRunning)
        {
            // 1. 미세한 점수를 float 변수에 누적
            scoreAccumulator += scorePerSecond * Time.deltaTime;

            // 2. 누적된 점수가 1점 이상일 때 정수 score에 반영하고 소수점 이하를 남김
            if (scoreAccumulator >= 1f)
            {
                int pointsToAdd = Mathf.FloorToInt(scoreAccumulator); // 추가할 정수 점수
                score += pointsToAdd; // 최종 정수 score 증가
                scoreAccumulator -= pointsToAdd; // 누적 변수에서 정수만큼 빼고 소수점만 남김
            }

            // 3. UI 업데이트
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score; // 정수 score만 표시
            }

            yield return null;
        }
    }

    private void SaveScoreToPlayerPrefs()
    {
        // "FinalScore"라는 키(Key)로 최종 정수 점수를 저장
        PlayerPrefs.SetInt("FinalScore", score);

        // 데이터 저장 후 호출
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }
    }
}