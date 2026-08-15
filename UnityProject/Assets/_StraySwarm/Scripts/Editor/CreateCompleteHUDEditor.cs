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
    /// Master tool to automatically generate and wire all visual HUD elements onto the Scene Canvas.
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
            topBarRect.sizeDelta = new Vector2(0, 100);

            // A. Level Title Text (Top Left)
            Transform titleObj = FindOrCreateUIChild(topBar, "LevelTitleText");
            TextMeshProUGUI titleText = EnsureTMP(titleObj, "LEVEL 1-1", 36, FontStyles.Bold, TextAlignmentOptions.Left);
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.5f);
            titleRect.anchorMax = new Vector2(0, 0.5f);
            titleRect.pivot = new Vector2(0, 0.5f);
            titleRect.anchoredPosition = new Vector2(40, 0);
            titleRect.sizeDelta = new Vector2(260, 60);

            // B. Timer Text (Top Center)
            Transform timerObj = FindOrCreateUIChild(topBar, "TimerText");
            TextMeshProUGUI timerText = EnsureTMP(timerObj, "01:00", 48, FontStyles.Bold, TextAlignmentOptions.Center);
            timerText.color = new Color(0.6f, 0.2f, 0.07f);
            RectTransform timerRect = timerObj.GetComponent<RectTransform>();
            timerRect.anchorMin = new Vector2(0.5f, 0.5f);
            timerRect.anchorMax = new Vector2(0.5f, 0.5f);
            timerRect.pivot = new Vector2(0.5f, 0.5f);
            timerRect.anchoredPosition = new Vector2(0, 0);
            timerRect.sizeDelta = new Vector2(220, 70);

            // C. Coin Text (Top Right)
            Transform coinObj = FindOrCreateUIChild(topBar, "CoinText");
            TextMeshProUGUI coinText = EnsureTMP(coinObj, "🪙 0", 36, FontStyles.Bold, TextAlignmentOptions.Right);
            coinText.color = new Color(0.95f, 0.75f, 0.1f);
            RectTransform coinRect = coinObj.GetComponent<RectTransform>();
            coinRect.anchorMin = new Vector2(1, 0.5f);
            coinRect.anchorMax = new Vector2(1, 0.5f);
            coinRect.pivot = new Vector2(1, 0.5f);
            coinRect.anchoredPosition = new Vector2(-120, 0);
            coinRect.sizeDelta = new Vector2(180, 60);

            // D. Pause Button (Top Far Right)
            Transform pauseBtn = FindOrCreateUIChild(topBar, "PauseButton");
            Button pButton = EnsureButton(pauseBtn);
            RectTransform pauseRect = pauseBtn.GetComponent<RectTransform>();
            pauseRect.anchorMin = new Vector2(1, 0.5f);
            pauseRect.anchorMax = new Vector2(1, 0.5f);
            pauseRect.pivot = new Vector2(1, 0.5f);
            pauseRect.anchoredPosition = new Vector2(-30, 0);
            pauseRect.sizeDelta = new Vector2(70, 70);

            Transform pLabel = FindOrCreateUIChild(pauseBtn, "Label");
            TextMeshProUGUI pText = EnsureTMP(pLabel, "⏸", 32, FontStyles.Bold, TextAlignmentOptions.Center);
            RectTransform pLabelRect = pLabel.GetComponent<RectTransform>();
            pLabelRect.anchorMin = Vector2.zero;
            pLabelRect.anchorMax = Vector2.one;
            pLabelRect.sizeDelta = Vector2.zero;

            // 4. Setup Status Sub-Bar (Below Top Bar)
            Transform statusSubBar = FindOrCreateUIChild(root, "StatusSubBar");
            RectTransform subRect = statusSubBar.GetComponent<RectTransform>();
            subRect.anchorMin = new Vector2(0, 1);
            subRect.anchorMax = new Vector2(1, 1);
            subRect.pivot = new Vector2(0.5f, 1);
            subRect.anchoredPosition = new Vector2(0, -120);
            subRect.sizeDelta = new Vector2(0, 70);

            // A. Quota Text (Left Sub-Bar)
            Transform quotaObj = FindOrCreateUIChild(statusSubBar, "QuotaText");
            TextMeshProUGUI quotaText = EnsureTMP(quotaObj, "🐾 0 / 10", 34, FontStyles.Bold, TextAlignmentOptions.Left);
            quotaText.color = new Color(0.2f, 0.6f, 0.2f);
            RectTransform quotaRect = quotaObj.GetComponent<RectTransform>();
            quotaRect.anchorMin = new Vector2(0, 0.5f);
            quotaRect.anchorMax = new Vector2(0, 0.5f);
            quotaRect.pivot = new Vector2(0, 0.5f);
            quotaRect.anchoredPosition = new Vector2(40, 0);
            quotaRect.sizeDelta = new Vector2(320, 50);

            // 5. Setup Pause Panel
            Transform pausePanel = FindOrCreateUIChild(root, "PausePanel");
            Image pauseBg = pausePanel.GetComponent<Image>();
            if (pauseBg == null) pauseBg = pausePanel.gameObject.AddComponent<Image>();
            pauseBg.color = new Color(0f, 0f, 0f, 0.75f);
            RectTransform pausePanelRect = pausePanel.GetComponent<RectTransform>();
            pausePanelRect.anchorMin = Vector2.zero;
            pausePanelRect.anchorMax = Vector2.one;
            pausePanelRect.sizeDelta = Vector2.zero;

            Transform pauseTitle = FindOrCreateUIChild(pausePanel, "PauseTitle");
            EnsureTMP(pauseTitle, "GAME PAUSED", 54, FontStyles.Bold, TextAlignmentOptions.Center);
            RectTransform pTitleRect = pauseTitle.GetComponent<RectTransform>();
            pTitleRect.anchorMin = new Vector2(0.5f, 0.7f);
            pTitleRect.anchorMax = new Vector2(0.5f, 0.7f);
            pTitleRect.sizeDelta = new Vector2(500, 100);

            // Resume Button
            Transform resumeBtn = FindOrCreateUIChild(pausePanel, "ResumeButton");
            EnsureButton(resumeBtn);
            RectTransform resumeRect = resumeBtn.GetComponent<RectTransform>();
            resumeRect.anchorMin = new Vector2(0.5f, 0.52f);
            resumeRect.anchorMax = new Vector2(0.5f, 0.52f);
            resumeRect.sizeDelta = new Vector2(320, 80);
            Transform resumeLabel = FindOrCreateUIChild(resumeBtn, "Label");
            EnsureTMP(resumeLabel, "▶ RESUME", 34, FontStyles.Bold, TextAlignmentOptions.Center);

            // Restart Button in Pause Panel
            Transform restartBtn = FindOrCreateUIChild(pausePanel, "PauseRestartButton");
            EnsureButton(restartBtn);
            RectTransform pRestartRect = restartBtn.GetComponent<RectTransform>();
            pRestartRect.anchorMin = new Vector2(0.5f, 0.42f);
            pRestartRect.anchorMax = new Vector2(0.5f, 0.42f);
            pRestartRect.sizeDelta = new Vector2(320, 80);
            Transform pRestartLabel = FindOrCreateUIChild(restartBtn, "Label");
            EnsureTMP(pRestartLabel, "🔄 RESTART", 34, FontStyles.Bold, TextAlignmentOptions.Center);

            // Mute Button in Pause Panel
            Transform muteBtn = FindOrCreateUIChild(pausePanel, "MuteButton");
            EnsureButton(muteBtn);
            RectTransform muteRect = muteBtn.GetComponent<RectTransform>();
            muteRect.anchorMin = new Vector2(0.5f, 0.32f);
            muteRect.anchorMax = new Vector2(0.5f, 0.32f);
            muteRect.sizeDelta = new Vector2(320, 80);
            Transform muteLabel = FindOrCreateUIChild(muteBtn, "Label");
            EnsureTMP(muteLabel, "🔊 MUTE AUDIO", 34, FontStyles.Bold, TextAlignmentOptions.Center);

            pausePanel.gameObject.SetActive(false); // Hide pause panel by default

            // 6. Connect Button Listeners
            UIManager uiManager = Object.FindAnyObjectByType<UIManager>();
            if (uiManager != null)
            {
                pButton.onClick.RemoveAllListeners();
                UnityEditor.Events.UnityEventTools.AddPersistentListener(pButton.onClick, uiManager.TogglePause);

                Button rBtn = resumeBtn.GetComponent<Button>();
                rBtn.onClick.RemoveAllListeners();
                UnityEditor.Events.UnityEventTools.AddPersistentListener(rBtn.onClick, uiManager.OnResumeButtonClicked);

                Button prBtn = restartBtn.GetComponent<Button>();
                prBtn.onClick.RemoveAllListeners();
                UnityEditor.Events.UnityEventTools.AddPersistentListener(prBtn.onClick, uiManager.OnRestartButtonClicked);

                Button mBtn = muteBtn.GetComponent<Button>();
                mBtn.onClick.RemoveAllListeners();
                UnityEditor.Events.UnityEventTools.AddPersistentListener(mBtn.onClick, uiManager.OnMuteToggleClicked);
            }

            // 7. Auto-Wire Scene References
            AutoWireSceneReferences.AutoWireAll();

            EditorUtility.SetDirty(canvas.gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

            Debug.Log("🎉 [CreateCompleteHUDEditor] Visual HUD setup complete!");
            EditorUtility.DisplayDialog("Stray Swarm HUD", "Successfully created and wired the visual HUD elements:\n\n• Level Title (Top Left)\n• Timer (Top Center)\n• Coin Counter (Top Right)\n• Pause Button & Pause Menu\n• Quota Progress Counter\n\nTarget animals are now shown in the floating cloud speech bubble above each Rescue Van!", "Awesome!");
        }

        private static Transform FindOrCreateUIChild(Transform parent, string name)
        {
            Transform child = parent.Find(name);
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

        private static Button EnsureButton(Transform target)
        {
            Image img = target.GetComponent<Image>();
            if (img == null) img = target.gameObject.AddComponent<Image>();
            img.color = new Color(0.2f, 0.2f, 0.2f, 0.85f);

            Button btn = target.GetComponent<Button>();
            if (btn == null) btn = target.gameObject.AddComponent<Button>();
            return btn;
        }
    }
}
#endif
