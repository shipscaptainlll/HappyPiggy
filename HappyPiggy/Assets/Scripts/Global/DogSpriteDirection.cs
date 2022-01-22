using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSpriteDirection : MonoBehaviour
{
    [SerializeField] DogMovement movementController;
    [SerializeField] Sprite verticalUpMovement;
    [SerializeField] Sprite verticalDownMovement;
    [SerializeField] Sprite horizontalLeftMovement;
    [SerializeField] Sprite horizontalRigthMovement;
    [SerializeField] Sprite muddyVerticalUpMovement;
    [SerializeField] Sprite muddyVerticalDownMovement;
    [SerializeField] Sprite muddyHorizontalLeftMovement;
    [SerializeField] Sprite muddyHorizontalRigthMovement;
    SpriteRenderer localSpriteRenderer;

    void Start()
    {
        movementController = transform.GetComponent<DogMovement>();
        movementController.OrientationChanged += updateCharacterSprite;
        localSpriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateCharacterSprite(string newOrientation) { 
        if (newOrientation == "horizontal_left")
        {
            localSpriteRenderer.sprite = horizontalLeftMovement;
        } else if (newOrientation == "horizontal_right")
        {
            localSpriteRenderer.sprite = horizontalRigthMovement;
        } else if (newOrientation == "vertical_up")
        {
            localSpriteRenderer.sprite = verticalUpMovement;
        } else if (newOrientation == "vertical_down")
        {
            localSpriteRenderer.sprite = verticalDownMovement;
        }
    }

    public void getMuddySprites(string currentOrientation)
    {
        if (currentOrientation == "horizontal_left")
        {
            localSpriteRenderer.sprite = muddyHorizontalLeftMovement;
        }
        else if (currentOrientation == "horizontal_right")
        {
            localSpriteRenderer.sprite = muddyHorizontalRigthMovement;
        }
        else if (currentOrientation == "vertical_up")
        {
            localSpriteRenderer.sprite = muddyVerticalUpMovement;
            
        }
        else if (currentOrientation == "vertical_down")
        {
            localSpriteRenderer.sprite = muddyVerticalDownMovement;
        }
    }
}
