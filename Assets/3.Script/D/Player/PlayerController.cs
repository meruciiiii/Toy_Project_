using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	protected float playerSpeed = 10f;

	[SerializeField] protected PlayerInput Input;
	protected Rigidbody player_r;

	[SerializeField] private MapSize size;

	[SerializeField] private GameObject assignmentSpawnner;
	protected AssignmentSpawner spawner;

	[SerializeField] private AudioClip itemSoundClip;
	[SerializeField] private AudioClip debuffSoundClip;
	[SerializeField] private AudioClip hitSoundClip;
	private AudioSource audioSource;

	[Header("시각 효과 설정")]
	// [수정 1] 하나만 담던 변수를 '배열(Array)'로 변경합니다.
	[SerializeField] private Renderer[] playerRenderers;

	// 원래 색상들도 파츠별로 다 저장해야 하므로 배열이나 리스트가 필요하지만,
	// 간단하게 구현하기 위해 '첫 번째 파츠'의 색이나 흰색을 기준으로 잡겠습니다.
	// (보통 스킬 끝나면 원래대로 돌릴 때 흰색(Color.white)을 넣으면 텍스처 본연의 색이 나옵니다)
	private Color originalColor = Color.white;
	private Coroutine visualCoroutine;

	protected virtual void Awake()
	{
		TryGetComponent(out player_r);
		TryGetComponent(out audioSource);
		assignmentSpawnner.TryGetComponent(out spawner);

		// [수정 2] GetComponentsInChildren (뒤에 s 붙음!)으로 자식에 있는 '모든' 렌더러를 다 긁어옵니다.
		playerRenderers = GetComponentsInChildren<Renderer>();

		if (playerRenderers == null || playerRenderers.Length == 0)
		{
			Debug.LogError($"{gameObject.name}: 렌더러를 하나도 못 찾았습니다!");
		}
		else
		{
			// 디버깅: 몇 개나 찾았는지 확인
			Debug.Log($"{gameObject.name}: 렌더러 {playerRenderers.Length}개 발견! (다 칠해버립니다)");
		}

		transform.position = new Vector3(-9f, 0.2f, -8f);
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void LateUpdate()
	{
		//플레이어의 가속은 항상 zero로 맞춰줍니다. 캐릭터는 입력값으로 움직이고, 가속은 존재하지 않기 때문.
		player_r.linearVelocity = Vector3.zero;

		//맵 범위에 맞춘 이동 제한
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, size.LimitMin.x, size.LimitMax.x),
			transform.position.y,
			Mathf.Clamp(transform.position.z, size.LimitMin.z, size.LimitMax.z)
			);
	}

	protected virtual void Move()
	{
		//입력받은 키에 따른 벡터 저장.
		Vector3 playerDirection = new Vector3(Input.playerDirection_x, 0f, Input.playerDirection_z);

		//이동하는 방향에 맞춰 회전 및 이동시킵니다.
		transform.LookAt(transform.position + playerDirection);
		player_r.MovePosition(player_r.position + (playerDirection * playerSpeed * Time.deltaTime));

	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Item"))
		{
			audioSource.PlayOneShot(itemSoundClip);
			Skill();
		}
		else if (other.transform.CompareTag("Debuff"))
        {
			audioSource.PlayOneShot(debuffSoundClip);
			spawner.spawn_assignment_from_debuff(10);
        }
		else if (other.transform.CompareTag("Obstacle"))
		{
			audioSource.PlayOneShot(hitSoundClip);
			OnHitObstacle(other.gameObject);
		}
	}
	protected virtual void OnHitObstacle(GameObject obstacle)
	{
		// 1. 게임매니저에게 패널티 부과 요청
		//if (GameManager.Instance != null)
		//{
		//	GameManager.Instance.AddPenaltyTime(hitPenaltyTime);
		//}

		// 2. 충돌한 과제 오브젝트 삭제 (또는 비활성화)
		// 충돌 후에도 오브젝트가 남아있으면 계속 충돌 판정이 날 수 있으므로 처리 필요
		obstacle.SetActive(false);
		Debug.Log("과제랑 충돌 퇴근시간 지연");
	}
	protected virtual void Skill()
	{

	}
	protected void StartSkillVisual(Color skillColor, float duration)
	{
		if (playerRenderers == null) return;

		// 기존 효과가 돌고 있다면 끄고 새로 시작
		if (visualCoroutine != null) StopCoroutine(visualCoroutine);
		visualCoroutine = StartCoroutine(SkillVisualRoutine(skillColor, duration));
	}
	private IEnumerator SkillVisualRoutine(Color targetColor, float duration)
	{
		// 1. 모든 렌더러 색상 변경
		ChangeAllRenderersColor(targetColor);

		// 2. 대기 (깜빡임 전까지)
		float blinkDuration = 1.5f;
		if (duration > blinkDuration) yield return new WaitForSeconds(duration - blinkDuration);
		else blinkDuration = duration;

		// 3. 깜빡임 효과
		float blinkTimer = 0f;
		bool isColored = true; // 현재 스킬 색인지 여부

		while (blinkTimer < blinkDuration)
		{
			blinkTimer += 0.15f;
			isColored = !isColored;

			// 깜빡일 때: 원래 색(흰색) <-> 스킬 색
			ChangeAllRenderersColor(isColored ? targetColor : originalColor);

			yield return new WaitForSeconds(0.15f);
		}

		// 4. 종료 후 원래대로 복구 (흰색을 넣으면 텍스처 원래 색이 나옵니다)
		ChangeAllRenderersColor(originalColor);
		visualCoroutine = null;
	}
	private void ChangeAllRenderersColor(Color color)
	{
		if (playerRenderers == null) return;

		foreach (var renderer in playerRenderers)
		{
			if (renderer == null) continue;

			// URP 대응
			if (renderer.material.HasProperty("_BaseColor"))
				renderer.material.SetColor("_BaseColor", color);

			// 일반 대응
			renderer.material.color = color;

			// [추가 꿀팁] 만약 색 변화가 너무 미미하다면 'Emission(발광)'을 건드려야 할 수도 있습니다.
			// 필요하면 아래 주석을 해제하세요.
			// if (renderer.material.HasProperty("_EmissionColor")) {
			//     renderer.material.EnableKeyword("_EMISSION");
			//     renderer.material.SetColor("_EmissionColor", color * 0.5f); // 은은하게 빛남
			// }
		}
	}
}




