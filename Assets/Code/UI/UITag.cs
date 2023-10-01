using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class UITag : MonoBehaviour
    {
        public static RectTransform SelfRect;
        public static Canvas SelfCanvas;
        public static CanvasScaler SelfScaler;

        private void Awake()
        {
            if (TryGetComponent(out RectTransform rectTransform))
            {
                SelfRect = rectTransform;
            }
            else
            {
                Debug.LogError("Something wrong!");
            }
            
            if (TryGetComponent(out CanvasScaler canvasScaler))
            {
                SelfScaler = canvasScaler;
            }
            else
            {
                Debug.LogError("Something wrong!");
            }
            
            if (TryGetComponent(out Canvas canvas))
            {
                SelfCanvas = canvas;
            }
            else
            {
                Debug.LogError("Something wrong!");
            }
        }
    }
}