using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolderTransform;
    [SerializeField] private Transform weaponRaycastPointTransform;
    [SerializeField] private Weapon equippedWeapon;

    private void Update()
    {
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

        equippedWeapon.Attack(weaponRaycastPointTransform);
    }
}
