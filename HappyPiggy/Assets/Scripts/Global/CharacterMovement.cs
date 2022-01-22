using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D characterRigidbody;
    Transform characterTransform;
    string currentOrientation;
    Vector2 raycastDirection;
    [SerializeField] float playerSpeed;
    System.Random random;
    string nextDirection;
    bool isMuddy = false;
    bool isAngry = false;

    float XStep;
    float YStep;

    public event Action<string> OrientationChanged = delegate { };
    public event Action CleanedYourself = delegate { };
    public event Action<string> BecameAngry = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
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
        RaycastHit2D[] Hit = Physics2D.RaycastAll(transform.position, raycastDirection, 10f);
        if (Hit != null)
        {
            Debug.Log(raycastDirection);
            bool foundPlayer = false;
            foreach (RaycastHit2D Object in Hit)
            {
                GameObject detectedVerticalUpObject = Object.transform.gameObject;
                if (detectedVerticalUpObject.GetComponent<PlayerMovement>() != null)
                {
                    becomeAngry();
                    foundPlayer = true;
                    break;
                }
            }
            if (!foundPlayer)
            {
                becomeNormal();
            }
            
        }

    }

    void becomeAngry()
    {
        if (!isAngry)
        {
            playerSpeed = 1.5f;
            isAngry = true;
            if (BecameAngry != null)
            {
                BecameAngry(currentOrientation);
            }
        }
    }

    void becomeNormal()
    {
        if (isAngry)
        {
            Debug.Log("Became normal");
            playerSpeed = 1;
            isAngry = false;
        }
        
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
        if (!isAngry)
        {
            yield return new WaitForSeconds(1);
        }
        
        if (!isMuddy)
        {
            StartWalking();
        }
    }

    void decideDirection()
    {
        if (!isAngry)
        {
            bool wallsDetected = true;
            while (wallsDetected)
            {
                var i = random.Next(0, 4);
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
            }
        }
    }

    void calculateSteps()
    {
        if (nextDirection == "horizontal_left")
        {
            XStep = -1.5f;
            YStep = 0;
            raycastDirection = new Vector2(-1, 0);
        }
        else if (nextDirection == "horizontal_right")
        {
            XStep = 1.5f;
            YStep = 0;
            raycastDirection = new Vector2(1, 0);
        }
        else if (nextDirection == "vertical_up")
        {
            XStep = 0.2f;
            YStep = 1.6f;
            raycastDirection = new Vector2(0, 1);
        }
        else if (nextDirection == "vertical_down")
        {
            XStep = -0.2f;
            YStep = -1.6f;
            raycastDirection = new Vector2(0, -1);
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
                    return true;
                }
            }
        }
        return false;
    }

    void becomeMuddy()
    {
        transform.GetComponent<CharacterSpriteDirection>().getMuddySprites(currentOrientation);
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
        if (!isAngry && OrientationChanged != null)
        {
            
            OrientationChanged(newOrientation);
        }
    }
}
