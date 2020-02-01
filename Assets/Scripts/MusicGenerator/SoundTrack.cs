using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrack : MonoBehaviour
{
    public GameObject grid;
    public GameObject player;

    public AudioClip[] baseTrackClipList;
    public AudioSource[] baseTrack;
    bool baseTrackOn=false;
    bool trackTriggered = false;

    int lastFrameSongInBeats;
    //beats per minute of a song
    public float bpm;
    //the current position of the song (in seconds)
    float songPosition;

    //the current position of the song (in beats)
    public int songPositionInBeats;

    //the duration of a beat
    float secPerBeat;

    //how much time (in seconds) has passed since the song started
    float dspSongTime;

    private TileMap tileMapObject;
    private Vector2 currentGridPosition;

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
        }

        lastFrameSongInBeats = songPositionInBeats;

        triggerPositionBasedMusic();
    }

    void beginSoundTrack(){ 
        baseTrackOn = true;
    }

    void playOneShot(AudioClip a, AudioSource s, int beatPosition){
        
        // For every 0th beat
        if(beatPosition == 0) {s.PlayOneShot(a, 0.2f);} // Always play the base track
        else if (tileMapObject.GetTileUnderPoint(player.transform.position) != null){
            currentGridPosition = tileMapObject.GetTileUnderPoint(player.transform.position).pGridPos;
            // For every 2nd beat
            // If position is even
            if (beatPosition == 2){
                if((int)currentGridPosition[0] % 2 == 0){
                    baseTrack[(int)currentGridPosition[0]].PlayOneShot(baseTrackClipList[(int)currentGridPosition[0]], 0.2f);
                }
                if(currentGridPosition[1] % 2 == 0){
                    baseTrack[(int)currentGridPosition[1]].PlayOneShot(baseTrackClipList[(int)currentGridPosition[1]], 0.2f);
                }
            }
            if (beatPosition == 1){
                if((int)currentGridPosition[0] % 2 == 1){
                    baseTrack[(int)currentGridPosition[0]].PlayOneShot(baseTrackClipList[(int)currentGridPosition[0]], 0.2f);
                }
                if(currentGridPosition[1] % 2 == 1){
                    baseTrack[(int)currentGridPosition[1]].PlayOneShot(baseTrackClipList[(int)currentGridPosition[1]], 0.2f);
                }
            }
            
        }
        
        
    }

    void triggerPositionBasedMusic(){
        Debug.Log(tileMapObject.GetTileUnderPoint(player.transform.position));
        if (tileMapObject.GetTileUnderPoint(player.transform.position) != null){
            currentGridPosition = tileMapObject.GetTileUnderPoint(player.transform.position).pGridPos;
        }        
    }
}
