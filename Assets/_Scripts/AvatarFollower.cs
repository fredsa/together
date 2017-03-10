using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class AvatarFollower : MonoBehaviour {

    DatabaseReference ourRoot;

    Quaternion targetRot;

    void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Avatar.LERP_RATE);
    }

    public void Init (DatabaseReference ourRoot) {
        this.ourRoot = ourRoot;
        ourRoot.ChildChanged += OnChildChanged;
        ourRoot.ChildRemoved += OnChildRemoved;
	}

    void OnChildChanged (object sender, ChildChangedEventArgs args)
    {
        switch(args.Snapshot.Key) {
        case Avatar.HEADSET:
            TransformChanged(transform, args.Snapshot);
            break;
        case Avatar.CONTROLLER:
//            TransformChanged(controllerTransform, args.Snapshot);
            break;
        }
    }

    void OnChildRemoved (object sender, ChildChangedEventArgs e)
    {
        ourRoot.ChildChanged -= OnChildChanged;
        ourRoot.ChildRemoved -= OnChildRemoved;
        Destroy(gameObject);
    }

    void TransformChanged(Transform t, DataSnapshot snapshot) {
        t.position = GetPos(snapshot.Child(Avatar.POS));
        targetRot = GetRot(snapshot.Child(Avatar.ROT));
    }

    Vector3 GetPos(DataSnapshot snapshot) {
        return Avatar.POS_PRECISION * ParseLongVector3(snapshot);
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
