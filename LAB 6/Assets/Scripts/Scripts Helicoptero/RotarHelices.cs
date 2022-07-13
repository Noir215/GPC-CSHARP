using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarHelices : MonoBehaviour
{
    public bool engine = false;
    float speed = 0;
    public float turnSpeed = 100;
    public float acceleration = 5;

    // Update is called once per frame
    void Update()
    {
        // Quaternion de rotacion
        Quaternion rotation = Quaternion.identity;

        // Encendido del motor
        if (Input.GetKeyDown(KeyCode.Space) && engine == false)
            engine = true;
        // Apagado del motor
        else if (Input.GetKeyDown(KeyCode.Space) && engine == true)
            engine = false;

        // Rotacion de aceleracion
        if (engine) {
            if (speed <= turnSpeed)
                speed += acceleration;
            Debug.Log("Speed: " + speed);
            rotation = Quaternion.Euler(0, speed * Time.deltaTime, 0);
            transform.rotation *= rotation;
        }
        else if (!engine) {
            if (speed > 0)
                speed -= acceleration;
            Debug.Log("Speed: " + speed);
            rotation = Quaternion.Euler(0, speed * Time.deltaTime, 0);
            transform.rotation *= rotation;
        }
    }
}
