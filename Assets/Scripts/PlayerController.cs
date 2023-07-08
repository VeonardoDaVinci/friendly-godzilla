using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public GameObject Hands;
    public bool IsHolding = false;
    [SerializeField] private InputAction movement;
    [SerializeField] private InputAction rotation;
    private Rigidbody playerRigidBody;
    private float forwardVelocityValue = 0f;
    private float rotationValue = 0f;
    private Vector3 playerDirection = new(0, 0, 1);
    private Vector3 playerRotation = new(0, 0, 0);
    private float playerVelocity = 0f;
    private float maxPlayerVelocity = 4f;

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
        playerRotation.y += rotationValue * 100f * Time.deltaTime;
        playerVelocity += forwardVelocityValue * 5f * Time.deltaTime;
        BleedMomentum(5f);
        transform.eulerAngles = playerRotation;
        playerVelocity = Mathf.Clamp(playerVelocity, -maxPlayerVelocity, maxPlayerVelocity);
        playerRigidBody.velocity = Vector3.Scale(playerVelocity*Vector3.one, transform.forward.normalized);
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
