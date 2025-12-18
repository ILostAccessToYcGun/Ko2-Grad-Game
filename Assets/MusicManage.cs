using UnityEngine;
using UnityEngine.Audio;

public class MusicManage : MonoBehaviour
{
    public static MusicManage instance;
    public AudioClip[] Music;
    public AudioClip[] Main;
    public AudioClip[] AOT;
    public AudioClip[] OSU;
    public AudioClip[] BASS;
    public AudioSource player;

    private void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        if ((player.clip != null && !player.isPlaying) || player.clip == null)
        {
            // The audio has stopped. You can now perform an action.
            Debug.Log("Music stopped playing. Playing next track.");

            // Example action: load a new clip and play it
            bool isDupe = true;
            AudioClip lastClip = player.clip;
            int rand = 0;
            while (isDupe)
            {
                rand = Random.Range(0, Music.Length);
                if (Music[rand] != lastClip)
                {
                    isDupe = false;
                    player.clip = Music[rand];
                    player.Play();
                }
                
            }
            
        }
    }

    public void SelectAOT()
    {
        Music = AOT;
    }

    public void SelectOSU()
    {
        Music = OSU;
    }

    public void SelectBASS()
    {
        Music = BASS;
    }


}
