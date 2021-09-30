using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerHealthSystem : NetworkBehaviour
{
    [SerializeField] private const float MaxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private HealthView healthView;
    [SerializeField] private bool isDead;

    [SerializeField] private GameObject[] objectsToDisable;
    [SerializeField] private Behaviour[] behavioursToDisable;
    
    // TODO : SUPER IMPORTANT, WE CANNOT CHANGE TRANSFORM IF A CHARACTER CONTROLLER IS ATTACHED.
    // WEIRD BUT TRUE
    [SerializeField] private CharacterController characterController;

    public static Dictionary<uint, PlayerHealthSystem> playerHealthSystems = new Dictionary<uint, PlayerHealthSystem>();

    private void Start()
    {
        if (isServer)
        {
            playerHealthSystems.Add(GetComponent<NetworkIdentity>().netId, this);
            Debug.Log($"Added Player: {playerHealthSystems.Count}");
        }
        ResetHealth();
    }

    public override void OnStartClient()
    {
        if (isServer)
        {
            return;
        }
        base.OnStartClient();
        playerHealthSystems.Add(GetComponent<NetworkIdentity>().netId, this);
        Debug.Log($"Added Player: {playerHealthSystems.Count}");
    }

    [ClientRpc]
    public void TakeDamageRpc(float damageAmount)
    {
        ReduceHealth(damageAmount);
    }

    public void ReduceHealth(float amount)
    {
        if (isDead)
        {
            return;
        }
        float _currentHealth = GetHealth();
        _currentHealth -= amount;
        SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        isDead = true;
        //Disable Stuff
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }

        for (int i = 0; i < behavioursToDisable.Length; i++)
        {
            behavioursToDisable[i].enabled = false;
        }

        characterController.enabled = false;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        if (isLocalPlayer)
        {
            Transform newStartPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = newStartPoint.position;
            Debug.Log($"Respawned Player at {transform.position}");
        }
        
        yield return new WaitForSecondsRealtime(2f);
        
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(true);
        }

        for (int i = 0; i < behavioursToDisable.Length; i++)
        {
            behavioursToDisable[i].enabled = true;
        }
        characterController.enabled = true;
        isDead = false;
        ResetHealth();
    }

    public void IncreaseHealth(float amount)
    {
        float _currentHealth = GetHealth();
        _currentHealth += amount;
        SetHealth(_currentHealth);
    }

    private void ResetHealth()
    {
        SetHealth(MaxHealth);
    }
    
    private void SetHealth(float newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, MaxHealth);
        currentHealth = newHealth;
        healthView.UpdateView(currentHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public override void OnStopClient()
    {
        playerHealthSystems.Remove(GetComponent<NetworkIdentity>().netId);
    }
}
