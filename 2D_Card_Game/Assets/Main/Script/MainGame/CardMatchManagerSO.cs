using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardMatchManagerSO", menuName = "Card Game/Card Match Manager")]
public class CardMatchManagerSO : ScriptableObject
{
    [System.Serializable]
    public class OpenedCard
    {
        public int cardId;
        public Sprite cardImage;
        public Action revertCallback;
        public GameObject cardObject; 
    }


    public List<OpenedCard> openedCards = new List<OpenedCard>();
    private Sprite currentComboImage = null;

    public Action<int> OnComboUpdated;

    
    public void RegisterCard(int cardId, Sprite image, Action revertCallback, GameObject cardObject)

    {
        // If no cards opened, start new combo
        if (openedCards.Count == 0)
        {
            currentComboImage = image;
        }
        else if (currentComboImage != image)
        {
            // If a mismatch, only revert if we had a valid combo (2 or more)
            if (openedCards.Count >= 2)
            {
                UILivesManager.OnLifeLost?.Invoke();
                Debug.Log("Mismatch! Combo broken. Reverting all.");
                RevertAllCards();
            }
            else
            {
                Debug.Log("First mismatch — only one card was opened, so no revert needed.");
                openedCards.Clear(); // Clear the single unmatched card

            }

            currentComboImage = image;
        }

        // Add the new card
        openedCards.Add(new OpenedCard
        {
            cardId = cardId,
            cardImage = image,
            revertCallback = revertCallback,
            cardObject = cardObject
        });



        if (openedCards.Count >= 2)
        {
            ComboScoreManager.RegisterComboHit(openedCards.Count);
            Debug.Log($"Combo continuing: {openedCards.Count} cards matched.");
            OnComboUpdated?.Invoke(openedCards.Count);
            
        }
    }

    private void RevertAllCards()
    {
        Debug.Log("Mismatch! Combo broken. Reverting all.");

        foreach (var card in openedCards)
        {
            
            card.revertCallback?.Invoke();

            
            if (card.cardObject != null)
            {
                var canvasGroup = card.cardObject.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = card.cardObject.AddComponent<CanvasGroup>();
                }

                canvasGroup.alpha = 0; 
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        openedCards.Clear();
    }


    public void ClearMatches()
    {
        RevertAllCards();
        currentComboImage = null;
    }
}
