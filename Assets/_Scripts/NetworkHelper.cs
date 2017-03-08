using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHelper : MonoBehaviour {

    NetworkManager networkManager;

    void Awake () {
        networkManager = GetComponent<NetworkManager>();
        networkManager.StartHost();
	}
	
}
