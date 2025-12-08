using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAssignmentController : MonoBehaviour
{
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
}
