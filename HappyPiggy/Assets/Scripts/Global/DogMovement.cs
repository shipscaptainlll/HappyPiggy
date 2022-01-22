using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DogMovement : MonoBehaviour
{
    Rigidbody2D characterRigidbody;
    Transform characterTransform;
    string currentOrientation;
    [SerializeField] float playerSpeed;
    System.Random random;
    System.Random random2;
    string nextDirection;
    bool isMuddy = false;

    float XStep;
    float YStep;

    public event Action<string> OrientationChanged = delegate { };
    public event Action CleanedYourself = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        random2 = new System.Random();
        playerSpeed = 1f;
        characterTransform = transform;
        characterRigidbody = GetComponent<Rigidbody2D>();
        currentOrientation = "horizontal_left";
        StartCoroutine(TakeABreak());
        transform.Find("HealthBar").GetComponent<CharacterHealthBar>().LostAllHealth += becomeMuddy;
    }

    // Update is called once per frame
    void Update()
    {


    }

    
    void StartWalking()
    {
        decideDirection();
        notifySubscribers(nextDirection);
        calculateSteps();
        StartCoroutine(WalkOneStep());
    }

    IEnumerator WalkOneStep()
    {
        float elapsedTime = 0f;
        float stepTime = 2f;
        float startXLocalPosition = transform.localPosition.x;
        float startYLocalPosition = transform.localPosition.y;
        float startXPosition = transform.position.x;
        float startYPosition = transform.position.y;
        float currentXPosition = startXPosition;
        float currentYPosition = startYPosition;
        if (XStep < 0 || YStep < 0)
        {
            while (Mathf.Abs(transform.localPosition.x) > Mathf.Abs(startXLocalPosition) + XStep
            || Mathf.Abs(transform.localPosition.y) > Mathf.Abs(startYLocalPosition) + YStep)
            {
                elapsedTime += Time.deltaTime;
                Vector3 move = new Vector3(XStep, YStep, 0);
                characterRigidbody.MovePosition(characterTransform.position + move * Time.fixedDeltaTime * playerSpeed);
                currentXPosition = transform.position.x;
                currentYPosition = transform.position.y;
                yield return null;
            }
        } else
        {
            while (Mathf.Abs(transform.localPosition.x) < Mathf.Abs(startXLocalPosition) + XStep
            || Mathf.Abs(transform.localPosition.y) < Mathf.Abs(startYLocalPosition) + YStep)
            {
                elapsedTime += Time.deltaTime;
                Vector3 move = new Vector3(XStep, YStep, 0);
                characterRigidbody.MovePosition(characterTransform.position + move * Time.fixedDeltaTime * playerSpeed);
                currentXPosition = transform.position.x;
                currentYPosition = transform.position.y;
                yield return null;
            }
        }
        
        StartCoroutine(TakeABreak());
        yield return null;
    }

    IEnumerator TakeABreak()
    {
        yield return new WaitForSeconds(1);
        if (!isMuddy)
        {
            StartWalking();
        }
    }

    void decideDirection()
    {
        bool wallsDetected = true;
        int tries = 0;
        while (wallsDetected)
        {
            var i = random2.Next(0, 4);
            if (i == 0)
            {
                nextDirection = "horizontal_left";
                wallsDetected = detectLeftWall();
            }
            else if (i == 1)
            {
                nextDirection = "horizontal_right";
                wallsDetected = detectRightWall();
            }
            else if (i == 2)
            {
                nextDirection = "vertical_up";
                wallsDetected = detectUpperWall();
            }
            else if (i == 3)
            {
                nextDirection = "vertical_down";
                wallsDetected = detectBottomWall();
            }
            tries++;

        }
    }

    void calculateSteps()
    {
        if (nextDirection == "horizontal_left")
        {
            XStep = -1.5f;
            YStep = 0;
        }
        else if (nextDirection == "horizontal_right")
        {
            XStep = 1.5f;
            YStep = 0;
        }
        else if (nextDirection == "vertical_up")
        {
            XStep = 0.2f;
            YStep = 1.6f;
        }
        else if (nextDirection == "vertical_down")
        {
            XStep = -0.2f;
            YStep = -1.6f;
        }
    }

    bool detectUpperWall()
    {
        
        RaycastHit2D[] verticalUpHit = Physics2D.RaycastAll(transform.position, new Vector2(0, 1), 1.6f);
        if (verticalUpHit != null)
        {
            foreach (RaycastHit2D Object in verticalUpHit)
            {
                GameObject detectedVerticalUpObject = Object.transform.gameObject;
                if (detectedVerticalUpObject.layer == 3)
                {
                    Debug.Log("Detected uppr");
                    return true;
                }
            }
        }
        return false;
    }

    bool detectBottomWall()
    {
        
        RaycastHit2D[] verticalDownHit = Physics2D.RaycastAll(transform.position, new Vector2(0, -1), 1.6f);
        if (verticalDownHit != null)
        {
            foreach (RaycastHit2D Object in verticalDownHit)
            {
                GameObject detectedVerticalDownObject = Object.transform.gameObject;
                if (detectedVerticalDownObject.layer == 3)
                {
                    Debug.Log("Detected btm");
                    return true;
                }
            }
            
        }
        return false;
    }

    bool detectLeftWall()
    {
        
        RaycastHit2D[] horizontalLeftHit = Physics2D.RaycastAll(transform.position, new Vector2(-1, 0), 1.6f);
        if (horizontalLeftHit != null)
        {
            foreach (RaycastHit2D Object in horizontalLeftHit)
            {
                GameObject detectedHorizontalLeftObject = Object.transform.gameObject;
                if (detectedHorizontalLeftObject.layer == 3)
                {
                    Debug.Log("Detected lft");
                    return true;
                }
            }
        }
        return false;
    }

    bool detectRightWall()
    {
        
        RaycastHit2D[] horizontalRightHit = Physics2D.RaycastAll(transform.position, new Vector2(1, 0), 1.6f);
        if (horizontalRightHit != null)
        {
            foreach (RaycastHit2D Object in horizontalRightHit)
            {
                GameObject detectedhorizontalRightObject = Object.transform.gameObject;
                if (detectedhorizontalRightObject.layer == 3)
                {
                    Debug.Log("Detected rght");
                    return true;
                }
            }
        }
        return false;
    }

    void becomeMuddy()
    {
        transform.GetComponent<DogSpriteDirection>().getMuddySprites(currentOrientation);
        isMuddy = true;
        StartCoroutine(cleanYourself());
    }

    IEnumerator cleanYourself()
    {
        yield return new WaitForSeconds(10f);
        isMuddy = false;
        if (CleanedYourself != null)
        {
            CleanedYourself();
        }
        returnToRoutine();
    }
    
    void returnToRoutine()
    {
        StartWalking();
    }

    void notifySubscribers(string newOrientation)
    {
        //notifies SpriteDirection that characters orientationchanged
        if (OrientationChanged != null)
        {
            OrientationChanged(newOrientation);
        }
    }
}
