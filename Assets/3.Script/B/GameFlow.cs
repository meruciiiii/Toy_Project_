using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private GameObject Ppipi_ob;
    [SerializeField] private GameObject Sin_ob;
    [SerializeField] private GameObject Byon_ob;

    [SerializeField] private bool isRankingScene;

    private void Start()
    {
        //Ppipi_ob.SetActive(false);
        //Ppipi_ob.SetActive(false);
        //Ppipi_ob.SetActive(false);

        switch (GameManager.instance.player_chractor)
        {
            case Charactor.Ppipi:
                {
                    Ppipi_ob.SetActive(true);
                    break;
                }
            case Charactor.Sin:
                {
                    Sin_ob.SetActive(true);
                    break;
                }
            case Charactor.Byon:
                {
                    Byon_ob.SetActive(true);
                    break;
                }
        }
        if (!isRankingScene)
        {
            GameManager.instance.ResetScore();
        }
    }
}
