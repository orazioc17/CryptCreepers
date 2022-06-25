using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField] AudioClip buttonClip;
    public void PlayGame()
    {
        AudioSource.PlayClipAtPoint(buttonClip, transform.position, 1);
        StartCoroutine(PlayButtonClip());
    }

    IEnumerator PlayButtonClip()
    {
        
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game");
    }
}
