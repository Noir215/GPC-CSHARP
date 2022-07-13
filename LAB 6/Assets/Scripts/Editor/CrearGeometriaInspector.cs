using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrearGeometria))]
[CanEditMultipleObjects]
public class CrearGeometriaInspector : Editor {
    CrearGeometria crearGeometria;

    private void OnEnable() {
        crearGeometria = target as CrearGeometria;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generar Geometría")) {
            crearGeometria.crear();
        }

        if (GUILayout.Button("Regenerar Mesh")) {
            crearGeometria.crearMesh();
        }
    }
}
