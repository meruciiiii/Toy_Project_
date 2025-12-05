using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private float playerSpeed = 10f;

	[SerializeField] private PlayerInput Input;
	private Rigidbody player_r;

	private void Awake() {
		TryGetComponent(out player_r);
	}

	private void FixedUpdate() {
		Move();
	}

	private void Move() {
		Vector3 playerMovement = new Vector3(Input.playerDirection_x, 0f, Input.playerDirection_z) * playerSpeed * Time.deltaTime;

		player_r.MovePosition(player_r.position + playerMovement);
	}
}
