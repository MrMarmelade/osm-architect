using UnityEngine;

namespace UI
{
    /**
     * Skript přiřazen k LoadingCirclePart v MainLoading scene
     */
    public class ProgressBarFunction : MonoBehaviour
    {
        private RectTransform RectTransform;
        private const float RotateSpeed = 200f;

        /**
         * 
         */
        private void Start()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        /**
         * 
         */
        private void Update()
        {
            RectTransform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
        }
    }
}