using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ByonController : PlayerController {

    [Header("B 스킬 설정")]
    public float skillDuration = 5f;
    [Tooltip("힌트 아이템 획득 시 단축될 시간(초)")]
    [SerializeField] private float hintBonusTime = 2.0f;

    private AssignmentSpawner spawner;

    protected override void Awake() {
        base.Awake();
        // 씬에서 스포너 미리 찾기 (민찬님 스크립트)
        spawner = Object.FindFirstObjectByType<AssignmentSpawner>();
    }

    protected override void Skill() {
        StopCoroutine("TeacherChanceRoutine");
        StartCoroutine("TeacherChanceRoutine");
    }

    IEnumerator TeacherChanceRoutine() {
        Debug.Log("B: 선생님 찬스 ON (과제만 느려짐)");

        // 1. 앞으로 생성될 과제들을 위해 스포너에게 알림
        if (spawner != null) spawner.DelaySpawn(skillDuration);

        // 2. [Unity 6 변경점] 현재 화면에 있는 모든 과제 찾기
        // FindObjectsOfType -> FindObjectsByType
        // FindObjectsSortMode.None : 정렬 안 함 (가장 빠름)
        var activeAssignments = Object.FindObjectsByType<AssignmentController>(FindObjectsSortMode.None);

        foreach (var assignment in activeAssignments) {
            assignment.SetSlowMode(true);
        }
        // 3. 지속 시간 대기
        yield return new WaitForSeconds(skillDuration);

        // 4. 스킬 종료: 스포너 원상 복구
        // 자동적으로 5초뒤 다시 스폰될거임.

        // 5. 화면에 있는 과제들 다시 원래 속도로 복구
        // 여기도 마찬가지로 최신 API 사용
        activeAssignments = Object.FindObjectsByType<AssignmentController>(FindObjectsSortMode.None);
        foreach (var assignment in activeAssignments) {
            assignment.SetSlowMode(false);
        }
        Debug.Log("B: 선생님 찬스 OFF");
    }
}
