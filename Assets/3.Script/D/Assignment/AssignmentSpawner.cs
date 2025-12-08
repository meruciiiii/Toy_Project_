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
	private bool isSlowMode = false;

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
		//매개변수로 프리팹을 받는 이유
		// - Assignment와 Hint의 소환 방식이 동일하기 때문에, 코드 재사용성을 위하여.
		assignment = Instantiate(prefabs);
		Vector3 rnd_pos = Vector3.zero;
		int safe = 0;
		while(safe < 5) {
			//safe = 무한으로 위치 탐색으로 인한 오류를 막기 위한 안전 장치 변수
			safe++;

			//무작위 위치를 찾습니다. [MapSize] Scriptable Object 범위에 맞춰 작용.
			rnd_pos = new Vector3(Random.Range(size.LimitMin.x, size.LimitMax.x), 10f,Random.Range(size.LimitMin.z, size.LimitMax.z));

			//BoxCast로 겹침을 확인합니다. BoxCast(위치/박스크기/방향/부딫힌 오브젝트 반환/회전값/거리) 매개변수를 가지고 있습니다.
			if(Physics.BoxCast(	
				rnd_pos,
				assignment.transform.localScale * 0.5f,
				Vector3.down,
				out RaycastHit hit,
				Quaternion.identity,
				10f
			)) {
				//만약 부딫힌 오브젝트의 태그가 Platform일 경우, Assignment 위치 변경 후 while 반복문을 탈출합니다.
				if(hit.transform.CompareTag("Platform")) {
					assignment.transform.position = rnd_pos;

					//랜덤값 만큼 Assigment을 회전시킵니다.
					assignment.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
					break;
				}
			}
		}
	}

	public void SetSlowMode(bool active) {
		isSlowMode = active;
	}
	private void spawn_assignment() {
		assignment = Instantiate(assignmentPrefabs);

		// [추가] 생성되자마자 슬로우 모드 적용 여부 확인
		if (isSlowMode) {
			var controller = assignment.GetComponent<AssignmentController>();
			if (controller != null) controller.SetSlowMode(true);
		}
	}
}
