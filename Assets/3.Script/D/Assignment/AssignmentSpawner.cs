using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentSpawner : MonoBehaviour
{
	[SerializeField] private float spawn_timer = 0.2f;
	[SerializeField] private MapSize size;
	[SerializeField] private GameObject assignmentPrefabs;
	[SerializeField] private GameObject hintPrefabs;
	private GameObject assignment;
	private int hint_count = 0;

	private void OnEnable() {
		StartCoroutine(Assainment());
	}

	private IEnumerator Assainment() {
		WaitForSeconds wfs = new WaitForSeconds(spawn_timer);
		while(true) {
			spawn_assignment(assignmentPrefabs);
			yield return wfs;
		}
	}

	public void spawn_hint(int hint_count) {
		this.hint_count = hint_count;
		StartCoroutine(Hint());
	}

	private IEnumerator Hint() {
		WaitForSeconds wfs = new WaitForSeconds(spawn_timer);
		for(int i = 0; i < hint_count; i++) {
			spawn_assignment(hintPrefabs);
			yield return wfs;
		}
	}
	
	private void spawn_assignment(GameObject prefabs) {
		assignment = Instantiate(prefabs);
		Vector3 rnd_pos = Vector3.zero;
		int safe = 0;
		while(safe < 5) {
			safe++;
			rnd_pos = new Vector3(Random.Range(size.LimitMin.x, size.LimitMax.x), 10f,Random.Range(size.LimitMin.z, size.LimitMax.z));
			if(Physics.BoxCast(	
				rnd_pos,
				assignment.transform.localScale * 0.5f,
				Vector3.down,
				out RaycastHit hit,
				Quaternion.identity,
				10f
			)) {
				if(hit.transform.CompareTag("Platform")) {
					assignment.transform.position = rnd_pos;
					assignment.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
					break;
				}
			}
		}
	}
}
