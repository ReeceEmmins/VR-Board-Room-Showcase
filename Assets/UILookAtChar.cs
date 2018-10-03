using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtChar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        GameObject PlayerChar = GameObject.FindGameObjectWithTag("Respawn");

        if (PlayerChar)
        {
            Vector3 relativePos = PlayerChar.transform.position - transform.position;
           
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            rotation.x = transform.rotation.x;
            rotation.z = transform.rotation.z; 
            transform.rotation = rotation;

          
        }
        else
        {
            Debug.Log("not Found");
        }



    }
}
