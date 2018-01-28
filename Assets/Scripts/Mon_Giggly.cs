using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Giggly : MonsterScript {

    float attackDistance = 3.5f;
    float quiterDistance = 8f;

    public bool attacking = false;
    bool capturing = false;

    // Capturing
    float capTime = 3f;
    float capTimer = 0;
    float movementSpeed = 1;

    public AudioClip[] giggles;
    public AudioClip attack;
    public AudioClip capture;

    AudioSource audioSource;
    public AudioTimer audioManager;

    protected override void Start() {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        id = MonsterContainerScript.MonsterID.GigglyWiggly;
    }

    private void Update() {
        BehaviourCriteria();
        AttackCriteria();

        if (capturing) {
            Capturing();
        }
    }

    void BehaviourCriteria() {

        float distance = CalculateDistance();

        if (distance <= quiterDistance) {
            if (containerAudio == null)
                currentContainer.GetComponent<AudioSource>();

            if (containerAudio)
                containerAudio.volume = distance / quiterDistance + 0.12f;
        }

    }

    void AttackCriteria() {
        if (!attacking && CalculateDistance() <= attackDistance) {

            attacking = true;

            Debug.Log("Attacking...");

            // Move and Spawn
            transform.position = currentContainer.transform.position;
            transform.LookAt(player);
            if (attacking) {
                // Play attack animation
                attacking = false;
                controller.clock.ReduceTimer(20f);
                StartCoroutine(AttackSequence());
                audioSource.clip = attack;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
    }

    IEnumerator AttackSequence() {
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        controller.BlinkPlayer(5f);

        MoveMe(false);

        transform.position = originalPos;

        anim.SetTrigger("StopAttack");
    }

    float CalculateDistance() {
        return (player.position - currentContainer.transform.position).sqrMagnitude;
    }

    public void Capture() {
        // Move and Spawn
        transform.position = currentContainer.transform.position;
        transform.LookAt(controller.book.transform);

        movementSpeed = (controller.book.transform.position - transform.position).magnitude / capTime;

        capturing = true;
        audioSource.clip = capture;
        audioSource.loop = true;
        audioSource.Play();


        anim.SetTrigger("Capture");
    }

    void Capturing() {
        capTimer += Time.deltaTime;

        transform.LookAt(player);
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
        transform.localScale -= Vector3.one * 0.3f * Time.deltaTime;

        if (capTimer >= capTime) { 
            capturing = false;
            controller.book.ReplacePage(1); // hard coded giggly

            transform.position = originalPos;
            currentContainer.currentResident = null;
            currentContainer = null;

            audioManager.PlayWinSource();
            audioSource.Stop();
            gameObject.SetActive(false);

        }
    }

    public AudioClip RandomGiggle() {
        return giggles[Random.Range(0, giggles.Length)];
    }
}
