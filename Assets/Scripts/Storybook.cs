using UnityEngine;
using System.Collections;
using VRTK;

public class Storybook : MonoBehaviour {

    public GameObject openBook;
    public GameObject closeBook;
    Transform myParent;
    GameController controller;

    public VRTK_ControllerEvents leftController;
    public VRTK_ControllerEvents rightController;
    public VRTK_Pointer pointerScript;
    public Transform raycastPoint;
    LineRenderer lineRend;
    public Transform playerHead;

    public Material[] pages;
    MeshRenderer renderer;

    // Capture sequence
    float minSqrDist = 0.1f;
    float minOpenDist = 1.2f;
    bool leftTrigger = false;
    bool rightTrigger = false;

    bool bookClosed = false;
    MonsterContainerScript currentSelection;
    public AlarmClock clock;
    float timePenalty = 10;
    Quaternion originalRotation;
    Quaternion originalBookRotation;

    // Page sequence
    int currentPage = 0;
    int monsterPage1 = 1;
    int monsterPage2 = 6;
    public Material p2;
    public Material p7;


    private void Start() {
        renderer = openBook.GetComponent<MeshRenderer>();
        lineRend = raycastPoint.GetComponent<LineRenderer>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        lineRend.enabled = false;
        myParent = transform.parent;

        originalRotation = transform.rotation;
        originalBookRotation = openBook.transform.localRotation;

        leftController.TouchpadPressed += new ControllerInteractionEventHandler(OnTouchPad);
    }

    // Detect both triggers
    // Detect distance of hands
    // initialise capture selection
    // raycast? simple pointer script

    private void Update() {
        DetectPlayerHands();

        CanCapture();
    }


    void DetectPlayerHands() {
        // Check both controllers pressed
        leftTrigger = leftController.triggerPressed;
        rightTrigger = rightController.triggerPressed;

        if (leftTrigger && rightTrigger) {
            // Check distance
            if (!bookClosed && GetControllerDistance() <= minSqrDist) {
                ReadyCapturing();
                
            }
        } else {
            if (bookClosed) {
                UnReadyCapturing();

            }
            
        }
    }

    void CanCapture() {
        if (bookClosed) {
            if (GetControllerDistance() >= minOpenDist) {

                if (currentSelection && currentSelection.currentResident != null) {

                    controller.CaptureTarget(currentSelection.currentResident.id);

                    currentSelection = null;
                    Debug.Log("book: cap target");

                } else if (currentSelection) {
                    
                    IncorrectSelection();
                }

                closeBook.SetActive(false);
                openBook.SetActive(true);

                //UnReadyCapturing();

            } else {
                // raycast
                RaycastHit hit;
                if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out hit)) {

                    lineRend.SetPosition(0, raycastPoint.position);
                    lineRend.SetPosition(1, hit.point);
                    if (hit.transform.CompareTag("Possessable")) {
                        currentSelection = hit.transform.GetComponent<MonsterContainerScript>();

                    } else {
                        currentSelection = null;
                    }
                }

                KeepBookBetweenControllers();
            }
        }
    }

    IEnumerator DelayResetCapture(bool capture) {
        if (capture) {
            
            yield return new WaitForSeconds(3f);
        } else {
            yield return new WaitForSeconds(0.8f);
        }

        UnReadyCapturing();
    }

    void ReadyCapturing() {
        bookClosed = true;
        closeBook.SetActive(true);
        openBook.SetActive(false);
        lineRend.enabled = true;

        transform.parent = null;
        openBook.transform.localRotation = Quaternion.Euler(180, -90, 0);
    }

    void UnReadyCapturing() {
        bookClosed = false;
        closeBook.SetActive(false);
        openBook.SetActive(true);
        lineRend.enabled = false;

        transform.parent = myParent;
        transform.rotation = originalRotation;
        currentSelection = null;

        transform.localRotation = Quaternion.Euler(0, 0, 180);
        
        transform.localPosition = Vector3.zero;
    }

    void IncorrectSelection() {
        // Incorrect
        // Play sound

        UnReadyCapturing();

        clock.ReduceTimer(timePenalty);
    }

    void KeepBookBetweenControllers() {
        Vector3 localRight = rightController.transform.position - leftController.transform.position;

        localRight *= 0.5f;
        
        transform.position = localRight + leftController.transform.position;

        transform.rotation = playerHead.rotation;
        transform.Rotate(new Vector3(100, 180, 0));
    }

    float GetControllerDistance() {
        return (leftController.transform.position - rightController.transform.position).sqrMagnitude;
    }

    // Flip pages
    
    public void OnTouchPad(object sender, ControllerInteractionEventArgs e) {
        if (e.touchpadAxis.x > 0) {
            FlipPage(1);
        } else {
            FlipPage(-1);
        }
    }

    void FlipPage(int direction) {
        currentPage = Mathf.Clamp(currentPage + direction, 0, pages.Length - 1);

        renderer.material = pages[currentPage];
    }

    public void ReplacePage(int monsterPage) {
        if (monsterPage == 1)
            pages[monsterPage] = p2;
        else if (monsterPage == 6)
            pages[monsterPage] = p7;
    }


    private void OnDestroy() {
        leftController.TouchpadPressed -= OnTouchPad;
    }
}
