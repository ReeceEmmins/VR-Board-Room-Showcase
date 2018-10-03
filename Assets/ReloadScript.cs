using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ReloadScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reload()
    {
        EditorSceneManager.LoadScene(EditorSceneManager.GetActiveScene().buildIndex);
    }
}
