#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using StraySwarm.UI;
using StraySwarm.Core;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Master tool to generate and wire all visual HUD elements onto the Scene Canvas.
    /// Preserves user's existing TimerText, PauseButton, and PausePanel without creating duplicates.
    /// Menu: Stray Swarm -> 🎨 Setup Complete Visual HUD (Canvas)
    /// </summary>
    public static class CreateCompleteHUDEditor
    {
        [MenuItem("Stray Swarm/🎨 Setup Complete Visual HUD (Canvas)", false, 15)]
        public static void SetupCompleteHUD()
        {
            // 1. Locate or Create Canvas
            Canvas canvas = Object.FindAnyObjectByType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvas = canvasObj.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1080, 1920);
                scaler.matchWidthOrHeight = 0.5f;
            }

            Transform root = canvas.transform;

            // 2. Ensure EventSystem exists
            if (Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject es = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
            }

            // 3. Setup Top HUD Bar Container
            Transform topBar = FindOrCreateUIChild(root, "TopHUDBar");
            RectTransform topBarRect = topBar.GetComponent<RectTransform>();
            topBarRect.anchorMin = new Vector2(0, 1);
            topBarRect.anchorMax = new Vector2(1, 1);
            topBarRect.pivot = new Vector2(0.5f, 1);
            topBarRect.anchoredPosition = new Vector2(0, -20);
            topBarRect.sizeDelta = new Vector2(0, 80);

            // A. Level Title Text (Top Left)
            Transform titleObj = FindOrCreateUIChild(topBar, "LevelTitleText");
            TextMeshProUGUI titleText = EnsureTMP(titleObj, "LEVEL 1-1", 36, FontStyles.Bold, TextAlignmentOptions.Left);
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.5f);
            titleRect.anchorMax = new Vector2(0, 0.5f);
            titleRect.pivot = new Vector2(0, 0.5f);
            titleRect.anchoredPosition = new Vector2(40, 0);
            titleRect.sizeDelta = new Vector2(260, 60);

            // B. Coin Text (Top Right)
            Transform coinObj = FindOrCreateUIChild(topBar, "CoinText");
            TextMeshProUGUI coinText = EnsureTMP(coinObj, "🪙 0", 36, FontStyles.Bold, TextAlignmentOptions.Right);
            coinText.color = new Color(0.95f, 0.75f, 0.1f);
            RectTransform coinRect = coinObj.GetComponent<RectTransform>();
            coinRect.anchorMin = new Vector2(1, 0.5f);
            coinRect.anchorMax = new Vector2(1, 0.5f);
            coinRect.pivot = new Vector2(1, 0.5f);
            coinRect.anchoredPosition = new Vector2(-40, 0);
            coinRect.sizeDelta = new Vector2(180, 60);

            // 4. Setup Status Sub-Bar (Below Top Bar)
            Transform statusSubBar = FindOrCreateUIChild(root, "StatusSubBar");
            RectTransform subRect = statusSubBar.GetComponent<RectTransform>();
            subRect.anchorMin = new Vector2(0, 1);
            subRect.anchorMax = new Vector2(1, 1);
            subRect.pivot = new Vector2(0.5f, 1);
            subRect.anchoredPosition = new Vector2(0, -100);
            subRect.sizeDelta = new Vector2(0, 60);

            // Quota Text (Left Sub-Bar)
            Transform quotaObj = FindOrCreateUIChild(statusSubBar, "QuotaText");
            TextMeshProUGUI quotaText = EnsureTMP(quotaObj, "🐾 0 / 10", 34, FontStyles.Bold, TextAlignmentOptions.Left);
            quotaText.color = new Color(0.2f, 0.6f, 0.2f);
            RectTransform quotaRect = quotaObj.GetComponent<RectTransform>();
            quotaRect.anchorMin = new Vector2(0, 0.5f);
            quotaRect.anchorMax = new Vector2(0, 0.5f);
            quotaRect.pivot = new Vector2(0, 0.5f);
            quotaRect.anchoredPosition = new Vector2(40, 0);
            quotaRect.sizeDelta = new Vector2(320, 50);

            // 5. Clean up any accidental duplicate TimerText or PauseButton inside TopHUDBar
            Transform dupTimer = topBar.Find("TimerText");
            if (dupTimer != null) Object.DestroyImmediate(dupTimer.gameObject);

            Transform dupPause = topBar.Find("PauseButton");
            if (dupPause != null) Object.DestroyImmediate(dupPause.gameObject);

            // 6. Auto-Wire Scene References
            AutoWireSceneReferences.AutoWireAll();

            EditorUtility.SetDirty(canvas.gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

            Debug.Log("🎉 [CreateCompleteHUDEditor] Visual HUD setup complete!");
            EditorUtility.DisplayDialog("Stray Swarm HUD", "Successfully synced all HUD elements:\n\n• Level Title (Top Left)\n• Coin Counter (Top Right)\n• Quota Progress Counter (Sub-Bar)\n\nYour existing Timer and Pause Button have been kept clean and untouched!", "Awesome!");
        }

        private static Transform FindOrCreateUIChild(Transform parent, string name)
        {
            Transform child = parent.Find(name);
            if (child != null && !(child is RectTransform))
            {
                Object.DestroyImmediate(child.gameObject);
                child = null;
            }

            if (child == null)
            {
                GameObject obj = new GameObject(name, typeof(RectTransform));
                obj.transform.SetParent(parent, false);
                child = obj.transform;
            }
            return child;
        }

        private static TextMeshProUGUI EnsureTMP(Transform target, string text, float fontSize, FontStyles style, TextAlignmentOptions align)
        {
            TextMeshProUGUI tmp = target.GetComponent<TextMeshProUGUI>();
            if (tmp == null) tmp = target.gameObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.fontStyle = style;
            tmp.alignment = align;
            tmp.color = Color.white;
            return tmp;
        }
    }
}
#endif
