using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{
	private void OnEnable() {
		StartCoroutine("despawn");
	}

	private IEnumerator despawn() {
		yield return new WaitForSeconds(4.5f);
		Destroy(gameObject);
	}
}
