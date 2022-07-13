using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoInfo : MonoBehaviour
{
    public TextMesh velocidad;
    public GameObject helicoptero;
    float speed, altitude;
    // Update is called once per frame
    void Update()
    {
        speed = helicoptero.GetComponent<MoverHelicoptero>().curr_speed;
        altitude = helicoptero.GetComponent<MoverHelicoptero>().transform.position.y;
        velocidad.text = "Velocidad: " + speed + "\n"
                       + "Altitud: " + altitude;
    }
}
