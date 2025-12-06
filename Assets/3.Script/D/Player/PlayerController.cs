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
		player_r.linearVelocity = Vector3.zero;
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, size.LimitMin.x, size.LimitMax.x),
			transform.position.y,
			Mathf.Clamp(transform.position.z, size.LimitMin.z, size.LimitMax.z)
			);
	}

	protected virtual void Move() {
		Vector3 playerDirection = new Vector3(Input.playerDirection_x, 0f, Input.playerDirection_z);
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
