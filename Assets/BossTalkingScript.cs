using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTalkingScript : MonoBehaviour {



    private Animator ThisAnimatorCon;

    public string InitializeAnime;
    float dist = 0;


    // Use this for initialization
    void Start () {

        ThisAnimatorCon = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        GameObject PlayerChar = GameObject.FindGameObjectWithTag("Respawn");

        if (PlayerChar)
        {
            Vector3 relativePos = PlayerChar.transform.position - transform.position;
            dist = Vector3.Distance(PlayerChar.transform.position, transform.position);
            if (dist>4) {
                CharacterStopsTalking();
            }
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            rotation.x = transform.rotation.x;
            rotation.z = transform.rotation.z;
            transform.rotation = rotation;


        } 



    }

    


    private void AnimationToPlay(string BoolName)
    {

        DisableOtherAnime(ThisAnimatorCon, BoolName);

        ThisAnimatorCon.SetBool(BoolName, true);



    }

    private void DisableOtherAnime(Animator ThisAnimatorCon, string AnimeString)
    {

        foreach (AnimatorControllerParameter parame in ThisAnimatorCon.parameters)
        {

            if (parame.name != AnimeString)
            {

                ThisAnimatorCon.SetBool(parame.name, false);
            }

        }

    }

    void OnTriggerStay(Collider other)        
    {
         
        if (other.gameObject.tag.Contains("ThePointerCube") == true && dist <= 4)
        {
            CharacterStartsTalking();
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("ThePointerCube") == true)
        {
            CharacterStopsTalking();

        }
    }




    void CharacterStartsTalking() {

        AnimationToPlay("talking");



    }


    void CharacterStopsTalking()
    {

        AnimationToPlay("idle");



    }



}
