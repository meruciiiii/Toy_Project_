using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentSpawner : MonoBehaviour
{
	[SerializeField] private float spawn_timer = 0.2f;
	[SerializeField] private MapSize size;
	[SerializeField] private GameObject assignmentPrefabs;
	private GameObject assignment;

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
		assignment = Instantiate(assignmentPrefabs);
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
					break;
				}
			}
		}
	}
}
