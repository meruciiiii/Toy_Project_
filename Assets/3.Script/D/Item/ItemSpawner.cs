using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	[SerializeField] GameObject Item1Prefabs;
	[SerializeField] GameObject Item2Prefabs;
	[SerializeField] MapSize size;
	[SerializeField] float Item_Spawn_Timer = 15f;
	private GameObject[] ItemPrefabses;

	private void OnEnable() {
		StartCoroutine("Item");
		ItemPrefabses = new GameObject[2];
		ItemPrefabses[0] = Item1Prefabs;
		ItemPrefabses[1] = Item2Prefabs;
	}

	private IEnumerator Item() {
		WaitForSeconds wfs = new WaitForSeconds(Item_Spawn_Timer);
		while(true) {
			yield return wfs;
			spawn_item();
		}
	}

	private void spawn_item() {
		Vector3 rnd_pos = new Vector3(Random.Range(size.LimitMin.x, size.LimitMax.x), 1f, Random.Range(size.LimitMin.z, size.LimitMax.z));
		int rnd_int = Random.Range(0, 2);
		Debug.Log(rnd_int);
		Instantiate(ItemPrefabses[rnd_int], rnd_pos, Quaternion.identity);
	}
}
