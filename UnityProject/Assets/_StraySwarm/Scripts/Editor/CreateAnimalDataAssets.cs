#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using StraySwarm.Data;
using System.IO;

namespace StraySwarm.Editor
{
    public static class CreateAnimalDataAssets
    {
        [MenuItem("Stray Swarm/Generate 6 Animal Data Assets")]
        public static void GenerateAssets()
        {
            string folderPath = "Assets/_StraySwarm/Data/Animals";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            CreateOrUpdateAsset(AnimalType.Puppy, "Puppy", new Color(0.36f, 0.72f, 1f), folderPath);
            CreateOrUpdateAsset(AnimalType.Kitten, "Kitten", new Color(1f, 0.49f, 0.7f), folderPath);
            CreateOrUpdateAsset(AnimalType.Pigeon, "Pigeon", new Color(1f, 0.8f, 0.01f), folderPath);
            CreateOrUpdateAsset(AnimalType.Frog, "Frog", new Color(0.49f, 0.85f, 0.62f), folderPath);
            CreateOrUpdateAsset(AnimalType.Mouse, "Mouse", new Color(1f, 0.62f, 0.26f), folderPath);
            CreateOrUpdateAsset(AnimalType.Bunny, "Bunny", new Color(0.64f, 0.61f, 1f), folderPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("🎉 Generated all 6 AnimalData ScriptableObject assets in Assets/_StraySwarm/Data/Animals/");
        }

        private static void CreateOrUpdateAsset(AnimalType type, string displayName, Color color, string folderPath)
        {
            string path = $"{folderPath}/{type}.asset";
            AnimalData data = AssetDatabase.LoadAssetAtPath<AnimalData>(path);

            if (data == null)
            {
                data = ScriptableObject.CreateInstance<AnimalData>();
                AssetDatabase.CreateAsset(data, path);
            }

            data.Type = type;
            data.DisplayName = displayName;
            data.PrimaryColor = color;
            EditorUtility.SetDirty(data);
        }
    }
}
#endif
