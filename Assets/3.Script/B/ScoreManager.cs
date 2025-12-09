using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour //얘도 전역으로 관리 싱글톤은 아님
{
    public static ScoreManager instance;

    private float START_TIME_MINUTES;// 슬라이드 (MIN)
    private float Finish_Time_MINUTES; // 슬라이드 (MAX)
    private float GAME_OVER_TIME_MINUTES;// 슬라이드 (MAX)

    [Header("1초에 흐르는 시간")]
    public float timeScaleFactor = 10.0f; //1초에 10분 증가

    private float currentGameTimeMinutes; // 슬라이드 밸류값(현재 value)

    private int score = 0;
    [Header("1초에 늘어나는 점수")]
    public int scorePerSecond = 10;  // 1초당 증가할 점수//나중에 점수 배율 변경(재호님)
    private float scoreAccumulator = 0f; // 미세한 소수점 점수를 누적할 변수 (float) Time.deltaTime 이녀석이 float임
    private Coroutine scoreCoroutine;   // 코루틴 참조를 저장할 변수

    [Header("UI 연결 ")]//현재 미작업
    public TextMeshProUGUI scoreText; //점수를 표시할 UI Text 컴포넌트
    public TextMeshProUGUI timeText;
    public Slider current_timeSlider;
    public Slider GameOver_timeSlider;

    private bool isRunning = false;

    [Header("게임 종료")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip Whistle;
    [SerializeField] private TextMeshProUGUI GameOverText;      //이거 카운트다운 텍스트 UI 재사용합니다~~ 착오 없음 바람.
    

    public float GetCurrentTimeMinutes()
    {
        return currentGameTimeMinutes;
    }
    public float GetStartTime() { return START_TIME_MINUTES; }
    public float GetMaxDifficultyTime() { return 18 * 60; } // 18:00 (1080분)을 기준으로 난이도 MAX

    private void Awake()
    {
        instance = this;
        START_TIME_MINUTES = 9 * 60 + 30;     // 9:30 AM (570분)
        Finish_Time_MINUTES = 18 * 60;       //18:00 pm (1080분)
        GAME_OVER_TIME_MINUTES = 22 * 60;   // 22:00 PM (1320분)
        currentGameTimeMinutes = START_TIME_MINUTES;
        TryGetComponent(out audioSource);
    }

    private void Start()
    {
        ScoreIncreaseRoutine();
    }

    // 점수 증가 코루틴 시작/정지
    public void ScoreIncreaseRoutine()
    {
       scoreCoroutine = StartCoroutine(ScoreIncreaseRoutine_co());
    }

    // Update() 대신 코루틴을 사용하여 점수를 올립니다.
    private IEnumerator ScoreIncreaseRoutine_co()
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
            if (Finish_Time_MINUTES >= GAME_OVER_TIME_MINUTES)
            {
                Finish_Time_MINUTES = GAME_OVER_TIME_MINUTES;
                currentGameTimeMinutes = Finish_Time_MINUTES;
                isRunning = false;
                GameOverText.text = "게임 종료!";               //텍스트 "게임종료"
                GameOverText.color = Color.red;                 //텍스트 색 변경
                GameOverText.enabled = true;                    //텍스트 활성화
                Time.timeScale = 0f;                            //일시정지
                audioSource.PlayOneShot(Whistle);               //휘슬 사운드
                yield return new WaitForSecondsRealtime(2f);    //휘슬 소리가 2초정도 나오더라구요. 그래서 2초로 설정
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameOverScene"); // 게임 오버 씬 string을 넣어주세요
                StopCoroutine(scoreCoroutine);
                yield break; // 코루틴 즉시 종료
            }

            // 3. 게임 종료 조건 체크 클리어
            if (currentGameTimeMinutes >= Finish_Time_MINUTES)
            {
                currentGameTimeMinutes = Finish_Time_MINUTES;
                isRunning = false;
                SaveScore(); // 점수 저장
                Time.timeScale = 0f;                            //일시정지
                GameOverText.text = "게임 종료!";               //텍스트 "게임종료"
                GameOverText.color = Color.red;                 //텍스트 색 변경
                GameOverText.enabled = true;                    //텍스트 활성화
                audioSource.PlayOneShot(Whistle);               //휘슬 사운드
                yield return new WaitForSecondsRealtime(2f);        //휘슬 소리가 2초정도 나오더라구요. 그래서 2초로 설정
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameClearScene"); // 랭킹 씬 string을 넣어주세요
                StopCoroutine(scoreCoroutine);
                yield break; // 코루틴 즉시 종료
            }

            // 5. UI 업데이트
            if (scoreText != null)
            {
                scoreText.text = "" + score; // 정수 score만 표시
            }

            if (timeText != null)
            {
                int RoundCurrent;
                RoundCurrent = Mathf.RoundToInt(currentGameTimeMinutes);
                int hours = (int)RoundCurrent / 60;
                int minutes = (int)RoundCurrent % 60;
                timeText.text = $"{hours:00}:{minutes:00}";//00:00 형식 포맷팅
            }

            if (current_timeSlider != null&& GameOver_timeSlider !=null)
            {
                current_timeSlider.value = currentGameTimeMinutes;
                GameOver_timeSlider.value = Finish_Time_MINUTES;
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
        if (!isRunning) return;

        Finish_Time_MINUTES += damage;
        Debug.Log($"피해 발생! 클리어 목표 시간이 {Finish_Time_MINUTES}로 늦춰졌습니다.");
    }

    public void SinSkill(bool On_Skill)
    {
        if (On_Skill)
        {
            scorePerSecond *= 2;
            Debug.Log("SinSkill_On!!");
        }
        else
        {
            scorePerSecond /= 2;
            Debug.Log("SinSkill_Off!!");
        }
    }

    private void SaveScore()
    {
        int remainingMinutes = (int)(GAME_OVER_TIME_MINUTES - Finish_Time_MINUTES);

        if (remainingMinutes > 0)
        {
            int bonusScore = remainingMinutes * 10; //남은 시간 배율 보너스
            int scoretemp = score;
            PlayerPrefs.SetInt("bouns", bonusScore);
            PlayerPrefs.SetInt("baseScore", scoretemp);
            score += bonusScore;
            Debug.Log($"클리어 보너스: 여유분 {remainingMinutes}분 * 50 = {bonusScore}점 추가!");
        }
        GameManager.instance.SetFinalScore(score);
        int RoundCurrent = Mathf.RoundToInt(currentGameTimeMinutes);
        GameManager.instance.SetClearTimeMinutes(RoundCurrent);
        Debug.Log($"최종 점수 저장: {score}점");
    }

    private void OnDisable()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }
    }

    private IEnumerator Whi()
    {
        audioSource.PlayOneShot(Whistle);               //휘슬 사운드
        yield return new WaitForSecondsRealtime(2f);
    }
}