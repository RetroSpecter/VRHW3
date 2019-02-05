using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeMachineBehavior : PlayableBehaviour
{
    public TimeMachineAction action;
    public Condition condition;
    public string markerToJumpTo, markerLabel;
    public float timeToJumpTo;

    public TimelineTrigger trigger;

    [HideInInspector]
    public bool clipExecuted = false; //the user shouldn't author this, the Mixer does

    public bool ConditionMet()
    {
        switch (condition)
        {
            case Condition.Always:
                return true;

            case Condition.TriggerOff:
                if (trigger != null) {
                    return !trigger.Triggered();
                }
                return false;

            case Condition.TriggerOn :
                if (trigger != null)
                {
                    return trigger.Triggered();
                }
                return false;

            case Condition.Never:
            default:
                return false;
        }
    }

    public enum TimeMachineAction
    {
        Marker,
        JumpToTime,
        JumpToMarker,
        Pause,
    }

    public enum Condition
    {
        Always,
        Never,
        TriggerOff,
        TriggerOn,
    }
}
