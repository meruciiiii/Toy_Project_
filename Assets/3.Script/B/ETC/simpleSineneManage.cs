using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class simpleSineneManage : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SceneLoad(string Scenename)
    {
        SceneManager.LoadScene(Scenename);
    }
}
