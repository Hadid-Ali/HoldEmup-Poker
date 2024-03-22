using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GizmoObjectType
{
   Sphere,
   Cube,
   WiredCube
}

public class SimpleGizmo : MonoBehaviour
{
   [SerializeField,Tooltip("Uses X for single")] private Vector3 m_GizmoSize;
   [SerializeField] private GizmoObjectType m_GizmoType;

   [SerializeField] private Color m_GizmoColor;

   [SerializeField] private bool m_DrawGizmos = true;

   private Transform m_Transform;

   private void OnDrawGizmos()
   {
      m_Transform ??= transform;
      DrawGizmosInternal();
   }

   private void DrawGizmosInternal()
   {
      Gizmos.color = m_GizmoColor;

      switch (m_GizmoType)
      {
         case GizmoObjectType.Sphere:
            Gizmos.DrawSphere(m_Transform.position, m_GizmoSize.x);
            break;
         
         case GizmoObjectType.Cube:
            Gizmos.DrawCube(m_Transform.position,m_GizmoSize);
            break;
         
         case GizmoObjectType.WiredCube:
            Gizmos.DrawWireCube(m_Transform.position,m_GizmoSize);
            break;
      }
   }
}
