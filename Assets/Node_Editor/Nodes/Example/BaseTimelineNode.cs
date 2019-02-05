using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework.Utilities;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor;

namespace NodeEditorFramework.Standard
{
    [Node(false, "TimelineEditor/Root Timeline Node")]
    public class BaseTimelineNode : Node
    {
        public const string ID = "baseTimelineNode";
        public override string GetID { get { return ID; } }
        public override bool AllowRecursion { get { return true; } }

        public override string Title { get { return "Root Timeline Node"; } }
        public override Vector2 MinSize { get { return new Vector2(300, 60); } }
        public override bool AutoLayout { get { return true; } } // IMPORTANT -> Automatically resize to fit list

        public TimelineClipManager rootTimeline;
        [ValueConnectionKnob("", Direction.Out, "System.String")]
        public ValueConnectionKnob outputKnob;

        public override void NodeGUI() {
            GUILayout.Label("This is a base timeline node");

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            outputKnob.DisplayLayout();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            rootTimeline = EditorGUILayout.ObjectField("timeline clip", rootTimeline, typeof(TimelineClipManager), true) as TimelineClipManager;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("construct timeline")) {
                constructTimeline();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public void constructTimeline() {
            Node n = outputKnob.connection(0).body;         
            if (n != null && n is TimelineNode) {
                List<TimelineClipManager.ClipSettings> triggerMappings = new List<TimelineClipManager.ClipSettings>();
                ((TimelineNode)n).buildTimeline(triggerMappings);
                rootTimeline.timelineClips = triggerMappings;
                rootTimeline.BuildTimeline();
            }
        }
    }
}
