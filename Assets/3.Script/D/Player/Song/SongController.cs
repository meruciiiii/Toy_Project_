using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongController : PlayerController {
    [Header("A 스킬 설정")]
    [SerializeField] private float skillDuration = 4f;
    private bool isInvincible = false;
    
    // 시각 효과를 위한 렌더러 캐싱
    

    protected override void Awake() {
        base.Awake(); // 부모의 Awake(Rigidbody 설정 등) 호출
    }

    // 부모 클래스(PlayerController)의 Skill 메서드 오버라이드
    protected override void Skill() {
        // 이미 스킬 중이라면 중복 실행 방지 (선택 사항)
        StopCoroutine("Invincible"); 
        StartCoroutine("Invincible");
    }

    IEnumerator Invincible() {
        Debug.Log("A: 과제 파괴 모드 ON (무적)");
        isInvincible = true;

        StartSkillVisual(Color.red, skillDuration);


        // 2. 지속 시간 대기
        // 주의: Player B와 달리 A는 시간이 정상적으로 흐르는 게 유리하므로 WaitForSeconds 사용
        yield return new WaitForSeconds(skillDuration);

        // 3. 원상 복구
        isInvincible = false;
        Debug.Log("A: 스킬 종료");
    }

    protected override void OnHitObstacle(GameObject obstacle)
    {
        if (isInvincible)
        {
            // [무적 상태]
            Debug.Log($"A: 과제 격파! (점수 획득)");

            // 점수 추가 로직 (예시)
            // if (GameManager.Instance != null) GameManager.Instance.AddScore(100);

            // 여기도 Destroy 대신 비활성화로 변경해야 합니다.
            obstacle.SetActive(false);
        }
        else
        {
            // [일반 상태] 부모의 로직(패널티 + 비활성화)을 그대로 따름
            base.OnHitObstacle(obstacle);
        }
    }
}



