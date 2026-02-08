using UnityEditor;
using UnityEngine;

namespace SpaceShooter
{
    public class RaycastCircleTest : MonoBehaviour
    {
        [SerializeField] private float m_Radius;

        // Update is called once per frame
        void Update()
        {
            TestCircleCastAll();
        }

        private void TestRaycast()
        {
            var hit = Physics2D.Raycast(transform.position, transform.up, m_Radius);

            Debug.DrawLine(transform.position, transform.position + transform.up * m_Radius);

            if (hit) Debug.Log("Raycast: " + hit.collider.gameObject.name);
        }

        private void TestCircleCastAll()
        {
            //RaycastHit2D[] cols = Physics2D.CircleCastAll(
            //    origin: transform.position,
            //    radius: m_Radius,
            //    direction: transform.up,
            //    distance: Time.deltaTime * 5
            //);

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, m_Radius);

            Debug.DrawLine(transform.position, transform.position + transform.up * m_Radius);

            if (cols != null && cols.Length > 0)
            {
                Debug.Log("Found " + cols.Length + " targets");

                foreach (var item in cols)
                {
                    Debug.Log("Raycast: " + item.gameObject.name);
                }
            }
        }

#if UNITY_EDITOR
        private static Color GizmoColor1 = new Color(0, 1, 0, 0.1f);
        private static Color GizmoColor2 = new Color(1, 1, 0, 0.1f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor1;
            Handles.DrawSolidDisc(transform.position + transform.up * m_Radius, transform.forward, m_Radius);

            Handles.color = GizmoColor2;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
#endif
    }
}