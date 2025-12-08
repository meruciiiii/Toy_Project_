using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCountDown : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI countdownText;
	[SerializeField] private GameObject scoreManager;

	private void Awake() {
		scoreManager.SetActive(false);
		Time.timeScale = 0f;
		StartCoroutine(StartGame());
	}

	private IEnumerator StartGame() {
		//WaitForSecondsRealtime << 뒤에 Reatime을 쓰는 이유.
		//빼먹고 그냥 그거 쓰면, Time.timeScale으로 멈추는 동시에 이것도 같이 멈춤.
		//따라서 영향을 안받는 Realtime을 붙힌걸 쓰는것...

		countdownText.text = "3";
		countdownText.color = Color.red;
		yield return new WaitForSecondsRealtime(1f);
		countdownText.text = "2";
		countdownText.color = Color.orange;
		yield return new WaitForSecondsRealtime(1f);
		countdownText.text = "1";
		countdownText.color = Color.green;
		yield return new WaitForSecondsRealtime(1f);
		countdownText.text = "START!";
		countdownText.color = Color.white;
		yield return new WaitForSecondsRealtime(1f);
		Time.timeScale = 1f;
		scoreManager.SetActive(true);
		countdownText.enabled = false;
	}
}
