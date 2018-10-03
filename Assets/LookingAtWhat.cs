using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtWhat : MonoBehaviour {


    public PlayerController ScPlayer;

    public Animator ChangerBar;



    public float startMarker;
    public float endMarker;
 

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Character") == true) {

            if (other.gameObject.tag == "CharacterChanger")
            {
                StartCoroutine(Example());

                return;

            }

            ScPlayer.ChangeGameChar(other.gameObject.tag);

        }

       
    }

    IEnumerator Example()
    {
        ChangerBar.SetBool("open", true);
        yield return new WaitForSeconds(15);
        ChangerBar.SetBool("open", false);
    }






}
