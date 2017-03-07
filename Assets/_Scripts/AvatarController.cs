using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarController : NetworkBehaviour {

    public override void OnStartLocalPlayer() {
        // Look towards origin
        transform.rotation = Quaternion.LookRotation(-transform.localPosition, transform.up);
        Camera.main.transform.parent.localPosition = transform.localPosition;
        Camera.main.transform.parent.localRotation = transform.localRotation;
    }
	
}
