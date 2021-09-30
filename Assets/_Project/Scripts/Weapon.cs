using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private BodyPartToDamage[] bodyPartToDamage;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private GameObject decalPrefab;

    [SerializeField] private int MagCapacity = 30;
    [SerializeField] private int currentBulletsLeft = 0;

    private void Start()
    {
        currentBulletsLeft = MagCapacity;
    }

    public float GetDamageAmount(BodyPartType bodyPartType)
    {
        for (int i = 0; i < bodyPartToDamage.Length; i++)
        {
            if (bodyPartType == bodyPartToDamage[i].bodyPartType)
            {
                return bodyPartToDamage[i].damageAmount;
            }
        }

        return 0;
    }
    
    public void Attack(Transform raycastPointTransform, LayerMask hittableMask)
    {
        if (!CanShoot())
        {
            return;
        }
        
        if (Physics.Raycast(raycastPointTransform.position,
            raycastPointTransform.forward, out RaycastHit hit, 1000f, hittableMask))
        {
            SpawnDecal(hit);
        }

        weaponAnimator.SetTrigger("Shoot");
        AudioManager.Instance.PlayWeaponShotAudio();
        currentBulletsLeft--;
    }

    public void ReloadMag(Action onReloadAction)
    {
        StartCoroutine(ReloadMagRoutine(onReloadAction));
    }

    private IEnumerator ReloadMagRoutine(Action onReloadAction)
    {
        yield return new WaitForSecondsRealtime(1f);
        currentBulletsLeft = MagCapacity;
        onReloadAction?.Invoke();
    }

    public bool CanShoot()
    {
        return currentBulletsLeft > 0;
    }

    public int GetAvailableBullets()
    {
        return currentBulletsLeft;
    }

    public int GetMagCapacity()
    {
        return MagCapacity;
    }

    private void SpawnDecal(RaycastHit hit)
    {
        GameObject decal = Instantiate(decalPrefab, hit.point + (hit.normal * .01f), Quaternion.identity);
        decal.transform.forward = hit.normal;
        Destroy(decal, 5f);
    }
}

[System.Serializable]
public class BodyPartToDamage
{
    public BodyPartType bodyPartType;
    public float damageAmount;
}
