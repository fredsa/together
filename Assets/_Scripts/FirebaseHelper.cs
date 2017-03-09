using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseHelper : MonoBehaviour {

    public DatabaseReference ourRoot { get; private set; }

    public GameObject AvatarFollowerPrefab;

     DatabaseReference sharedRoot;

     void Awake() {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://together-34547.firebaseio.com/");
        sharedRoot = FirebaseDatabase.DefaultInstance.RootReference;
        ourRoot = sharedRoot.Child(SystemInfo.deviceUniqueIdentifier);
    }

    void OnEnable() {
        sharedRoot.ChildAdded += OnChildAdded;
    }

    void OnDisable() {
        sharedRoot.ChildAdded -= OnChildAdded;
    }

    void OnChildAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        GameObject avatar = Instantiate(AvatarFollowerPrefab);
        AvatarFollower follower = avatar.AddComponent<AvatarFollower>();
        follower.Init(args.Snapshot.Reference);
    }
}
