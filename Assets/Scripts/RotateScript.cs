using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public float speed, autoSpeed;
    public bool autoRotate;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up * speed * Time.deltaTime);

        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(-Vector3.up * speed * Time.deltaTime);

        else if(Input.GetKey(KeyCode.R))
        {
            autoRotate = !autoRotate;
        }
        else
        {
            if(autoRotate) transform.Rotate(-Vector3.up * autoSpeed * Time.deltaTime);
        }
    }
}
