using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDirection : MonoBehaviour
{
    [SerializeField] PlayerMovement piggyMovementController;
    CapsuleCollider2D localCollider;

    void Start()
    {
        piggyMovementController.OrientationChanged += updateCharacterSprite;
        localCollider = transform.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateCharacterSprite(string newOrientation) { 
        if (newOrientation == "horizontal_left")
        {
            localCollider.direction = CapsuleDirection2D.Horizontal;
            localCollider.offset = new Vector2(-0.699999988f, -1.14999998f);
            localCollider.size = new Vector2(3.06f, 2.48000002f);
        } else if (newOrientation == "horizontal_right")
        {
            localCollider.direction = CapsuleDirection2D.Horizontal;
            localCollider.offset = new Vector2(0.775697291f, -1.14999998f);
            localCollider.size = new Vector2(3.06f, 2.48000002f);
        } else if (newOrientation == "vertical_up")
        {
            localCollider.direction = CapsuleDirection2D.Vertical;
            localCollider.offset = new Vector2(-0.239999995f, -0.00999999978f);
            localCollider.size = new Vector2(1.69000006f, 5.69000006f);
        } else if (newOrientation == "vertical_down")
        {
            localCollider.direction = CapsuleDirection2D.Vertical;
            localCollider.offset = new Vector2(0.479999989f, 1.35000002f);
            localCollider.size = new Vector2(1.69000006f, 5.69000006f);
        }
    }
}
