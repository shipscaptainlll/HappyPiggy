using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody2D playerRigidBody;
    string currentOrientation;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float playerSpeed;

    public event Action<string> OrientationChanged = delegate { };
    public event Action PiggyRespawned = delegate { };
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        currentOrientation = "horizontal_left";
        transform.Find("HealthBar").GetComponent<PiggyHealthBar>().LostAllHealth += respawnPiggy;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputFromStick = playerInput.actions["Move"].ReadValue<Vector2>();
        calculateOrientation(inputFromStick);
        movePlayer(inputFromStick);
    }

    void movePlayer(Vector2 inputFromStick)
    {
        Vector3 move = new Vector3(inputFromStick.x, inputFromStick.y);
        Vector3 playerPosition = new Vector3(playerRigidBody.position.x, playerRigidBody.position.y, 0);
        move.z = 0;
        playerRigidBody.MovePosition(playerPosition + move * Time.fixedDeltaTime * playerSpeed);
    }

    void calculateOrientation(Vector2 inputFormStick)
    {
        string newOrientation = "";
        if (inputFormStick.x != 0 && inputFormStick.x != 0)
        {
            if (Mathf.Abs(inputFormStick.x) > Mathf.Abs(inputFormStick.y))
            {
                if (inputFormStick.x > 0)
                {
                    newOrientation = "horizontal_right";
                }
                else { newOrientation = "horizontal_left"; }
            }
            else
            {
                if (inputFormStick.y > 0)
                {
                    newOrientation = "vertical_up";
                }
                else { newOrientation = "vertical_down"; }
            }
        }
        
        checkOrientationChanges(newOrientation);
    }

    void checkOrientationChanges(string newOrientation)
    {
        if (currentOrientation != null)
        {
            if (newOrientation != currentOrientation)
            {
                notifySubscribers(newOrientation);
            } 
        }
    }

    void notifySubscribers(string newOrientation)
    {
        //notifies SpriteDirection and ColliderDirection that players orientations changed
        if (OrientationChanged != null)
        {
            OrientationChanged(newOrientation);
            currentOrientation = newOrientation;
        }
    }

    void respawnPiggy()
    {
        transform.position = spawnPoint.position;
        if (PiggyRespawned != null)
        {
            PiggyRespawned();
        }
    }
}
