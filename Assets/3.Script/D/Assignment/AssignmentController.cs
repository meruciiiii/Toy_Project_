using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentController : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision) {
		if(collision.transform.CompareTag("Platform")) {
			TryGetComponent(out Rigidbody rigid);
			rigid.AddForce(Vector3.up * 100f);		//땅에 부딫혔을때 튀어오르기
			StartCoroutine(deleteSelf());			//일정 시간 후 삭제 코루틴
		}
		else if (collision.transform.CompareTag("Player")) {
			Destroy(gameObject);
			//시간 늘어나는 시스템 구현
		}
	}

	private IEnumerator deleteSelf() {
		yield return new WaitForSeconds(0.75f);
		Destroy(gameObject);
	}
}
