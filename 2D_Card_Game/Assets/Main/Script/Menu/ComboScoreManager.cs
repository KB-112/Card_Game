using System;
using TMPro;
using UnityEngine;

public class ComboScoreManager : MonoBehaviour
{
   
    public static Action<int> OnComboScored;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
  
    [Header("Combo Settings")]
    public int pointsPerCombo = 100;

    private int currentCombo = 0;
    private int totalScore = 0;

    private void OnEnable()
    {
        OnComboScored += HandleCombo;
    }

    private void OnDisable()
    {
        OnComboScored -= HandleCombo;
    }

  
    public static void RegisterComboHit(int i )
    {
        OnComboScored?.Invoke(i);
    }

  
    public void ResetScore()
    {
        currentCombo = 0;
        totalScore = 0;
        UpdateUI();
        Debug.Log("Score and combo reset.");
    }

    private void HandleCombo(int comboIncrement)
    {
        currentCombo += comboIncrement;
        int scoreEarned = pointsPerCombo * currentCombo;
        totalScore += scoreEarned;

        Debug.Log($"Combo x{currentCombo} — Earned {scoreEarned} points. Total: {totalScore}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {totalScore}";

     
    }
}
