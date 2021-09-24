using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR;

public class PlayerWeaponController : NetworkBehaviour
{
    [SerializeField] private Transform weaponHolderTransform;
    [SerializeField] private Transform weaponRaycastPointTransform;
    [SerializeField] private Weapon equippedWeapon;
    [SerializeField] private LayerMask hittableMask;

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (equippedWeapon == null)
        {
            Debug.Log("No Weapon Equipped!");
        }

        equippedWeapon.Attack(weaponRaycastPointTransform, hittableMask);
        WeaponShoot(equippedWeapon.headShotDamage, equippedWeapon.bodyShotDamage, equippedWeapon.legShotDamage);
    }
    
    private void WeaponShoot(float headShotDamage, float bodyShotDamage, float legShotDamage)
    {
        if (Physics.Raycast(weaponRaycastPointTransform.position,
            weaponRaycastPointTransform.forward, out RaycastHit hit, 1000f, hittableMask))
        {
            Debug.Log($"I Hit {hit.collider.name}");
            if (hit.collider.gameObject.TryGetComponent(out BodyPart bodyPart))
            {
                float damageAmount = 0;
                if (bodyPart.GetType() == typeof(Head))
                {
                    damageAmount = headShotDamage;
                }
                else if(bodyPart.GetType() == typeof(MiddleBody))
                {
                    damageAmount = bodyShotDamage;
                }
                else if(bodyPart.GetType() == typeof(Legs))
                {
                    damageAmount = legShotDamage;
                }
                
                uint netId = bodyPart.playerHealthSystem.netId;
                ProcessDamage(netId, damageAmount);
            }
        }
    }

    [Command]
    private void ProcessDamage(uint netId, float damageAmount)
    {
        PlayerHealthSystem.playerHealthSystems[netId].TakeDamageRpc(damageAmount);
    }
}
