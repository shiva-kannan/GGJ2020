using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrack : MonoBehaviour
{
    public GameObject grid;
    public GameObject player1;
    public GameObject player2;

    public AudioClip[] baseTrackClipList;
    public AudioSource[] baseTrack;
    bool baseTrackOn=false;
    bool trackTriggered = false;

    public AudioClip[] bassTracks;
    public AudioSource drumTrack;
    public AudioSource bassAudioSource;

    int lastFrameSongInBeats;
    //beats per minute of a song
    public float bpm;
    //the current position of the song (in seconds)
    float songPosition;

    //the current position of the song (in beats)
    public int songPositionInBeats;

    //the duration of a beat
    float secPerBeat;
    public float playOneShotVolume=0.15f;

    //how much time (in seconds) has passed since the song started
    float dspSongTime;

    private TileMap tileMapObject;
    private Vector2 currentGridPositionPlayer1;
    private Vector2 currentGridPositionPlayer2;

    public bool stopRandomGeneration = true;

    // Start is called before the first frame update
    void Start()
    {
        tileMapObject = grid.GetComponent<TileMap>();

        //calculate how many seconds is one beat
        //we will see the declaration of bpm later
        secPerBeat = 60f / bpm;
    
        //record the time when the song starts
        dspSongTime = (float) AudioSettings.dspTime;

        //start the song
        //GetComponent<AudioSource>().Play();
        beginSoundTrack();

    }

    // Update is called once per frame
    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        //determine how many beats since the song started
        songPositionInBeats = (int)(songPosition / secPerBeat);

        if(!stopRandomGeneration){
            //Start playing the base track when the game starts
            if (baseTrackOn){
                if(songPositionInBeats % 4 == 0 && songPositionInBeats !=lastFrameSongInBeats){
                    //baseTrack.PlayOneShot(baseTrackClip, 0.4f);
                    playOneShot(baseTrackClipList[0], baseTrack[0], 0);
                    
                }
                if((songPositionInBeats + 2) % 4 == 0 && songPositionInBeats !=lastFrameSongInBeats){
                    //baseTrack.PlayOneShot(baseTrackClip, 0.4f);
                    playOneShot(baseTrackClipList[0], baseTrack[0], 2);
                }
                if((songPositionInBeats + 1) % 4 == 0 && songPositionInBeats !=lastFrameSongInBeats){
                    //baseTrack.PlayOneShot(baseTrackClip, 0.4f);
                    playOneShot(baseTrackClipList[0], baseTrack[0], 1);
                }
                // For the bass track
                if((songPositionInBeats) % 8 == 0 && songPositionInBeats !=lastFrameSongInBeats){
                    //baseTrack.PlayOneShot(baseTrackClip, 0.4f);
                    playOneShot(baseTrackClipList[0], baseTrack[0], 8);
                }
            }
        }
        

        lastFrameSongInBeats = songPositionInBeats;

        //triggerPositionBasedMusic();
    }

    void beginSoundTrack(){ 
        baseTrackOn = true;
        drumTrack.Play();
        //bassAudioSource.clip = baseTrackClipList[0];
        //bassAudioSource.Play();
    }

    void playOneShot(AudioClip a, AudioSource s, int beatPosition){
        
        // For every 0th beat
        if(beatPosition == 0) {
            s.PlayOneShot(a, playOneShotVolume);
        } // Always play the base track
        else if (tileMapObject.GetTileUnderPoint(player1.transform.position) != null && 
        tileMapObject.GetTileUnderPoint(player2.transform.position) != null){
            currentGridPositionPlayer1 = tileMapObject.GetTileUnderPoint(player1.transform.position).pGridPos;
            currentGridPositionPlayer2 = tileMapObject.GetTileUnderPoint(player2.transform.position).pGridPos;
            // For every 2nd beat
            // If position is even
            if (beatPosition == 2){
                if((int)currentGridPositionPlayer2[0] % 2 == 0){
                    baseTrack[(int)currentGridPositionPlayer2[0]].PlayOneShot(baseTrackClipList[(int)currentGridPositionPlayer2[0]], playOneShotVolume);
                }
                if(currentGridPositionPlayer2[1] % 2 == 0){
                    baseTrack[(int)currentGridPositionPlayer2[1]].PlayOneShot(baseTrackClipList[(int)currentGridPositionPlayer2[1]], playOneShotVolume);
                }
            }
            if (beatPosition == 1){
                if((int)currentGridPositionPlayer1[0] % 2 == 1){
                    baseTrack[(int)currentGridPositionPlayer1[0]].PlayOneShot(baseTrackClipList[(int)currentGridPositionPlayer1[0]], playOneShotVolume);
                }
                if(currentGridPositionPlayer1[1] % 2 == 1){
                    baseTrack[(int)currentGridPositionPlayer1[1]].PlayOneShot(baseTrackClipList[(int)currentGridPositionPlayer1[1]], playOneShotVolume);
                }
            }
            // if(beatPosition == 8){
                
            //     if(currentGridPositionPlayer1[0] < (tileMapObject._mapSize[0] / 2)){// && 
            //     //currentGridPositionPlayer2[0] < (tileMapObject._mapSize[0] / 2)){
            //         bassAudioSource.clip = bassTracks[0];
            //     }
            //     else{
            //         bassAudioSource.clip = bassTracks[1];
            //     }
            //     if (!bassAudioSource.isPlaying){
            //         bassAudioSource.Play();
            //     }
            // }
            
        }
        
        
    }

    // void triggerPositionBasedMusic(){
    //     Debug.Log(tileMapObject.GetTileUnderPoint(player.transform.position));
    //     if (tileMapObject.GetTileUnderPoint(player.transform.position) != null){
    //         currentGridPosition = tileMapObject.GetTileUnderPoint(player.transform.position).pGridPos;
    //     }        
    // }
    // public void stopRandomSoundTrackGeneration(){
    //     stopRandomGeneration = true;
    // }

    // public void startRandomGeneration(){
    //     stopRandomGeneration = false;
    // }
}
