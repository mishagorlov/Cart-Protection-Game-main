using UnityEngine;
using UnityEngine.UI; // Импорт для работы с UI элементами

public class ButtonSound : MonoBehaviour
{
    public AudioClip sound; // Аудиоклип, который будет проигрываться
    private Button button; // Ссылка на компонент Button
    private AudioSource audioSource; // Источник аудио

    void Start()
    {
        button = GetComponent<Button>(); // Получаем компонент Button
        audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем компонент AudioSource
        audioSource.clip = sound; // Присваиваем аудиоклип
        audioSource.playOnAwake = false; // Отключаем воспроизведение при старте

        // Добавляем слушатель к событию нажатия кнопки
        button.onClick.AddListener(() => PlaySound());
    }

    void PlaySound()
    {
        if (sound != null)
        {
            audioSource.Play(); // Воспроизводим звук
        }
    }
}
