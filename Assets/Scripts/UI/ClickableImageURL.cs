using System;
using SparkCore.Runtime.Core;
using SparkGames.Portfolio3D.Stations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SparkGames.Portfolio3D.UI
{
    public class ClickableUIImage : InjectableMonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Texture2D handCursor;
        private Vector2 cursorHotspot = new Vector2(0, 0);

        private string url = "https://spark-games.co.uk/";

        private void OnEnable() => SubscribeEvent<StationEntered>(entered => url = entered.ProjectData.URL);
        private void OnDisable() => UnsubscribeEvent<StationEntered>(stationInfo => url = String.Empty);

        private void Start()
        {
            cursorHotspot = new Vector2((float)handCursor.width / 2, (float)handCursor.height / 2);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (String.IsNullOrEmpty(url)) return;
            ResetCursor();
            PublishEvent(new StationExited());
            Application.OpenURL(url);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Change cursor to hand cursor
            Cursor.SetCursor(handCursor, cursorHotspot, CursorMode.Auto);
        }
        

        public void OnPointerExit(PointerEventData eventData)
        {
            // Revert to the default cursor
            ResetCursor();
        }

        private static void ResetCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}