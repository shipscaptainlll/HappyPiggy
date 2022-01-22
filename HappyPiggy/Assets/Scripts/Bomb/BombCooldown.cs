using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombCooldown : MonoBehaviour
{
    Image bombSprite;
    float bombCooldownTime;
    float fillUpdateSpeed;
    public event Action CooldownEnded = delegate { };
    // Start is called before the first frame update
    void Start()
    {
        bombCooldownTime = 5;
        fillUpdateSpeed = 0.1f;
        bombSprite = transform.Find("Bomb").GetComponent<Image>();
        transform.GetComponent<BombPlace>().BombPlaced += startCooldown;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void startCooldown()
    {
        StartCoroutine(processCooldown());
    }

    IEnumerator processCooldown()
    {
        float elapsedTime = 0f;
        bombSprite.fillAmount = 0;
        float startFillAmmount = bombSprite.fillAmount;
        float finishFillAmmount = 1;
        
        while (elapsedTime < bombCooldownTime)
        {
            elapsedTime += Time.deltaTime;
            bombSprite.fillAmount += ((finishFillAmmount - startFillAmmount) / bombCooldownTime) * Time.deltaTime;
            yield return null;
        }

        if (CooldownEnded != null)
        {
            CooldownEnded();
        }

        yield return null;
    }
}
