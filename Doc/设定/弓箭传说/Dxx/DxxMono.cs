namespace Dxx
{
    using UnityEngine;

    public class DxxMono : MonoBehaviour
    {
        private Transform m_transform;

        public Transform trans
        {
            get
            {
                if (this.m_transform == null)
                {
                    this.m_transform = base.GetComponent<Transform>();
                }
                return this.m_transform;
            }
        }
    }
}

