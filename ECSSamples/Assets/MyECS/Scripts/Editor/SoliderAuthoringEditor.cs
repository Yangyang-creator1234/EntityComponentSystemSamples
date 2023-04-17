using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor.UIElements;

namespace Yangyang
{
    [CustomEditor(typeof(SoliderAuthoring))]
    public class SoliderAuthoringEditor : Editor
    {
        private SerializedProperty m_Paramf;
        private SerializedProperty m_Paramz;
        private SerializedProperty m_Paramr;
        private SerializedProperty m_Curve;

        private void OnEnable()
        {
            m_Paramf = serializedObject.FindProperty("SecondOrderParamf");
            m_Paramz = serializedObject.FindProperty("SecondOrderParamz");
            m_Paramr = serializedObject.FindProperty("SecondOrderParamr");
            m_Curve = serializedObject.FindProperty("SecondOrderCurve");
        }

        public VisualTreeAsset VisualTree;
        public override VisualElement CreateInspectorGUI()
        {
            var myInspector = new VisualElement();
            VisualTree.CloneTree(myInspector);
            for (int i = 0; i < myInspector.childCount; i++)
            {
                var childElement = myInspector.ElementAt(i);
                if (childElement.name.Contains("SecondOrderParam"))
                {
                    var floatElement = childElement as PropertyField;
                    if (floatElement != null)
                    {
                        floatElement.RegisterValueChangeCallback((val) =>
                            {
                                RefreshAnimation();
                            });
                    }
                }
            }
            return myInspector;
        }

        private void RefreshAnimation()
        {
            serializedObject.Update();
            var soliderAuthoring = target as SoliderAuthoring;
            if (soliderAuthoring.SecondOrderParamf > 0f)
            {
                var div = Mathf.PI * soliderAuthoring.SecondOrderParamf;
                var paramk1 = soliderAuthoring.SecondOrderParamz / div;
                var paramk2 = 1f / ((2f * div) * (2f * div));
                var paramk3 = soliderAuthoring.SecondOrderParamr * soliderAuthoring.SecondOrderParamz / (2f * div);
                var secondOrder = new SecondOrderDynamicComponent
                {
                    xp = float3.zero,
                    y = float3.zero,
                    yd = 0f,
                    k1 = paramk1,
                    k2 = paramk2,
                    k3 = paramk3,
                };

                var deltaTime = 1f / 30f;
                var newCurve = new AnimationCurve();
                for (int i = 0; i <= 30; i++)
                {
                    secondOrder.Update(deltaTime, soliderAuthoring.MoveSpeedPerSeconds);
                    newCurve.AddKey(i * deltaTime, secondOrder.y.z);
                }
                m_Curve.animationCurveValue = newCurve;
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}