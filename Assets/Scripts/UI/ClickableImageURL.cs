using System;
using SparkCore.Runtime.Core;
using SparkGames.Portfolio3D.Stations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SparkGames.Portfolio3D.UI
{
    public class ClickableUIImage : InjectableMonoBehaviour, IPointerClickHandler
    {
        private string url = "https://spark-games.co.uk/";

        private void OnEnable()
        {
            SubscribeEvent<StationURL>(stationUrl => this.url = stationUrl.URL);
        }

        private void OnDisable()
        {
            UnsubscribeEvent<StationURL>(stationUrl => this.url = stationUrl.URL);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (String.IsNullOrEmpty(url)) return;
            Application.OpenURL(url);
        }
    }
}