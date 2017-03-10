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

        AvatarController self = Instantiate(avatarPrefab).AddComponent<AvatarController>();
        self.name += " - self";
        GhostIt(self.gameObject);
    }

    void GhostIt (GameObject go)
    {
        foreach (MeshRenderer mr in go.GetComponentsInChildren<MeshRenderer>()) {
            Color color = mr.material.color;
            color.a *= .5f;
            mr.material.color = color;
        }

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
        follower.name += " - follower " + args.Snapshot.Key;
        follower.Init(args.Snapshot.Reference);
    }
}
