using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShinController : PlayerController
{
    
    [Header("C 스킬 설정")]
    public float boostSpeed = 20f; // 스킬 시 이동 속도
    public float skillDuration = 5f;
    private bool isDrifting = false; // 스킬 사용 중 여부
    private float RotateSpeed = 180f;

    protected override void Skill()
    {
        StartCoroutine(Wheelchair());
    }

    protected override void Move() {
        float turn = Input.playerDirection_z * RotateSpeed * Time.deltaTime;
        player_r.rotation = player_r.rotation * Quaternion.Euler(0, turn, 0);
        player_r.MovePosition(player_r.position + (transform.forward * playerSpeed * Time.deltaTime));
    }

    private IEnumerator Wheelchair()
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
	protected override void OnCollisionEnter(Collision collision) {
		if (isDrifting)
        {
            if(collision.transform.CompareTag("Obstacle")) {
                Debug.Log("C: 크리티컬 패널티! (휠체어 사고)");
                //GameManager에게 2배, 3배 페널티 요청
			}
        } else {
            base.OnCollisionEnter(collision);
		}
	}
}
