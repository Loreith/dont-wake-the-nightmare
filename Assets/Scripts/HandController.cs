using UnityEngine;
using VRTK;

public class HandController : MonoBehaviour {

    Animator anim;
    VRTK_ControllerEvents controllerEvents;
    MeshRenderer[] controllerModels;

    public GameObject pointerCollider;
    public Transform book;
    float fingerPointDist = 0.15f;

    string xAxis = "XBlend";
    string yAxis = "YBlend";

    float timer = 0;
    float time = 0.5f;
    bool turnedOff = false;

    public bool hideMe = false;

    void Start() {
        anim = GetComponent<Animator>();
        controllerEvents = transform.parent.GetComponent<VRTK_ControllerEvents>();
    }

    private void Update() {
        UpdateHand();

        if (!turnedOff) {
            TurnOff();
        }
    }

    void UpdateHand() {
        float mainAxis = controllerEvents.GetTriggerAxis();
        //if (pointerCollider) {
        //    float distanceFromBook = (transform.position - book.position).sqrMagnitude;
        //    float axis = Mathf.Clamp(1.5f - distanceFromBook / fingerPointDist, 0, 1);
        //    anim.SetFloat(yAxis, axis);

        //    if (axis >= 0.5f) {
        //        pointerCollider.SetActive(true);
        //    } else {
        //        pointerCollider.SetActive(false);
        //    }

        //    if (mainAxis < axis) {
        //        mainAxis = axis;
        //    }
        //}

        anim.SetFloat(xAxis, mainAxis);
    }

    void TurnOff() {
        timer += Time.deltaTime;

        // Check if it's time to execute
        if (timer >= time) {

            Transform controllerModel = transform.parent.parent.GetChild(0);

            // Get all the meshrenderers
            controllerModels = controllerModel.GetComponentsInChildren<MeshRenderer>();
            
            // Check if the model parts have been created
            if (controllerModels.Length <= 0) { // no parts = reset timer
                timer = 0;

            } else {

                for (int i = 0; i < controllerModels.Length; i++) {
                    controllerModels[i].enabled = false;
                }

                // Hide hand
                if (hideMe)
                    gameObject.SetActive(false);

                turnedOff = true;
            }
            
        }
    }
}
