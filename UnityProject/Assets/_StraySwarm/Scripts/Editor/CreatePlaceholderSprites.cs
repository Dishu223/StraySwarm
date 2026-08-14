#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Generates high-res 512x512 procedural Rounded Cube and Arrow sprites for animal and obstacle prefabs.
    /// Menu: Stray Swarm -> 🎨 Generate High-Res Cube Placeholder Sprites
    /// </summary>
    public static class CreatePlaceholderSprites
    {
        [MenuItem("Stray Swarm/🎨 Generate High-Res Cube Placeholder Sprites")]
        public static void GenerateSprites()
        {
            string dir = "Assets/_StraySwarm/Art/Placeholders";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string cubePath = $"{dir}/RoundedCube.png";
            string arrowPath = $"{dir}/ArrowIcon.png";

            // 1. Create 512x512 Rounded Cube Texture
            int size = 512;
            Texture2D cubeTex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            float radius = size * 0.28f; // Corner roundness
            float inner = (size / 2f) - radius;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = Mathf.Max(0, Mathf.Abs(x - size / 2f) - inner);
                    float dy = Mathf.Max(0, Mathf.Abs(y - size / 2f) - inner);
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist < radius - 1.5f)
                    {
                        cubeTex.SetPixel(x, y, Color.white);
                    }
                    else if (dist < radius)
                    {
                        float alpha = 1f - (dist - (radius - 1.5f)) / 1.5f;
                        cubeTex.SetPixel(x, y, new Color(1f, 1f, 1f, Mathf.Clamp01(alpha)));
                    }
                    else
                    {
                        cubeTex.SetPixel(x, y, Color.clear);
                    }
                }
            }
            cubeTex.Apply();
            File.WriteAllBytes(cubePath, cubeTex.EncodeToPNG());

            // 2. Create 512x512 Arrow Texture
            Texture2D arrowTex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    arrowTex.SetPixel(x, y, Color.clear);
                }
            }

            // Draw bold white chevron arrow pointing right (X > 0)
            int midY = size / 2;
            int startX = size / 4;
            int tipX = size * 3 / 4;
            int thickness = size / 8;

            for (int x = startX; x <= tipX; x++)
            {
                float t = (float)(x - startX) / (tipX - startX);
                int halfSpread = (int)(t * (size / 3f));

                for (int ty = midY - halfSpread - thickness; ty <= midY + halfSpread + thickness; ty++)
                {
                    if (ty >= 0 && ty < size)
                    {
                        float distToEdge = Mathf.Min(
                            Mathf.Abs(ty - (midY - halfSpread)),
                            Mathf.Abs(ty - (midY + halfSpread))
                        );
                        if (distToEdge <= thickness)
                        {
                            arrowTex.SetPixel(x, ty, Color.white);
                        }
                    }
                }
            }
            arrowTex.Apply();
            File.WriteAllBytes(arrowPath, arrowTex.EncodeToPNG());

            AssetDatabase.Refresh();

            // Configure texture import settings (Sprite 2D, PPU = 512, No Compression)
            ConfigureSpriteImporter(cubePath, 512);
            ConfigureSpriteImporter(arrowPath, 512);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("🎉 [CreatePlaceholderSprites] High-res 512x512 RoundedCube.png and ArrowIcon.png created successfully!");
            EditorUtility.DisplayDialog("Stray Swarm", "High-res 512x512 RoundedCube.png and ArrowIcon.png created in Assets/_StraySwarm/Art/Placeholders/!", "Great!");
        }

        private static void ConfigureSpriteImporter(string path, int ppu)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = ppu;
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }
        }
    }
}
#endif
