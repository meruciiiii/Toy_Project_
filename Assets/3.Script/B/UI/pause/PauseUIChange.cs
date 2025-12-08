using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIChange : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField] private GameObject ingameUI_ob; // 현재 보여주는 UI 그룹 ingameUI
    [SerializeField] private GameObject pauseUI_ob;    // 다음에 보여줄 UI 그룹 pause

    private void Start()
    {
        audio = transform.GetComponent<AudioSource>();
    }

    public void Resume()//로드 씬이랑 같이쓰면 스테이지 셀럭트랑 넥스트 스테이지 가능.
    {
        audio.Play();
        Debug.Log("pause 풀기");
        Time.timeScale = 1;
        GameManager.instance.isPause = false;
        pauseUI_ob.SetActive(false);
        ingameUI_ob.SetActive(true);
    }
    public void gotoStartScene()
    {
        audio.Play();
        Debug.Log("gotoStartScene 풀기");
        Time.timeScale = 1;
        GameManager.instance.isPause = false;
        pauseUI_ob.SetActive(false);
        ingameUI_ob.SetActive(true);
        SceneManager.LoadScene("StartScene");
    }
}
