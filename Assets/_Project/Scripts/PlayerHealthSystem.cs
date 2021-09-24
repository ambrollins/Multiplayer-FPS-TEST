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
    
    public static Dictionary<uint, PlayerHealthSystem> playerHealthSystems = new Dictionary<uint, PlayerHealthSystem>();

    private void Start()
    {
        ResetHealth();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        playerHealthSystems.Add(GetComponent<NetworkIdentity>().netId, this);
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
        
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(3f);
        if (isLocalPlayer)
        {
            Transform newStartPoint = NetworkManager.singleton.GetStartPosition();
            Debug.Log("New Start Point " + newStartPoint.position);
            transform.position = newStartPoint.position;
        }
        
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(true);
        }

        for (int i = 0; i < behavioursToDisable.Length; i++)
        {
            behavioursToDisable[i].enabled = true;
        }
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
