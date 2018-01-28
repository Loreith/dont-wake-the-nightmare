using UnityEngine;
using System.Collections;

public class MonsterContainerScript : MonoBehaviour {

    public enum MonsterID {
        None,
        GigglyWiggly,
        BadDog
    }

    public MonsterScript currentResident;

    public bool extraShake = false;

    public float shakeTimer = 0;
    protected Vector2 shakeTime = new Vector2(20, 3);
    protected Vector2 angleShake = new Vector2(10, 5);
    protected Transform player;

    protected AudioSource audioSource;

    private void Start() {
        shakeTimer = Random.Range(shakeTime.x - shakeTime.y, shakeTime.x + shakeTime.y);
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Shake());
    }

    protected virtual void Update() {
        if (currentResident && currentResident.id != MonsterID.None)
            ShakeTimer();
    }

    public void ShakeTimer() {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0) {
                shakeTimer = Random.Range(shakeTime.x - shakeTime.y, shakeTime.x + shakeTime.y);
                StartCoroutine(Shake());
            }
        }
    }

    IEnumerator Shake() {
        RotateMe();
        
        if (extraShake) {
            yield return new WaitForSeconds(0.1f);
            RotateMe();
        } else {
            yield return new WaitForEndOfFrame();
        }
    }

    void RotateMe() {
        transform.RotateAround(transform.position, Vector3.up, Random.Range(angleShake.x - angleShake.y, angleShake.x + angleShake.y));

        if (currentResident && currentResident.id == MonsterID.GigglyWiggly) {
            audioSource.clip = currentResident.GetComponent<Mon_Giggly>().RandomGiggle();
            audioSource.Play();
        }
    }

    public void Capture() {

    }

    protected float PlayerDistCheck() {
        if (currentResident.id != MonsterID.None) {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").transform;
            return (transform.position - player.position).sqrMagnitude;
        }

        return -1;
    }
}
