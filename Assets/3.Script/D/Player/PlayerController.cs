using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	protected float playerSpeed = 10f;

	[SerializeField] protected PlayerInput Input;
	protected Rigidbody player_r;

	[SerializeField] private MapSize size;

	protected virtual void Awake() {
		TryGetComponent(out player_r);
	}

	private void FixedUpdate() {
		Move();
	}

	private void LateUpdate() {
		//플레이어의 가속은 항상 zero로 맞춰줍니다. 캐릭터는 입력값으로 움직이고, 가속은 존재하지 않기 때문.
		player_r.linearVelocity = Vector3.zero;

		//맵 범위에 맞춘 이동 제한
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, size.LimitMin.x, size.LimitMax.x),
			transform.position.y,
			Mathf.Clamp(transform.position.z, size.LimitMin.z, size.LimitMax.z)
			);
	}

	protected virtual void Move() {
		//입력받은 키에 따른 벡터 저장.
		Vector3 playerDirection = new Vector3(Input.playerDirection_x, 0f, Input.playerDirection_z);

		//이동하는 방향에 맞춰 회전 및 이동시킵니다.
		transform.LookAt(transform.position + playerDirection);
		player_r.MovePosition(player_r.position + (playerDirection * playerSpeed * Time.deltaTime));

	}

	protected virtual void OnTriggerEnter(Collider other) {
		if (other.transform.CompareTag("Item")) {
			Skill();
		}
	}
	protected virtual void Skill() {

	}
}
