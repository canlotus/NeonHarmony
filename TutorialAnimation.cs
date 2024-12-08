using UnityEngine;
using System.Collections;

public class TutorialAnimation : MonoBehaviour
{
    [Header("Tutorial Elements")]
    public GameObject[] rightArrows; // Sağ oklar
    public GameObject[] leftArrows;  // Sol oklar
    public GameObject touchIndicator; // Dokunma işareti (GameObject)

    [Header("Animation Settings")]
    public float arrowDelay = 0.3f; // Her okun aktif olma gecikmesi
    public float touchMoveDuration = 1.0f; // Dokunma işaretinin hareket süresi

    private Vector3 touchStartPosition = new Vector3(0, -1.5f, 0); // Dokunma işaretinin başlangıç pozisyonu
    private Vector3 rightTargetPosition = new Vector3(1.2f, -1.5f, 0); // Sağ hedef pozisyon
    private Vector3 leftTargetPosition = new Vector3(-1.2f, -1.5f, 0); // Sol hedef pozisyon

    private void Start()
    {
        // Dokunma işaretinin başlangıç pozisyonunu ayarla
        touchIndicator.transform.localPosition = touchStartPosition;

        // Öğreticiyi başlat
        StartCoroutine(PlayTutorial());
    }

    private IEnumerator PlayTutorial()
    {
        // Sağ okları sırayla aç ve dokunma işaretini aynı anda sağa hareket ettir
        yield return StartCoroutine(AnimateArrowsAndTouch(rightArrows, rightTargetPosition));

        // Sağ okları kapat
        SetActiveState(rightArrows, false);

        // Sol okları sırayla aç ve dokunma işaretini aynı anda sola hareket ettir
        yield return StartCoroutine(AnimateArrowsAndTouch(leftArrows, leftTargetPosition));

        // Sol okları ve dokunma işaretini kapat
        SetActiveState(leftArrows, false);
        touchIndicator.SetActive(false);

        Debug.Log("Tutorial Animation Completed");
    }

    private IEnumerator AnimateArrowsAndTouch(GameObject[] arrows, Vector3 targetPosition)
    {
        // Dokunma işaretinin hedef pozisyona hareketini paralel başlat
        StartCoroutine(MoveTouchIndicator(targetPosition));

        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive(true); // Oku görünür yap
            yield return new WaitForSeconds(arrowDelay); // Her ok için bekleme süresi
        }

        // Dokunma işaretinin hareketi tamamlanana kadar bekle
        yield return new WaitForSeconds(touchMoveDuration);
    }

    private IEnumerator MoveTouchIndicator(Vector3 targetPosition)
    {
        Vector3 startPosition = touchIndicator.transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < touchMoveDuration)
        {
            touchIndicator.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / touchMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        touchIndicator.transform.localPosition = targetPosition; // Hedef pozisyonda sabitle
    }

    private void SetActiveState(GameObject[] arrows, bool state)
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(state);
        }
    }
}