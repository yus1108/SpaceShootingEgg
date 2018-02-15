using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovingObject {
    public bool isGameOver;
    public float TOP_BOUNDARY = 0;
    public float BTM_BOUNDARY = Screen.height;
    public float Left_BOUNDARY = 0;
    public float Right_BOUNDARY = Screen.width;
    public float size = 5f;

    protected override void OverrideStart()
    {
        base.OverrideStart();
        isGameOver = false;
    }
    protected override void OverrideUpdate()
    {
        base.OverrideUpdate();

        TOP_BOUNDARY = (1 - size / 100) * Screen.height;
        BTM_BOUNDARY = Screen.height * (0.1f - size / 100);
        Left_BOUNDARY = size / 100 * Screen.width;
        Right_BOUNDARY = Screen.width - size / 100 * Screen.width;
        Vector3 pPos = myCam.WorldToScreenPoint(transform.position);
        if (pPos.y >= TOP_BOUNDARY || pPos.y <= BTM_BOUNDARY)
        {
            rbody.velocity = new Vector3(rbody.velocity.x, rbody.velocity.y * -1, rbody.velocity.z);
        }
        if (pPos.x >= Right_BOUNDARY || pPos.x <= Left_BOUNDARY)
        {
            rbody.velocity = new Vector3(rbody.velocity.x * -1, rbody.velocity.y, rbody.velocity.z);
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 heading = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            rbody.AddForce(heading * initAccel);
        }

        if (Input.GetButton("Fire1"))
        {
            Vector3 heading = (Input.mousePosition -  transform.position).normalized;
            rbody.AddForce(heading * initAccel);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGameOver = true;
    }
}
