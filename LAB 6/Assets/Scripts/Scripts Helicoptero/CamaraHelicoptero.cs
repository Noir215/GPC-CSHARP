using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraHelicoptero : MonoBehaviour
{
    public bool third = true;
    public Transform firstP, thirdP;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            third = !third;

        if (third) {
            transform.position = thirdP.position;
            transform.rotation = thirdP.rotation;
        }
        else {
            transform.position = firstP.position;
            transform.rotation = firstP.rotation;
        }
    }
}
