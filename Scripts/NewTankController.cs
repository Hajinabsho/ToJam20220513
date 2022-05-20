using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewTankController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    //private const string JUMP = "JUMP";

    private float horizontalInput;
    private float verticalInput;
    private float jumpInput; 

    private bool isBreaking;
    private float currentSteerAngle;
    private float currentbreakForce;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;


    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    //flags
    private bool jumpQued = false;

    //movement Field
    private Rigidbody rb;
    private Vector3 forceDirection = Vector3.zero;
    public float jumpForce = 100000;




    Vector3 pos;
    Quaternion rot;


    //mouse movement

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    public Transform turretRotation;


    //Shoot
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 30.0f;

    private bool shoot;

    void RotateTurret()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -=  Input.GetAxis("Mouse Y");


        //speedH
        //speedV

        yaw = Mathf.Clamp(yaw, -45f, 45f);
        //the rotation range

        pitch = Mathf.Clamp(pitch, -90f, 30f);
        //the rotation range

        turretRotation.localEulerAngles = new Vector3(pitch, yaw, 0.0f);

        //Debug.Log(turretRotation.eulerAngles);
    }

    void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        //GetMouseInput();




        if (jumpQued == true)
        {
            Jump();
            //Debug.Log("jump qued");
            jumpQued = false;
        }

        rb.AddForce(forceDirection, ForceMode.Impulse);


        forceDirection = Vector3.zero;
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {

        RotateTurret();

        if (Input.GetButtonDown("Jump"))
        {
            jumpQued = true;
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GetMouseInput();
            SoundManagerScript.PlaySound("Fire");
        }

    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);

        //jumpInput = Input.GetButton(JUMP);

        //isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void GetMouseInput()
    {
        //if (shoot == true)
        //{
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
            shoot = false;
        //}

    }



    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        breakForce = isBreaking ? breakForce : 0f;

        if (isBreaking)
        {
            ApplyBreaking();
        }
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);

    }

    private void Jump()
    {
        //rb.AddForce(forceDirection, ForceMode.Impulse);
        //Debug.Log(forceDirection);

        //forceDirection = Vector3.zero;

        //if (rb.velocity.y < 0f)
        //    rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;


        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
            Debug.Log("Jump clicked");
        }

    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.2f))
        {
            Debug.Log("onGround");
            return true;
        }
        else
        {
            Debug.Log("not onGround");
            return false;
        }
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {

        wheelCollider.GetWorldPose(out pos, out rot);

        wheelTransform.rotation = rot * Quaternion.Euler(0, 0, 0);
        wheelTransform.position = pos;
    }
}
