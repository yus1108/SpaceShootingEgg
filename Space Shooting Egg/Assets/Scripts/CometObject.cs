using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometObject : MovingObject {
    public GameObject cometParticle;

    protected override void OverrideStart()
    {
        initAccel = Random.Range(-2000, -500);
        base.OverrideStart();
        Instantiate(cometParticle, transform.position, Quaternion.Euler(-90, 0, 0)).GetComponent<ParticleScript>().parent = gameObject.transform;
    }
    protected override void OverrideUpdate()
    {
        base.OverrideUpdate();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (rbody.velocity.x * 1.1f >= collision.gameObject.GetComponent<MovingObject>().rbody.velocity.x)
                Destroy(gameObject);
        }
    }
}
