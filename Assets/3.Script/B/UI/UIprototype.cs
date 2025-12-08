using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIprototype : MonoBehaviour
{
    [SerializeField] private GameObject currentUIObject; //현재 보여주는 UI Group 활성화 해주세요
    [SerializeField] private GameObject nextUIObject; //현재 보여주는 UI Group 비활성화 해주세요

    public void SwitchUIObject()
    {
        if (currentUIObject != null) currentUIObject.SetActive(false);

        if (nextUIObject != null) nextUIObject.SetActive(true);
        else Debug.Log("UIprototype nextUIObject 이 없습니다. SerializeField이기에 컴포넌트에 GameObject를 연결해주세요");

        //교체 해서 재사용하겠습니다.
        GameObject tempObject = currentUIObject;
        currentUIObject = nextUIObject;
        nextUIObject = tempObject;
        Debug.Log("ShowNextUIObjectandSwitch");
    }


    public void SceneLoader(string scenename) //버튼용 메소드 입니다.
    {
        StartCoroutine(LoadSceneandButtonDelay(scenename));
    }

    public IEnumerator LoadSceneandButtonDelay(string scenename)//바로 로드되면 버튼 사운드가 끊겨서 그 사운드에 맞춰서
    {
        yield return new WaitForSeconds(0.68f);// 이 시간을 맞춰 수정해주세요.
        Debug.Log("SceneLoad!");
        SceneManager.LoadScene(scenename);
    }

    public void ExitGame()//게임 종료용 메서드입니다.
    {
        Debug.Log("ExitGame!!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
