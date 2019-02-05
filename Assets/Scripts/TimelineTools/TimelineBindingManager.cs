using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineBindingManager : MonoBehaviour
{

    public List<GameObject> trackList = new List<GameObject>();
    public PlayableDirector timeline;
    public TimelineAsset timelineAsset;
    public bool autoBindTracks = true;

    // Start is called before the first frame update
    void Start() {
        if (autoBindTracks)
            BindTimelineTracks();
    }

    public void BindTimelineTracks()
    {
        Debug.Log("Binding Timeline Tracks!");
        timelineAsset = (TimelineAsset)timeline.playableAsset;
        // iterate through tracks and map the objects appropriately
        for (var i = 0; i < trackList.Count; i++)
        {
            if (trackList[i] != null)
            {
                var track = (TrackAsset)timelineAsset.GetOutputTrack(i);
                timeline.SetGenericBinding(track, trackList[i]);
            }
        }
    }
}
