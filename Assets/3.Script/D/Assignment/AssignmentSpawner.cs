using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentSpawner : MonoBehaviour
{
	[SerializeField] private float spawn_timer = 0.2f;
	[SerializeField] private MapSize size;
	[SerializeField] private GameObject assainmentPrefabs;
	private GameObject assainment;

	private void OnEnable() {
		StartCoroutine(Assainment());
	}

	private IEnumerator Assainment() {
		WaitForSeconds wfs = new WaitForSeconds(spawn_timer);
		while(true) {
			spawn_assignment();
			yield return wfs;
		}
	}
	
	private void spawn_assignment() {
		assainment = Instantiate(assainmentPrefabs);
		assainment.transform.position = new Vector3(
			Random.Range(size.LimitMin.x, size.LimitMax.x),
			10f,
			Random.Range(size.LimitMin.z, size.LimitMax.z)
			);
	}
}
