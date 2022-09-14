using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform orientation;
    public GameObject respawnPanel;
    public GameObject finishPanel;

    [Header("Movement")]
    private float moveSpeed = 200; // in fact - accelaration?
    private float initialDrag = 4;
    private float startX = 400;
    private float startZ = 100;


    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;

    private Rigidbody rigidBody;



    // Start is called before the first frame update
    void Start()
    {
        // Don't show panels
        respawnPanel.SetActive(false);
        finishPanel.SetActive(false);

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.drag = initialDrag;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        SpeedControl();
    }

    private void FixedUpdate()
    {
        // moving the player in POV 
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; 
        rigidBody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }


    // the player cannot speed up over the limit
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            rigidBody.velocity = flatVelocity.normalized * moveSpeed;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            StartCoroutine(ShowPanel());
            rigidBody.position = new Vector3(startX, 0, startZ); // Respawn at the start

            if (moveSpeed > 35)
            {
                moveSpeed *= 0.7f; // slow down
            }
            Debug.Log(moveSpeed);
        }

        if (other.gameObject.layer == 8)
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }


    IEnumerator ShowPanel()
    {
        respawnPanel.SetActive(true); // Show the "Wrong" panel
        yield return new WaitForSecondsRealtime(2);
        respawnPanel.SetActive(false); // Don't show the "Wrong" panel
    }

}
