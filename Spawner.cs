using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static event System.Action OnShapeSpawned;

    public static Spawner Instance { get; private set; }

    public GameObject circlePrefab;
    public GameObject squarePrefab;
    public Color[] shapeColors;

    public Transform[] initialSpawnPoints; // Başlangıç pozisyonları için boş GameObject'ler

    private Vector3 spawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        spawnPosition = transform.position;
    }

    private void Start()
    {
        SpawnInitialShapes(); // Oyun başlarken bir defa çalıştırılacak
    }

    public Circle SpawnRandom()
    {
        int randomNumber = GenerateRandomNumber();

        GameObject prefabToSpawn;

        // GameMode'a göre spawn mantığı
        int gameMode = PlayerPrefs.GetInt("GameMode", 0);
        if (gameMode == 2) // Karışık mod
        {
            prefabToSpawn = Random.value > 0.5f ? circlePrefab : squarePrefab;
        }
        else // Diğer modlarda sadece Circle
        {
            prefabToSpawn = circlePrefab;
        }

        Circle newShape = Spawn(randomNumber, spawnPosition, prefabToSpawn);
        OnShapeSpawned?.Invoke();
        return newShape;
    }

    public Circle Spawn(int number, Vector3 position, GameObject prefab)
    {
        GameObject shapeObject = Instantiate(prefab, position, Quaternion.identity);
        Circle newShape = shapeObject.GetComponent<Circle>();

        newShape.SetNumber(number);
        newShape.SetColor(GetColor(number));
        newShape.SetScale(number);

        // Bounce değerini GameMode'a göre ayarla
        int gameMode = PlayerPrefs.GetInt("GameMode", 0);
        float bounciness = (gameMode == 1) ? 1.2f : 0.2f; // Bounce modda 1.2, diğer modlarda 0.2

        newShape.SetPhysicsMaterial(bounciness);

        // Oyun sırasında normalde düşmeyecek
        newShape.CircleRigidbody.gravityScale = 0;

        return newShape;
    }

    private void SpawnInitialShapes()
    {
        if (initialSpawnPoints.Length == 0) return;

        foreach (Transform spawnPoint in initialSpawnPoints)
        {
            int randomNumber = GenerateRandomNumber();

            GameObject prefabToSpawn;

            // GameMode'a göre spawn mantığı
            int gameMode = PlayerPrefs.GetInt("GameMode", 0);
            if (gameMode == 2) // Karışık mod
            {
                prefabToSpawn = Random.value > 0.5f ? circlePrefab : squarePrefab;
            }
            else
            {
                prefabToSpawn = circlePrefab;
            }

            Circle newShape = Spawn(randomNumber, spawnPoint.position, prefabToSpawn);

            // Başlangıçta spawn edilenlerin gravityScale normal olsun
            newShape.CircleRigidbody.gravityScale = 1;
        }
    }

    private int GenerateRandomNumber()
    {
        return (int)Mathf.Pow(2, Random.Range(1, 6));
    }

    public Color GetColor(int number)
    {
        return shapeColors[(int)(Mathf.Log(number) / Mathf.Log(2)) - 1];
    }
}