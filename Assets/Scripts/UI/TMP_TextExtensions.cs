using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;

namespace SparkGames.Portfolio3D.UI
{
    public static class TMP_TextExtensions
    {
        /// <summary>
        /// Types out a given string in a specified duration on a TMP_Text object. 
        /// </summary>
        /// <param name="tmpText">The TMP_Text object on which to type out the text.</param>
        /// <param name="textToType">The string to type out.</param>
        /// <param name="durationPerChar">The duration of the typing effect.</param>
        /// <param name="adjustSizeToFit">If true, adjusts the font size of the TMP_Text object to fit the entire text.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A UniTask that completes when the typing effect is finished.</returns>
        public static async UniTask<string> DOText(this TMP_Text tmpText,
            string textToType, float durationPerChar, bool adjustSizeToFit = false,
            CancellationToken cancellationToken = default)
        {
            StringBuilder displayedText = new();

            foreach (char ch in textToType)
            {
                displayedText.Append(ch);
                tmpText.text = displayedText.ToString();

                if (adjustSizeToFit)
                {
                    tmpText.fontSizeMax = tmpText.fontSize;
                    while (tmpText.textBounds.size.x > tmpText.rectTransform.rect.width)
                    {
                        tmpText.fontSize--;
                        tmpText.ForceMeshUpdate();
                    }
                }

                try
                {
                    await UniTask.Delay((int)(durationPerChar * 1000), cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // Handle cancellation
                    return displayedText.ToString();
                }
            }
            return displayedText.ToString();
        }
    }
}