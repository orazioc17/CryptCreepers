using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public int time = 30;
    /*
     Cuando hacemos public una variable, no hace falta agregarle SerializeField porque ya lo tendra,
     No todas las variables se deben hacer publicas, seria una mala practica, las variables publicas deben ser solo 
     Las que se utilicen en otro script
    */
    public int difficulty = 1;
    public bool gameOver = false;
    [SerializeField] int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            UIManager.sharedInstance.UpdateUIScore(score);
            if(score % 1000 == 0)
            {
                difficulty++;
            }
        }
    }

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.sharedInstance.UpdateUIScore(score);
        StartCoroutine(CountdownRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountdownRoutine()
    {
        while(time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            UIManager.sharedInstance.UpdateUITime(time);
        }

        gameOver = true;
        UIManager.sharedInstance.ShowGameOverScreen();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
