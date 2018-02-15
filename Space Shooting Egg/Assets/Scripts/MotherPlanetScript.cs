using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherPlanetScript : MovingObject {
    public float gravity;
    public LayerMask detectLayer;

    protected override void OverrideUpdate()
    {
        base.OverrideUpdate();
        Vector3 instPos = myCam.ScreenToWorldPoint(new Vector3(0, 0, myCam.transform.position.z * -1));
        Vector3 instPos2 = myCam.ScreenToWorldPoint(new Vector3(myCam.pixelWidth, myCam.pixelHeight, myCam.transform.position.z * -1));
        Collider[] others = Physics.OverlapSphere(transform.position, Vector3.Distance(instPos, instPos2), detectLayer);
        foreach (Collider other in others)
        {
            if (other.tag != "Moon" && other.tag != "MotherPlanet")
            {
                float dist = Vector3.Distance(transform.position, other.transform.position);
                Vector3 heading = (transform.position - other.transform.position).normalized;
                MovingObject obj = other.GetComponent<MovingObject>();
                if (obj.rbody != null)
                    obj.rbody.AddForce(heading * (gravity / dist) * obj.rbody.mass);
            }
        }
    }
}
