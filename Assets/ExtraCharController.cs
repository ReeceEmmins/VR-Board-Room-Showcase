using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCharController : MonoBehaviour {



    private Animator ThisAnimatorCon;

    public string InitializeAnime;


    // Use this for initialization
    void Start () {

        ThisAnimatorCon = GetComponent<Animator>();

        if (InitializeAnime == "") {

            InitializeAnime = "talking";

        }

        AnimationToPlay(InitializeAnime);

    }
	
	// Update is called once per frame
	void Update () {
		
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

}
