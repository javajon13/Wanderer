using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]private float rotationSpeed = 10f;
    private Vector2 cameraRotation;
    [SerializeField]private Transform cameraFollowTarget;
    private Vector3 cameraRotationValue;

    [SerializeField]private float maxY;
    [SerializeField]private float minY;

    private Transform playerModel;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerModel = GetComponent<PlayerMovement>().playerModel;
    }

    private void OnRotateCamera(InputValue value)
    {
        cameraRotation.x = value.Get<Vector2>().x;
        cameraRotation.y = value.Get<Vector2>().y;
        
        //RotateCamera(cameraRotation);
    }

    private void OnResetCamera(InputValue value)
    {
        cameraRotationValue.x = 0f;
        cameraRotationValue.y = playerModel.localEulerAngles.y;
        //RotateCamera(new Vector2(0, 0));
    }

    private void RotateCamera(Vector2 rotationVector)
    {
        cameraRotationValue.x -= rotationVector.y * rotationSpeed * Time.deltaTime;
        cameraRotationValue.y += rotationVector.x * rotationSpeed * Time.deltaTime;

        cameraRotationValue.x = Mathf.Clamp(cameraRotationValue.x, minY, maxY);
        if(cameraRotationValue.y > 360f) cameraRotationValue.y = 0f;
        if(cameraRotationValue.y < -360f) cameraRotationValue.y = 0f;

        cameraFollowTarget.rotation = Quaternion.Euler(cameraRotationValue.x, cameraRotationValue.y, cameraRotationValue.z);
    }
}
