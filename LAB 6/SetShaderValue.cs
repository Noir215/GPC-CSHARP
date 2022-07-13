using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderValue : MonoBehaviour
{
    public float minValue = 0;
    public float maxValue = 1;
    public float velocity = 1.0f;
    public float currentValue = 0;
    public string shaderVariableName = "_MixValue";
    
    // Update is called once per frame
    void Update()
    {
        currentValue += velocity * Time.deltaTime;

        float inRangeValue = minValue + Mathf.PingPong(currentValue, (maxValue - minValue));
        GetComponent<MeshRenderer>().sharedMaterial.SetFloat(shaderVariableName, inRangeValue);
    }
}
