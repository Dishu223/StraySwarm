#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using StraySwarm.Core;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Generates a standardized PlayerCat prefab with full Kawaii Cube wobble physics and tail tracking.
    /// Menu: Stray Swarm -> 🐱 Create Player Cat Prefab
    /// </summary>
    public static class CreatePlayerPrefab
    {
        [MenuItem("Stray Swarm/🐱 Create Player Cat Prefab", false, 5)]
        public static void GeneratePlayerPrefab()
        {
            string charDir = "Assets/_StraySwarm/Prefabs/Characters";
            if (!Directory.Exists(charDir)) Directory.CreateDirectory(charDir);

            string prefabPath = $"{charDir}/PlayerCat.prefab";

            // 1. Create Root GameObject
            GameObject root = new GameObject("Player");
            root.tag = "Player";

            // 2. SpriteRenderer
            SpriteRenderer sr = root.AddComponent<SpriteRenderer>();
            Sprite catSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Placeholders/RoundedCube.png");
            if (catSprite != null) sr.sprite = catSprite;
            sr.color = new Color(1f, 0.62f, 0.26f, 1f); // Warm Tabby Orange #FF9F43
            sr.sortingOrder = 10; // Above tilemap
            root.transform.localScale = new Vector3(0.85f, 0.85f, 1f);

            // 3. Collider
            BoxCollider2D col = root.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(0.9f, 0.9f);

            // 4. PlayerController & InputHandler
            InputHandler input = root.AddComponent<InputHandler>();
            PlayerController pc = root.AddComponent<PlayerController>();

            // 5. PathHistory & TailManager
            PathHistory ph = root.AddComponent<PathHistory>();
            TailManager tm = root.AddComponent<TailManager>();

            // 6. Visual Juice
            CubeWobble wobble = root.AddComponent<CubeWobble>();
            DropShadow shadow = root.AddComponent<DropShadow>();

            // Save Prefab
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            Object.DestroyImmediate(root);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🐱 [CreatePlayerPrefab] Created {prefabPath} successfully!");
            EditorUtility.DisplayDialog("Stray Swarm", "PlayerCat prefab generated successfully in Assets/_StraySwarm/Prefabs/Characters/PlayerCat.prefab!", "Awesome!");
        }
    }
}
#endif
