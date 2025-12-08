using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
	public float playerDirection_x { get; private set; }
	public float playerDirection_z { get; private set; }

	[SerializeField] private Vector2 playerDirection = Vector2.zero;

	[SerializeField] private Animator playerAnimator;

	public void Event_Movement(InputAction.CallbackContext context) {
		//만약 키를 눌렀을 때, Vector값 조정.
		if (context.phase.Equals(InputActionPhase.Performed)) {
			playerDirection = context.ReadValue<Vector2>();
			if (playerAnimator != null)
			{
				playerAnimator.SetBool("isRun", true);

			}
		}
		//만약 키를 땠을 때, Vector값 조정.
		else if (context.phase.Equals(InputActionPhase.Canceled)) {
			playerDirection = Vector2.zero;
			if (playerAnimator != null)
			{
				playerAnimator.SetBool("isRun", false);

			}
		}

		//플레이어가 W,S를 누르면 벡터의 Y가 조정이 되고 / A,D를 누르면 벡터의 X가 조정이 됩니다.
		//우리는 탑뷰로 앞뒤좌우로 움직이는걸 구현하기 위해, Y는 x(앞뒤) / X는 z(좌우)로 교체합니다.
		//만약 x와 z를 바꾸고 싶다면 Scene의 카메라의 위치를 바꿔주면 됩니다.
		playerDirection_x = -playerDirection.y;
		playerDirection_z = playerDirection.x;
	}
}
