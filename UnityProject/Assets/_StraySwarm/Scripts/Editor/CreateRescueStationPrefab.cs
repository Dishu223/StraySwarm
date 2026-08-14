#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using StraySwarm.Gameplay;
using StraySwarm.Utils;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Generates a ready-to-use RescueStation prefab for handcrafted level creation.
    /// Menu: Stray Swarm -> 🏠 Create Rescue Station Prefab
    /// </summary>
    public static class CreateRescueStationPrefab
    {
        [MenuItem("Stray Swarm/🏠 Create Rescue Station Prefab")]
        public static void GenerateStationPrefab()
        {
            string envDir = "Assets/_StraySwarm/Prefabs/Environment";
            string prefabsDir = "Assets/_StraySwarm/Prefabs";

            if (!Directory.Exists(envDir)) Directory.CreateDirectory(envDir);

            // 1. Create Root GameObject
            GameObject root = new GameObject("RescueStation");

            // 2. Add SpriteRenderer
            SpriteRenderer sr = root.AddComponent<SpriteRenderer>();
            Sprite stationSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Environment/station_board.jpeg");
            if (stationSprite == null) stationSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Placeholders/RoundedCube.png");
            if (stationSprite != null) sr.sprite = stationSprite;
            sr.sortingOrder = 3;
            sr.color = Color.white;
            root.transform.localScale = new Vector3(1f, 1f, 1f);

            // 3. Add BoxCollider2D (Delivery Zone Trigger)
            BoxCollider2D col = root.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(1.2f, 1.2f);

            // 4. Add DeliveryCrate component
            DeliveryCrate crate = root.AddComponent<DeliveryCrate>();
            crate.Capacity = 3;
            crate.TargetAnimalType = Data.AnimalType.Puppy;

            // 5. Add RescueStation component
            RescueStation station = root.AddComponent<RescueStation>();

            // 6. Add Child VanParkingSpot
            GameObject parkingSpot = new GameObject("VanParkingSpot");
            parkingSpot.transform.SetParent(root.transform, false);
            station.VanParkingSpot = parkingSpot.transform;

            // 7. Add StationPulse & DropShadow
            root.AddComponent<StationPulse>();
            root.AddComponent<DropShadow>();

            // 8. Add GridSnap for clean path center snapping
            root.AddComponent<GridSnap>();

            // Save Prefab cleanly in Environment folder
            string envPath = $"{envDir}/RescueStation.prefab";

            PrefabUtility.SaveAsPrefabAsset(root, envPath);
            Object.DestroyImmediate(root);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🏠 [CreateRescueStationPrefab] Successfully created {envPath}!");
            EditorUtility.DisplayDialog("Rescue Station Prefab Created!", 
                $"RescueStation.prefab is now ready in:\n\n" +
                $"Assets/_StraySwarm/Prefabs/Environment/RescueStation.prefab\n\n" +
                $"You can now drag it directly onto your level drop-off point!", "Awesome!");
        }
    }
}
#endif
