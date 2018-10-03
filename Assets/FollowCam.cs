using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Transform>().SetPositionAndRotation(CamTransform.position, CamTransform.rotation);
    
    
	}

    public Transform CamTransform;

}
