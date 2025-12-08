using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	[SerializeField] GameObject ItemPrefabs;
	[SerializeField] MapSize size;
	[SerializeField] float Item_Spawn_Timer = 15f;

	private void OnEnable() {
		StartCoroutine("Item");
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
		Instantiate(ItemPrefabs, rnd_pos, Quaternion.identity);
	}
}
