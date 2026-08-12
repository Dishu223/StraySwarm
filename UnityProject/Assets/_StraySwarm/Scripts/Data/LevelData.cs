using System.Collections.Generic;
using UnityEngine;

namespace StraySwarm.Data
{
    /// <summary>
    /// A ScriptableObject that holds all the data for a specific level.
    /// You can create hundreds of these in the Project window without writing code!
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData_01", menuName = "Stray Swarm/Level Data")]
    public class LevelData : ScriptableObject
    {
        public string LevelName = "Level 1";
        
        [Tooltip("Total time in seconds to beat the level.")]
        public float TimeLimit = 60f; 
        
        [Tooltip("Time remaining needed to get a 3-star rating.")]
        public float ThreeStarTimeRemaining = 30f;
        
        [Tooltip("Time remaining needed to get a 2-star rating.")]
        public float TwoStarTimeRemaining = 15f;
        
        [Tooltip("The sequence of vans required to beat the level (e.g. 'Blue', 'Pink', 'Yellow').")]
        public List<string> VanSequence = new List<string>();
    }
}
