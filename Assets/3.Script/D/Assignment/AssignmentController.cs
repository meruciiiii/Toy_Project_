using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentController : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision) {
		if(collision.transform.CompareTag("Platform")) {
			StartCoroutine(deleteSelf());
		}
	}

	private IEnumerator deleteSelf() {
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
