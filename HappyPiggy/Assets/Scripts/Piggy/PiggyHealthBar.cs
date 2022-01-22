using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiggyHealthBar : MonoBehaviour
{

    float maxHealth = 100;
    float currentHealth = 100;
    float damagePerHit = 50;
    float reduceUpdateSpeed = 0.5f;
    Image healthFrontImage;

    public event Action LostAllHealth = delegate { };
    // Start is called before the first frame update
    void Start()
    {
        healthFrontImage = transform.Find("Front").GetComponent<Image>();
        transform.parent.GetComponent<PlayerMovement>().PiggyRespawned += restoreHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reduceHealth()
    {
        StartCoroutine(graduallyReductHP(damagePerHit));
    }

    IEnumerator graduallyReductHP(float damage)
    {
        float elapsed = 0f;
        while (elapsed < reduceUpdateSpeed)
        {
            elapsed += Time.deltaTime;
            healthFrontImage.fillAmount = Mathf.Lerp(currentHealth/maxHealth, (currentHealth - damage)/maxHealth, elapsed / reduceUpdateSpeed);
            yield return null;
        }

        healthFrontImage.fillAmount = (currentHealth - damage) / maxHealth;
        Debug.Log((currentHealth - damage) / maxHealth);
        currentHealth -= damage;
        checkHealthStatus();
    }

    void checkHealthStatus()
    {
        if (currentHealth <= 0)
        {
            if (LostAllHealth != null)
            {
                LostAllHealth();
            }
        }
    }

    void restoreHealth()
    {
        StartCoroutine(graduallyRestoreHP());
    }

    IEnumerator graduallyRestoreHP()
    {
        float elapsed = 0f;
        while (elapsed < reduceUpdateSpeed)
        {
            elapsed += Time.deltaTime;
            healthFrontImage.fillAmount = Mathf.Lerp(currentHealth / maxHealth, maxHealth / maxHealth, elapsed / reduceUpdateSpeed);
            yield return null;
        }

        healthFrontImage.fillAmount = maxHealth / maxHealth;
        currentHealth = maxHealth;
    }
}
