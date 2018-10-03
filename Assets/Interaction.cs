using
UnityEngine;

using
System.Collections;



public
class Interactions : MonoBehaviour
{



    public
void SetGazedAt(bool
gazedAt)
    {

        GetComponent<Renderer>().material.color = gazedAt ? Color.red : Color.black;

    }



    public
void MoveUp()
    {

        transform.position += new
Vector3(0f, 1f, 0f);

    }



    void
Start()
    {

        SetGazedAt(false);

    }



    public
void OnGazeEnter()
    {

        SetGazedAt(true);

    }



    public
void OnGazeExit()
    {

        SetGazedAt(false);

    }



    public
void OnGazeTrigger()
    {

        MoveUp();

    }



}