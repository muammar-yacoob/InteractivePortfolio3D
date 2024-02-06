using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using TMPro;
using UnityEngine;

namespace SparkGames.Portfolio3D.UI
{
    public class WhiteBoard : InjectableMonoBehaviour
    {
        [SerializeField] TMP_Text workExperienceUI;
        [Inject] private readonly ICVLoader cvLoader;
        private void Start() => workExperienceUI.text = cvLoader.CVData.WorkExperience;
    }
}