using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverHelicoptero : MonoBehaviour
{
    public float max_speed_up = 2;
    public float max_speed = 10;
    public float turnSpeed = 20;
    public float acceleration = 0.2f;
    public float turbulencia = 0.01f;
    public GameObject motor;
    float curr_speed_up = 0;
    public float curr_speed = 0;
    

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        // Traslación del helicoptero (Arriba y Abajo)
        if (motor.GetComponent<RotarHelices>().engine) {
            transform.Translate(Random.Range(-turbulencia, turbulencia), Random.Range(-turbulencia, turbulencia), Random.Range(-turbulencia, turbulencia));

            if (Input.GetKey(KeyCode.UpArrow)) {
                if (curr_speed_up < max_speed_up)
                    curr_speed_up += acceleration;

                moveVector += (Vector3.up * Time.deltaTime * curr_speed_up);
                transform.position += moveVector;
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                if (curr_speed_up < max_speed_up)
                    curr_speed_up += acceleration;

                moveVector += (Vector3.up * Time.deltaTime * curr_speed_up);
                transform.position -= moveVector;
            }

            // Traslación del helicoptero (Alante y Atrás)
            if (Input.GetKey(KeyCode.W)) {
                if (curr_speed < max_speed)
                    curr_speed += acceleration;

                transform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * curr_speed));
            }
            if (Input.GetKey(KeyCode.S)) {
                if (curr_speed < max_speed)
                    curr_speed += acceleration;

                transform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * -curr_speed));
            }

            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                curr_speed_up = 0;

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                curr_speed = 0;

            // Rotación del helicoptero
            if (Input.GetKey(KeyCode.A)) {
                rotation = Quaternion.Euler(0, -turnSpeed * Time.deltaTime, 0);
                transform.rotation *= rotation;
            }

            if (Input.GetKey(KeyCode.D)) {
                rotation = Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
                transform.rotation *= rotation;
            }
        }
    }

}
