using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bulletsTextView;
    [SerializeField] private GameObject reloadingMessageGO;
    
    public void UpdateView(int availableBullets, int magCapacity)
    {
        bulletsTextView.SetText($"{availableBullets}/{magCapacity}");
    }

    public void SetReloading(bool state)
    {
        reloadingMessageGO.SetActive(state);
    }
}
