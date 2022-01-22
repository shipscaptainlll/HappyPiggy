using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BombPlace : MonoBehaviour
{
    [SerializeField] Transform characterTransform;
    [SerializeField] Transform bombTransform;
    Button placeBombButton;
    bool bombPlaceAvailable;

    public event Action BombPlaced = delegate { };
    // Start is called before the first frame update
    void Start()
    {
        bombPlaceAvailable = true;
        placeBombButton = transform.GetComponent<Button>();
        placeBombButton.onClick.AddListener(placeBomb);
        transform.GetComponent<BombCooldown>().CooldownEnded += enableBombPlacer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void placeBomb()
    {
        if (bombPlaceAvailable)
        {
            instantiateBomb();
            if (BombPlaced != null)
            {
                BombPlaced();
            }
            disableBombPlacer();
        }
    }

    void instantiateBomb()
    {
        Instantiate(bombTransform, characterTransform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    void disableBombPlacer()
    {
        bombPlaceAvailable = false;
    }

    void enableBombPlacer()
    {
        bombPlaceAvailable = true;
    }
}
