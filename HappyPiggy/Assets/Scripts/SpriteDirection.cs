using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDirection : MonoBehaviour
{
    [SerializeField] PlayerMovement piggyMovementController;
    [SerializeField] Sprite verticalUpMovement;
    [SerializeField] Sprite verticalDownMovement;
    [SerializeField] Sprite horizontalLeftMovement;
    [SerializeField] Sprite horizontalRigthMovement;
    SpriteRenderer localSpriteRenderer;

    void Start()
    {
        piggyMovementController.OrientationChanged += updateCharacterSprite;
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
}
