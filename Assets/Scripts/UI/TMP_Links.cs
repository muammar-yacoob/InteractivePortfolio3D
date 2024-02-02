using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SparkGames.Portfolio3D.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_Links : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text textUI;
        private void Awake() => textUI = GetComponent<TMP_Text>();

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textUI, Input.mousePosition, null);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = textUI.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }
}