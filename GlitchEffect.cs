using UnityEngine;
using UnityEngine.UI;

public class SmoothFlowEffect : MonoBehaviour
{
    private Image image;
    private Color originalColor;

    [Header("Alpha Settings")]
    public float minAlpha = 0.5f; // Minimum şeffaflık
    public float maxAlpha = 1.0f; // Maksimum şeffaflık
    public float alphaSpeed = 2.0f; // Şeffaflık değişim hızı

    [Header("Scale Settings")]
    public float minScale = 0.9f; // Minimum boyut
    public float maxScale = 1.1f; // Maksimum boyut
    public float scaleSpeed = 2.0f; // Boyut değişim hızı

    private void Start()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            originalColor = image.color;
        }
    }

    private void Update()
    {
        if (image == null) return;

        // Şeffaflık değişimi
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * alphaSpeed) + 1) / 2);
        Color newColor = originalColor;
        newColor.a = alpha;
        image.color = newColor;

        // Boyut değişimi
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * scaleSpeed) + 1) / 2);
        image.transform.localScale = new Vector3(scale, scale, 1);
    }
}