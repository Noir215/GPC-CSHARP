using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DibujarEjes : MonoBehaviour
{
    // Tamanyo configurable de los ejes
    public float tamanyo = 1.0f;

    private void OnDrawGizmos() {
        // Dibujar 3 lineas una para cada eje

        // En Uniyt right = eje X positivo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * tamanyo);

        // up = eje Y positivo
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * tamanyo);

        // forware = eje Z positivo
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * tamanyo);
    }
}
