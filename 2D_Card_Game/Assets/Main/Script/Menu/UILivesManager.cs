using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILivesManager : MonoBehaviour
{
    [Header("Lives UI")]
    public List<Image> lifeIcons;
    public float fadeDuration = 0.5f;
    private int currentLifeIndex = 0;

    public static Action OnLifeLost;
    public static Action<string> OnStartTextNotify;
    public GameObject tempPanel;
    private void OnEnable()
    {
        OnLifeLost += FadeNextLife;
    }

    private void OnDisable()
    {
        OnLifeLost -= FadeNextLife;
    }

    private void Start()
    {
        ResetLives(); 
    }

    private void FadeNextLife()
    {
        if (currentLifeIndex < lifeIcons.Count)
        {
            StartCoroutine(FadeOut(lifeIcons[currentLifeIndex]));
            currentLifeIndex++;
        }
        else if (currentLifeIndex == lifeIcons.Count)
        {
            // Result
            tempPanel.SetActive(true);
            OnStartTextNotify?.Invoke("Restart");
            Debug.Log("All lives used.");
        }
    }

    private IEnumerator FadeOut(Image img)
    {
        float elapsed = 0f;
        Color startColor = img.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsed < fadeDuration)
        {
            img.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        img.color = endColor;
    }

    public void ResetLives()
    {
        foreach (var img in lifeIcons)
        {
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
        currentLifeIndex = 0;
    }
}
