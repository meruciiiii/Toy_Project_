using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
	public float playerDirection_x { get; private set; }
	public float playerDirection_z { get; private set; }

	[SerializeField] private Vector2 playerDirection = Vector2.zero;

	public void Event_Movement(InputAction.CallbackContext context) {
		if (context.phase.Equals(InputActionPhase.Performed)) {
			playerDirection = context.ReadValue<Vector2>();
		}
		else if (context.phase.Equals(InputActionPhase.Canceled)) {
			playerDirection = Vector2.zero;
		}
		playerDirection_x = -playerDirection.y;
		playerDirection_z = playerDirection.x;
	}
}
