using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroMovement : MonoBehaviour
{
    public float positionY;
    public float speedMove;
    public float speedRun;
    public Camera Cam;
    public KeyCode runKey;
    private Vector3 moveVector;
    private CharacterController ch_controller;
    private Vector3 _lookPosition;
    private Vector3 _lookDirection;

    public Vector3 LookPosition { get { return _lookPosition; } }
    public Vector3 LookDirection { get { return _lookDirection; } }

    // Start is called before the first frame update
    void Start()
    {
        ch_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CharterMove();
        TransRotation();
        if (transform.position.y != positionY)
            transform.position = new Vector3(transform.position.x, positionY + transform.lossyScale.y / 2, transform.position.z);
    }
    public float Speed
    {
        get
        {
            float speed;
            if (Input.GetKey(runKey))
                speed = speedRun;
            else
                speed = speedMove;

            if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("Horizontal") != 0) speed /= (float)Math.Sqrt(2);
            return speed;
        }
    }

    public Vector3 MoveVector
    {
        get
        {
            return moveVector;
        }
    }

    private void CharterMove()
    {
        float speed = Speed;

        if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("Horizontal") != 0) speed /= (float)Math.Sqrt(2);

        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");
        moveVector.y = 0;
        ch_controller.Move(moveVector * Time.deltaTime * Speed);

    }
    private void TransRotation()
    {
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            _lookPosition = hit.point;
        }
        _lookDirection = _lookPosition - transform.position;

        transform.LookAt(transform.position + new Vector3(_lookDirection.x, 0, _lookDirection.z));
    }
}
