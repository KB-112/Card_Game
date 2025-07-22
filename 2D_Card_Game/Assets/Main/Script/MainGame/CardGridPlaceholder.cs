using UnityEngine;
using UnityEngine.UI;

public class CardGridPlaceholder : MonoBehaviour
{
    [Header("Card Reference")]
    public RectTransform cardReference;
    public RectTransform cardHolderPanel;
    public GameObject cardPrefab;

    [Header("Layout Settings")]
    public int columns = 2;
    public int rows = 2;
    public float spacingX = 10f;
    public float spacingY = 10f;

    [Header("Grid Padding (around edges)")]
    public float paddingX = 20f;
    public float paddingY = 20f;

    void Start()
    {
        CardComponentInitializer();
        SetupHolder();
    }

    void CardComponentInitializer()
    {
        if (cardReference == null || cardHolderPanel == null || cardPrefab == null)
        {
            Debug.LogWarning("Missing references (cardReference, cardHolderPanel or cardPrefab)");
            return;
        }
    }

    void SetupHolder()
    {
        if (cardReference == null || cardHolderPanel == null) return;

        Vector2 cardSize = cardReference.rect.size;

        float totalWidth = columns * cardSize.x + (columns - 1) * spacingX + paddingX * 2;
        float totalHeight = rows * cardSize.y + (rows - 1) * spacingY + paddingY * 2;

        cardHolderPanel.sizeDelta = new Vector2(totalWidth, totalHeight);
        cardHolderPanel.anchorMin = new Vector2(0.5f, 0.5f);
        cardHolderPanel.anchorMax = new Vector2(0.5f, 0.5f);
        cardHolderPanel.pivot = new Vector2(0.5f, 0.5f);
        cardHolderPanel.anchoredPosition = Vector2.zero;

        GenerateCards(cardSize);
    }

    void GenerateCards(Vector2 cardSize)
    {
        if (cardReference == null || cardHolderPanel == null || cardPrefab == null)
            return;
        int cardId = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject card = Instantiate(cardPrefab, cardHolderPanel);
                RectTransform rt = card.GetComponent<RectTransform>();

                rt.sizeDelta = cardSize;
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);

                float posX = paddingX + col * (cardSize.x + spacingX);
                float posY = -paddingY - row * (cardSize.y + spacingY);

                rt.anchoredPosition = new Vector2(posX, posY);

              
                SetPivotAndKeepPosition(rt, new Vector2(0.5f, 0.5f));
                CardFlip flip = card.GetComponent<CardFlip>();
                flip.cardId = cardId;
                cardId++;
            }
        }
    }

   


    void SetPivotAndKeepPosition(RectTransform rt, Vector2 newPivot)
    {
        Vector2 size = rt.rect.size;
        Vector2 oldPivot = rt.pivot;
        Vector2 delta = newPivot - oldPivot;

        Vector2 deltaPosition = new Vector2(delta.x * size.x, delta.y * size.y);
        rt.pivot = newPivot;
        rt.anchoredPosition += deltaPosition;
    }


}
