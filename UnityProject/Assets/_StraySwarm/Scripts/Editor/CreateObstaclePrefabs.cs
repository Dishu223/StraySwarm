#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using TMPro;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Custom Unity Editor tool to generate ready-to-use Obstacle Prefabs (Arrows & Walls) with 1 click!
    /// Menu: Stray Swarm -> 🧱 Generate Obstacle Prefabs (Arrows & Walls)
    /// </summary>
    public static class CreateObstaclePrefabs
    {
        [MenuItem("Stray Swarm/🧱 Generate Obstacle Prefabs (Arrows & Walls)")]
        public static void GenerateObstacles()
        {
            string prefabDir = "Assets/_StraySwarm/Prefabs/Obstacles";
            if (!Directory.Exists(prefabDir))
            {
                Directory.CreateDirectory(prefabDir);
            }

            Sprite defaultSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            Material spriteMat = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
            TMP_FontAsset font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/LilitaOne-Regular SDF.asset");

            // 1. Generate One-Way Arrows (Up, Right, Down, Left)
            ArrowDirection[] directions = { ArrowDirection.Up, ArrowDirection.Right, ArrowDirection.Down, ArrowDirection.Left };
            foreach (var dir in directions)
            {
                string path = $"{prefabDir}/OneWayArrow_{dir}.prefab";
                GameObject go = new GameObject($"OneWayArrow_{dir}");

                // Sprite Background (Golden Yellow)
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = defaultSprite;
                sr.color = new Color(1f, 0.8f, 0f, 0.9f); // Golden yellow
                if (spriteMat != null) sr.material = spriteMat;
                sr.sortingOrder = 2;
                go.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

                // Arrow Script & Trigger
                BoxCollider2D col = go.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = new Vector2(1f, 1f);

                OneWayArrow arrow = go.AddComponent<OneWayArrow>();
                arrow.SetDirection(dir);

                PrefabUtility.SaveAsPrefabAsset(go, path);
                GameObject.DestroyImmediate(go);
            }

            // 2. Generate Numbered Walls (HP: 1, 2, 3)
            int[] hpValues = { 1, 2, 3 };
            foreach (var hp in hpValues)
            {
                string path = $"{prefabDir}/NumberedWall_{hp}.prefab";
                GameObject go = new GameObject($"NumberedWall_{hp}");

                // Wall Base Sprite (Stone Gray)
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = defaultSprite;
                sr.color = new Color(0.45f, 0.5f, 0.58f); // Stone Slate
                if (spriteMat != null) sr.material = spriteMat;
                sr.sortingOrder = 6;
                go.transform.localScale = new Vector3(0.85f, 0.85f, 1f);

                // Solid Box Collider
                BoxCollider2D col = go.AddComponent<BoxCollider2D>();
                col.isTrigger = false;
                col.size = new Vector2(1f, 1f);

                // Number Text Child
                GameObject textObj = new GameObject("NumberText");
                textObj.transform.SetParent(go.transform, false);
                textObj.transform.localPosition = new Vector3(0, 0, -0.1f);

                TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
                tmp.text = hp.ToString();
                tmp.fontSize = 7;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.color = Color.white;
                if (font != null) tmp.font = font;
                tmp.sortingOrder = 7;

                // Numbered Wall Script
                NumberedWall wall = go.AddComponent<NumberedWall>();
                SerializedObject so = new SerializedObject(wall);
                so.FindProperty("_hitPoints").intValue = hp;
                so.FindProperty("_numberText").objectReferenceValue = tmp;
                so.FindProperty("_wallRenderer").objectReferenceValue = sr;
                so.FindProperty("_wallCollider").objectReferenceValue = col;
                so.ApplyModifiedProperties();

                PrefabUtility.SaveAsPrefabAsset(go, path);
                GameObject.DestroyImmediate(go);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🎉 [CreateObstaclePrefabs] Successfully created all Obstacle Prefabs in {prefabDir}!");
            EditorUtility.DisplayDialog("Stray Swarm", "Successfully generated Obstacle Prefabs (OneWayArrows & NumberedWalls)!\n\nCheck Assets/_StraySwarm/Prefabs/Obstacles/.", "Awesome!");
        }
    }
}
#endif
