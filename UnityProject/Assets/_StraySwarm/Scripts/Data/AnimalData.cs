using UnityEngine;

namespace StraySwarm.Data
{
    public enum AnimalType
    {
        BluePuppy,
        PinkKitten,
        YellowPigeon,
        GreenFrog,
        OrangeHamster,
        PurpleBunny
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
