using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    // Initialise sound files
    public AudioClip[] soundList;
    private int soundIndex =0;

    void Start() {
        soundList = new AudioClip[]
                    {
                        (AudioClip)Resources.Load("Rapp/Rapp_1"),
                        (AudioClip)Resources.Load("McCarthy/McCarthy_1"),
                        (AudioClip)Resources.Load("Rapp/Rapp_2"),
                        (AudioClip)Resources.Load("McCarthy/McCarthy_4"),
                        (AudioClip)Resources.Load("Rapp/Rapp_3"),
                        (AudioClip)Resources.Load("McCarthy/McCarthy_6"),
                        (AudioClip)Resources.Load("Rapp/Rapp_6"),
                        (AudioClip)Resources.Load("McCarthy/McCarthy_9"),
                        (AudioClip)Resources.Load("Rapp/Rapp_7"),
                        (AudioClip)Resources.Load("McCarthy/McCarthy_10")
                    };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayNext()
    {
        //cycle through the list of sound files
            if (soundIndex <= soundList.Length)
            {
                GetComponent<AudioSource>().clip = soundList[soundIndex];
                GetComponent<AudioSource>().Play();
                Debug.Log("Current Sound: " + soundList[soundIndex]);
                soundIndex++ ;
            }
    }
}

