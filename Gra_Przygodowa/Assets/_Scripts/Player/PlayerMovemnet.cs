using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    public float sprintMultiplier = 1.5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public KeyCode Jump = SettingsManager.Instance.GetKeyCode("Jump");
    public KeyCode Sprint = SettingsManager.Instance.GetKeyCode("Sprint");

    Vector3 velocity;
    bool isGrounded;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    public bool isMoving;

    // Update is called once per frame
    void Update()
    {
        if (MovementManager.Instance.canMove)
            Movement();
    }

    public void Movement()
    {
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = speed;
        MovementManager.Instance.canSprinting = false;
        // Przyspieszenie, je�li lewy Shift jest wci�ni�ty
        if (Input.GetKey(Sprint))
        {
            currentSpeed *= sprintMultiplier;
            MovementManager.Instance.canSprinting = true;
        }

        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(Jump)) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //jesli ostatnia pozycja rozni sie od terazniejszej ozacza to ze sie poruszamy
        if ((lastPosition != gameObject.transform.position && isGrounded == true) && MovementManager.Instance.canMove)
        {
            isMoving = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }
        lastPosition = gameObject.transform.position;
    }
}