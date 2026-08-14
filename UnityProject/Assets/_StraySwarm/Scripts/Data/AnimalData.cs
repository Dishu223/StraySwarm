using UnityEngine;

namespace StraySwarm.Data
{
    public enum AnimalType
    {
        Puppy = 0,
        Kitten = 1,
        Frog = 2,
        Mouse = 3,
        Pigeon = 4,
        Bunny = 5
    }

    /// <summary>
    /// ScriptableObject defining an animal species, its color, sprites, and sounds.
    /// </summary>
    [CreateAssetMenu(fileName = "NewAnimalData", menuName = "Stray Swarm/Data/Animal Data")]
    public class AnimalData : ScriptableObject
    {
        [Header("Identity")]
        public AnimalType Type;
        public string DisplayName = "Puppy";
        public Color PrimaryColor = Color.blue;

        [Header("Visuals")]
        public Sprite WorldSprite;
        public Sprite ColorblindIcon;

        [Header("Audio")]
        public AudioClip CollectSound;
    }
}
