using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Database;

public class AvatarController : MonoBehaviour {

    FirebaseHelper firebaseHelper;
    WaitForSeconds heartBeatDelay;

    void Awake() {
        firebaseHelper = GameObject.FindObjectOfType<FirebaseHelper>();
        heartBeatDelay = new WaitForSeconds(Avatar.HEART_BEAT_INTERVAL);
    }

    void Start() {
        // Look towards origin
        transform.rotation = Quaternion.LookRotation(-transform.localPosition, transform.up);
        // Set player position
        transform.localPosition = RandomPosition();

        Camera.main.transform.parent.localPosition = transform.localPosition;
        Camera.main.transform.parent.localRotation = transform.localRotation;
        StartCoroutine(HeartBeat());
    }

    Vector3 RandomPosition ()
    {
        float y = Avatar.HEAD_HEIGHT + Random.Range (-.2f, .2f);
        float x = Mathf.Sign(Random.Range(-1f,1f)) * 2f;
        float z = Mathf.Sign(Random.Range(-1f,1f)) * 2f;
        return new Vector3(x, y, z);
    }

    void Update() {
        transform.rotation = Camera.main.transform.rotation;
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
        WriteLongVector3(db, pos / Avatar.POS_PRECISION);
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
