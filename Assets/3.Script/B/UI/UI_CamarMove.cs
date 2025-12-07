using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine; 패키지 설치후 사용, 그런데 이거 다 같이 설치해야 돼는거 아닌지(main에서?)

public class UI_CamarMove : MonoBehaviour
{
    //카메라 이동 로직으로 단순하게 transform을 사용하면 오차가 생겨 원래 위치로 돌아가기 힘들다.
    //(강제 보정해도 힘든듯함, 코드도 더러움)
    //Cinemachine으로 구현하는게 가장 바람직해보임
    //UI 교체는 전이랑 비슷함

    /*
    // 인스펙터에 할당
    [SerializeField] private CinemachineVirtualCamera wideVcam; // 초기 넓은 시점
    [SerializeField] private CinemachineVirtualCamera focusVcam; // 집중 시점

    // 현재 UI 그룹과 다음 UI 그룹도 필요
    [SerializeField] private GameObject currentUIGroup;
    [SerializeField] private GameObject nextUIGroup;

    // 현재 활성화된 시점이 Wide인지 Focus인지 추적합니다.
    private bool isFocused = false;

    public void ToggleFocus()
    {
        // 1. 현재 UI 숨기기 (이동 시작 전)
        if (currentUIGroup != null)
            currentUIGroup.SetActive(false);

        // 2. 카메라 우선순위 변경 (이동 시작)
        if (!isFocused)
        {
            // Wide(20) -> Focus(30): 집중 시점으로 이동
            focusVcam.Priority = 30;
            wideVcam.Priority = 20;
        }
        else
        {
            // Focus(30) -> Wide(20): 원래 시점으로 복귀
            wideVcam.Priority = 30;
            focusVcam.Priority = 20;
        }

        isFocused = !isFocused;

        // 3. UI 전환 대기 코루틴 시작
        StartCoroutine(WaitForBlendAndSwitchUI());
    }

    private IEnumerator WaitForBlendAndSwitchUI()
    {
        // Cinemachine Brain의 블렌드 시간(예: 1초)보다 약간 더 길게 기다립니다.
        // 이 시간 동안 카메라가 부드럽게 이동합니다.
        yield return new WaitForSeconds(1.1f);

        // 4. 이동 완료 후 UI 그룹 전환 및 보이기
        if (nextUIGroup != null)
            nextUIGroup.SetActive(true);

        GameObject tempUI = currentUIGroup;
        currentUIGroup = nextUIGroup;
        nextUIGroup = tempUI; // UI 그룹 교체
    }
    */


    /*2차 이동로직(transform)
    //[Header("Camera 이동")]
    //[SerializeField] private GameObject camera_ob;
    //[SerializeField] private Transform cameraStartTransform;      // 시작 위치
    //[SerializeField] private Transform camaraTargetTransform;     // 목표 위치
    //[SerializeField] private float moveSpeed;      // 이동 속도
    //[SerializeField] private float rotationSpeed;      // 회전 속도
    //private bool moveCamera = false;
    //
    //[SerializeField] private GameObject currentUIGroup; // 현재 보여주는 UI 그룹
    //[SerializeField] private GameObject nextUIGroup;    // 다음에 보여줄 UI 그룹
    //
    //// 화면 전환과 버튼에 넣을 메서드 public
    //public void SwitchUIWithCamera()
    //{
    //    // UI 이동 중에는 현재 UI 숨기기
    //    if (currentUIGroup != null)
    //        currentUIGroup.SetActive(false);
    //
    //    // 배경 이동 시작
    //    StartCoroutine(MoveCameraRoutine());
    //
    //    // 이동 완료후 UI 전환 코루틴 시작
    //    StartCoroutine(WaitForBgAndShowUI());
    //}
    //
    //private IEnumerator MoveCameraRoutine()
    //{
    //    if (moveCamera) yield break;
    //
    //    moveCamera = true; // 이동 시작 (WaitForBgAndShowUI를 위해)
    //
    //    camera_ob.transform.position = cameraStartTransform.position;
    //    camera_ob.transform.rotation = cameraStartTransform.rotation;
    //
    //    while (Vector3.Distance(camera_ob.transform.position, camaraTargetTransform.position) > 0.2f)
    //    {
    //        //camera_ob.transform.position = Vector3.MoveTowards(
    //        //    camera_ob.transform.position,
    //        //    camaraTargetTransform.position,
    //        //    moveSpeed * Time.deltaTime);
    //        //
    //        //moveSpeed -= 0.00005f;
    //
    //        camera_ob.transform.position = Vector3.Lerp(
    //        camera_ob.transform.position,
    //        camaraTargetTransform.position,
    //        moveSpeed * Time.deltaTime); // Lerp로 인해 감속 효과가 발생합니다. Vector용
    //        ///비율 보간 이동
    //
    //        yield return null; // 위치 이동 루프
    //    }
    //
    //    // 위치 스냅 (오차 제거)
    //    camera_ob.transform.position = camaraTargetTransform.position;
    //
    //    // 2단계: 회전 이동 (Position 이동 완료 후 Rotation만 이동)
    //    // 회전 이동 완료 시점까지 루프 실행
    //    // Quaternion.Angle을 사용하여 두 회전 사이의 각도를 체크
    //    while (Quaternion.Angle(camera_ob.transform.rotation, camaraTargetTransform.rotation) > 0.3f)
    //    {
    //        //camera_ob.transform.rotation = Quaternion.RotateTowards(
    //        //    camera_ob.transform.rotation,
    //        //    camaraTargetTransform.rotation,
    //        //    rotationSpeed * Time.deltaTime);
    //        //
    //        //moveSpeed -= 0.00005f;
    //        camera_ob.transform.rotation = Quaternion.Slerp(
    //        camera_ob.transform.rotation,
    //        camaraTargetTransform.rotation,
    //        rotationSpeed * Time.deltaTime);// Slerp로 인해 감속 효과가 발생합니다. Quaternion 용
    //        //비율 보간 이동
    //
    //        yield return null; // 회전 이동 루프
    //    }
    //
    //    // 회전 스냅 (오차 제거)
    //    camera_ob.transform.rotation = camaraTargetTransform.rotation;
    //
    //    Vector3 tempPos = cameraStartTransform.position;
    //    Quaternion tempRot = cameraStartTransform.rotation;
    //
    //    cameraStartTransform.position = camaraTargetTransform.position;
    //    cameraStartTransform.rotation = camaraTargetTransform.rotation;
    //
    //    camaraTargetTransform.position = tempPos;
    //    camaraTargetTransform.rotation = tempRot;
    //
    //    moveCamera = false; // 이동 종료
    //}
    //
    //// UI 전환 대기 코루틴
    //private IEnumerator WaitForBgAndShowUI()
    //{
    //    // 카메라 이동이 끝날 때까지 기다림 (MoveCameraRoutine이 moveCamera를 false로 만들 때까지)
    //    while (moveCamera)
    //        yield return null;
    //
    //    // 이동 완료 후 다음 UI 보여주기
    //    if (nextUIGroup != null)
    //        nextUIGroup.SetActive(true);
    //    GameObject temp = currentUIGroup;
    //    currentUIGroup = nextUIGroup;
    //    nextUIGroup = temp;//재사용 하기 위해 ui 교체, position 교체와 동일.
    //    Debug.Log("대기및 ui교체");
    //}
    */

    //기존 Update로직
    //private void Update()//배경 이동루틴
    //{
    //    if (moveCamera && cameraStartTransform != null && camaraTargetTransform != null)
    //    {
    //        camera_ob.transform.position = Vector3.MoveTowards(
    //        camera_ob.transform.position,
    //        camaraTargetTransform.position,
    //        moveSpeed * Time.deltaTime);
    //
    //        camera_ob.transform.rotation = Quaternion.RotateTowards(
    //        camera_ob.transform.rotation,
    //        camaraTargetTransform.rotation,
    //        rotationSpeed * Time.deltaTime);
    //
    //        if (Vector3.Distance(camera_ob.transform.position, camaraTargetTransform.position) < 0.01f)
    //        {
    //            camera_ob.transform.position = camaraTargetTransform.position;//위치 스냅 시키기
    //            camera_ob.transform.rotation = camaraTargetTransform.rotation;//회전값도 시키기
    //
    //            moveCamera = false;//이동 종료
    //
    //            Vector3 tempVec = cameraStartTransform.position;//위치 교체 셔플알고리즘과 비슷하게.
    //            cameraStartTransform.position = camaraTargetTransform.position;
    //            camaraTargetTransform.position = tempVec;
    //
    //            Quaternion tempQua = cameraStartTransform.rotation;//위치 교체 셔플알고리즘과 비슷하게.
    //            cameraStartTransform.rotation = camaraTargetTransform.rotation;
    //            camaraTargetTransform.rotation = tempQua;
    //        }
    //    }
    //}
    //
    ////화면 전환과 버튼에 넣을 메서드 public
    //public void SwitchUIWithBg()
    //{
    //    // UI 이동 중에는 현재 UI 숨기기
    //    if (currentUIGroup != null)
    //        currentUIGroup.SetActive(false);
    //
    //    // 배경 이동 시작
    //    moveCamera = true;
    //
    //    // 코루틴에서 배경 이동 완료 후 UI 전환
    //    StartCoroutine(WaitForBgAndShowUI());
    //}
    //
    //private IEnumerator WaitForBgAndShowUI()
    //{
    //    // 카메라 이동이 끝날 때까지 기다림
    //    while (moveCamera)
    //        yield return null;
    //
    //    // 이동 완료 후 다음 UI 보여주기
    //    if (nextUIGroup != null)
    //        nextUIGroup.SetActive(true);
    //    GameObject temp = currentUIGroup;
    //    currentUIGroup = nextUIGroup;
    //    nextUIGroup = temp;//재사용 하기 위해 ui 교체, position 교체와 동일.
    //    Debug.Log("대기및 ui교체");
    //}
}
