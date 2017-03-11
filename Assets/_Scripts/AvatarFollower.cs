using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class AvatarFollower : MonoBehaviour {

    DatabaseReference ourRoot;

    Vector3 headTargetPos;
    Vector3 controllerTargetPos;

    Quaternion headTargetRot;
    Quaternion controllerTargetRot;

    Transform headTransform;
    Transform controllerTransform;

    void Awake() {
        headTransform = transform.Find("Head");
        controllerTransform = transform.Find("Controller");
    }

    void Update() {
        headTransform.position = Vector3.Lerp(headTransform.position, headTargetPos, Avatar.LERP_RATE);
        headTransform.rotation = Quaternion.Lerp(headTransform.rotation, headTargetRot, Avatar.LERP_RATE);

        controllerTransform.position = Vector3.Lerp(controllerTransform.position, controllerTargetPos, Avatar.LERP_RATE);
        controllerTransform.rotation = Quaternion.Lerp(controllerTransform.rotation, controllerTargetRot, Avatar.LERP_RATE);
    }

    public void Init (DatabaseReference ourRoot) {
        this.ourRoot = ourRoot;
        ourRoot.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogWarningFormat("ourRoot.GetValueAsync faulted: {0}", task.Exception);
                Destroy(gameObject);
            }
            // set intial targets
            SetTargets(task.Result.Child(Avatar.HEADSET));
            SetTargets(task.Result.Child(Avatar.CONTROLLER));
        });
        ourRoot.ChildChanged += OnChildChanged;
        ourRoot.ChildRemoved += OnChildRemoved;
	}

    void OnChildChanged (object sender, ChildChangedEventArgs args)
    {
        // update targets
        SetTargets(args.Snapshot);
    }

    void SetTargets (DataSnapshot snapshot)
    {
        switch(snapshot.Key) {
        case Avatar.HEADSET:
            headTargetPos = GetPos(snapshot.Child(Avatar.POS));
            headTargetRot = GetRot(snapshot.Child(Avatar.ROT));
            break;
        case Avatar.CONTROLLER:
            controllerTargetPos = GetPos(snapshot.Child(Avatar.POS));
            controllerTargetRot = GetRot(snapshot.Child(Avatar.ROT));
            break;
        }
    }

    void OnChildRemoved (object sender, ChildChangedEventArgs e)
    {
        Destroy(gameObject);
    }
    
    void OnDestroy() {
        ourRoot.ChildChanged -= OnChildChanged;
        ourRoot.ChildRemoved -= OnChildRemoved;
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
