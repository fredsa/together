using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Database;

public class AvatarController : NetworkBehaviour {

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
        DatabaseReference headsetRoot = firebaseHelper.ourRoot.Child(Avatar.HEADSET);
        DatabaseReference controllerRoot = firebaseHelper.ourRoot.Child(Avatar.CONTROLLER);

        while (GvrPointerManager.Pointer == null) {
            Debug.LogWarning("Waiting for GvrPointerManager.Pointer to be set…");
            yield return heartBeatDelay;
        }
        Debug.LogWarningFormat("GvrPointerManager.Pointer = {0}", GvrPointerManager.Pointer);

        Transform trackedHeadsetTransform = Camera.main.transform;
        Transform trackedControllerTransform  = GvrPointerManager.Pointer.PointerTransform;

        // Fixed position, so set it just once.
        WritePosition(headsetRoot, trackedHeadsetTransform.position);

        while (true) {
            // Always be rotating, so update every heart beat.
            WriteRotation(headsetRoot, trackedHeadsetTransform.rotation);
            WritePosition(controllerRoot, trackedControllerTransform.position);
            WriteRotation(controllerRoot, trackedControllerTransform.rotation);

            yield return heartBeatDelay;
        }
    }

    void WritePosition(DatabaseReference transformRoot, Vector3 pos) {
        DatabaseReference db = transformRoot.Child(Avatar.POS);
        WriteLongVector3(db, pos);
    }

    void WriteRotation(DatabaseReference transformRoot, Quaternion rot) {
        DatabaseReference db = transformRoot.Child(Avatar.ROT);
        WriteLongVector3(db, rot.eulerAngles);
    }

    void WriteLongVector3(DatabaseReference db, Vector3 v) {
        db.Child(Avatar.X).SetValueAsync((int)v.x);
        db.Child(Avatar.Y).SetValueAsync((int)v.y);
        db.Child(Avatar.Z).SetValueAsync((int)v.z);
    }

}
