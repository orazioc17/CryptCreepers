using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance; // Haciendo el UIManager un Singleton

    [SerializeField] Text healthText, scoreText, timeText, finalScore;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] AudioClip gameOverClip;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this; // Inicializando el UIManager como singleton
        }
    }

    public void UpdateUIScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void UpdateUIHealth(int newHealth)
    {
        healthText.text = newHealth.ToString();
    }

    public void UpdateUITime(int newTime)
    {
        timeText.text = newTime.ToString();
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Camera camera = FindObjectOfType<Camera>();
        AudioSource cameraAudio = Camera.FindObjectOfType<AudioSource>();
        cameraAudio.Stop();
        AudioSource.PlayClipAtPoint(gameOverClip, transform.position);
        finalScore.text = GameManager.sharedInstance.Score.ToString();
    }
}
