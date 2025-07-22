using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardImageSwitcher : MonoBehaviour
{
    // Stores original back sprite for each card by ID
    private Dictionary<int, Sprite> originalBackSprites = new Dictionary<int, Sprite>();

    [System.Serializable]
    public class CardAssignment
    {
        public int id;
        public Sprite assignedImage;
    }

    public List<CardAssignment> cardAssignments = new List<CardAssignment>();
    private bool isInitialized = false;

    public void InitializePairs(CardImageList imageList, int totalCards)
    {
        if (imageList == null || imageList.powerCardImages.Length == 0 || totalCards % 2 != 0)
        {
            Debug.LogError("Image list invalid or totalCards must be even.");
            return;
        }

        cardAssignments.Clear();
        originalBackSprites.Clear();

        int totalPairs = totalCards / 2;
        List<Sprite> selectedImages = new List<Sprite>();

        // Pick images for pairing
        for (int i = 0; i < totalPairs; i++)
        {
            selectedImages.Add(imageList.powerCardImages[i % imageList.powerCardImages.Length]);
        }

        // Create two of each
        List<Sprite> imagePool = new List<Sprite>(selectedImages);
        imagePool.AddRange(selectedImages);
        Shuffle(imagePool);

        // Assign shuffled images to cards by ID
        for (int i = 0; i < totalCards; i++)
        {
            cardAssignments.Add(new CardAssignment
            {
                id = i,
                assignedImage = imagePool[i]
            });
        }

        isInitialized = true;
    }

    public void AssignImageById(int id, Image targetImage)
    {
        if (!isInitialized)
        {
            Debug.LogError("CardImageSwitcher not initialized.");
            return;
        }

        if (targetImage == null)
        {
            Debug.LogWarning("Target image is null.");
            return;
        }

        CardAssignment assignment = cardAssignments.Find(card => card.id == id);
        if (assignment == null)
        {
            Debug.LogWarning($"No assignment found for card ID {id}");
            return;
        }

        // Store the back sprite only once
        if (!originalBackSprites.ContainsKey(id))
            originalBackSprites[id] = targetImage.sprite;

        targetImage.sprite = assignment.assignedImage;
    }

    public void RevertToBack(int id, Image targetImage)
    {
        if (originalBackSprites.ContainsKey(id) && targetImage != null)
        {
            targetImage.sprite = originalBackSprites[id];
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            T temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
        }
    }
}
