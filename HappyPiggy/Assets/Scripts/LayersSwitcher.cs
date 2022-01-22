using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersSwitcher : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.parent.position.y > collision.transform.position.y)
        {
            collision.GetComponent<SpriteRenderer>().sortingOrder = 2;
        } else
        {
            collision.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        
    }
}
