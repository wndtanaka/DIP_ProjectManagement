using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehaviour
{
    public float offset = 1;
    public float radius = 1;
    public float jitter = 0.2f;

    Vector3 targetDir;
    Vector3 randomDir;
    public override Vector3 GetForce()
    {
        Vector3 force = Vector3.zero;
        float randX = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);
        float randZ = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);

        #region Calculate Random Direction
        randomDir = new Vector3(randX, 0, randZ);
        randomDir = randomDir.normalized;
        randomDir *= jitter;
        #endregion

        #region Calculate Target Direction
        targetDir += randomDir;
        targetDir = targetDir.normalized;
        targetDir *= radius;
        #endregion

        Vector3 seekPos = transform.position + targetDir;
        seekPos += transform.forward * offset;

        //#region GizmosGL
        //Vector3 forwardPos = transform.position + transform.forward * offset;
        //GizmosGL.color = Color.red;
        //GizmosGL.AddCircle(forwardPos, radius, Quaternion.LookRotation(Vector3.down));
        //GizmosGL.color = Color.blue;
        //GizmosGL.AddCircle(seekPos, radius * 0.6f, Quaternion.LookRotation(Vector3.down));
        //#endregion

        #region Wander
        Vector3 direction = seekPos - transform.position;
        if (direction.magnitude > 0)
        {
            Vector3 desiredForce = direction.normalized * weighting;
            force = desiredForce - owner.velocity;
        }
        #endregion
        return force;
    }
}
