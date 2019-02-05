using UnityEngine;
using System.Collections.Generic;
using NodeEditorFramework.Utilities;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor;

namespace NodeEditorFramework.Standard {
    [Node(false, "TimelineEditor/Timeline Node")]
    public class TimelineNode : Node {
        public const string ID = "timelineNode";
        public override string GetID { get { return ID; } }
        public override bool AllowRecursion { get { return true; } }

        public override string Title { get { return "Timeline Node"; } }
        public override Vector2 MinSize { get { return new Vector2(300, 60); } }
        public override bool AutoLayout { get { return true; } } // IMPORTANT -> Automatically resize to fit list


        public PlayableDirector currentTrack;
        public List<TriggerMapping> triggers;

        [ValueConnectionKnob("", Direction.In, "System.String", ConnectionCount.Multi)]
        public ValueConnectionKnob inputKnob;

        private ValueConnectionKnobAttribute dynaCreationAttribute = new ValueConnectionKnobAttribute("Output", Direction.Out, "System.String");

        public override void NodeGUI()
        {
            if (triggers == null)
                triggers = new List<TriggerMapping>();

            if (dynamicConnectionPorts.Count != triggers.Count)
            { // Make sure labels and ports are synchronised
                while (dynamicConnectionPorts.Count > triggers.Count)
                    DeleteConnectionPort(dynamicConnectionPorts.Count - 1);
                while (dynamicConnectionPorts.Count < triggers.Count)
                    CreateValueConnectionKnob(dynaCreationAttribute);
            }

            GUILayout.Label("This is a timeline node");

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            inputKnob.DisplayLayout();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            currentTrack = EditorGUILayout.ObjectField("timeline clip", currentTrack, typeof(PlayableDirector), true) as PlayableDirector;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))){
                triggers.Add(new TriggerMapping());
                CreateValueConnectionKnob(dynaCreationAttribute);
            }
         
            for (int i = 0; i < triggers.Count; i++)
            { // Display label and delete button
                GUILayout.BeginHorizontal();
                GUILayout.Label("trigger " + i);
                ((ValueConnectionKnob)dynamicConnectionPorts[i]).SetPosition();
                if (GUILayout.Button("x", GUILayout.ExpandWidth(false)))
                { // Remove current label
                    triggers.RemoveAt(i);
                    DeleteConnectionPort(i);
                    i--;
                }
                GUILayout.EndHorizontal();

                TriggerMapping curTrigger = triggers[i];
                curTrigger.trigger = EditorGUILayout.ObjectField("timeline clip", curTrigger.trigger, typeof(TimelineTrigger), true) as TimelineTrigger;
                curTrigger.type = (TriggerType)EditorGUILayout.EnumPopup("trigger type", curTrigger.type);
                triggers[i] = curTrigger;
                GUILayout.Space(10);
            }                      
        }

        public TimelineClipManager.ClipSettings buildTimeline(List<TimelineClipManager.ClipSettings> mapToTiming) {
            TimelineClipManager.ClipSettings clip = new TimelineClipManager.ClipSettings();
            clip.timelineTrack = currentTrack;
            clip.triggers = triggers;

            if (mapToTiming.Contains(clip)) {
                return clip;
            }
            mapToTiming.Add(clip);
            for (int i = 0; i < dynamicConnectionPorts.Count; i++) {
                Node n = dynamicConnectionPorts[i].connection(0).body;
                if (n != null && n is TimelineNode) {
                    if (((TimelineNode)n).currentTrack == null) {
                        Debug.LogError("missing current track");
                        return null;
                    }

                    TriggerMapping curTrigger = triggers[i];
                    curTrigger.targetTrack = ((TimelineNode)n).buildTimeline(mapToTiming);
                    triggers[i] = curTrigger;
                }
            }

            return clip;
        }
    }
}