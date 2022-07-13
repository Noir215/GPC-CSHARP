using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarHelicoptero : MonoBehaviour
{
    public float turnSpeed = 0.1f;
    public GameObject motor;

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        if (motor.GetComponent<RotarHelices>().engine)
        {
            // Rotación del helicoptero
            if (Input.GetKey(KeyCode.W)) {
                if (rotation.x < 30)
                    transform.Rotate(turnSpeed, 0, 0);
            }
            else if (rotation.x > 0.1 && rotation.x < 30.1)
                transform.Rotate(-turnSpeed, 0, 0);

            if (Input.GetKey(KeyCode.S)) {
                if (rotation.x > 330 || rotation.x < 0.1) {
                    transform.Rotate(-turnSpeed, 0, 0);
                    Debug.Log("Rotation: " + rotation.x);
                }
            }
            else if (rotation.x < 359.9 && rotation.x > 229.9)
                transform.Rotate(turnSpeed, 0, 0);
        }
    }
}
