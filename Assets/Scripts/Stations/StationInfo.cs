using UnityEngine;
using UnityEngine.Serialization;

namespace SparkGames.Portfolio3D.Stations
{
    [CreateAssetMenu(fileName = "StationInfo", menuName = "Station/StationInfo", order = 1)]
    public class StationInfo : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] [TextArea] private string dialogue;
        [SerializeField] private Sprite icon;
        
        public string Title => title;
        public string Dialogue => dialogue;
        public Sprite Icon => icon;
    }
}