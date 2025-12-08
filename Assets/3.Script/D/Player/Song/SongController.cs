using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongController : PlayerController {
    [Header("A 스킬 설정")]
    public float skillDuration = 4f;
    private bool isInvincible = false;

    protected override void Skill() {
        StartCoroutine(Invincible());
        Debug.Log("스킬발동! 무적이셈");
    }
    IEnumerator Invincible() {
        Debug.Log("A: 과제 하기 모드 발동 (무적/파괴)");
        isInvincible = true;

        // 태그 변경 (과제들이 닿으면 파괴되도록 로직 변경 필요)
        // 기존 "Player" 태그에서 "Destroyer" 같은 태그로 변경한다고 가정

        // 2. 시각 효과 (빨개짐 등)
        //GetComponent<Renderer>().material.color = Color.red;

        yield return new WaitForSeconds(skillDuration);

        // 3. 원래대로 복구
        isInvincible = false;
        //GetComponent<Renderer>().material.color = Color.white;
        Debug.Log("A: 스킬 종료");
    }

    protected override void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Item")) {
            Skill();
        }
        // 과제와 부딪혔을 때
        if (collision.gameObject.CompareTag("Obstacle")) {
            if (isInvincible) {
                // 무적 상태일 때 -> 과제 파괴 & 점수 획득
                Debug.Log($"A: 과제 파괴함! (+점수추가)");

                // 1. 과제 오브젝트 삭제
                Destroy(collision.gameObject);

                // 2. 점수 추가 (GameManager에 요청)
                // GameManager.Instance.AddScore(destructionScore); 
            }
            else {
                // 평상시 -> 부모의 기본 로직(패널티) 실행
                base.OnTriggerEnter(collision);
                //퇴근 시간 늦춰주기
                Debug.Log("옵스타클 충돌 판정! 퇴근 시간 늦춰주세요");
            }
        }
    }
}



