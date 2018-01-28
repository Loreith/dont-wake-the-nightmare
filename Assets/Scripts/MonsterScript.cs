using UnityEngine;

public class MonsterScript : MonoBehaviour {

    // base behaviour
    protected Animator anim;
    public Transform player;
    protected AudioSource containerAudio;
    public MonsterContainerScript.MonsterID id = MonsterContainerScript.MonsterID.None;

    protected GameController controller;

    public MonsterContainerScript currentContainer;

    protected Vector3 originalPos;

    protected virtual void Start() {

        originalPos = transform.position;

        anim = GetComponent<Animator>();

        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    protected void MoveMe(bool extraShaking) {

        MonsterContainerScript newContainer = controller.GetRandomContainer();

        while (newContainer.currentResident != null) {
            newContainer = controller.GetRandomContainer();
        }

        // Reset Previous
        currentContainer.currentResident = null;

        

        // Get New
        currentContainer = controller.GetRandomContainer();

        currentContainer.currentResident = this;

        currentContainer.extraShake = extraShaking;
    }
}
