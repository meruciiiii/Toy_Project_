using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        // y축 회전만 변경, xy는 그대로
        transform.Rotate(0f, rotationSpeed * Time.deltaTime,0f, Space.Self);
    }
}