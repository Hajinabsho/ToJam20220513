using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class TankController : MonoBehaviour
{
    //inputfields;
    private InputController inputController;
    //private PlayerInput playerInput;
    private InputAction move;
    private InputAction steer;

    //Movement field
    private Rigidbody rb;


    [SerializeField] private float movementForce = 1f;
    private Vector3 forceDirection = Vector3.zero;

    private float steeringAngle;
    private float changeInAngle = 1.0f;
    public float angle;
    
    public float maxSteerAngle = 30;
    public float minSteerAngle = 30;

    public float motorForce = 50;
    public float jumpForce = 100000;

    //wheel colliders

    public WheelCollider frontLeftWheel, frontRightWheel;
    public WheelCollider rearLeftWheel, rearRightWheel;


    public Transform frontLeftT, frontRightT;
    public Transform rearLeftT, rearRightT;


    //Camera
    [SerializeField] private Camera playerCamera;

    //Shoot
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 30.0f;


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        inputController = new InputController();

        

    }


    private void OnEnable()
    {
        inputController.Player.Jump.started += DoJump;
        inputController.Player.Fire.started += DoAttack;
        inputController.Player.SteeringAngleRight.started += x => SteerRight();
        inputController.Player.SteeringAngleLeft.performed += x => SteerLeft();

        move = inputController.Player.Movement;
        //steer = inputController.Player.SteeringAngleRight;
        

        inputController.Player.Enable();

    }

    private void HandleMotor()
    {
        frontLeftWheel.motorTorque = move.ReadValue<Vector2>().x * motorForce;
        Debug.Log(move.ReadValue<Vector2>().x);
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheel, frontLeftT);
        UpdateSingleWheel(frontRightWheel, frontRightT);
        UpdateSingleWheel(rearLeftWheel, rearLeftT);
        UpdateSingleWheel(rearRightWheel, rearRightT);

    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        wheelTransform.rotation = rot; //* Quaternion.Euler(0, 0, 0);
        wheelTransform.position = pos;
    }


    private void DoAttack(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {

        //IsGrounded();

        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
            Debug.Log("Jump clicked");
        }
    }

    private void SteerRight()
    {
        if(steeringAngle < maxSteerAngle)
        {
            steeringAngle += changeInAngle;
            Debug.Log(steeringAngle);
        }
    }

    private void SteerLeft()
    {
        if(steeringAngle > minSteerAngle)
        {
            steeringAngle -= changeInAngle;
            Debug.Log(steeringAngle);
        }



    }

    private void OnDisable()
    {
        inputController.Player.Jump.started -= DoJump;
        inputController.Player.Fire.started -= DoAttack;
        //inputController.Player.SteeringAngle.started -= Steer;

        inputController.Disable();
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

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        //if(isDelay == true)
        //{
        //    direction = new Vector3(0,0,0);
        //}

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);

        else
            rb.angularVelocity = Vector3.zero;
    }



    //private void DoJump(inputController.CallbackContext obj)
    //{
    //    if (IsGrounded())
    //    {
    //        forceDirection += Vector3.up * jumpForce;
    //    }
    //}



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float move = inputController.Player.Movement.ReadValue<float>();
        //Debug.Log(move);

       // inputController.Player.Jump.ReadValue<float>();
        //if (inputController.Player.Jump.triggered)
        //    Debug.Log("Jump");

    }

    private void FixedUpdate()
    {

        HandleMotor();
        UpdateWheels();

        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * motorForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * motorForce;

        frontLeftWheel.steerAngle = steeringAngle;
        frontRightWheel.steerAngle = steeringAngle;

        //forceDirection += Action.ReadValue<Vector2>().x * movementForce;
        //forceDirection -= Action.ReadValue<Vector2>().x * movementForce;



        rb.AddForce(forceDirection, ForceMode.Impulse);
        Debug.Log(forceDirection);

        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;




        Vector3 horinzontalVelocity = rb.velocity;
        horinzontalVelocity.y = 0;

        if (horinzontalVelocity.sqrMagnitude > motorForce * motorForce)
            rb.velocity = horinzontalVelocity.normalized * motorForce + Vector3.up * rb.velocity.y;

        LookAt();

    }
}
