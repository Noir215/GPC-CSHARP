using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent indica que este script solo se puede añadir a objetos que ya tengan un componente de ese tipo
// En este caso para dibujar las normales, necesitamos que haya una mesh que tenga toda la información
// En Unity las mesh se asignan a un objeto utilizando el componente MeshFilter
[RequireComponent(typeof(MeshFilter))]
public class DibujarNormales : MonoBehaviour
{
    private Mesh mesh;
    public float tamanyo = 0.1f;

    public bool dibujarSoloSeleccionado = false;

    private void OnDrawGizmos() {
        // Si no se ha seleccionado dibujar las normales solo con el objeto seleccionado
        if (!dibujarSoloSeleccionado)
            DrawNormals();
    }

    void OnDrawGizmosSelected() {
        DrawNormals();
    }

    void DrawNormals() {
        // Leer la mesh que tiene toda la información sobre los vértices
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null)
            return;

        // Seleccionar el color del gizmo
        Gizmos.color = Color.yellow;

        // Para cada una de las normales
        for (int i = 0; i < mesh.normals.Length; i++) {

            // Calcular la posición en coordenadas globales de cada vertice
            // Las coordenadas de un vertice almacenadas en la mesh son locales y están transformadas por traslaciones, rotaciones y escalados
            Vector3 pos = transform.TransformPoint(mesh.vertices[i]);
            Vector3 normalPos = transform.TransformPoint(mesh.vertices[i] + (mesh.normals[i] * tamanyo));

            // El tamaño de la esfera es siempe una decimo de la linea
            float tamanyoSphere = (normalPos - pos).magnitude * 0.1f;

            // Dibujar los gizmos
            Gizmos.DrawSphere(pos, tamanyoSphere);
            Gizmos.DrawLine(pos, normalPos);
        }
    }
}
