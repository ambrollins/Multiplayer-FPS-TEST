using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float bodyShotDamage = 10f;
    [SerializeField] private float headShotDamage = 40f;
    [SerializeField] private float legShotDamage = 40f;
    
    public void Attack(Transform raycastPointTransform)
    {
        if (Physics.Raycast(raycastPointTransform.position,
            raycastPointTransform.forward, out RaycastHit hit, 1000f))
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
                
                bodyPart.TakeDamage(damageAmount);
                Debug.Log("HELLO");
            }
            
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new[] { shootPoint.position, hit.point });
            lineRenderer.enabled = true;
            StopCoroutine(DisableLineRenderer());
            StartCoroutine(DisableLineRenderer());
        }
    }

    private IEnumerator DisableLineRenderer()
    {
        yield return new WaitForSecondsRealtime(.05f);
        lineRenderer.enabled = false;
    }
}
