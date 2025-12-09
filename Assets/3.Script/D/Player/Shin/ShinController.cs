using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShinController : PlayerController
{
    
    [Header("C 스킬 설정")]
    public float boostSpeed = 15f; // 스킬 시 이동 속도
    public float skillDuration = 5f;
    private bool isDrifting = false; // 스킬 사용 중 여부
    private float RotateSpeed = 180f;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out player_r);
        transform.position = new Vector3(-9f, 0.45f, -8f);
    }

    protected override void Skill()
    {
        StartCoroutine(Wheelchair());
    }

    protected override void Move() {
        //A,D 키를 누르면 회전값을 변경합니다.
        float turn = Input.playerDirection_z * RotateSpeed * Time.deltaTime;

        //변경된 회전값을 바라봅니다.
        player_r.rotation = player_r.rotation * Quaternion.Euler(0, turn, 0);

        //Shin 캐릭터를 앞으로 꾸준히 이동시킵니다.
        player_r.MovePosition(player_r.position + (transform.forward * playerSpeed * Time.deltaTime));
    }

    private IEnumerator Wheelchair()
    {
        Debug.Log("C: 휠체어 폭주 시작 (시간 가속 + 조작 어려움)");

        StartSkillVisual(Color.green, skillDuration);

        isDrifting = true;

        // 이동 속도 증가
        float originalSpeed = playerSpeed;
        playerSpeed = boostSpeed;

        // [ScoreManager 연동 1] 패시브 점수 2배 혜택 켜기
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SinSkill(true);
        }

        float elapsedTime = 0f;
        int driftBonusScore = 10; // 0.1초당 추가 보너스 점수

        while (elapsedTime < skillDuration)
        {
            elapsedTime += 0.1f;

            // 폭주하는 맛을 위해 점수를 계속 넣어줍니다.
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.SkillAddScore(driftBonusScore);
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(skillDuration);

        // 원상 복구
        playerSpeed = originalSpeed;
        isDrifting = false;
        
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SinSkill(false);
        }
        Debug.Log("C: 스킬 종료");
    }
	protected override void OnTriggerEnter(Collider collision) { 
		if (isDrifting)
        {
            StopCoroutine(Wheelchair());
        } else {
            base.OnTriggerEnter(collision);
		}
	}
}
