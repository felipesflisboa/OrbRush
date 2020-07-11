using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] Transform[] spawnPointTransformArray;
    [SerializeField] GameObject[] playerPrefabArray;
    [SerializeField] GameObject[] cardPrefabArray;

    internal Player[] playerArray; //TODO protect
    internal Player humanPlayer;
    internal bool occuring;
    internal Card selectedCard;
    internal List<Segment> segmentList;
    AI[] aiArray;

    void Start() {
        Time.timeScale = 0;
        segmentList = FindObjectsOfType<Segment>().ToList();
        segmentList.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        // zoneList.Sort((a, b) => Mathf.Approximately(a.transform.position.z, b.transform.position.z) ? a.transform.position.x.CompareTo(b.transform.position.x) : a.transform.position.z.CompareTo(b.transform.position.z));

        playerArray = new Player[spawnPointTransformArray.Length];
        for (int i = 1; i < spawnPointTransformArray.Length; i++) {
            playerArray[i] = Instantiate(
                playerPrefabArray[i], spawnPointTransformArray[i].position, spawnPointTransformArray[i].rotation
            ).GetComponent<Player>();
            playerArray[i].number = i;
        }

        CanvasController.I.startText.gameObject.SetActive(true);
    }

    public void StartGame() {
        CanvasController.I.startText.gameObject.SetActive(false);
        CanvasController.I.playerText.text = $"Player {humanPlayer.m_name}";
        Time.timeScale = 1;

        aiArray = new AI[3];
        int playerI = 1;
        for (int aiI = 0; aiI < aiArray.Length; aiI++) {
            if(humanPlayer== playerArray[playerI])
                playerI++;
            aiArray[aiI] = new AI(playerArray[playerI]);
            aiArray[aiI].StartRoutine(this);
            playerI++;
        }

        StartCoroutine(CardRoutine());
        occuring = true;
    }

    IEnumerator CardRoutine() {
        yield return null;
        while (occuring) {
            CanvasController.I.cardZone.Add(cardPrefabArray[Mathf.FloorToInt(Random.value * cardPrefabArray.Length)]);
            foreach (var ai in aiArray)
                ai.cardTypeDeck.Add(EnumUtil.GetRandomValueFromEnum<CardType>(1, -1));
            yield return new WaitForSeconds(5);
        }
    }

    public void OnReachGoal(Player player) {
        if (!occuring)
            return;
        Debug.Log($"Player {player.number} won in {Time.timeSinceLevelLoad}s!");
        Time.timeScale = 0;
        this.Schedule(new WaitForSecondsRealtime(1.2f), () => {
            CanvasController.I.victoryText.gameObject.SetActive(true);
            CanvasController.I.victoryText.text = $"Player {player.m_name} won!";
        });
        occuring = false;
    }

    public void ExecuteCardEffect(Player player, Card card, CardType cardType) {
        switch (cardType) {
            case CardType.Neo:
                Debug.Log("Activated=" + cardType);
                GameManager.I.playerArray[1].Boost();
                if (card != null)
                    Destroy(card.gameObject);
                break;
            case CardType.Fire:
            case CardType.Water:
            case CardType.Earth:
            case CardType.Air:
                if (card != null)
                    card.Highlight();
                else
                    AIApplyOnNearSegment(player, cardType);
                break;
            default:
                Debug.Log("Activated=" + cardType);
                break;
        }
    }

    void AIApplyOnNearSegment(Player player, CardType cardType) {
        for (int i = 0; i < 10; i++) { //TODO count
            var segmentIndex = segmentList.IndexOf(player.currentSegment);
            var nearIndexArray = new[] { segmentIndex - 1, segmentIndex, segmentIndex + 1 };
            int randomIndex = nearIndexArray[Mathf.FloorToInt(Random.value * nearIndexArray.Length)];
            //TODO break method
            if (0<=randomIndex && randomIndex<segmentList.Count && segmentList[randomIndex].cardType != cardType) {
                segmentList[randomIndex].ApplyEffect(cardType);
                return;
            }
        }
        // Try any segment
        for (int i = 0; i < 40; i++) {
            int randomIndex = Mathf.FloorToInt(Random.value * segmentList.Count);
            if (segmentList[randomIndex].cardType != cardType) {
                segmentList[randomIndex].ApplyEffect(cardType);
                return;
            }
        }
        Debug.LogWarning("Can't apply effect on valid segment!");
    }
}
