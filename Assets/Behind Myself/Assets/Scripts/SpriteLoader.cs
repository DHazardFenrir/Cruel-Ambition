using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteLoader
{
     private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public static Sprite GetSprite(string spriteKey, string pathSheet = "Sprites-16x16")
    {
        if (spriteCache.TryGetValue(spriteKey, out Sprite cached))
            return cached;

        Sprite[] allSprites = Resources.LoadAll<Sprite>(pathSheet);

        foreach (var s in allSprites)
        {
            spriteCache[s.name] = s;
        }

        if (spriteCache.TryGetValue(spriteKey, out Sprite found))
            return found;
        Debug.LogWarning($"Sprite {spriteKey} no encontrado en {pathSheet}");
        return null;
    }

}
