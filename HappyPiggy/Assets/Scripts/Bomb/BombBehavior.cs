using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    Transform localSprite;
    Transform destructionCircleSprite;
    float bombExplosionTime;

    // Start is called before the first frame update
    void Start()
    {
        bombExplosionTime = 3;
        localSprite = transform.GetChild(0);
        destructionCircleSprite = transform.GetChild(1);
        StartCoroutine(selfDestruct());
        StartCoroutine(drawDestructionCircle());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void checkDestructionVictims()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2.3f);
        foreach (Collider2D damagedObject in hitColliders)
        {
            Debug.Log(damagedObject + " " + damagedObject.transform.gameObject.layer);
            if (damagedObject.gameObject.layer == 6)
            {
                if (damagedObject.transform.Find("HealthBar") != null && damagedObject.transform.Find("HealthBar").GetComponent<PiggyHealthBar>() != null)
                {
                    damagedObject.transform.Find("HealthBar").GetComponent<PiggyHealthBar>().reduceHealth();
                } else if (damagedObject.transform.Find("HealthBar") != null && damagedObject.transform.Find("HealthBar").GetComponent<CharacterHealthBar>() != null)
                {
                    damagedObject.transform.Find("HealthBar").GetComponent<CharacterHealthBar>().reduceHealth();
                } 
            }
            
        }
    }

    IEnumerator drawDestructionCircle()
    {
        float elapsedTime = 0f;
        float currentXScale = 0.1f;
        float currentYScale = 0.1f;
        destructionCircleSprite.localScale = new Vector2(currentXScale, currentYScale);
        float finishXScale = 3f;
        float finishYScale = 3f;

        while (elapsedTime < bombExplosionTime)
        {
            elapsedTime += Time.deltaTime;
            currentXScale += ((finishXScale - currentXScale) / bombExplosionTime) * Time.deltaTime;
            currentYScale += ((finishYScale - currentYScale) / bombExplosionTime) * Time.deltaTime;
            destructionCircleSprite.localScale = new Vector2(currentXScale, currentYScale);
            yield return null;
        }
    }

    IEnumerator selfDestruct()
    {
        float elapsedTime = 0f;
        float currentXScale = 1;
        float currentYScale = 1;
        float finishXScale = 1.5f;
        float finishYScale = 1.5f;

        while (elapsedTime < bombExplosionTime)
        {
            elapsedTime += Time.deltaTime;
            currentXScale += ((finishXScale - currentXScale) / bombExplosionTime) * Time.deltaTime;
            currentYScale += ((finishYScale - currentYScale) / bombExplosionTime) * Time.deltaTime;
            localSprite.localScale = new Vector2(currentXScale, currentYScale);
            yield return null;
        }
        checkDestructionVictims();
        Destroy(gameObject);
    }
}
