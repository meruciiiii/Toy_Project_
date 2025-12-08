using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentSpawner : MonoBehaviour {
	[SerializeField] public float spawn_timer = 0.2f;
	[SerializeField] private MapSize size;
	[SerializeField] private GameObject assignmentPrefabs;
	[SerializeField] private GameObject assignment_debuff_Prefabs;
	[SerializeField] private GameObject hintPrefabs;

	private float currentSpawnTimer;
	private int hint_count = 0;

	[Header("난이도 설정")]
	[Tooltip("게임 시작 시 스폰 주기 (쉬움)")]
	[SerializeField] private float maxSpawnTimer = 1.5f;

	[Tooltip("퇴근 직전 스폰 주기 (어려움)")]
	[SerializeField] private float minSpawnTimer = 0.3f; // 너무 빠르면 0.2f 추천

	public GameObject[] pooling;
	[SerializeField] private int pool_count = 35;
	private int currnet_pool = 0;

	private void Awake() {
		pooling = new GameObject[pool_count];

		for (int i = 0; i < pool_count; i++) {
			pooling[i] = Instantiate(assignmentPrefabs);
			pooling[i].SetActive(false);
		}
		currentSpawnTimer = maxSpawnTimer;
	}

	private void OnEnable() {
		StartCoroutine("Assaignment");
	}

	private IEnumerator Assaignment() {
		while (true)
		{
			// 1. 현재 난이도(스폰 시간) 계산 업데이트
			UpdateDifficulty();

			// 2. 계산된 시간만큼 대기 (매번 새로운 시간 적용)
			yield return new WaitForSeconds(currentSpawnTimer);

			spawn_assignment(assignmentPrefabs);
		}
	}
	private void UpdateDifficulty()
	{
		if (ScoreManager.instance == null) return;

		float currentTime = ScoreManager.instance.GetCurrentTimeMinutes();
		float startTime = ScoreManager.instance.GetStartTime();
		float maxDiffTime = ScoreManager.instance.GetMaxDifficultyTime(); // 18:00

		// 진행률 계산 (0.0 ~ 1.0)
		// (현재시간 - 시작시간) / (목표시간 - 시작시간)
		float progress = Mathf.Clamp01((currentTime - startTime) / (maxDiffTime - startTime));

		// Lerp(선형 보간)를 이용해 진행률에 따라 스폰 주기를 부드럽게 줄임
		// progress가 0이면 maxSpawnTimer(1.5초), 1이면 minSpawnTimer(0.3초)가 됨
		currentSpawnTimer = Mathf.Lerp(maxSpawnTimer, minSpawnTimer, progress);
	}

	public void spawning_hint(int hint_count) {
		this.hint_count = hint_count;
		StartCoroutine(Hint());
	}
	private IEnumerator Hint() {
		WaitForSeconds wfs = new WaitForSeconds(spawn_timer);
		for (int i = 0; i < hint_count; i++) {
			spawn_hint(hintPrefabs);
			yield return wfs;
		}
	}

	private void spawn_assignment(GameObject prefabs) {
		//매개변수로 프리팹을 받는 이유
		// - Assignment와 Hint의 소환 방식이 동일하기 때문에, 코드 재사용성을 위하여.
		GameObject assignment = pooling[currnet_pool];
		currnet_pool++;
		if (currnet_pool >= pool_count) {
			currnet_pool = 0;
		}
		spawn_rnd(assignment);
	}
	public void spawn_hint(GameObject prefabs) {
		GameObject hint = Instantiate(prefabs);
		spawn_rnd(hint);
	}
	
	private void spawn_rnd(GameObject falling_object) {
		Vector3 rnd_pos = Vector3.zero;
		int safe = 0;
		while (safe < 5) {
			//safe = 무한으로 위치 탐색으로 인한 오류를 막기 위한 안전 장치 변수
			safe++;

			//무작위 위치를 찾습니다. [MapSize] Scriptable Object 범위에 맞춰 작용.
			rnd_pos = new Vector3(Random.Range(size.LimitMin.x, size.LimitMax.x), 10f, Random.Range(size.LimitMin.z, size.LimitMax.z));

			//BoxCast로 겹침을 확인합니다. BoxCast(위치/박스크기/방향/부딫힌 오브젝트 반환/회전값/거리) 매개변수를 가지고 있습니다.
			if (Physics.BoxCast(
				rnd_pos,
				falling_object.transform.localScale * 0.5f,
				Vector3.down,
				out RaycastHit hit,
				Quaternion.identity,
				10f
			)) {
				//만약 부딫힌 오브젝트의 태그가 Obstacle가 아닐 경우, Assignment 위치 변경 후 while 반복문을 탈출합니다.
				if (!hit.transform.CompareTag("Obstacle")) {
					falling_object.SetActive(true);
					falling_object.transform.position = rnd_pos;

					//랜덤값 만큼 Assigment을 회전시킵니다.
					falling_object.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
					break;
				}
			}
		}
	}

	public void spawn_assignment_from_debuff(int count) {
		for(int i = 0; i < count; i++) {
			GameObject assignment_debuff = Instantiate(assignment_debuff_Prefabs);
			spawn_rnd(assignment_debuff);
			Debug.Log("민찬짱");
		}
	}

	public void DelaySpawn(float skillDuration) {
		StartCoroutine(DelaySpawn_co(skillDuration));
	}
	private IEnumerator DelaySpawn_co(float skillDuration) {
		StopCoroutine("Assaignment");
		yield return new WaitForSeconds(skillDuration);
		StartCoroutine("Assaignment");
	}
}
