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

    [SerializeField] private WeaponView _weaponView;

    private void Start()
    {
        _weaponView.UpdateView(equippedWeapon.GetAvailableBullets(), equippedWeapon.GetMagCapacity());
    }

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Reload()
    {
        if (equippedWeapon == null)
        {
            Debug.Log("No Weapon Equipped!");
            return;
        }

        _weaponView.SetReloading(true);
        equippedWeapon.ReloadMag(() =>
        {
            _weaponView.UpdateView(equippedWeapon.GetAvailableBullets(), equippedWeapon.GetMagCapacity());
            _weaponView.SetReloading(false);
        });
    }

    private void Shoot()
    {
        if (equippedWeapon == null)
        {
            Debug.Log("No Weapon Equipped!");
            return;
        }

        if (!equippedWeapon.CanShoot())
        {
            return;
        }
        equippedWeapon.Attack(weaponRaycastPointTransform, hittableMask);
        WeaponShoot(equippedWeapon);

        _weaponView.UpdateView(equippedWeapon.GetAvailableBullets(), equippedWeapon.GetMagCapacity());
    }
    
    private void WeaponShoot(Weapon weaponToShoot)
    {
        if (Physics.Raycast(weaponRaycastPointTransform.position,
            weaponRaycastPointTransform.forward, out RaycastHit hit, 1000f, hittableMask))
        {
            Debug.Log($"I Hit {hit.collider.name}");
            if (hit.collider.gameObject.TryGetComponent(out BodyPart bodyPart))
            {
                float damageAmount = equippedWeapon.GetDamageAmount(bodyPart.bodyPartType);
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
