using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Database;

public class AvatarController : NetworkBehaviour {

    const string SEP = "-";

    const string HEADSET = "headset";
    const string CONTROLLER = "controller";

    const string POS = "pos";
    const string ROT = "rot";

    const string X = "x";
    const string Y = "y";
    const string Z = "z";
    const string W = "w";

    FirebaseHelper firebaseHelper;

    WaitForSeconds heartBeatDelay;

    void Awake() {
        firebaseHelper = GameObject.FindObjectOfType<FirebaseHelper>();
        heartBeatDelay = new WaitForSeconds(1f);
    }

    public override void OnStartLocalPlayer() {
        // Look towards origin
        transform.rotation = Quaternion.LookRotation(-transform.localPosition, transform.up);
        Camera.main.transform.parent.localPosition = transform.localPosition;
        Camera.main.transform.parent.localRotation = transform.localRotation;
        StartCoroutine(HeartBeat());
    }

    void OnDestroy() {
        StopAllCoroutines();
    }

    IEnumerator HeartBeat() {
        while (GvrPointerManager.Pointer == null) {
            yield return heartBeatDelay;
        }
        Transform trackedHeadsetTransform = Camera.main.transform;
        Transform trackedControllerTransform  = GvrPointerManager.Pointer.PointerTransform;

        // Fixed position, so set it just once.
        WritePosition(HEADSET + SEP + POS, trackedHeadsetTransform.position);
        WritePosition(CONTROLLER + SEP + POS, trackedControllerTransform.position);

        while (true) {
            // Always be rotating, so update every heart beat.
            WriteRotation(HEADSET + SEP + ROT, trackedHeadsetTransform.rotation);
            WriteRotation(CONTROLLER + SEP + ROT, trackedControllerTransform.rotation);

            yield return heartBeatDelay;
        }
    }

    void WritePosition(string prefix, Vector3 pos) {
        firebaseHelper.ourRoot.Child(prefix + SEP + X).SetValueAsync(pos.x);
        firebaseHelper.ourRoot.Child(prefix + SEP + Y).SetValueAsync(pos.y);
        firebaseHelper.ourRoot.Child(prefix + SEP + Z).SetValueAsync(pos.z);
    }

    void WriteRotation(string prefix, Quaternion rot) {
        firebaseHelper.ourRoot.Child(prefix + SEP + X).SetValueAsync(rot.x);
        firebaseHelper.ourRoot.Child(prefix + SEP + Y).SetValueAsync(rot.y);
        firebaseHelper.ourRoot.Child(prefix + SEP + Z).SetValueAsync(rot.z);
        firebaseHelper.ourRoot.Child(prefix + SEP + W).SetValueAsync(rot.w);
    }

}
