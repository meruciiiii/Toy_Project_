using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 점수를 UI Text에 표시하려면 필요
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour //얘도 전역으로 관리 싱글톤은 아님
{
    public static ScoreManager instance;

    private int START_TIME_MINUTES;
    private int Finish_Time_MINUTES;
    private int GAME_OVER_TIME_MINUTES;

    public float timeScaleFactor = 10.0f; //1초에 10분 증가//나중에 시간 배율 변경(재호님)

    private float currentGameTimeMinutes;

    [Header("점수 설정")]
    public int score = 0;
    public float scorePerSecond = 10;  // 1초당 증가할 점수//나중에 점수 배율 변경(재호님)
    private float scoreAccumulator = 0f; // 미세한 소수점 점수를 누적할 변수 (float) Time.deltaTime 이녀석이 float임
    private Coroutine scoreCoroutine;   // 코루틴 참조를 저장할 변수

    [Header("UI 연결 ")]//현재 미작업
    public Text scoreText; // 점수를 표시할 UI Text 컴포넌트
    public Text timeText;

    private bool isRunning = false;

    private void Awake()
    {
       instance = this;
       START_TIME_MINUTES = 9 * 60 + 30;     // 9:30 AM (570분)
       Finish_Time_MINUTES = 18 * 60;       //18:00 pm (1080분)
       GAME_OVER_TIME_MINUTES = 22 * 60;   // 22:00 PM (1320분)
       currentGameTimeMinutes = START_TIME_MINUTES;
    }

    private void Start()
    {
        ToggleScore();
    }

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
            Debug.Log("점수 증가 정지");
        }
    }

    // Update() 대신 코루틴을 사용하여 점수를 올립니다.
    private IEnumerator ScoreIncreaseRoutine()
    {
        isRunning = true;

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

            currentGameTimeMinutes += Time.deltaTime * timeScaleFactor;

            // 3. 게임 종료 조건 체크 (22:00 도달)
            if (Finish_Time_MINUTES <= currentGameTimeMinutes)
            {
                SaveScore(); // 점수 저장
                SceneManager.LoadScene(""); // 랭킹 씬 string을 넣어주세요
                yield break; // 코루틴 즉시 종료
            }
            else if (Finish_Time_MINUTES >= GAME_OVER_TIME_MINUTES)
            {
                SaveScore(); // 점수 저장
                SceneManager.LoadScene(""); // 게임 오버 씬 string을 넣어주세요
                yield break; // 코루틴 즉시 종료
            }

            // 4. UI 업데이트
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score; // 정수 score만 표시
            }

            if (timeText != null)
            {
                // 분을 시:분 형태로 변환하여 표시
                int hours = (int)currentGameTimeMinutes / 60;
                int minutes = (int)currentGameTimeMinutes % 60;
                timeText.text = $"Time: {hours}:{minutes}"; // 00:00 형식 포맷팅
            }
            yield return null;
        }
        isRunning = false;
    }

    public void SkillAddScore(int score)
    {
        this.score += score;
    }

    public void OnHit(int damage)
    {
        Finish_Time_MINUTES += damage;
        Debug.Log($"피해 발생! 클리어 목표 시간이 {Finish_Time_MINUTES}로 늦춰졌습니다.");
    }

    public void SinSkill(bool On_Skill)
    {
        if (On_Skill)
        {
            timeScaleFactor = 20.0f;
            scorePerSecond = 20;
        }
        else
        {
            timeScaleFactor = 10.0f;
            scorePerSecond = 10;
        }
    }

    private void SaveScore()
    {
        GameManager.instance.SetFinalScore(score);
    }

    private void OnDisable()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }
    }
}