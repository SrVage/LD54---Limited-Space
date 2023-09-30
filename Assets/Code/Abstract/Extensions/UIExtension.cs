using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Abstract.Extensions
{
    public static class UIExtension
    {
        public static void SafedArea(this RectTransform rectTransform)
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        public static Sequence DoUIMove(this RectTransform rectTransform, Vector2 endPosition, float duration)
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
                    endPosition, duration));
            return sequence;
        }

        public static IDisposable RxSubscribe(this Button button, Action action)
        {
            return button
                .OnClickAsObservable()
                .Subscribe(_ => { action?.Invoke(); });
        }

        public static IDisposable RxSubscribe(this Button button, Action action, CompositeDisposable disposables)
        {
            return button
                .OnClickAsObservable()
                .Subscribe(_ => { action?.Invoke(); })
                .AddTo(disposables);
        }

        public static bool TryKill(this Sequence sequence)
        {
            if (sequence != null)
            {
                sequence.Kill();

                return true;
            }

            return false;
        }
    }
}