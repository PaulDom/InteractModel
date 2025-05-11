using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 20f;
    [Range(0, 1)] public float focusSpeed = 0.5f;
    public float distance = 10f;
    public float distanceFocus = 10f;
    [Range(1, 1000)] public float rotationSpeed = 2.0f;
    private Transform _target = null;
    private Renderer _rendererTarget = null;
    private Camera _camera;
    private bool _isMovingOnTarget = false;

    public float zoomSpeed = 2f;
    public float minDistance = 1f;
    public float maxDistance = 20f;
    
    public ContentEdit uiContentScript;


    void Update()
    {
        ControlCamera();
        FocusTarget();
        
        FollowTarget();
    }

    void ControlCamera()
    {
        if (Input.GetMouseButton(1))
        {
            MoveCamera();
            RotateAroundTarget();
        }
        ZoomAlongLocalZ();
    }
    

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void MoveCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        if (mouseY != 0)
        {
            bool canMove = true;

            if (_target != null)
            {
                // Проверяем запрещено ли двигаться вверх
                if ((mouseY > 0 && distance >= maxDistance && transform.position.y > _target.position.y))
                {
                    canMove = false;
                }
                // Проверяем запрещено ли двигаться вниз
                else if ((mouseY < 0 && distance >= maxDistance && transform.position.y < _target.position.y))
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                Vector3 movement = new Vector3(0, mouseY * moveSpeed * Time.deltaTime * 10, 0);
                transform.position += movement;
                transform.position = new Vector3(transform.position.x, Mathf.Max(1, transform.position.y),
                    transform.position.z);
            }

        }
    }

    void FocusTarget()
    {
        HandleObjectSelection();
        FocusOnTarget();
    }

    void HandleObjectSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _target = hit.transform;
                _rendererTarget = _target.GetComponent<Renderer>();

                if (_rendererTarget != null)
                {
                    distance = _rendererTarget.bounds.extents.y * 10f;
                    _isMovingOnTarget = true;
                    uiContentScript.addNewObject(_target.gameObject);
                }
            }
        }
    }

    void FocusOnTarget()
    {
        if (_target != null && _isMovingOnTarget)
        {
            Vector3 targetPosition = _target.position - (transform.forward * distanceFocus);
            Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);

            float positionDifference = Vector3.Distance(transform.position, targetPosition);
            float rotationDifference = Quaternion.Angle(transform.rotation, targetRotation);

            if (positionDifference > 0.01f || rotationDifference > 0.1f)
            {
                float speed = focusSpeed * positionDifference;
                transform.position =
                    Vector3.Lerp(transform.position, targetPosition, Mathf.Max(2, speed) * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                    Mathf.Max(10, speed) * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                _isMovingOnTarget = false;
            }
        }
    }

    void FollowTarget()
    {
        if (_target != null)
        {
            transform.LookAt(_target);
            distance = Vector3.Distance(transform.position, _target.position);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            //print(distance);
        }
    }

    void ZoomAlongLocalZ()
    {
        if (_target != null && !_isMovingOnTarget)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                distance -= scroll * zoomSpeed;
                transform.position = _target.position - transform.forward * distance;
            }
        }
    }

    void RotateAroundTarget()
    {
        if (_target != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            
            if (mouseX != 0)
            {
                float angle = mouseX * rotationSpeed * Time.deltaTime;

                // Рассчитываем позицию камеры вокруг цели
                Vector3 direction = transform.position - _target.position; // Направление от цели к камере
                Quaternion rotation = Quaternion.Euler(0, angle, 0); // Вращение вокруг оси Y
                direction = rotation * direction; // Применяем вращение к направлению

                // Обновляем позицию камеры, сохраняя расстояние
                transform.position = _target.position + direction;

                // Камера всегда "смотрит" на цель
                transform.LookAt(_target);
            }
        }
    }
}