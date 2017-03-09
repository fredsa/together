using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseHelper : MonoBehaviour {

    public DatabaseReference ourRoot { get; private set; }

    public GameObject avatarPrefab;

     DatabaseReference sharedRoot;

     void Awake() {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://together-34547.firebaseio.com/");
        sharedRoot = FirebaseDatabase.DefaultInstance.RootReference;
        ourRoot = sharedRoot.Child(SystemInfo.deviceUniqueIdentifier);

        Instantiate(avatarPrefab).AddComponent<AvatarController>();
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

        AvatarFollower follower = Instantiate(avatarPrefab).AddComponent<AvatarFollower>();
        follower.Init(args.Snapshot.Reference);
    }
}
