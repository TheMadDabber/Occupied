using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
public float speed = 0.5f;
    public float runSpeed = 2f;

public Vector3 velocity;
public float gravityModifier;

public CharacterController myController;
public Transform myCameraHead;
public Animator myAnimator;
public float mouseSensitivity = 750f;

private float cameraVerticalRotation;

public GameObject bullet;
public Transform firePosition;
public GameObject muzzleFlash, bulletHole, goopSpray;


//Jumping
public float jumpHeight = 10f;
private Boolean readyToJump;
public Transform ground;
public LayerMask groundLayer;
public float groundDistance = 0.5f;

 //Crouching
 private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
 private Vector3 bodyScale;
 public Transform myBody;
 public Transform myEyes;
 private float initialControllerHeight;
 public float crouchSpeed;
 private bool isCrouching = false;

    //Sliding
    public bool isRunning = false;
    public bool startSliderTimer;
    public float currentSlideTimer;
    public float maxSlideTime = 2f;
    public float slideSpeed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        bodyScale = myBody.localScale;
        initialControllerHeight = myController.height;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        CameraMovement();

        Jump();

        Crouching();

        SlideCounter();
    }

    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCrouching();
        }

        if(Input.GetKeyUp(KeyCode.C) || currentSlideTimer > maxSlideTime)
        {
           StopCrouching();
        }
    }

   private void StartCrouching()
    {
        myBody.localScale = crouchScale;
        myCameraHead.position -= new Vector3(0, 0.5f, 0);
        myEyes.position -= new Vector3(0, 0.5f, 0);
        myController.height /= 2;
        isCrouching = true; 

        if(isRunning)
        {
            velocity = Vector3.ProjectOnPlane(myCameraHead.transform.forward, Vector3.up).normalized * slideSpeed * Time.deltaTime;
            startSliderTimer = true;
        }

    }

    private void StopCrouching()
    {
        currentSlideTimer = 0f;
        velocity = new Vector3(0, 0, 0);
        startSliderTimer = false;

        myBody.localScale = bodyScale;
        myCameraHead.position += new Vector3(0, 0.5f, 0);
        myEyes.position += new Vector3(0, 0.5f, 0);
        myController.height = initialControllerHeight;
        isCrouching = false;
    }

    void Jump()
    {
      readyToJump =  Physics.OverlapSphere(ground.position, groundDistance, groundLayer).Length > 0;
        if (Input.GetButtonDown("Jump") && readyToJump)
        {
            velocity.y = MathF.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        }

        myController.Move(velocity);
    }



    private void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= +mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);

        myCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }

    void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching) {
            movement = movement * runSpeed * Time.deltaTime;

            isRunning = true;
        }

        else if (isCrouching)
        {
            movement = movement * crouchSpeed * Time.deltaTime;
        }
        else
        {
            movement = movement * speed * Time.deltaTime;
            isRunning = false; 

        }

        myAnimator.SetFloat("PlayerSpeed", movement.magnitude);
        


        myController.Move(movement);

        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;

        if(myController.isGrounded)
        { velocity.y = Physics.gravity.y * Time.deltaTime; }


        myController.Move(velocity);
    }

    private void SlideCounter()
    {
        if (startSliderTimer) {
            currentSlideTimer += Time.deltaTime;
        }
    }

}
