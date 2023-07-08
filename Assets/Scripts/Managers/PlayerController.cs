using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public GameObject Hands;
    public bool IsHolding = false;

    [SerializeField] private GameObject rightLeg;
    [SerializeField] private GameObject leftLeg;

    [SerializeField] private InputAction movement;
    [SerializeField] private InputAction rotation;
    private Rigidbody playerRigidBody;
    private float forwardVelocityValue = 0f;
    private float rotationValue = 0f;
    private Vector3 playerDirection = new(0, 0, 1);
    private Vector3 playerRotation = new(0, 0, 0);
    private float playerVelocity = 0f;
    private float maxPlayerVelocity = 2f;

    private float playerTilt = 0;
    private string playerTiltDirection = "left";

    private bool standingUpInProgress = false;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerDirection = transform.forward.normalized;
        movement.performed += MovementStart;
        movement.canceled += MovementEnd;
        rotation.performed += RotationStart;
        rotation.canceled += RotationEnd;
    }

    private void OnEnable()
    {
        movement.Enable();
        rotation.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        rotation.Disable();
    }
    private void Update()
    {
        // NORBERT TEST START

        // dead if dead
        if(Mathf.Abs(playerTilt) == 90) { // you dead bro
            standingUpInProgress = true;
        }

        float tiltCriticalRangeForMovingForwardTiltDirectionChange = 12f; // just an arbitrary tilt max value
        float tiltCriticalRangeForFalling = 35f; // just an arbitrary tilt max value
        float tiltForce = 25f + (50f * (Mathf.Abs(playerVelocity) / 3)); // 25,30,3 = values that were meant to make tilting look good. but they should be still adjusted

        if(Mathf.Abs(playerTilt) >= tiltCriticalRangeForFalling && !standingUpInProgress) { // you dead bro

            float newTiltForFalling;

            if(playerTilt > 0) {
              newTiltForFalling = playerTilt + (tiltForce * 5 * Time.deltaTime); // * 5 to fall like a freakin brick

              if(newTiltForFalling > 90) {
                  playerTilt = 90;
                  Camera.main.transform.DOShakePosition(0.1f,1,50);
              } else {
                  playerTilt = newTiltForFalling;
              }
            }

            if(playerTilt < 0) {
              newTiltForFalling = playerTilt - (tiltForce * 5 * Time.deltaTime); // * 5 to fall like a freakin brick

              if(newTiltForFalling < -90) {
                  playerTilt = -90;
                  Camera.main.transform.DOShakePosition(0.1f,1,50);
              } else {
                  playerTilt = newTiltForFalling;
              }
            }

            // set rotation to tilt value
            playerRotation.z = playerTilt;

            // this is shit, I just copy pasted it from the bottom part of the function, sorry :(
            playerVelocity = 0; // stop the train
            transform.eulerAngles = playerRotation;
            playerVelocity = Mathf.Clamp(playerVelocity, -maxPlayerVelocity, maxPlayerVelocity);
            playerRigidBody.velocity = Vector3.Scale(playerVelocity*Vector3.one, transform.forward.normalized);

            return;
        }

        // change left-right tilt when turning or when going forward
        if((playerTilt >= tiltCriticalRangeForMovingForwardTiltDirectionChange && rotationValue == 0)
            || rotationValue < 0
            || (standingUpInProgress && playerTiltDirection == "left")) {
            // camera shake when walking
            if(((int)Mathf.Abs(playerTilt) == tiltCriticalRangeForMovingForwardTiltDirectionChange) && rotationValue == 0) {
                DoWalkingShakeWithShakeDebounce();
            }

            playerTiltDirection = "right";
        }
        if((playerTilt <= -tiltCriticalRangeForMovingForwardTiltDirectionChange && rotationValue == 0)
            || rotationValue > 0
            || (standingUpInProgress && playerTiltDirection == "right")) {
            // camera shake when walking
            if(((int)Mathf.Abs(playerTilt) == tiltCriticalRangeForMovingForwardTiltDirectionChange) && rotationValue == 0) {
                DoWalkingShakeWithShakeDebounce();
            }

            playerTiltDirection = "left";
        }

        // movin` so tiltin`
        if(forwardVelocityValue != 0 || rotationValue != 0) {
            if(playerTiltDirection == "left") {
                playerTilt += tiltForce * Time.deltaTime;
            }
            if(playerTiltDirection == "right") {
                playerTilt -= tiltForce * Time.deltaTime;
            }
        }

        float maxRightLegAngle = 145;
        Vector3 rotationRightLeg = new Vector3(0, 0, 180 - ((180 - maxRightLegAngle) / (tiltCriticalRangeForMovingForwardTiltDirectionChange * 2)) * playerTilt);
        rightLeg.transform.localEulerAngles = rotationRightLeg;

        float maxLeftLegAngle = -35;
        Vector3 rotationLeftLeg = new Vector3(0, 0, 0 + ((0 - maxLeftLegAngle) / (tiltCriticalRangeForMovingForwardTiltDirectionChange * 2)) * playerTilt);
        leftLeg.transform.localEulerAngles = rotationLeftLeg;

        // not movin` so straightin` up
        if(forwardVelocityValue == 0 && rotationValue == 0) {
            float newTilt;

            if(playerTilt < 0) {
                newTilt = playerTilt + (tiltForce * Time.deltaTime);

                if(newTilt > 2) {
                    playerTilt = 0;
                    standingUpInProgress = false;
                } else {
                    playerTilt = newTilt;
                }
            }

            if(playerTilt > 0) {
                newTilt = playerTilt - (tiltForce * Time.deltaTime);

                if(newTilt < 0) {
                    playerTilt = 0;
                    standingUpInProgress = false;
                } else {
                    playerTilt = newTilt;
                }
            }
        }

        // set rotation to tilt value
        playerRotation.z = playerTilt;

        if(standingUpInProgress) {
            rotationValue = 0;
            forwardVelocityValue = 0;
        }

        // NORBERT TEST END


        playerRotation.y += rotationValue * 100f * Time.deltaTime;
        playerVelocity += forwardVelocityValue * 5f * Time.deltaTime;
        BleedMomentum(5f);
        transform.eulerAngles = playerRotation;
        playerVelocity = Mathf.Clamp(playerVelocity, -maxPlayerVelocity, maxPlayerVelocity);
        playerRigidBody.velocity = Vector3.Scale(playerVelocity*Vector3.one, transform.forward.normalized);
    }

    private void DoWalkingShakeWithShakeDebounce()
    {
        Debug.Log("do shake !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        Camera.main.transform.DOShakePosition(0.1f,0.1f,50);
    }

    private void MovementStart(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.ReadValue<float>() > 0f)
        {
            forwardVelocityValue = 1f;
        }
        if (callbackContext.ReadValue<float>() < 0f)
        {
            forwardVelocityValue = -1f;
        }
    }
    private void MovementEnd(InputAction.CallbackContext callbackContext)
    {
        forwardVelocityValue = 0f;
    }

    private void RotationStart(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.ReadValue<float>() > 0f)
        {
            rotationValue = 1f;
        }
        if (callbackContext.ReadValue<float>() < 0f)
        {
            rotationValue = -1f;
        }
    }

    private void RotationEnd(InputAction.CallbackContext callbackContext)
    {
        rotationValue = 0f;
    }

    private void BleedMomentum(float bleedIntensity)
    {
        if(forwardVelocityValue == 0f)
        {
            if (Mathf.Abs(playerVelocity) < 0.1f)
            {
                playerVelocity = 0f;
            }
            else if(playerVelocity > 0f)
            {
                playerVelocity -=  bleedIntensity * Time.deltaTime;
            }
            else if(playerVelocity < 0f)
            {
                playerVelocity +=  bleedIntensity * Time.deltaTime;
            }
        }
    }
}
