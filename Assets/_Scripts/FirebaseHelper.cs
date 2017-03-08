using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseHelper : MonoBehaviour {

    const string POSITION = "pos";
    const string ROTATION = "rot";
    const string PRECISION = ".000";

    Transform trackedTransform;
    DatabaseReference myRef;
    WaitForSecondsRealtime heartBeatDelay;

    void Awake() {
        heartBeatDelay = new WaitForSecondsRealtime(1f);
    }

    void Start() {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://together-34547.firebaseio.com/");
        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;

        myRef = root.Child(SystemInfo.deviceUniqueIdentifier);
    }

    public void SetTrackedTransform(Transform trackedTransform) {
        StopCoroutine(HeartBeat());
        this.trackedTransform = trackedTransform;
        StartCoroutine(HeartBeat());
    }

    IEnumerator HeartBeat() {
        myRef.Child(POSITION).SetValueAsync(trackedTransform.rotation.ToString(PRECISION));
        while (true) {
            myRef.Child(ROTATION).SetValueAsync(trackedTransform.rotation.ToString(PRECISION));
            yield return heartBeatDelay;
        }
    }
}
