using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomEditor(typeof(TimelineBindingManager))]
public class TimelineBindingManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TimelineBindingManager tbm = (TimelineBindingManager)target;
        base.OnInspectorGUI();

        if (tbm.timelineAsset == null || tbm.timeline == null) {
            return;
        }

        EditorGUILayout.LabelField("Bindings", EditorStyles.boldLabel);

        IEnumerable<TrackAsset> output = tbm.timelineAsset.GetOutputTracks();

        int i = 0;
        foreach (TrackAsset ta in output) {
            if (tbm.trackList.Count <= i) {                
                tbm.trackList.Add(null);
            }
            
            tbm.trackList[i] = (GameObject)EditorGUILayout.ObjectField(ta.name,tbm.trackList[i], typeof(Object), true);
            i++;
        }
    }
}
