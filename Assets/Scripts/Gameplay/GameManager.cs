using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    public static int level = 1;

    [SerializeField] Transform[] spawnPointTransformArray;
    [SerializeField] GameObject[] playerPrefabArray;
    [SerializeField] GameObject[] cardPrefabArray;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] AudioSource cycloneSFX;
    [SerializeField] AudioSource squidSFX;
    [SerializeField] AudioSource earthquakeSFX;
    [SerializeField] AudioSource explodeSFX;
    [SerializeField] AudioSource startSFX;
    [SerializeField] AudioSource endSFX;

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

        FindObjectOfType<CameraController>().GoToOriginalPos();

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
        startSFX.Play();
        occuring = true;
    }

    IEnumerator CardRoutine() {
        yield return null;
        while (occuring) {
            CanvasController.I.cardZone.Add(cardPrefabArray[Mathf.FloorToInt(Random.value * cardPrefabArray.Length)]);
            foreach (var ai in aiArray)
                ai.cardTypeDeck.Add(EnumUtil.GetRandomValueFromEnum<CardType>(1, -4));
            yield return new WaitForSeconds(5);
        }
    }

    public void OnReachGoal(Player player) {
        if (!occuring)
            return;
        StartCoroutine(EndGameRoutine(player));
    }

    IEnumerator EndGameRoutine(Player player) {
        Debug.Log($"Player {player.number} won in {Time.timeSinceLevelLoad}s!");
        Time.timeScale = 0;
        occuring = false;
        endSFX.Play();
        yield return new WaitForSecondsRealtime(1.2f);
        CanvasController.I.victoryText.gameObject.SetActive(true);
        CanvasController.I.victoryText.text = $"Player {player.m_name} won!";
        yield return new WaitForSecondsRealtime(4f);
        FindObjectOfType<Fader>().FadeOut(() => {
            if (player == humanPlayer) {
                level++;
                SceneManager.LoadScene("Game");
            } else {
                SimpleScoreListTimedDrawer.lastScore = level;
                ScoreListTimed scoreList = new ScoreListTimed();
                scoreList.Load();
                scoreList.AddScore((int)SimpleScoreListTimedDrawer.lastScore);
                scoreList.Save();
                SceneManager.LoadScene("MainMenu");
            }
        });
    }

    public void ExecuteCardEffect(Player player, Card card, CardType cardType) {
        Player selectedPlayer = null;
        switch (cardType) {
            case CardType.Neo:
                Debug.Log("Activated=" + cardType);
                GameManager.I.playerArray[1].Boost();
                if (card != null)
                    Destroy(card.gameObject);
                break;
            case CardType.Fire:
                selectedPlayer = GameManager.I.playerArray.First((p) => p!=null && p.element == Element.Fire);
                const float radius = 7f;
                foreach(var item in Physics.OverlapSphere(selectedPlayer.transform.position, radius)) {
                    Player p = item.GetComponentInParent<Player>();
                    if(p!= null && p != selectedPlayer) {
                        p.rigidBody.AddExplosionForce(700, selectedPlayer.transform.position, radius);
                    }
                }
                explodeSFX.Play();
                Destroy(Instantiate(explosionPrefab, selectedPlayer.transform.position, selectedPlayer.transform.rotation), 8f);
                if (card != null)
                    Destroy(card.gameObject);
                break;
            case CardType.Earth:
                selectedPlayer = GameManager.I.playerArray.First((p) => p!=null && p.element == Element.Earth);
                if(selectedPlayer.currentSegment != null) {
                    selectedPlayer.currentSegment.ApplyEffect(CardType.Earthquake);
                    if (card != null)
                        Destroy(card.gameObject);
                    earthquakeSFX.Play();
                }
                break;
            case CardType.Water:
                selectedPlayer = GameManager.I.playerArray.First((p) => p != null && p.element == Element.Water);
                if (selectedPlayer.currentSegment != null) {
                    selectedPlayer.currentSegment.ApplyEffect(CardType.Lake);
                    if (card != null)
                        Destroy(card.gameObject);
                    squidSFX.Play();
                }
                break;
            case CardType.Air:
                selectedPlayer = GameManager.I.playerArray.First((p) => p != null && p.element == Element.Air);
                if (selectedPlayer.currentSegment != null) {
                    selectedPlayer.currentSegment.ApplyEffect(CardType.Tornado);
                    if (card != null)
                        Destroy(card.gameObject);
                    cycloneSFX.Play();
                }
                break;
                /*
                if (card != null)
                    card.Highlight();
                else
                    AIApplyOnNearSegment(player, cardType);
                break;
                */
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
