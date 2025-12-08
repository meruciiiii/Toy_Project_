using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class RestartScene : MonoBehaviour
{
    public void SceneLoader(string scenename) //버튼용 메소드 입니다.
    {
        SceneManager.LoadScene(scenename);
    }

}
