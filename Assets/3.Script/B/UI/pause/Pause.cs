using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField] private GameObject ingameUI_ob; // 현재 보여주는 UI 그룹 ingameUI
    [SerializeField] private GameObject pauseUI_ob;    // 다음에 보여줄 UI 그룹 pause

    private void Start()
    {
        audio = transform.GetComponent<AudioSource>();
    }

    // 레거시로 돼어있는데 뉴 인풋시스템으로 바꿔 넣어야 됌.
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.instance.isPause)
        {
            audio.Play();
            GameManager.instance.isPause = true;
            Time.timeScale = 0;
            pauseUI_ob.SetActive(ture);
            ingameUI_ob.SetActive(false);
            Debug.Log("일시정지");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.isPause)
        {
            audio.Play();
            GameManager.instance.isPause = false;
            Time.timeScale = 1;
            pauseUI_ob.SetActive(false);
            ingameUI_ob.SetActive(true);
            Debug.Log("일시정지해제");
        }
      }
     */
}
