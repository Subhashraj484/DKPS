using UnityEngine;

public class CharacterControllerS : MonoBehaviour
{
    //need to clamp the camera rotation properly 
    private static CharacterControllerS instance;
    private CharacterController characterController;
    private IInputManager inputManager;

     public float walkSpeed = 3f;
    public float sprintSpeed = 8f;
    public float sprintAcceleration = 2f;
    public float StoppingSpeed = 6f; 

    public float currentSpeed;// in public for testing purose
    private bool isSprinting;

    public static CharacterControllerS Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterControllerS>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("CharacterControllerS");
                    instance = obj.AddComponent<CharacterControllerS>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        characterController = GetComponent<CharacterController>();
        inputManager = FindObjectOfType<OldInputManager>();

        // inputManager = FindObjectOfType<NewInputManager>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleActions();
    }

    private void HandleMovement()
    {
        Vector2 movementInput = inputManager.GetPlayerMovement();
        Vector3 moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        float speed = isSprinting ? currentSpeed : walkSpeed;
        characterController.Move(moveDirection * speed * Time.deltaTime);
        Debug.Log(speed);
    }

    private void HandleRotation()
    {
        Vector2 rotateInput = inputManager.GetCameraRotate();
        float xRotation = rotateInput.y;
        transform.Rotate(Vector3.up, rotateInput.x);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.z = 0f;
        transform.rotation = Quaternion.Euler(currentRotation);
        transform.Rotate(Vector3.right, xRotation);
    }

    private void HandleActions()
    {
        if (inputManager.GetFlashButtonDown())
        {
            TurnFlashlight();
        }

        if (inputManager.GetShiftDown())
        {
            StartSprint();
            Debug.Log("sprint");
        }
        else
        {
            StopSprint();
        }
    }

    private void TurnFlashlight()
    {
        // varan varan puchandi light adi da puchandi
    }
    private void StartSprint()
    {
        isSprinting = true;
        currentSpeed = Mathf.MoveTowards(currentSpeed, sprintSpeed, sprintAcceleration * Time.deltaTime);
    }
    private void StopSprint()
    {
        isSprinting = false;
        currentSpeed = Mathf.MoveTowards(currentSpeed, walkSpeed, StoppingSpeed * Time.deltaTime);
    }
}
