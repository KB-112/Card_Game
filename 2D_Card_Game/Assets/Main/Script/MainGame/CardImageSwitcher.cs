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

    /// <summary>
    /// Initializes pairs such that each sprite is used exactly twice per pair, and
    /// evenly distributes reuse when totalPairs exceeds available images.
    /// </summary>
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
        int available = imageList.powerCardImages.Length;

        // Determine base usage and remainder for distributing pairs
        int basePairs = totalPairs / available;
        int extraPairs = totalPairs % available;

        // Build a list of which image indices to use for each pair
        List<int> pairIndices = new List<int>(totalPairs);
        for (int i = 0; i < available; i++)
        {
            for (int j = 0; j < basePairs; j++)
                pairIndices.Add(i);
        }
        // Distribute extra pairs among random images
        List<int> leftovers = new List<int>(available);
        for (int i = 0; i < available; i++) leftovers.Add(i);
        Shuffle(leftovers);
        for (int k = 0; k < extraPairs; k++)
            pairIndices.Add(leftovers[k]);

        // Now build the sprite pool: two copies per pair index
        List<Sprite> imagePool = new List<Sprite>(totalCards);
        foreach (int idx in pairIndices)
        {
            var sprite = imageList.powerCardImages[idx];
            imagePool.Add(sprite);
            imagePool.Add(sprite);
        }

        // Shuffle the final pool of cards
        Shuffle(imagePool);

        // Assign to card IDs
        for (int id = 0; id < totalCards; id++)
        {
            cardAssignments.Add(new CardAssignment
            {
                id = id,
                assignedImage = imagePool[id]
            });
        }

        // Optional sanity check
        VerifyPairing(imagePool);
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
        var assignment = cardAssignments.Find(c => c.id == id);
        if (assignment == null)
        {
            Debug.LogWarning($"No assignment found for card ID {id}");
            return;
        }
        if (!originalBackSprites.ContainsKey(id))
            originalBackSprites[id] = targetImage.sprite;

        targetImage.sprite = assignment.assignedImage;
    }

    public void RevertToBack(int id, Image targetImage)
    {
        if (targetImage != null && originalBackSprites.ContainsKey(id))
            targetImage.sprite = originalBackSprites[id];
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void VerifyPairing(List<Sprite> pool)
    {
        var counts = new Dictionary<string, int>();
        foreach (var sprite in pool)
        {
            var key = sprite.name;
            counts.TryGetValue(key, out int c);
            counts[key] = c + 1;
        }
        foreach (var kvp in counts)
        {
            if (kvp.Value != 2)
                Debug.LogError($"Sprite '{kvp.Key}' appears {kvp.Value} times (expected 2).");
            else
                Debug.Log($"Sprite '{kvp.Key}' correctly paired.");
        }
    }
}
