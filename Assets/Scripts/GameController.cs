using UnityEngine;
using VRTK;

public class GameController : MonoBehaviour {

    public Transform player;
    public AlarmClock clock;
    public VRTK_BasicTeleport teleportScript;
    public Storybook book;

    public GameObject gigglyWigglyObject;
    public GameObject BadDog;

    public MonsterContainerScript[] objects;

    Mon_Giggly giggly;
    Mon_BadDog badDog;

    private void Start() {
        gigglyWigglyObject = GameObject.FindGameObjectWithTag("Giggle");
        giggly = gigglyWigglyObject.GetComponent<Mon_Giggly>();
        BadDog = GameObject.FindGameObjectWithTag("BadDog");
        badDog = BadDog.GetComponent<Mon_BadDog>();

        PossessRandom();
    }

    void PossessRandom() {

        MonsterContainerScript container1 = GetRandomContainer();
        container1.currentResident = giggly;
        giggly.currentContainer = container1;
        container1.extraShake = false;
        Debug.Log("Chosen: " + container1.name);

        MonsterContainerScript container2 = GetRandomContainer();
        while (container2.currentResident != null)
            container2 = GetRandomContainer();

        container2.currentResident = badDog;
        badDog.currentContainer = container2;
        container2.extraShake = true;
        Debug.Log("Chosen: " + container2.name);
    }

    public MonsterContainerScript GetRandomContainer() {
        return objects[Random.Range(0, objects.Length)];
    }

    public void BlinkPlayer(float blinkTime) {
        float originalTime = teleportScript.distanceBlinkDelay;
        teleportScript.distanceBlinkDelay = blinkTime;

        Vector3 floorPos = player.position;
        floorPos.y = 0;
        teleportScript.ForceTeleport(Vector3.zero);

        teleportScript.distanceBlinkDelay = originalTime;
    }

    public void CaptureTarget(MonsterContainerScript.MonsterID monsterID) {
        switch(monsterID) {
            case MonsterContainerScript.MonsterID.GigglyWiggly:
                giggly.Capture();
                Debug.Log("Capturing giggly");
                break;

            case MonsterContainerScript.MonsterID.BadDog:
                break;
        }
    }
}
