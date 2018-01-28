using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VoiceOVer : MonoBehaviour {

    AudioSource audioSource;
    public Image screen;
    public Sprite gameLogo;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayVoiceOver());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PlayVoiceOver() {

        yield return new WaitForSeconds(4f);
        screen.sprite = gameLogo;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length + 3);
        SceneManager.LoadScene(1);
    }
}
