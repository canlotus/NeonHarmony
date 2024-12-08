using UnityEngine;
using UnityEngine.EventSystems; // UI tıklamalarını kontrol etmek için gerekli

public class Player : MonoBehaviour
{
    private Circle mainCircle;

    private void Start()
    {
        SpawnCircle();
    }

    private void Update()
    {
        if (mainCircle != null && mainCircle.IsMainCircle)
        {
            // Eğer dokunma/tıklama bir UI elemanında değilse input'u işleyelim
            if (!IsPointerOverUIElement())
            {
                HandleInput();
            }
        }
    }

    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // Mouse ile kontrol
        if (Input.GetMouseButtonDown(0)) // Tıklanırsa
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            float newX = Mathf.Clamp(mousePosition.x, -1.56f, 1.56f);
            mainCircle.transform.position = new Vector3(newX, mainCircle.transform.position.y, 0f); // Doğrudan taşınır
        }

        if (Input.GetMouseButton(0)) // Tıklama devam ederken
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            float newX = Mathf.Clamp(mousePosition.x, -1.56f, 1.56f);
            mainCircle.transform.position = new Vector3(newX, mainCircle.transform.position.y, 0f);
        }

        if (Input.GetMouseButtonUp(0)) // Tıklama bırakılırsa
        {
            ReleaseCircle();
        }
#endif

        // Dokunma ile kontrol
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;

            if (touch.phase == TouchPhase.Began) // Dokunma başladığında
            {
                float newX = Mathf.Clamp(touchPosition.x, -1.56f, 1.56f);
                mainCircle.transform.position = new Vector3(newX, mainCircle.transform.position.y, 0f); // Doğrudan taşınır
            }
            else if (touch.phase == TouchPhase.Moved) // Dokunma hareket ettiğinde
            {
                float newX = Mathf.Clamp(touchPosition.x, -1.56f, 1.56f);
                mainCircle.transform.position = new Vector3(newX, mainCircle.transform.position.y, 0f);
            }
            else if (touch.phase == TouchPhase.Ended) // Dokunma sona erdiğinde
            {
                ReleaseCircle();
            }
        }
    }

    private void ReleaseCircle()
    {
        if (mainCircle != null)
        {
            mainCircle.IsMainCircle = false;

            // Çarpışmaları yeniden etkinleştir
            mainCircle.ToggleColliders(true);

            // Fiziksel hareketi yeniden etkinleştir
            mainCircle.CircleRigidbody.gravityScale = 1;
            mainCircle.CircleRigidbody.isKinematic = false;

            mainCircle = null;
        }

        // 0.3 saniye gecikmeyle yeni circle spawn et
        Invoke("SpawnCircle", 0.3f);
    }

    private void SpawnCircle()
    {
        mainCircle = Spawner.Instance.SpawnRandom();
        mainCircle.IsMainCircle = true;

        // Çarpışmaları devre dışı bırak
        mainCircle.ToggleColliders(false);

        // Fiziksel kuvvetleri devre dışı bırak
        mainCircle.CircleRigidbody.gravityScale = 0;
        mainCircle.CircleRigidbody.isKinematic = true;
    }

    private void SpawnNewCircle()
    {
        SpawnCircle();
    }

    // Pointer'ın UI elemanında olup olmadığını kontrol et 
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}