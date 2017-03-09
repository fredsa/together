using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class AvatarFollower : MonoBehaviour {

	void Awake () {
		
	}
	
    public void Init (DatabaseReference ourRoot) {
        ourRoot.Child(Avatar.HEADSET).ChildChanged += OnHeadsetChanged;
        ourRoot.Child(Avatar.CONTROLLER).ChildChanged += OnControllerChanged;
	}

    void OnHeadsetChanged(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        TransformChanged(transform, args.Snapshot);
    }

    void OnControllerChanged(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        //TransformChanged(controllerTransform, args.Snapshot);
    }

    void TransformChanged(Transform t, DataSnapshot snapshot) {
        switch (snapshot.Key) {
            case Avatar.POS:
                t.position = GetPos(snapshot);
                break;
            case Avatar.ROT:
                t.rotation = GetRot(snapshot);
                break;
        }
    }

    Vector3 GetPos(DataSnapshot snapshot) {
        return ParseLongVector3(snapshot);
    }

    Quaternion GetRot(DataSnapshot snapshot) {
        return Quaternion.Euler(ParseLongVector3(snapshot));
    }

    Vector3 ParseLongVector3(DataSnapshot snapshot) {
        float x = (float)(long) snapshot.Child(Avatar.X).Value;
        float y = (float)(long) snapshot.Child(Avatar.Y).Value;
        float z = (float)(long) snapshot.Child(Avatar.Z).Value;
        return new Vector3(x, y, z);
    }

}
