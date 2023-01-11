using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmFX : MonoBehaviour
{
    [SerializeField]
    private AudioClip shotClip;
    private AudioSource audioSource;
    [SerializeField]
    private GameObject bulletHolePrefab;

    private float defPitch;
    private const float PITCH_OFFSET = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        defPitch = audioSource.pitch;
    }

    // TODO:
    // Change to Fire which also handles anims
    public void PlayShotSound()
    {
        audioSource.pitch = Random.Range(defPitch - PITCH_OFFSET, defPitch + PITCH_OFFSET);
        audioSource.PlayOneShot(shotClip);
    }

    public void SpawnBulletHole(Vector3 hit, Vector3 hitNormal)
    {
        GameObject bulletHole = Instantiate(bulletHolePrefab, hit, Quaternion.identity);
        bulletHole.transform.forward = -hitNormal;
        Destroy(bulletHole, 5f);
        
    }
}
