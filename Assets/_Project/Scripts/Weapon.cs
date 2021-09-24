using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LineRenderer lineRenderer;
    public float bodyShotDamage = 10f;
    public float headShotDamage = 40f;
    public float legShotDamage = 40f;

    [SerializeField] private PlayerWeaponController playerWeaponController;
    
    public void Attack(Transform raycastPointTransform, LayerMask hittableMask)
    {
        if (Physics.Raycast(raycastPointTransform.position,
            raycastPointTransform.forward, out RaycastHit hit, 1000f, hittableMask))
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new[] { shootPoint.position, hit.point });
            lineRenderer.enabled = true;
            StopCoroutine(DisableLineRenderer());
            StartCoroutine(DisableLineRenderer());
        }
    }

    private IEnumerator DisableLineRenderer()
    {
        yield return new WaitForSecondsRealtime(.02f);
        lineRenderer.enabled = false;
    }
}
