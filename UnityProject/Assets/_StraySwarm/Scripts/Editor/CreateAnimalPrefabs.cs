#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using StraySwarm.Data;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Custom Unity Editor tool to generate all 6 animal prefabs with 1 click!
    /// Menu: Stray Swarm -> 🐾 Generate 6 Animal Prefabs
    /// </summary>
    public static class CreateAnimalPrefabs
    {
        [MenuItem("Stray Swarm/🐾 Generate 6 Animal Prefabs")]
        public static void GeneratePrefabs()
        {
            string prefabDir = "Assets/_StraySwarm/Prefabs/Animals";
            if (!Directory.Exists(prefabDir))
            {
                Directory.CreateDirectory(prefabDir);
            }

            string dataDir = "Assets/_StraySwarm/Data/Animals";
            string[] animalNames = { "BluePuppy", "PinkKitten", "YellowPigeon", "GreenFrog", "OrangeHamster", "PurpleBunny" };

            // Find default high-res cube sprite
            Sprite defaultSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Placeholders/RoundedCube.png");
            if (defaultSprite == null) defaultSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            Material spriteMat = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");

            int createdCount = 0;

            foreach (var name in animalNames)
            {
                string dataPath = $"{dataDir}/{name}.asset";
                AnimalData data = AssetDatabase.LoadAssetAtPath<AnimalData>(dataPath);

                string prefabPath = $"{prefabDir}/Animal_{name}.prefab";

                // Create root GameObject
                GameObject go = new GameObject($"Animal_{name}");

                // Add components
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                if (data != null && data.WorldSprite != null) sr.sprite = data.WorldSprite;
                else sr.sprite = defaultSprite;

                if (data != null) sr.color = data.PrimaryColor;
                if (spriteMat != null) sr.material = spriteMat;
                sr.sortingOrder = 5;

                // Scale (0.75 on x,y to fit full grid tile)
                go.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

                // Circle Collider 2D
                CircleCollider2D col = go.AddComponent<CircleCollider2D>();
                col.isTrigger = true;
                col.radius = 0.5f;

                // Cube Wobble
                CubeWobble wobble = go.AddComponent<CubeWobble>();

                // Basket Bounce
                BasketBounce basket = go.AddComponent<BasketBounce>();
                basket.enabled = false; // Enabled upon collection

                // Drop Shadow
                DropShadow shadow = go.AddComponent<DropShadow>();

                // Follower Behavior
                FollowerBehavior follower = go.AddComponent<FollowerBehavior>();
                if (data != null)
                {
                    follower.SetAnimalData(data);
                }

                // Save as Prefab
                PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
                GameObject.DestroyImmediate(go);
                createdCount++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🎉 [CreateAnimalPrefabs] Successfully created {createdCount} Animal Prefabs in {prefabDir}!");
            EditorUtility.DisplayDialog("Stray Swarm", $"Successfully generated {createdCount} Animal Prefabs!\n\nCheck Assets/_StraySwarm/Prefabs/Animals/.", "Awesome!");
        }
    }
}
#endif
