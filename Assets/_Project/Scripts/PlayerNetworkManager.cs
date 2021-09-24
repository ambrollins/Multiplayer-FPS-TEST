using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerNetworkManager : NetworkBehaviour
{
    [SerializeField] private Camera playerMainCamera, playerGunCamera;
    [SerializeField] private AudioListener playerMainCameraAudioListener;
    [SerializeField] private GameObject playerCanvas;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            playerMainCamera.enabled = false;
            playerGunCamera.enabled = false;
            playerMainCameraAudioListener.enabled = false;
            playerCanvas.SetActive(false);
            PlayerHelper.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Enemies"));
        }
    }
}
