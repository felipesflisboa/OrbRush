using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour {
    [SerializeField] Transform[] spawnPointTransformArray;
    [SerializeField] Material[] marathonSkyboxMaterialPerLevel;
    internal List<Segment> segmentList;

    public Vector3 SpawnPointCenter {
        get {
            int validSize = 0;
            Vector3 ret = Vector3.zero;
            foreach (var spawnPoint in spawnPointTransformArray) {
                if (spawnPoint == null)
                    continue;
                ret += spawnPoint.position;
                validSize++;
            }
            return ret / validSize;
        }
    }

    void Start() {
        ConfigureSkybox();
        segmentList = CreateSegmentList();
    }

    void ConfigureSkybox() {
        if (GameManager.modeData is MarathonData) {
            RenderSettings.skybox = marathonSkyboxMaterialPerLevel[
                (GameManager.modeData as MarathonData).level % marathonSkyboxMaterialPerLevel.Length
            ];
        }
    }

    List<Segment> CreateSegmentList() {
        List<Segment> ret = FindObjectsOfType<Segment>().ToList();
        ret.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        return ret;
    }

    public void InstantiateOrbs(GameObject[] orbPrefabArray) {
        for (int i = 1; i < spawnPointTransformArray.Length; i++)
            Instantiate(orbPrefabArray[i], spawnPointTransformArray[i].position, spawnPointTransformArray[i].rotation);
    }

    public void OnReachGoal(Orb orb) {
        orb.reachGoal = true;
        if (GameManager.I.state == GameState.Ocurring)
            GameManager.I.EndGame(orb);
    }
}
