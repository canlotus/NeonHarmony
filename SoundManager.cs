using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;  // Singleton instance

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton instance atamasý
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ses yönetimi her sahnede kalsýn
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Belirli bir ses dosyasýný çal
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
