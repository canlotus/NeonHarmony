using UnityEngine;
using TMPro;

public class Circle : MonoBehaviour
{
    static int staticID = 0;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private PhysicsMaterial2D physicsMaterial; // Fizik materyali referansı

    [HideInInspector] public int CircleID;
    [HideInInspector] public Color CircleColor;
    [HideInInspector] public int CircleNumber;
    [HideInInspector] public Rigidbody2D CircleRigidbody;
    [HideInInspector] public bool IsMainCircle;
    [HideInInspector] public bool IsMerging = false;  // Birleştirme kontrolü için

    private SpriteRenderer circleSpriteRenderer;
    private Collider2D[] colliders;

    private void Awake()
    {
        CircleID = staticID++;
        circleSpriteRenderer = GetComponent<SpriteRenderer>();
        CircleRigidbody = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        CircleRigidbody.gravityScale = 0;  // Başlangıçta düşmeyecek
    }

    public void ReleaseCircle()
    {
        CircleRigidbody.gravityScale = 1;
    }

    // Çarpışmaları devre dışı bırak veya etkinleştir
    public void ToggleColliders(bool isEnabled)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = isEnabled;
        }
    }

    // Circle'ın rengini ayarla
    public void SetColor(Color color)
    {
        CircleColor = color;
        color.a = 1.0f;
        circleSpriteRenderer.color = color; // Şeklin rengini ayarla

        if (numberText != null)
        {
            // Yazının rengini de aynı yap
            numberText.color = color;
        }
    }

    public void SetScale(int number)
    {
        float scale = 1.0f;

        switch (number)
        {
            case 2: scale = 0.047f; break;
            case 4: scale = 0.055f; break;
            case 8: scale = 0.064f; break;
            case 16: scale = 0.071f; break;
            case 32: scale = 0.077f; break;
            case 64: scale = 0.084f; break;
            case 128: scale = 0.090f; break;
            case 256: scale = 0.096f; break;
            case 512: scale = 0.102f; break;
            case 1024: scale = 0.11f; break;
            case 2048: scale = 0.115f; break;
            case 4096: scale = 0.120f; break;
            default: scale = 1.0f; break;
        }

        transform.localScale = new Vector3(scale, scale, 1);
    }

    public void SetNumber(int number)
    {
        CircleNumber = number;
        numberText.text = number.ToString();

        // Sayıya göre mass ayarla (1 ile 1.5 arasında)
        CircleRigidbody.mass = CalculateMass(number);
    }

    private float CalculateMass(int number)
    {
        int minNumber = 2;
        int maxNumber = 4096;
        float minMass = 1.0f;
        float maxMass = 1.5f;
        float normalizedValue = Mathf.InverseLerp(minNumber, maxNumber, number);
        return Mathf.Lerp(minMass, maxMass, normalizedValue);
    }

    // Fizik materyalini ayarlamak için bir fonksiyon
    public void SetPhysicsMaterial(float bounciness)
    {
        if (physicsMaterial == null)
        {
            physicsMaterial = new PhysicsMaterial2D();  // Eğer fizik materyali yoksa oluştur
        }

        physicsMaterial.bounciness = bounciness;
        CircleRigidbody.sharedMaterial = physicsMaterial;  // Fizik materyalini Rigidbody'ye uygula
    }
}