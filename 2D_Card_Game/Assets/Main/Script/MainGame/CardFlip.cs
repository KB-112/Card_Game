using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardFlip : MonoBehaviour
{
    [Header("UI Button that triggers the flip")]
    public Button flipButton;

    [Header("Flip Settings")]
    public float flipDuration = 0.3f;
    public float showBackDelay = 2f;

    private bool isFlipping = false;
    private bool isBack = false;
    public Image backImage;
    public CardImageList cardImageList;
    public CardImageSwitcher cardImageSwitcher;
    public int cardId;
    public CardMatchManagerSO matchManager;
    void Start()
    {
        cardImageSwitcher.InitializePairs(cardImageList,20);
        flipButton.onClick.AddListener(OnCardTap);

    }

    void OnDestroy()
    {
        flipButton.onClick.RemoveListener(OnCardTap);
    }

    void OnCardTap()
    {
        if (!isFlipping)
            StartCoroutine(FlipCard());
    }

    IEnumerator FlipCard()
    {
        isFlipping = true;
        yield return StartCoroutine(RotateToY(90f));

        
        cardImageSwitcher.AssignImageById(cardId, backImage);

        yield return StartCoroutine(RotateToY(180f));

       
        matchManager.RegisterCard(cardId, backImage.sprite, () =>
        {
            StartCoroutine(RevertCard());
        },this.gameObject);

        isFlipping = false;

        if(matchManager.openedCards.Count <2)
        {
            StartCoroutine(RevertCard());
        }
       
    }

    IEnumerator RevertCard()
    {
        yield return StartCoroutine(RotateToY(90f));
        cardImageSwitcher.RevertToBack(cardId, backImage);
        yield return StartCoroutine(RotateToY(0f));
    }

    IEnumerator RotateToY(float targetY)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, targetY, 0);
        float elapsed = 0f;

        while (elapsed < flipDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / flipDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
}
