#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using StraySwarm.Core;
using StraySwarm.Gameplay;
using StraySwarm.Data;
using StraySwarm.Events;
using StraySwarm.Audio;
using StraySwarm.UI;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Custom Unity Editor tool to automatically scan the active scene and wire up
    /// EVERY SINGLE Inspector field on ALL GameObjects with 1 click!
    /// Menu: Stray Swarm -> 🔌 Auto-Wire All Scene Inspector Slots
    /// </summary>
    public static class AutoWireSceneReferences
    {
        [MenuItem("Stray Swarm/🔌 Auto-Wire All Scene Inspector Slots")]
        public static void AutoWireAll()
        {
            int wiredCount = 0;

            // 1. Wire Player GameObject
            GameObject playerGo = GameObject.Find("Player");
            if (playerGo != null)
            {
                TailManager tail = playerGo.GetComponent<TailManager>();
                PathHistory path = playerGo.GetComponent<PathHistory>();
                if (tail != null && path != null)
                {
                    SerializedObject so = new SerializedObject(tail);
                    SerializedProperty prop = so.FindProperty("_pathHistory");
                    if (prop != null)
                    {
                        prop.objectReferenceValue = path;
                        so.ApplyModifiedProperties();
                        wiredCount++;
                    }
                }
            }

            // 2. Wire RescueStation GameObject
            GameObject stationGo = GameObject.Find("RescueStation");
            if (stationGo != null)
            {
                RescueStation station = stationGo.GetComponent<RescueStation>();
                DeliveryCrate crate = stationGo.GetComponent<DeliveryCrate>();
                VanQueue queue = stationGo.GetComponent<VanQueue>();
                TailManager tail = playerGo != null ? playerGo.GetComponent<TailManager>() : null;
                Transform spot = stationGo.transform.Find("VanParkingSpot");

                if (station != null)
                {
                    SerializedObject so = new SerializedObject(station);
                    if (spot != null) so.FindProperty("VanParkingSpot").objectReferenceValue = spot;
                    if (crate != null) so.FindProperty("_attachedCrate").objectReferenceValue = crate;
                    if (queue != null) so.FindProperty("_vanQueue").objectReferenceValue = queue;
                    if (tail != null) so.FindProperty("_tailManager").objectReferenceValue = tail;
                    so.ApplyModifiedProperties();
                    wiredCount++;
                }

                if (crate != null)
                {
                    SerializedObject so = new SerializedObject(crate);
                    SpriteRenderer sr = stationGo.GetComponent<SpriteRenderer>();
                    if (sr != null) so.FindProperty("_crateRenderer").objectReferenceValue = sr;
                    so.ApplyModifiedProperties();
                    wiredCount++;
                }

                if (queue != null)
                {
                    SerializedObject so = new SerializedObject(queue);
                    if (station != null) so.FindProperty("_station").objectReferenceValue = station;
                    GameObject vanPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_StraySwarm/Prefabs/Core/VanPrefab.prefab");
                    if (vanPrefab != null) so.FindProperty("_vanPrefab").objectReferenceValue = vanPrefab;
                    so.ApplyModifiedProperties();
                    wiredCount++;
                }

                // Ensure StationManager exists
                StationManager sm = Object.FindAnyObjectByType<StationManager>();
                if (sm == null)
                {
                    stationGo.AddComponent<StationManager>();
                    wiredCount++;
                }
            }

            // 3. Wire GameManager GameObject
            GameObject gmGo = GameObject.Find("GameManager");
            if (gmGo != null)
            {
                GameManager gm = gmGo.GetComponent<GameManager>();
                GridManager grid = gmGo.GetComponent<GridManager>();

                if (gm != null)
                {
                    SerializedObject so = new SerializedObject(gm);
                    GameEvent winEvent = AssetDatabase.LoadAssetAtPath<GameEvent>("Assets/_StraySwarm/Data/Events/OnLevelWon.asset");
                    GameEvent loseEvent = AssetDatabase.LoadAssetAtPath<GameEvent>("Assets/_StraySwarm/Data/Events/OnLevelLost.asset");
                    LevelData lvl1 = AssetDatabase.LoadAssetAtPath<LevelData>("Assets/_StraySwarm/Data/Levels/World_01_Desert/Level_01.asset");

                    if (winEvent != null) so.FindProperty("_onLevelWonEvent").objectReferenceValue = winEvent;
                    if (loseEvent != null) so.FindProperty("_onLevelLostEvent").objectReferenceValue = loseEvent;
                    if (lvl1 != null) so.FindProperty("_currentLevel").objectReferenceValue = lvl1;
                    so.ApplyModifiedProperties();
                    wiredCount++;
                }

                if (grid != null)
                {
                    SerializedObject so = new SerializedObject(grid);
                    Tilemap tilemap = Object.FindAnyObjectByType<Tilemap>();
                    if (tilemap != null) so.FindProperty("_floorTilemap").objectReferenceValue = tilemap;
                    so.ApplyModifiedProperties();
                    wiredCount++;
                }
            }

            // 4. Wire Canvas / UIManager
            UIManager ui = Object.FindAnyObjectByType<UIManager>();
            if (ui != null)
            {
                SerializedObject so = new SerializedObject(ui);
                Transform canvasT = ui.transform.root;

                TextMeshProUGUI timerText = canvasT.GetComponentInChildren<TextMeshProUGUI>(true);
                Transform winPanel = FindDeepChild(canvasT, "WinPanel");
                Transform losePanel = FindDeepChild(canvasT, "LosePanel");

                if (timerText != null) so.FindProperty("_timerText").objectReferenceValue = timerText;
                if (winPanel != null) so.FindProperty("_winPanel").objectReferenceValue = winPanel.gameObject;
                if (losePanel != null) so.FindProperty("_losePanel").objectReferenceValue = losePanel.gameObject;

                // Wire Stars
                if (winPanel != null)
                {
                    Transform s1 = FindDeepChild(winPanel, "Star1");
                    Transform s2 = FindDeepChild(winPanel, "Star2");
                    Transform s3 = FindDeepChild(winPanel, "Star3");

                    SerializedProperty starsProp = so.FindProperty("_stars");
                    if (starsProp != null)
                    {
                        starsProp.arraySize = 3;
                        if (s1 != null) starsProp.GetArrayElementAtIndex(0).objectReferenceValue = s1.gameObject;
                        if (s2 != null) starsProp.GetArrayElementAtIndex(1).objectReferenceValue = s2.gameObject;
                        if (s3 != null) starsProp.GetArrayElementAtIndex(2).objectReferenceValue = s3.gameObject;
                    }
                }

                GameManager gm = Object.FindAnyObjectByType<GameManager>();
                if (gm != null) so.FindProperty("_gameManager").objectReferenceValue = gm;

                so.ApplyModifiedProperties();
                wiredCount++;
            }

            // 5. Wire PauseMenuUI
            PauseMenuUI pauseUI = Object.FindAnyObjectByType<PauseMenuUI>();
            if (pauseUI != null)
            {
                SerializedObject so = new SerializedObject(pauseUI);
                Transform canvasT = pauseUI.transform.root;
                Transform pausePanel = FindDeepChild(canvasT, "PausePanel");
                if (pausePanel != null) so.FindProperty("_pausePanel").objectReferenceValue = pausePanel.gameObject;
                so.ApplyModifiedProperties();
                wiredCount++;
            }

            // 6. Wire AudioManager
            AudioManager audio = Object.FindAnyObjectByType<AudioManager>();
            if (audio != null)
            {
                SerializedObject so = new SerializedObject(audio);
                string audioDir = "Assets/_StraySwarm/Audio";

                AudioClip bubble1 = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/bubble1.mp3");
                AudioClip chime1 = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/chime1.mp3");
                AudioClip winSfx = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/win.mp3");
                AudioClip boop2 = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/boop2.mp3");
                AudioClip allstars = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/allstars.mp3");
                AudioClip victory = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/victory.mp3");
                AudioClip boop1 = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/boop1.mp3");
                AudioClip bgm = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/tmkocbgmintense.mp3");
                if (bgm == null) bgm = AssetDatabase.LoadAssetAtPath<AudioClip>($"{audioDir}/TMKOC - RELAXING BGM.mp3");

                if (bubble1 != null) so.FindProperty("CollectSound").objectReferenceValue = bubble1;
                if (chime1 != null) so.FindProperty("DeliverSound").objectReferenceValue = chime1;
                if (winSfx != null) so.FindProperty("CrateFullSound").objectReferenceValue = winSfx;
                if (boop2 != null) so.FindProperty("WallBreakSound").objectReferenceValue = boop2;
                if (chime1 != null) so.FindProperty("Star1Sound").objectReferenceValue = chime1;
                if (chime1 != null) so.FindProperty("Star2Sound").objectReferenceValue = chime1;
                if (allstars != null) so.FindProperty("Star3Sound").objectReferenceValue = allstars;
                if (victory != null) so.FindProperty("WinSound").objectReferenceValue = victory;
                if (boop2 != null) so.FindProperty("LoseSound").objectReferenceValue = boop2;
                if (boop1 != null) so.FindProperty("ButtonClickSound").objectReferenceValue = boop1;
                if (bgm != null)
                {
                    so.FindProperty("MenuBGM").objectReferenceValue = bgm;
                    so.FindProperty("GameplayBGM").objectReferenceValue = bgm;
                }

                so.ApplyModifiedProperties();
                wiredCount++;
            }

            // 7. Wire LevelManager
            LevelManager lm = Object.FindAnyObjectByType<LevelManager>();
            if (lm != null)
            {
                SerializedObject so = new SerializedObject(lm);
                WorldData w1 = AssetDatabase.LoadAssetAtPath<WorldData>("Assets/_StraySwarm/Data/Worlds/World_01_Desert.asset");
                WorldData w2 = AssetDatabase.LoadAssetAtPath<WorldData>("Assets/_StraySwarm/Data/Worlds/World_02_Forest.asset");

                SerializedProperty worldsProp = so.FindProperty("_worlds");
                if (worldsProp != null)
                {
                    worldsProp.arraySize = 2;
                    if (w1 != null) worldsProp.GetArrayElementAtIndex(0).objectReferenceValue = w1;
                    if (w2 != null) worldsProp.GetArrayElementAtIndex(1).objectReferenceValue = w2;
                }
                so.ApplyModifiedProperties();
                wiredCount++;
            }

            // 8. Wire JuiceManager
            JuiceManager juice = Object.FindAnyObjectByType<JuiceManager>();
            if (juice != null)
            {
                SerializedObject so = new SerializedObject(juice);
                ParticleSystem confetti = AssetDatabase.LoadAssetAtPath<ParticleSystem>("Assets/_StraySwarm/Prefabs/WinConfettiPrefab.prefab");
                if (confetti == null) confetti = AssetDatabase.LoadAssetAtPath<ParticleSystem>("Assets/_StraySwarm/Prefabs/Juice/WinConfettiPrefab.prefab");
                if (confetti != null) so.FindProperty("_winConfettiPrefab").objectReferenceValue = confetti;
                so.ApplyModifiedProperties();
                wiredCount++;
            }

            // 9. Wire LevelBuilder
            LevelBuilder lb = Object.FindAnyObjectByType<LevelBuilder>();
            if (lb == null && gmGo != null)
            {
                lb = gmGo.AddComponent<LevelBuilder>();
            }
            if (lb != null)
            {
                SerializedObject so = new SerializedObject(lb);
                string animalDir = "Assets/_StraySwarm/Prefabs/Animals";
                string obstacleDir = "Assets/_StraySwarm/Prefabs/Obstacles";

                GameObject puppy = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_BluePuppy.prefab");
                GameObject kitten = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_PinkKitten.prefab");
                GameObject frog = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_GreenFrog.prefab");
                GameObject mouse = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_OrangeHamster.prefab");
                GameObject pigeon = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_YellowPigeon.prefab");
                GameObject bunny = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_PurpleBunny.prefab");

                if (puppy != null) so.FindProperty("_puppyPrefab").objectReferenceValue = puppy;
                if (kitten != null) so.FindProperty("_kittenPrefab").objectReferenceValue = kitten;
                if (frog != null) so.FindProperty("_frogPrefab").objectReferenceValue = frog;
                if (mouse != null) so.FindProperty("_mousePrefab").objectReferenceValue = mouse;
                if (pigeon != null) so.FindProperty("_pigeonPrefab").objectReferenceValue = pigeon;
                if (bunny != null) so.FindProperty("_bunnyPrefab").objectReferenceValue = bunny;

                GameObject arrowUp = AssetDatabase.LoadAssetAtPath<GameObject>($"{obstacleDir}/OneWayArrow_Up.prefab");
                GameObject arrowRight = AssetDatabase.LoadAssetAtPath<GameObject>($"{obstacleDir}/OneWayArrow_Right.prefab");
                GameObject arrowDown = AssetDatabase.LoadAssetAtPath<GameObject>($"{obstacleDir}/OneWayArrow_Down.prefab");
                GameObject arrowLeft = AssetDatabase.LoadAssetAtPath<GameObject>($"{obstacleDir}/OneWayArrow_Left.prefab");

                if (arrowUp != null) so.FindProperty("_arrowUpPrefab").objectReferenceValue = arrowUp;
                if (arrowRight != null) so.FindProperty("_arrowRightPrefab").objectReferenceValue = arrowRight;
                if (arrowDown != null) so.FindProperty("_arrowDownPrefab").objectReferenceValue = arrowDown;
                if (arrowLeft != null) so.FindProperty("_arrowLeftPrefab").objectReferenceValue = arrowLeft;

                GameObject wall1 = AssetDatabase.LoadAssetAtPath<GameObject>($"{obstacleDir}/NumberedWall_1.prefab");
                GameObject wall2 = AssetDatabase.LoadAssetAtPath<GameObject>($"{obstacleDir}/NumberedWall_2.prefab");
                GameObject wall3 = AssetDatabase.LoadAssetAtPath<GameObject>($"{obstacleDir}/NumberedWall_3.prefab");

                if (wall1 != null) so.FindProperty("_wall1Prefab").objectReferenceValue = wall1;
                if (wall2 != null) so.FindProperty("_wall2Prefab").objectReferenceValue = wall2;
                so.ApplyModifiedProperties();
                wiredCount++;
            }

            // 10. Wire WaveSpawner
            WaveSpawner ws = Object.FindAnyObjectByType<WaveSpawner>();
            if (ws == null && gmGo != null)
            {
                ws = gmGo.AddComponent<WaveSpawner>();
            }
            if (ws != null)
            {
                SerializedObject so = new SerializedObject(ws);
                string animalDir = "Assets/_StraySwarm/Prefabs/Animals";

                GameObject puppy = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_Puppy.prefab");
                if (puppy == null) puppy = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_BluePuppy.prefab");

                GameObject kitten = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_Kitten.prefab");
                if (kitten == null) kitten = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_PinkKitten.prefab");

                GameObject frog = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_Frog.prefab");
                if (frog == null) frog = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_GreenFrog.prefab");

                GameObject mouse = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_Mouse.prefab");
                if (mouse == null) mouse = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_OrangeHamster.prefab");

                GameObject pigeon = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_Pigeon.prefab");
                if (pigeon == null) pigeon = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_YellowPigeon.prefab");

                GameObject bunny = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_Bunny.prefab");
                if (bunny == null) bunny = AssetDatabase.LoadAssetAtPath<GameObject>($"{animalDir}/Animal_PurpleBunny.prefab");

                if (puppy != null) so.FindProperty("_puppyPrefab").objectReferenceValue = puppy;
                if (kitten != null) so.FindProperty("_kittenPrefab").objectReferenceValue = kitten;
                if (frog != null) so.FindProperty("_frogPrefab").objectReferenceValue = frog;
                if (mouse != null) so.FindProperty("_mousePrefab").objectReferenceValue = mouse;
                if (pigeon != null) so.FindProperty("_pigeonPrefab").objectReferenceValue = pigeon;
                if (bunny != null) so.FindProperty("_bunnyPrefab").objectReferenceValue = bunny;

                so.ApplyModifiedProperties();
                wiredCount++;
            }

            // Mark Scene Dirty and Save
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

            Debug.Log($"🎉 [AutoWireSceneReferences] Successfully wired {wiredCount} components across the entire scene!");
            EditorUtility.DisplayDialog("Stray Swarm", $"Successfully auto-wired {wiredCount} components in the scene!\n\nAll empty Inspector fields on Player, RescueStation, GameManager, UIManager, LevelManager, and AudioManager are now filled and saved!", "Awesome!");
        }

        private static Transform FindDeepChild(Transform parent, string name)
        {
            if (parent == null) return null;
            if (parent.name == name) return parent;

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform found = FindDeepChild(parent.GetChild(i), name);
                if (found != null) return found;
            }
            return null;
        }
    }
}
#endif
