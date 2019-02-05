using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineClipManager : MonoBehaviour
{

    public List<ClipSettings> timelineClips;
    public PlayableDirector timeline;
    private Dictionary<ClipSettings, double> clipToTime;


    public void BuildTimeline() {
        Debug.Log("Buidilng Timeline");
        TimelineAsset timelineAsset = (TimelineAsset)timeline.playableAsset;

        //TODO: make it so that it deletes all tracks and rebuilds them every each time
        for (int i = 0; i < timelineAsset.rootTrackCount; i++) {
            timelineAsset.DeleteTrack(timelineAsset.GetRootTrack(0));
        }
        for (int i = 0; i < timelineAsset.outputTrackCount; i++)
        {
            timelineAsset.DeleteTrack(timelineAsset.GetOutputTrack(0));
        }

        ControlTrack controlTrack = (ControlTrack)timelineAsset.CreateTrack(typeof(ControlTrack), null, "Control Track");
        clipToTime = new Dictionary<ClipSettings, double>();

        // map all timeline clips first
        for (int i = 0; i < timelineClips.Count; i++) {
            TimelineClip tc = controlTrack.CreateDefaultClip();
            ControlPlayableAsset cpa = tc.asset as ControlPlayableAsset;
            cpa.sourceGameObject.exposedName = UnityEditor.GUID.Generate().ToString();
            timeline.SetReferenceValue(cpa.sourceGameObject.exposedName, timelineClips[i].timelineTrack.gameObject);

            if(i != 0)
                tc.start = tc.start + 1;

            clipToTime.Add(timelineClips[i], tc.start);

            tc.duration = timelineClips[i].timelineTrack.duration;
        }

        //map remix clips
        int j = 0;
        foreach(TimelineClip tc in controlTrack.GetClips()) {
            foreach (TriggerMapping triggerMapping in timelineClips[j].triggers) {
                TimeMachineTrack timeMachineTrack = (TimeMachineTrack)timelineAsset.CreateTrack(typeof(TimeMachineTrack), null, "Trigger Track");
                if (triggerMapping.type == TriggerType.CONTINUOUS) {
                    TimelineClip triggerCip = timeMachineTrack.CreateDefaultClip();
                    TimeMachineAsset tma = triggerCip.asset as TimeMachineAsset;

                    tma.action = TimeMachineBehavior.TimeMachineAction.JumpToTime;
                    tma.condition = TimeMachineBehavior.Condition.TriggerOff;
                    tma.timeToJumpTo = (float)clipToTime[triggerMapping.targetTrack];
                    triggerCip.start = tc.start;
                    triggerCip.duration = tc.duration;
                   
                    
                    tma.trigger.exposedName = UnityEditor.GUID.Generate().ToString();
                    // tma.timeToJumpTo = triggerMapping.timeToJumpTo;
                    timeline.SetReferenceValue(tma.trigger.exposedName, triggerMapping.trigger);
                } else {
                    TimelineClip triggerCip = timeMachineTrack.CreateDefaultClip();
                    TimeMachineAsset tma = triggerCip.asset as TimeMachineAsset;

                    tma.action = TimeMachineBehavior.TimeMachineAction.JumpToTime;
                    tma.condition = TimeMachineBehavior.Condition.TriggerOff;
                    tma.timeToJumpTo = (float)clipToTime[triggerMapping.targetTrack];
                    triggerCip.start = tc.end;
                    triggerCip.duration = 1;

                    tma.trigger.exposedName = UnityEditor.GUID.Generate().ToString();
                    timeline.SetReferenceValue(tma.trigger.exposedName, triggerMapping.trigger);
                }
            }
            j++;
        }
        Debug.Log("Finished");
    }

    [System.Serializable]
    public class ClipSettings {
        public PlayableDirector timelineTrack;
        public List<TriggerMapping> triggers;

        public override bool Equals(object obj)
        {
            if (!(obj is ClipSettings))
                return false;

            ClipSettings other = (ClipSettings)obj;
            return this.timelineTrack == other.timelineTrack && this.triggers == other.triggers;
        }

        public override int GetHashCode()
        {
            return 31 * timelineTrack.GetHashCode() + 11 * triggers.GetHashCode();
        }
    }
}

[System.Serializable]
public struct TriggerMapping
{
    public TimelineTrigger trigger;
    public TriggerType type;
    public TimelineClipManager.ClipSettings targetTrack;
    //public float timeToJumpTo;

    public override bool Equals(object obj)
    {
        if (!(obj is TriggerMapping))
            return false;

        TriggerMapping other = (TriggerMapping)obj;
        return this.trigger == other.trigger && this.type == other.type && this.targetTrack == other.targetTrack;
    }

    public override int GetHashCode()
    {
        return 31 * trigger.GetHashCode() + 11 * type.GetHashCode() + 41 * targetTrack.GetHashCode();
    }
}

public enum TriggerType { CONTINUOUS, SINGLE };


