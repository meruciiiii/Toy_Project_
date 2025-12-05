using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
	TimeReducer, // 재미나이 (시간 단축)
	SkillTrigger // 전용 아이템 (스킬 발동)
}

public class PlayerController_C : MonoBehaviour 
{
	[Header("기본 설정")]
	[SerializeField] protected float playerSpeed = 10f; // 자식이 수정할 수 있게 protected로 변경
	[SerializeField] private PlayerInput Input;
	[SerializeField] private MapSize size;

	protected Rigidbody player_r;

	protected virtual void Awake() {
		TryGetComponent(out player_r);
	}

	protected virtual void FixedUpdate() {
		Move();

		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, size.LimitMin.x, size.LimitMax.x),
			transform.position.y,
			Mathf.Clamp(transform.position.z, size.LimitMin.z, size.LimitMax.z)
			);

	}

	private void Move() {
		Vector3 playerMovement = new Vector3(Input.playerDirection_x, 0f, Input.playerDirection_z) * playerSpeed * Time.deltaTime;

		player_r.MovePosition(player_r.position + playerMovement);
	}
	/*
	protected virtual void OnTriggerEnter(Collider other)
	{
		// 아이템 스크립트가 있다고 가정 (ItemController)
		if (other.CompareTag("Item"))
		{
			// 아이템 컴포넌트 가져오기
			ItemController item = other.GetComponent<ItemController>();
			if (item != null)
			{
				OnItemCollected(item.itemType);
				Destroy(other.gameObject); // 아이템 삭제
			}
		}
	}

	public void OnItemCollected(ItemType type)
    {
        if (type == ItemType.TimeReducer)
        {
			//제미나이 내용
            Debug.Log("공통: 퇴근 시간 단축!");
            GameManager.Instance.//퇴근 시간도 줄이고 코딩력도 올리고
        }
        else if (type == ItemType.SkillTrigger)
        {
            Debug.Log("전용: 스킬 발동!");
            UseSkill(); // 자식이 구현한 스킬 실행
        }
    }
	protected virtual void ActivateSkill() { }

	protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Assingnment"))
        {
            OnHitAssingnment();
        }
    }

	protected virtual void OnHitAssingnment()
    {
        Debug.Log("기본: 과제 피격 (퇴근 지연 5분)");
        // GameManager.Instance.AddPenalty(5);
    }

	*/

}
