using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Control Segments on Title Screen
/// </summary>
public class SegmentController : MonoBehaviour{
    List<Segment> segmentList;
    Orb[] orbArray;
    [SerializeField] float minSegmentOrbDistance;
    float segmentAverageDistance;

    float StepDistance => segmentAverageDistance * segmentList.Count;

    void Start(){
        segmentList = GetComponentsInChildren<Segment>().ToList();
        orbArray = FindObjectsOfType<Orb>();
        SortList();
        segmentAverageDistance = (segmentList.Last().transform.position.z - segmentList[0].transform.position.z) / (segmentList.Count - 1);
    }

    void SortList() => segmentList.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));

    void Update() {
        if(orbArray.FirstOrDefault(orb => segmentList.Last().transform.position.z - orb.transform.position.z < minSegmentOrbDistance ) != null)
            MoveFirstSegment();
    }

    void MoveFirstSegment() {
        segmentList[0].transform.position += StepDistance*Vector3.forward;
        SortList();
    }
}
