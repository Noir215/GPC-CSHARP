using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarPalanca : MonoBehaviour
{
    public float turnSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        // Rotación de la palanca
        if (Input.GetKey(KeyCode.W))
        {
            if (rotation.x < 30)
                transform.Rotate(turnSpeed, 0, 0);
        }
        else if (rotation.x > 0.1 && rotation.x < 30.1)
            transform.Rotate(-turnSpeed, 0, 0);

        if (Input.GetKey(KeyCode.S))
        {
            if (rotation.x > 330 || rotation.x < 0.1)
                transform.Rotate(-turnSpeed, 0, 0);
        }
        else if (rotation.x < 359.9 && rotation.x > 229.9)
            transform.Rotate(turnSpeed, 0, 0);

        if (Input.GetKey(KeyCode.A))
        {
            if (rotation.z < 30)
                transform.Rotate(0, 0, turnSpeed);
        }
        else if (rotation.z > 0.1 && rotation.z < 30.1)
            transform.Rotate(0, 0, -turnSpeed);

        if (Input.GetKey(KeyCode.D))
        {
            if (rotation.z > 330 || rotation.z < 0.1)
                transform.Rotate(0, 0, -turnSpeed);
        }
        else if (rotation.z < 359.9 && rotation.z > 229.9)
            transform.Rotate(0, 0, turnSpeed);
    }
}
