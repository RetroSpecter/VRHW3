using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimelineClipManager))]
public class TimelineClipManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TimelineClipManager tcm = (TimelineClipManager)target;
        if (GUILayout.Button("Generate Timeline")) {
            tcm.BuildTimeline();
        }

    }
}
