using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour
{
    [SerializeField] protected PlayerHealthSystem playerHealthSystem;

    public void TakeDamage(float amount)
    {
        if (playerHealthSystem == null)
        {
            Debug.Log("Please assign a player health System");
            return;
        }
        playerHealthSystem.TakeDamage(amount);
    }
}
