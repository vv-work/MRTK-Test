using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 _axis = new Vector3(0, 1, 0);

    [SerializeField]
    private float _speed = 1;

    // Update is called once per frame
    private void Update()
    {
        
        transform.Rotate(_axis,_speed*Time.deltaTime);
    }
}
