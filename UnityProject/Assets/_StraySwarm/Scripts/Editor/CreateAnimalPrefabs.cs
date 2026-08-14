#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using StraySwarm.Data;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Custom Unity Editor tool to generate all 6 clean animal prefabs with 1 click!
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
            string[] animalNames = { "Puppy", "Kitten", "Frog", "Mouse", "Pigeon", "Bunny" };

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

                sr.color = Color.white; // Keep pure custom artwork colors!
                if (spriteMat != null) sr.material = spriteMat;
                sr.sortingOrder = 5;

                // Scale (0.75 on x,y to fit full grid tile)
                go.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

                // Circle Collider 2D
                CircleCollider2D col = go.AddComponent<CircleCollider2D>();
                col.isTrigger = true;
                col.radius = 0.5f;

                // Follower Behavior
                FollowerBehavior follower = go.AddComponent<FollowerBehavior>();
                if (data != null)
                {
                    follower.SetAnimalData(data);
                }

                // Cube Wobble
                CubeWobble wobble = go.AddComponent<CubeWobble>();
                wobble.EnableIdleBob = true;
                wobble.IdleSpeed = 3f;
                wobble.IdleScaleAmount = 0.04f;

                // Basket Bounce
                BasketBounce bounce = go.AddComponent<BasketBounce>();
                bounce.enabled = false; // Enabled only when collected!

                // Drop Shadow
                DropShadow shadow = go.AddComponent<DropShadow>();
                shadow.Offset = new Vector2(0f, -0.3f);
                shadow.ShadowScale = new Vector2(0.8f, 0.3f);
                shadow.Alpha = 0.25f;

                // Save Prefab
                PrefabUtility.SaveAsPrefabAsset(go, prefabPath);

                // Also save legacy compatibility alias if needed
                if (name == "Puppy") PrefabUtility.SaveAsPrefabAsset(go, $"{prefabDir}/Animal_BluePuppy.prefab");
                if (name == "Kitten") PrefabUtility.SaveAsPrefabAsset(go, $"{prefabDir}/Animal_PinkKitten.prefab");
                if (name == "Frog") PrefabUtility.SaveAsPrefabAsset(go, $"{prefabDir}/Animal_GreenFrog.prefab");
                if (name == "Mouse") PrefabUtility.SaveAsPrefabAsset(go, $"{prefabDir}/Animal_OrangeHamster.prefab");
                if (name == "Pigeon") PrefabUtility.SaveAsPrefabAsset(go, $"{prefabDir}/Animal_YellowPigeon.prefab");
                if (name == "Bunny") PrefabUtility.SaveAsPrefabAsset(go, $"{prefabDir}/Animal_PurpleBunny.prefab");

                Object.DestroyImmediate(go);
                createdCount++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🐾 [CreateAnimalPrefabs] Successfully generated {createdCount} animal prefabs in {prefabDir}!");
            EditorUtility.DisplayDialog("Stray Swarm", $"Successfully generated all 6 animal prefabs (Puppy, Kitten, Frog, Mouse, Pigeon, Bunny) at 0.75 tile scale!", "Awesome!");
        }
    }
}
#endif
