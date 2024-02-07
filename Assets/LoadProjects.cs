using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace SparkGames.Portfolio3D
{
    public class LoadProjects : InjectableMonoBehaviour
    {
        [Inject] private readonly ICVLoader cvLoader;
        [SerializeField] private List<TMP_Text> projectLabels;

        private async void Start()
        {
            await UniTask.Delay(1500);
            AssignProjectTitles();
        }

        private async void AssignProjectTitles()
        {
            var cvData = await cvLoader.GetCVDataAsync();
            for (int i = 0; i < projectLabels.Count; i++)
            {
                if (i >= cvData.Projects.Count)
                {
                    projectLabels[i].text = "";
                    continue;
                }
                projectLabels[i].text = cvData.Projects[i].Title;
            }
        }
    }
}