using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ByonController : PlayerController
{

    [Header("B 스킬 설정")]
    public float skillDuration = 5f;
    [Tooltip("힌트 아이템 획득 시 단축될 시간(초)")]
    [SerializeField] private float hintBonusTime = 2.0f;
    [SerializeField] private int hintSpawnCount = 5;
    [SerializeField] private AudioClip hintSoundClip;

    protected override void Skill()
    {
        StopCoroutine("TeacherChanceRoutine");
        StartCoroutine("TeacherChanceRoutine");
    }

    IEnumerator TeacherChanceRoutine()
    {
        Debug.Log("B: 선생님 찬스 ON (과제만 느려짐)");

        StartSkillVisual(Color.blue, skillDuration);

        // 1. 앞으로 생성될 과제들을 위해 스포너에게 알림
        if (spawner != null)
        {
            // 1. 과제 생성 일시 정지 요청
            spawner.DelaySpawn(skillDuration);

            // 2. 힌트 아이템 생성 요청
            spawner.spawning_hint(hintSpawnCount);
        }

        // 2. [Unity 6 변경점] 현재 화면에 있는 모든 과제 찾기
        // FindObjectsOfType -> FindObjectsByType
        // FindObjectsSortMode.None : 정렬 안 함 (가장 빠름)
        //var activeAssignments = Object.FindObjectsByType<AssignmentController>(FindObjectsSortMode.None);

        foreach (GameObject assignment in spawner.pooling)
        {
            assignment.TryGetComponent(out AssignmentController assignmentController);
            assignmentController.SetSlowMode(true);
        }
        // 3. 지속 시간 대기
        yield return new WaitForSeconds(skillDuration);

        // 4. 스킬 종료: 스포너 원상 복구
        // 자동적으로 5초뒤 다시 스폰될거임.

        // 5. 화면에 있는 과제들 다시 원래 속도로 복구
        // 여기도 마찬가지로 최신 API 사용
        //activeAssignments = Object.FindObjectsByType<AssignmentController>(FindObjectsSortMode.None);
        foreach (GameObject assignment in spawner.pooling)
        {
            assignment.TryGetComponent(out AssignmentController assignmentController);
            assignmentController.SetSlowMode(false);
        }
        Debug.Log("B: 선생님 찬스 OFF");
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other); // 부모 클래스(PlayerController)의 로직 먼저 실행

        // 힌트 아이템 태그가 "Hint"라고 가정 (프리팹 설정 필요)
        if (other.CompareTag("Hint"))
        {
            int hintScore = 60; // 힌트 획득 점수
            audioSource.PlayOneShot(hintSoundClip);

            // ScoreManager를 통해 점수 획득
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.SkillAddScore(hintScore);
                Destroy(other.gameObject);
            }
        }
    }
}
