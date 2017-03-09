using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseHelper : MonoBehaviour {

    public DatabaseReference ourRoot { get; private set; }

     DatabaseReference sharedRoot;

     void Awake() {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://together-34547.firebaseio.com/");
        sharedRoot = FirebaseDatabase.DefaultInstance.RootReference;
        ourRoot = sharedRoot.Child(SystemInfo.deviceUniqueIdentifier);
    }

}
