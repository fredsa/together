﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityHelper : MonoBehaviour {

    int[] VALUES = new int[] {0, 2, 4, 8};

    int index;

    void Awake() {
        SetMsaa(0);
    }

    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            SetMsaa((index + 1) % VALUES.Length);
        }
    }

    void SetMsaa (int index)
    {
        this.index = index;
        QualitySettings.antiAliasing = VALUES[index];
        GetComponent<Text>().text = string.Format("{0}x MSAA", VALUES[index]);
    }

}