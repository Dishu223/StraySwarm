using System.Collections.Generic;
using UnityEngine;

namespace StraySwarm.Data
{
    public enum WorldTheme
    {
        Desert,
        Forest,
        Snow,
        City,
        Futuristic
    }

    /// <summary>
    /// ScriptableObject defining a themed World containing 20 handcrafted levels.
    /// </summary>
    [CreateAssetMenu(fileName = "World_01", menuName = "Stray Swarm/World Data")]
    public class WorldData : ScriptableObject
    {
        [Header("Identity")]
        public string WorldName = "Desert Oasis";
        public WorldTheme Theme = WorldTheme.Desert;
        public Sprite WorldIcon;
        public Color WorldColor = new Color(0.96f, 0.73f, 0.42f);

        [Header("Unlock Requirements")]
        [Tooltip("Total cumulative stars needed across all worlds to unlock this world.")]
        public int StarsRequiredToUnlock = 0;

        [Header("Audio")]
        public AudioClip WorldBGM;

        [Header("Handcrafted Levels")]
        public List<LevelData> Levels = new List<LevelData>();
    }
}
