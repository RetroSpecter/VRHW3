using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class TimeMachineAsset : PlayableAsset, ITimelineClipAsset
{

    [HideInInspector]
    public TimeMachineBehavior template = new TimeMachineBehavior();

    public TimeMachineBehavior.TimeMachineAction action;
    public TimeMachineBehavior.Condition condition;
    public string markerToJumpTo = "", markerLabel = "";
    public float timeToJumpTo = 0f;
    public ExposedReference<TimelineTrigger> trigger;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TimeMachineBehavior>.Create(graph, template);
        TimeMachineBehavior clone = playable.GetBehaviour();
        clone.trigger = trigger.Resolve(graph.GetResolver());
        clone.markerToJumpTo = markerToJumpTo;
        clone.action = action;
        clone.condition = condition;
        clone.markerLabel = markerLabel;
        clone.timeToJumpTo = timeToJumpTo;

        return playable;
    }
}
