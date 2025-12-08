[Header("C 스킬 설정")]
public float boostSpeed = 13f; // 스킬 시 이동 속도
public float skillDuration = 5f;
private bool isDrifting = false; // 스킬 사용 중 여부
private float RotateSpeed = 180f;

protected override void Awake() {
    TryGetComponent(out player_r);
    transform.position = new Vector3(-9f, 0.45f, -8f);
}

protected override void Skill() {
    StartCoroutine(Wheelchair());
}

protected override void Move() {
    //A,D 키를 누르면 회전값을 변경합니다.
    float turn = Input.playerDirection_z * RotateSpeed * Time.deltaTime;

    //변경된 회전값을 바라봅니다.
    player_r.rotation = player_r.rotation * Quaternion.Euler(0, turn, 0);

    //Shin 캐릭터를 앞으로 꾸준히 이동시킵니다.
    player_r.MovePosition(player_r.position + (transform.forward * playerSpeed * Time.deltaTime));
}

private IEnumerator Wheelchair() {
    Debug.Log("C: 휠체어 폭주 시작 (시간 가속 + 조작 어려움)");
    isDrifting = true;

    // 이동 속도 증가
    float originalSpeed = playerSpeed;
    playerSpeed = boostSpeed;

    // GameManager에게 게임 시간 배속 요청
    // GameManager.Instance.time??

    yield return new WaitForSeconds(skillDuration);

    // 원상 복구
    playerSpeed = originalSpeed;
    isDrifting = false;
    // GameManager.Instance.SetTimeMultiplier(1.0f);

    Debug.Log("C: 스킬 종료");
}
protected override void OnTriggerEnter(Collider collision) {
    if (isDrifting) {
        //스킬 사용중 부딫혔을 경우, 조금 더 많은 시간 추가.
    }
    else {
        base.OnTriggerEnter(collision);
    }
}