using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    InputManager inputManager;
    CanvasManager canvasManager;

    public Transform lookTarget;   //The object the camera wil follow
    public LayerMask collisionLayers;
    public float distFromTraget = 20;
    public Vector2 pitchMinMax = new Vector2(-40, 60);

    public float rotationSmoothTime = 0.12f;
    public float mouseSensitivity;
    Vector3 currentRotation;
    Vector3 positionVelocity;

    float yaw;
    float pitch;

    [Header("Camera Speed")]
    public float pushSpeed = 5f;
    public float normalSpeed = 20f;
    public float wallPush = 0.7f;

    [Header("Camera Collision")]
    public float evenCloserDistanceToPlayer = 1f;
    bool pitchLock;

    [Header("Camera Zoom")]
    public float cameraZoomSensitivity;
    public float farthestDistance, nearestDistance;

    [Header("Aim Mode")]
    public bool inAimMode;
    public float aimSpeed;
    public Transform camParent;
    public Transform aimPoint;
    public Transform finishAimPoint;
    Transform player;
    bool adjustingPos;


    private void Awake()
    {
        _instance = this;
        inputManager = FindObjectOfType<InputManager>();
        currentRotation = transform.localEulerAngles;
    }

    private void Start()
    {
        canvasManager = CanvasManager.instance;
        player = aimPoint.parent;
        yaw = transform.localEulerAngles.y;
    }


    public void LateUpdate()
    {
        if (CanvasManager.instance.freezeTime)
        {
            return;
        }
        HandleAllCameraMovement();
    }

    public void HandleAllCameraMovement()
    {
        if (!inAimMode)
        {
            HandleCamareZoom();
            CheckCollision(lookTarget.position - transform.forward * distFromTraget);
            RotateCamera();
            CheckWall();
        }
        else
        {
            RotateCameraAimMode();
        }
    }

    private void RotateCamera()
    {
        yaw += inputManager.cameraInputX * mouseSensitivity;
        pitch -= inputManager.cameraInputY * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        if (pitchLock)
        {
            pitch = pitchMinMax.y;
        }

        currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), 0.5f);
        transform.localEulerAngles = currentRotation;
    }

    private void CheckCollision(Vector3 retPoint)
    {
        RaycastHit hit;

        if (Physics.Linecast(lookTarget.position, retPoint, out hit, collisionLayers))
        {
            Vector3 norm = hit.normal * wallPush;
            Vector3 pushPoint = hit.point + norm;

            if (Vector3.Distance(Vector3.SmoothDamp(transform.position, pushPoint, ref positionVelocity, pushSpeed), lookTarget.position) <= evenCloserDistanceToPlayer)
            {

            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, pushPoint, ref positionVelocity, pushSpeed);
                //transform.position = Vector3.Lerp(transform.position, pushPoint, 0.8f);
            }
            return;
        }
        transform.position = Vector3.SmoothDamp(transform.position, retPoint, ref positionVelocity, normalSpeed);
        pitchLock = false;
    }

    private void CheckWall()
    {
        Ray ray = new Ray(lookTarget.position, -lookTarget.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.2f, out hit, 0.7f, collisionLayers))
        {
            pitchLock = true;
        }
        else
        {
            pitchLock = false;
        }
    }

    private void HandleCamareZoom()
    {
        if (inputManager.zoomInput != 0 )
        {
            if (inputManager.zoomInput > 0)
            {
                distFromTraget -= cameraZoomSensitivity;
            }
            else
            {
                distFromTraget += cameraZoomSensitivity;
            }
        }
        distFromTraget = Mathf.Clamp(distFromTraget, nearestDistance, farthestDistance);
    }

    public void EnterAimMode()
    {
        inAimMode = true;
        transform.SetParent(aimPoint);
        StartCoroutine(AdjustToAimPos());
    }
    IEnumerator AdjustToAimPos()
    {
        while (Vector3.Distance(transform.position, aimPoint.position) > aimSpeed)
        {
            transform.position = Vector3.Lerp(transform.position, aimPoint.position, aimSpeed);
            yield return null;
        }
        transform.position = aimPoint.position;
    }
    public void ExitAimMode()
    {
        transform.SetParent(camParent);
        inAimMode = false;
        //StartCoroutine(AdjustToFinishAimPos());
    }
    IEnumerator AdjustToFinishAimPos()
    {
        while (Vector3.Distance(transform.position, finishAimPoint.position) > aimSpeed)
        {
            transform.position = Vector3.Lerp(transform.position, finishAimPoint.position, aimSpeed);
            yield return null;
        }
        inAimMode = false;
        transform.SetParent(camParent);
        transform.position = finishAimPoint.position;
    }

    private void RotateCameraAimMode()
    {
        yaw += inputManager.cameraInputX * mouseSensitivity;
        pitch -= inputManager.cameraInputY * mouseSensitivity;
        Debug.Log(pitch);
        pitch = Mathf.Clamp(pitch, -40, 10);

        currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), 0.5f);

        transform.localEulerAngles = new Vector3 (currentRotation.x, 0, 0);
        player.localEulerAngles = new Vector3(0, currentRotation.y, 0);
    }

}