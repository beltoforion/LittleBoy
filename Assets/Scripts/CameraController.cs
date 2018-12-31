using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region ROTATE
    private float _sensitivity = 0.5f;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation = Vector3.zero;
    private bool _isRotating;


    #endregion

    void Update()
    {
        if (_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            // apply rotation
            //_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

            _rotation.z = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity; // rotate
            gameObject.transform.Rotate(_rotation); // store new mouse position
            _mouseReference = Input.mousePosition;
            // rotate
            //transform.Rotate(_rotation);
            transform.eulerAngles += _rotation;
            // store mouse
        }
    }

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;

        // store mouse position
        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }

}
