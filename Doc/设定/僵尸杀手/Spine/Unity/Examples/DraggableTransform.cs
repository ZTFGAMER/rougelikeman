namespace Spine.Unity.Examples
{
    using System;
    using UnityEngine;

    public class DraggableTransform : MonoBehaviour
    {
        private Vector2 mousePreviousWorld;
        private Vector2 mouseDeltaWorld;
        private Camera mainCamera;

        private void OnMouseDrag()
        {
            base.transform.Translate((Vector3) this.mouseDeltaWorld);
        }

        private void Start()
        {
            this.mainCamera = Camera.main;
        }

        private void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 vector2 = this.mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -this.mainCamera.transform.position.z));
            this.mouseDeltaWorld = vector2 - this.mousePreviousWorld;
            this.mousePreviousWorld = vector2;
        }
    }
}

