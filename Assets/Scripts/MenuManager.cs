using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] float fadeSpeed;

    GameObject currentScreen;

    private void Start() 
    {
        currentScreen = mainMenuScreen;
    }

    public void TransitionToNewGameScreen(GameObject newScreen)
    {
        StartCoroutine("FadeBetweenScreens", newScreen);
    }

    public void QuitGame()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }

    IEnumerator FadeBetweenScreens(GameObject newScreen)
    {
        CanvasGroup cg = currentScreen.GetComponent<CanvasGroup>();
        float iv = 0.0f;

        cg.interactable = false;

        while(iv <= 1.0f)
        {  
            iv += Time.deltaTime * fadeSpeed;
            cg.alpha = Mathf.Lerp(1.0f, 0.0f, iv);
            yield return null;
        }

        currentScreen.SetActive(false);
        currentScreen = newScreen;
        newScreen.SetActive(true);
        cg = newScreen.GetComponent<CanvasGroup>();
        cg.interactable = false;
        iv = 0.0f;

        while(iv <= 1.0f)
        {  
            iv += Time.deltaTime * fadeSpeed;
            cg.alpha = Mathf.Lerp(0.0f, 1.0f, iv);
            yield return null;
        }
        cg.interactable = true;
        yield return null;
    }

    IEnumerator FadeIntoPlay(GameManager.Difficulty difficulty)
    {
        CanvasGroup cg = currentScreen.GetComponent<CanvasGroup>();
        float iv = 0.0f;
        cg.interactable = false;

        while(iv <= 1.0f)
        {  
            iv += Time.deltaTime * fadeSpeed;
            cg.alpha = Mathf.Lerp(1.0f, 0.0f, iv);
            yield return null;
        }
        currentScreen.SetActive(false);
        GameManager.Instance.StartNewGame(difficulty);
        yield return null;
    }
    public void StartEasyGame()
    {
        GameManager.Difficulty difficulty = new GameManager.Difficulty(3,4);
        StartCoroutine("FadeIntoPlay",difficulty);
    }

    public void StartMediumGame()
    {
        GameManager.Difficulty difficulty = new GameManager.Difficulty(4,5);
        StartCoroutine("FadeIntoPlay",difficulty);
    }

    public void StartHardGame()
    {
        GameManager.Difficulty difficulty = new GameManager.Difficulty(4,7);
        StartCoroutine("FadeIntoPlay",difficulty);
    }
}