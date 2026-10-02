using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSControllerBlank : MonoBehaviour
{
    //movement Variables
    public float walkSpeed = 7f;
    public float jumpHeight = 7f;
    public float gravity = 9.81f;

    //camera Variables
    public Camera playerCamera;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    //Public keycode for unlocking the mouse
    public KeyCode unlockMouse = KeyCode.Delete;

    //Private Variables
    private CharacterController _characterController;
    private Vector3 _movementDirection = Vector3.zero;
    private float _rotationX;

    //Player HP display
    public int playerHP = 100;
    public bool isInvincible = false;
    public shootBlank weapon;

    public GameManager gameManager;
    public bool spaceJumpActive = false;

    //Data preservation
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(weapon.projectileParent);
    }

    // Start is called before the first frame update
    void Start()
    {
        //find the character controller on the gameObject, our movement script here will use functionality from the character controller component to move
        _characterController = GetComponent<CharacterController>();

        //locks the cursor to the center of the game window and hides it so it looks more like an fps
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        if (spaceJumpActive)
        {
            jumpHeight += 5f;
            gravity *= 0.75f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //let players get their mouse back if they want
        if (Input.GetKeyDown(unlockMouse))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Find new game manager after scene loading
        if (!gameManager)
        {
            gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        }

        //Local Vector Variables used to store 
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        //Local float Variables to calculate how fast we should move both forward and side to side based on player input
        float currentSpeedX = walkSpeed * Input.GetAxis("Vertical");
        float currentSpeedY = walkSpeed * Input.GetAxis("Horizontal");

        //local float variable to store the current veritcal direction of our player
        float jumpDirection = _movementDirection.y;

        //calculate movement vector based on our speed variables for moving forward and side to side 
        _movementDirection = (forward * currentSpeedX) + (right * currentSpeedY);

        //adds vertical movement to our player if the player is on the ground and pressed the jump button
        if (Input.GetButton("Jump") && _characterController.isGrounded)
        {
            _movementDirection.y = jumpHeight;
        }
        //stops adding vertical movement while the player is not jumping
        else
        {
            _movementDirection.y = jumpDirection;
        }

        //if the player is not on the ground subtracts our gravity force from vertical movement. This will allows for the player jump to slowly reach a peak and then return to the ground over time
        if (!_characterController.isGrounded)
        {
            _movementDirection.y -= gravity * Time.deltaTime;
        }
        //resets vertical movement to 0 after landing
        else if (_characterController.isGrounded && _movementDirection.y < 0)
        {
            _movementDirection.y = 0;
        }

        //apply our final move direction to the player in game using the built in characterController move funciton
        _characterController.Move(_movementDirection * Time.deltaTime);


        //calculate where our camera should rotate based on mouse input
        _rotationX += (Input.GetAxis("Mouse Y") * -1) * lookSpeed;

        //restrict how high or low the camera can rotate
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);

        //rotate the camera to match vertical mouse input, when using Quaternion.Euler the rotation is applied around the given axis not to it, this is why x and y appear to be flipped
        //hand pneumonic device
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

        /*rotate our character to match horizontal mouse input, we use multiply here because of the nature of quaternions. You can't use addition to add one to the other. 
        to combine 2 quaternions like we want here (our original quaternion rotation + the rotation we want to turn to based on mouse input) we multiply the original by the second*/
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxisRaw("Mouse X"), 0f);
    }

    IEnumerator displayPowerup(string info)
    {
        Debug.Log("DISPLAYING POWERUP");
        gameManager.powerupText.text = info;
        yield return new WaitForSeconds(1.5f);
        gameManager.powerupText.text = "";
    }

    IEnumerator takeDamage(int damage)
    {
        isInvincible = true;
        playerHP -= damage;
        gameManager.playerHealthText.text = "HP: " + playerHP.ToString();

        //Die
        if (playerHP <= 0)
        {
            gameManager.playerHealthText.text = "YOU DIED";
            weapon.canFire = false;
            this.gameObject.transform.rotation = Quaternion.Euler(this.gameObject.transform.rotation.x, this.gameObject.transform.rotation.y, -90);
            Destroy(this);

        }

        yield return new WaitForSeconds(0.25f);
        isInvincible = false;
    }

    IEnumerator loadNextLevel()
    {
        gameManager.scoreText.text = "Advancing to next level...";
        yield return new WaitForSeconds(3f);
        gameManager.loadNextLevel(gameManager.nextLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Enemy attacks
        if (other.CompareTag("enemy projectile") || other.CompareTag("enemy"))
        {
            if (!isInvincible)
            {
                StartCoroutine(takeDamage(1));
            }
        }

        //Health pickup
        if (other.CompareTag("health"))
        {
            Destroy(other.gameObject);
            playerHP += 1;
            gameManager.playerHealthText.text = "HP: " + playerHP.ToString();


        }

        //Fire Rate powerup
        if (other.CompareTag("Rapid Fire"))
        {
            Destroy(other.gameObject);

            if (weapon.fireRate > 0.5)
            {
                weapon.fireRate -= 0.5f;
            }
            else
            {
                weapon.fireRate *= 0.75f;
            }

            Debug.Log("New rate of fire: " + weapon.fireRate);
            StartCoroutine(displayPowerup("Rate of fire increased"));
        }

        //Projectile speed powerup
        if (other.CompareTag("Projectile Speed"))
        {
            Destroy(other.gameObject);
            weapon.projectileSpeed += 25f;
            Debug.Log("New projectile speed: " + weapon.projectileSpeed);
            StartCoroutine(displayPowerup("Projectile speed increased"));
        }

        //Non stackable powerups (Boss powerups that load the next level)
        //Double Jump powerup
        if (other.CompareTag("Space Jump"))
        {
            Destroy(other.gameObject);
            jumpHeight += 5f;
            gravity *= 0.75f;
            spaceJumpActive = true;
            Debug.Log("Space Jump picked up");
            StartCoroutine(displayPowerup("Space Jump activated!"));
            StartCoroutine(loadNextLevel());

        }

        if (other.CompareTag("Multi-shot"))
        {
            Destroy(other.gameObject);
            weapon.multishotActive = true;
            Debug.Log("Multi-shot picked up");
            StartCoroutine(displayPowerup("Triple shot activated!"));
            StartCoroutine(loadNextLevel());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (!isInvincible)
            {
                StartCoroutine(takeDamage(1));
            }
        }
    }

    public void resetPosition()
    {
        Debug.Log("POSITION RESET");
        _characterController.enabled = false;
        transform.position = new Vector3(0f, 1f, 0f);
        _characterController.enabled = true;
    }
}
