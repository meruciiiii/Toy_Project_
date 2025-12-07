using UnityEngine;

public class ItemController : MonoBehaviour
{
	[SerializeField] private float rotation_speed = 100f;
	[SerializeField] private float sin_speed = 1f;
	[SerializeField] private float sin_range = 0.5f;
	private Vector3 item_pos;
	private Vector3 temp_pos;
	
	private void OnEnable() {
		item_pos = transform.position;
	}

	private void Update() {
		temp_pos = item_pos;
		//time.fixedTime을 쓰는 이유 : 입력받는 수치에 영향이 없고, 계속 꾸준히 고정적으로 움직이기 때문.
		//time.deltaTime은 프레임 저하에도 부드럽게 움직임을 구현하기 위해서 사용.
		//Sin 함수는 위아래로 둥둥 떠다님을 표현하기 위해...
		temp_pos.y += Mathf.Sin(sin_speed * Mathf.PI * Time.fixedTime) * sin_range;

		//변경된 값으로 회전 및 이동(위아래)
		transform.Rotate(new Vector3(0f, rotation_speed, 0f) * Time.deltaTime);
		transform.position = temp_pos;
	}

	private void OnTriggerEnter(Collider collision) {
		if(collision.transform.CompareTag("Player")) {
			Debug.Log("플레이어와 아이템 충돌");
			Destroy(gameObject);
		}
	}
}
