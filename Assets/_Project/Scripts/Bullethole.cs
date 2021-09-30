using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullethole : MonoBehaviour
{
    private Renderer myRenderer;
    private MaterialPropertyBlock myPropertyBlock;
    [SerializeField] private Color originalColor;
    [SerializeField] private Color targetColor;
    private float lerpTime = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponentInChildren<Renderer>();
        myPropertyBlock = new MaterialPropertyBlock();
        myRenderer.GetPropertyBlock(myPropertyBlock);
        originalColor = myPropertyBlock.GetColor("_Color");
        originalColor.a = 1;
        targetColor = originalColor;
        targetColor.a = 0;
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0;
        while ((elapsedTime / lerpTime) < 1)
        {
            yield return new WaitForEndOfFrame();
            myRenderer.GetPropertyBlock(myPropertyBlock);
            Color updatedColor = Color.Lerp(originalColor, targetColor, elapsedTime / lerpTime);
            myPropertyBlock.SetColor("_Color",updatedColor);
            myRenderer.SetPropertyBlock(myPropertyBlock);
            elapsedTime += Time.deltaTime;
        }
    }
}
