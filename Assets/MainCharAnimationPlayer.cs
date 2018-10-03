using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharAnimationPlayer : MonoBehaviour {


    public PlayerController PlayerSc;
    private Animator ThisAnimatorCon;


    private const string IDLE_ANIME_BOOL = "idle";
    private const string WALK_ANIME_BOOL = "walking";
    private const string SIT_ANIME_BOOL = "sittingdown";
    private const string TALK_ANIME_BOOL = "talking";

    // Use this for initialization
    void Start () {

        ThisAnimatorCon = GetComponent<Animator>();
     
       

    }
	
	// Update is called once per frame
	void Update () {


        if (PlayerSc == null) {

            GameObject Player = GameObject.FindWithTag("Player");
            PlayerSc = Player.GetComponent<PlayerController>();
            return;
        }


        if (PlayerSc.overallSpeed == 0)
        {

            AnimationToPlay(IDLE_ANIME_BOOL);

        }
        else
        {

            AnimationToPlay(WALK_ANIME_BOOL);

        }

    }




    public void AnimationToPlay(string BoolName)
    {

        DisableOtherAnime(ThisAnimatorCon, BoolName);

        ThisAnimatorCon.SetBool(BoolName, true);



    }

    public void DisableOtherAnime(Animator ThisAnimatorCon, string AnimeString)
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
