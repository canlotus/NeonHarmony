using UnityEngine;
using System.Collections;

public class RedZoneTrigger : MonoBehaviour
{
    private bool gameOver = false;  // Oyunun bitti?ini kontrol eder

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // E?er Circle tag'ine sahip bir obje RedZone'a girerse
        if (collision.CompareTag("Circle") && !gameOver)
        {


            // Coroutine ile 1.5 saniye boyunca tetiklenme kontrol? yapal?m
            StartCoroutine(CheckForGameOver(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Circle RedZone'dan ??karsa, oyunu iptal edelim
        if (collision.CompareTag("Circle"))
        {
            StopAllCoroutines();  // Circle RedZone'dan ??karsa, t?m coroutine'leri durdur
        }
    }

    private IEnumerator CheckForGameOver(GameObject circle)
    {
        // 1.5 saniye boyunca Circle RedZone'da kal?yor mu diye kontrol edelim
        yield return new WaitForSeconds(1.5f);

        // E?er Circle hala RedZone'da ise oyun biter
        if (circle != null && !gameOver)
        {
            Debug.Log("Game Over");
            gameOver = true;

            // Oyun bitti?inde GameManager'daki GameOver fonksiyonunu ?a??r?yoruz
            GameManager.Instance.GameOver();
        }
    }
}
