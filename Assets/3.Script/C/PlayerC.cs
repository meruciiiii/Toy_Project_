using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerC : PlayerController_C
{
    /*
    [Header("C 스킬 설정")]
    public float boostSpeed = 10f; // 스킬 시 이동 속도
    public float skillDuration = 5f;
    private bool isDrifting = false; // 스킬 사용 중 여부

    protected override void UseSkill()
    {
        StartCoroutine(Wheelchair());
    }
    IEnumerator Wheelchair()
    {
        Debug.Log("C: 휠체어 폭주 시작 (시간 가속 + 조작 어려움)");
        isDrifting = true;

        // 이동 속도 증가
        float originalSpeed = playerSpeed;
        playerSpeed = boostSpeed;

        // GameManager에게 게임 시간 배속 요청
        // GameManager.Instance.time??

        yield return new WaitForSeconds(skillDuration);

        // 원상 복구
        playerSpeed = originalSpeed;
        isDrifting = false;
        // GameManager.Instance.SetTimeMultiplier(1.0f);

        Debug.Log("C: 스킬 종료");
    }
    protected override void OnHitObstacle() // 예시... 히트 메서드 생겼을 때 여기다 오버라이드 해서 데미지 2배로 받도록
    {
        if (isDrifting)
        {
            Debug.Log("C: 크리티컬 패널티! (휠체어 사고)");
            //GameManager에게 2배, 3배 페널티 요청
        }
        else
        {
            base.OnHitObstacle();
        }
    }
    */
}
