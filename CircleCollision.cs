using UnityEngine;

public class CircleCollision : MonoBehaviour
{
    public GameObject explosionEffectPrefab;  // Patlama efekt prefab'? i?in referans

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Circle otherCircle = collision.gameObject.GetComponent<Circle>();

        if (otherCircle != null)
        {
            Circle thisCircle = GetComponent<Circle>();

            // E?er iki circle'?n numaralar? ayn?ysa, onlar? birle?tir
            if (thisCircle.CircleNumber == otherCircle.CircleNumber)
            {
                MergeManager mergeManager = FindObjectOfType<MergeManager>();
                mergeManager.MergeShapes(thisCircle, otherCircle);

                // Birle?me s?ras?nda patlama efekti olu?tur
                Vector3 spawnPosition = (thisCircle.transform.position + otherCircle.transform.position) / 2;
                GameObject explosionEffect = Instantiate(explosionEffectPrefab, spawnPosition, Quaternion.identity);

                // 2 saniye sonra patlama efektini yok et
                Destroy(explosionEffect, 2.0f);

                // Circle ?zerindeki AudioSource'u kullanarak sesi ?al
                AudioSource thisAudioSource = thisCircle.GetComponent<AudioSource>();
                if (thisAudioSource != null && thisAudioSource.clip != null)
                {
                    thisAudioSource.Play();  // Sesi ?al
                }

                // Birle?me s?ras?nda fiziksel kuvvet ekle (iste?e ba?l?)
                Rigidbody2D rb = thisCircle.CircleRigidbody;
                rb.AddForce(new Vector2(0.5f, 0.5f), ForceMode2D.Impulse);  // Circle'? hafif?e yukar? do?ru ittir
            }
        }
    }
}
