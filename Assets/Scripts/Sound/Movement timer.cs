using System.Collections;
using UnityEngine;

public class SimpleMultiKey : Sounds
{
    [SerializeField] private float interval = 0.3f;

    private void Update()
    {
        HandleKeyWithSound(KeyCode.W);
        HandleKeyWithSound(KeyCode.A);
        HandleKeyWithSound(KeyCode.S);
        HandleKeyWithSound(KeyCode.D);
    }

    private void HandleKeyWithSound(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            StopAllCoroutines(); // Останавливаем предыдущие
            StartCoroutine(KeySoundCoroutine(key));
        }

        if (Input.GetKeyUp(key))
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator KeySoundCoroutine(KeyCode key)
    {
        // Первое воспроизведение сразу
        PlaySoundForKey(key);

        while (Input.GetKey(key))
        {
            yield return new WaitForSeconds(interval);

            if (Input.GetKey(key))
            {
                PlaySoundForKey(key);
            }
        }
    }

    private void PlaySoundForKey(KeyCode key)
    {

            PlaySound(1);
        }
    }
