using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using System;
using UnityEngine.Assertions;

public class VRHelper : MonoBehaviour {

    public bool editorEmulateCardboard;

    public GameObject GvrViewerMainPrefab;
    public GameObject GvrControllerMainPrefab;
    public GameObject GvrControllerPointerPrefab;
    public GameObject GvrReticlePointerPrefab;

	void Awake () {
        GvrViewer viewer = Instantiate(GvrViewerMainPrefab).GetComponent<GvrViewer>();
        if (Application.isEditor) {
            viewer.VRModeEnabled = false;
        }

        string deviceName = VRSettings.loadedDeviceName;
        if (Application.isEditor) {
            Assert.AreEqual("", deviceName);
            deviceName = editorEmulateCardboard ? "cardboard" : "daydream";
        }

        switch (deviceName) {
         case "cardboard":
            GvrReticlePointer cardboardPointer = Instantiate(GvrReticlePointerPrefab).GetComponent<GvrReticlePointer>();
            cardboardPointer.transform.SetParent(Camera.main.transform, false);
            break;
        case "daydream":
            GvrController controllerMain = Instantiate(GvrControllerMainPrefab).GetComponent<GvrController>();
            GvrControllerVisualManager daydreamPointer = Instantiate(GvrControllerPointerPrefab).GetComponent<GvrControllerVisualManager>();
            daydreamPointer.transform.SetParent(Camera.main.transform.parent, false);
            break;
        default:
            throw new NotSupportedException("\"" + deviceName + "\"");
        }
	}
	
}
