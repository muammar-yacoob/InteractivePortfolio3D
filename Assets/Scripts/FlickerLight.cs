using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class FlickerLight : MonoBehaviour
    {
        private Material materialInstance;

        private async void Start()
        {
            materialInstance = GetComponent<Renderer>().material;
            Flicker().Forget();
        }

        private async UniTask Flicker()
        {
            while (this.isActiveAndEnabled)
            {
                float intensity = Random.Range(0.0f, 1.0f);
                Color finalColor = Color.white * Mathf.LinearToGammaSpace(intensity);

                materialInstance.SetColor("_EmissionColor", finalColor);
            
                await UniTask.Delay(System.TimeSpan.FromSeconds(Random.Range(0.05f, 0.5f)));
                materialInstance.SetColor("_EmissionColor", Color.black);
                await UniTask.Delay(System.TimeSpan.FromSeconds(Random.Range(0.05f, 0.5f)));
            }
        }

        private void OnDestroy()
        {
            if (materialInstance != null)
            {
                Destroy(materialInstance);
            }
        }
    }
}