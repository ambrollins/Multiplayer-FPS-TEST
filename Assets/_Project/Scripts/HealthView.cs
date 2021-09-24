using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private TextMeshProUGUI currentHealthTextView;

    public void UpdateView(float currentHealth)
    {
        healthBarFillImage.fillAmount = currentHealth / 100f;
        int roundedHealth = Mathf.RoundToInt(currentHealth);
        currentHealthTextView.SetText(roundedHealth.ToString());
    }
}
