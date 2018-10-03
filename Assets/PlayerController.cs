using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public GameObject PlayerCamera;
    CharacterController PlayerCon;








    public float PlayerSpeed = 2;
    public float CameraSpeed = 2;
    public float Gravity = 50f;

    public float speed = 2.0f;
    public float toggleAngle = 50.0f;
    public bool moveForward;


    public float overallSpeed;



    float MoveFrontAndBack = 0;
    float MoveLeftAndRight = 0;

    float CamerRotateX = 0;
    float CamerRotateY = 0;

    
    float FallSpeed = 1;

 


    public float VRSpeeed;
    float ThisVRSpeeed;

    private bool cooldown = false;
    private const float ShootInterval = 1f;


    //main CharacterModels

    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;
    public GameObject Character4;
    public GameObject Character5;
    public GameObject Character6;
    public GameObject Character7;
    public GameObject Character9;


    // Use this for initialization
    void Start () {

        PlayerCon = GetComponent<CharacterController>();
        ChangeGameChar("Character5");




    }
	
	// Update is called once per frame
	void Update () {

       

          if (Camera.main.transform.localEulerAngles.x >= toggleAngle && Camera.main.transform.localEulerAngles.x < 90.0f)
        {
            moveForward = true;
        }
        else { moveForward = false;
        }
       
        if (moveForward)
        {

            ThisVRSpeeed = VRSpeeed;
            MoveFrontAndBack = ThisVRSpeeed * PlayerSpeed;
        }
        else {

            ThisVRSpeeed = 0f;
        }

        if (PlayerCon.isGrounded) {
            FallSpeed = 0;
        }
        else {

            FallSpeed = 1;


        }

        MoveFrontAndBack = ThisVRSpeeed * PlayerSpeed;

        Vector3 Movement = new Vector3(0, 0, MoveFrontAndBack);
        Movement = transform.rotation * Movement;
        Movement.y -= Gravity * Time.deltaTime * FallSpeed;
        PlayerCon.Move(Movement * Time.deltaTime);

        CamerRotateX = Input.GetAxis("Mouse X") * CameraSpeed;
        CamerRotateY = Input.GetAxis("Mouse Y") * CameraSpeed;

    


        var CharacterRotation = Camera.main.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;

        transform.rotation = CharacterRotation;



        Vector3 horizontalVelocity = PlayerCon.velocity;
        horizontalVelocity = new Vector3(PlayerCon.velocity.x, 0, PlayerCon.velocity.z);
        overallSpeed = PlayerCon.velocity.magnitude;

     

    }


    public void ChangeGameChar(string Name)
    {
        if (!cooldown)
        {
            cooldown = true;

            //and shoot bullet...
            Invoke("CoolDown", ShootInterval);
        }
        else {

            return;

        }



        GameObject ThisGame;
        GameObject Des = GameObject.FindGameObjectWithTag("Respawn");
        Destroy(Des);





        if (Name == "Character1")
        {

            ThisGame = Instantiate(Character1, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;

        }


        if (Name == "Character2")
        {

            ThisGame = Instantiate(Character2, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;
        }

        if (Name == "Character3")
        {

            ThisGame = Instantiate(Character3, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;
        }

        if (Name == "Character4")
        {

            ThisGame = Instantiate(Character4, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;
        }

        if (Name == "Character5")
        {

            ThisGame = Instantiate(Character5, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;
        }

        if (Name == "Character6")
        {
        
            ThisGame = Instantiate(Character6, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;
        }

        if (Name == "Character7")


        {
       
            ThisGame = Instantiate(Character7, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;  
        }

        if (Name == "Character9")
        {


            ThisGame = Instantiate(Character9, gameObject.transform.position, Quaternion.identity);
            ThisGame.transform.parent = gameObject.transform;
            ThisGame.transform.localScale = gameObject.transform.localScale / 3.2f;
            ThisGame.transform.rotation = transform.rotation;

        }





    }


    void CoolDown()
    {
        cooldown = false;
    }


}
