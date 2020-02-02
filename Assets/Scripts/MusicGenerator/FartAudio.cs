using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartAudio : MonoBehaviour
{

    private AudioSource fartAudioSource;
    public AudioClip[] fartAudioClips;
    public bool fartTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        fartAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator randomPlayFart(){
        fartTriggered = true;
        //Debug.Log("Starting audio fart");
        var randomInt = Random.Range(0,4);
        fartAudioSource.clip = fartAudioClips[randomInt];
        //AudioController.FadeIn(fartAudioSource, 0.5f);
        fartAudioSource.pitch = Random.Range(0.5f, 1.5f);
        fartAudioSource.Play();
        //yield return new WaitUntil(() => !fartAudioSource.isPlaying);
        yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
        //AudioController.FadeOut(fartAudioSource, 0.05f);
        if (fartTriggered) { StartCoroutine(randomPlayFart());};
    }

}
