using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip FireSound, Damaged;
    static AudioSource AudioSrc;

    // Start is called before the first frame update
    void Start()
    {
        FireSound = Resources.Load<AudioClip>("Fire");
        Damaged = Resources.Load<AudioClip>("Damaged");
        AudioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Fire":
                AudioSrc.PlayOneShot(FireSound);
                break;

            case "Damaged":
                AudioSrc.PlayOneShot(Damaged);
                break;



        }
    }
}
