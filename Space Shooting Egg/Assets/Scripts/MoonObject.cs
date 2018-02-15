using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonObject : MovingObject {
    public GameObject motherPlanet;
    public float angleSpeed;
    public Vector3 rotAroundAxis;

    protected override void OverrideStart()
    {
        base.OverrideStart();
        initAccel = motherPlanet.GetComponent<MovingObject>().initAccel;
        angleSpeed = Random.Range(Mathf.Abs(initAccel / 5), 45);
        rotAroundAxis = new Vector3(0, 0, Random.Range(-1f, 1f));
    }
    protected override void OverrideUpdate()
    {
        base.OverrideUpdate();
        if (motherPlanet != null)
        {
            transform.RotateAround(motherPlanet.transform.position, rotAroundAxis, angleSpeed * Time.deltaTime);
        }
        else
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MotherPlanet" || collision.gameObject.tag == "Moon")
        {
            Destroy(gameObject);
        }
    }
}
