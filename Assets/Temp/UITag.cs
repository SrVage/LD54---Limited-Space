using UnityEngine;

namespace Temp
{
    public class UITag : MonoBehaviour
    {
        public static RectTransform SelfRect;
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
        }
    }
}