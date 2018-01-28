using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTimer : MonoBehaviour {

    public float timer = 0f;

    public AudioClip[] birdClips;
    public AudioSource[] birdSources;
    Vector2 birdVar = new Vector2(40, 4);
    float birdTime = 0;

    public AudioSource[] pipeSources;
    public AudioClip[] pipeClips;
    Vector2 pipeVar = new Vector2(15, 4);
    float pipeTime = 0;

    public AudioClip[] dropsClips;

    public AudioSource winSource;
    public AudioClip[] winClips;


    private void Start() {
        birdTime = RandomRange(birdVar);
        pipeTime = RandomRange(pipeVar);
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= birdTime) {
            birdTime += RandomRange(birdVar);
            AudioSource source = RandomSource(birdSources);
            source.clip = RandomClip(birdClips);
            source.Play();
        }

        if (timer >= pipeTime) {
            pipeTime += RandomRange(pipeVar);
            AudioSource source = RandomSource(pipeSources);
            source.clip = RandomClip(pipeClips);
            source.Play();
        }

        
    }

    float RandomRange(Vector2 set) {
        return Random.Range(set.x - set.y, set.x + set.y);
    }

    AudioClip RandomClip(AudioClip[] clips) {
        return clips[Random.Range(0, clips.Length)];
    }

    AudioSource RandomSource(AudioSource[] sources) {
        return sources[Random.Range(0, sources.Length)];
    }

    public void PlayWinSource() {
        StartCoroutine(WinGame());
    }

    IEnumerator WinGame() {
        winSource.clip = winClips[0];
        winSource.Play();

        yield return new WaitForSeconds(winClips[0].length);

        //winSource.clip = winClips[1];
        //winSource.Play();
    }
}
