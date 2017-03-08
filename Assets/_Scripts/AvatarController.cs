using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarController : NetworkBehaviour {

    FirebaseHelper helper;

    public override void OnStartLocalPlayer() {
        // Look towards origin
        transform.rotation = Quaternion.LookRotation(-transform.localPosition, transform.up);
        Camera.main.transform.parent.localPosition = transform.localPosition;
        Camera.main.transform.parent.localRotation = transform.localRotation;

        helper = GameObject.FindObjectOfType<FirebaseHelper>();
        helper.SetTrackedTransform(Camera.main.transform);
    }
	
}
