using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ByonController : PlayerController {

    [Header("B 스킬 설정")]
    public float skillDuration = 5f;
    [Tooltip("힌트 아이템 획득 시 단축될 시간(초)")]
    [SerializeField] private float hintBonusTime = 2.0f;
    [SerializeField] private int hintSpawnCount = 5;

    [SerializeField] private GameObject assignmentSpawner;

    private AssignmentSpawner spawner;

    protected override void Awake() {
        base.Awake();
        // 씬에서 스포너 미리 찾기 (민찬님 스크립트)
        assignmentSpawner.TryGetComponent(out spawner);
    }

    protected override void Skill() {
        StopCoroutine("TeacherChanceRoutine");
        StartCoroutine("TeacherChanceRoutine");
    }

    IEnumerator TeacherChanceRoutine() {
        Debug.Log("B: 선생님 찬스 ON (과제만 느려짐)");

        // 1. 앞으로 생성될 과제들을 위해 스포너에게 알림
        if (spawner != null)
        {
            // [원본 메서드명 유지] 1. 과제 생성 일시 정지 요청
            spawner.DelaySpawn(skillDuration);

            // [원본 메서드명 유지] 2. 힌트 아이템 생성 요청
            spawner.spawning_hint(hintSpawnCount);
        }

        // 2. [Unity 6 변경점] 현재 화면에 있는 모든 과제 찾기
        // FindObjectsOfType -> FindObjectsByType
        // FindObjectsSortMode.None : 정렬 안 함 (가장 빠름)
        //var activeAssignments = Object.FindObjectsByType<AssignmentController>(FindObjectsSortMode.None);

        foreach (GameObject assignment in spawner.pooling) {
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
        foreach (GameObject assignment in spawner.pooling) {
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
            // 1. 게임 매니저를 통해 시간 단축
            //if (GameManager.Instance != null)
            //{
              //  GameManager.Instance.ReduceTime(hintBonusTime);
            //}

            // 2. 힌트 아이템 삭제
            // Hint는 풀링이 아닌 Instantiate로 생성되므로 Destroy로 삭제합니다.
            // (AssignmentSpawner.cs의 spawn_hint 메서드 확인 결과)
            Destroy(other.gameObject);
            Debug.Log("힌트 획득 퇴근 시간 앞당겨짐");
        }
    }
}
