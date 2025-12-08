using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AssignmentController : MonoBehaviour {
	private Rigidbody assignment_r;
	private float defaultDrag = 0f; // 기본 공기 저항 (0이면 슝 떨어짐)
	private float slowDrag = 10f;   // 스킬 발동 시 공기 저항 (높을수록 천천히 떨어짐)

	private void Awake() {
		TryGetComponent(out assignment_r);
		defaultDrag = assignment_r.linearDamping; // 원래 설정값 저장
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.transform.CompareTag("Platform")) {
			TryGetComponent(out Rigidbody rigid);
			rigid.AddForce(Vector3.up * 100f);
			StartCoroutine(deleteSelf());
		}
	}

	private IEnumerator deleteSelf() {
		yield return new WaitForSeconds(0.75f);
		gameObject.SetActive(false);
	}
	public void SetSlowMode(bool isSlow) {
		if (assignment_r == null) return;

		if (isSlow) {
			assignment_r.linearDamping = slowDrag; // 저항을 높여 천천히 떨어지게 함
										 // 만약 이미 가속도가 너무 붙어있다면 속도를 한번 깎아줌
			if (assignment_r.linearVelocity.y < 0) assignment_r.linearVelocity *= 0.1f;
		}
		else {
			assignment_r.linearDamping = defaultDrag; // 원상 복구
		}
	}
}
