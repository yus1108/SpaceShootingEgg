using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {
    public enum Directions {
        X_AXIS,
        Y_AXIS,
        Z_AXIS
    }
    protected float DESTROY_POS;
    public Camera myCam;

    public bool isScore = false;
    public float initAccel;
    public Directions direction;
    [HideInInspector]
    public Rigidbody rbody;
    protected Vector3 rotVelocity;

    // Use this for initialization
    void Start()
    {
        OverrideStart();
    }
    protected virtual void OverrideStart()
    {
        rbody = GetComponent<Rigidbody>();
        rbody.AddForce(GetDirection(direction) * initAccel * rbody.mass);
        rotVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        myCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        OverrideUpdate();
    }
    protected virtual void OverrideUpdate()
    {
        Vector3 instPos = myCam.ScreenToWorldPoint(new Vector3(0, 0, myCam.transform.position.z * -1));
        DESTROY_POS = instPos.x;
        Vector3 instPos2 = myCam.ScreenToWorldPoint(new Vector3(myCam.pixelWidth, myCam.pixelHeight, myCam.transform.position.z * -1));
        DESTROY_POS -= Vector3.Distance(instPos, instPos2);

        transform.Rotate(rotVelocity);
        if (transform.position.x < instPos.x)
        {
            if (transform.tag != "Moon")
            {
                if (!isScore)
                {
                    GameObject.FindObjectOfType<LevelScript>().score += GameObject.FindObjectOfType<LevelScript>().difficulty;
                    isScore = true;
                }
            }
        }
        if (transform.position.y < DESTROY_POS)
        {
            if (transform.tag == "Moon")
                    GameObject.FindObjectOfType<LevelScript>().score += GameObject.FindObjectOfType<LevelScript>().difficulty;
            Destroy(gameObject);
        }
    }

    public Vector3 GetDirection(Directions dir)
    {
        switch (dir)
        {
            case Directions.X_AXIS:
                return Vector3.right;
            case Directions.Y_AXIS:
                return Vector3.up;
            case Directions.Z_AXIS:
                return Vector3.forward;
        }

        return Vector3.zero;
    }

    public void DestScore(GameObject g)
    {

    }
}
