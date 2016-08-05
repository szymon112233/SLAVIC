using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

    public ParticleSystem bloodSplatter;
    public ParticleSystem sandStepping;

    public AudioClip deathClip;
    public AudioClip inPainClip;

    AudioSource audioSource;

    void Awake ()
    {
         audioSource = GetComponent<AudioSource>();
    }
	

	void Update ()
    {
        
	}

    public void PlayBloodSplatter()
    {
        PlayParticleSystem(bloodSplatter);
    }

    public void PlayBloodSplatterLooped()
    {
        PlayParticleSystemLooped(bloodSplatter);
    }

    public void StopBloodSplatter()
    {
        StopParticleSystem(bloodSplatter);
    }

    public void PlaySandStepping()
    {
        PlayParticleSystem(sandStepping);
    }

    public void PlaySandSteppingLooped()
    {
        PlayParticleSystemLooped(sandStepping);
    }

    public void StopSandStepping()
    {
        StopParticleSystem(sandStepping);
    }

    public void PlayInPainClip()
    {
        PlaySoundClip(inPainClip);
    }

    public void PlayDeathClip()
    {
        PlaySoundClip(deathClip);
    }
    
    private void PlaySoundClip(AudioClip clip)
    {        
        if(audioSource != null)
        {
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = clip;

            audioSource.Play();
        }
    }

    private void PlayParticleSystem(ParticleSystem particle)
    {
        if(!particle.isPlaying)
        {
            particle.Play(true);
        }
    }

    private void PlayParticleSystemLooped(ParticleSystem particle)
    {
        particle.loop = true;
        particle.Stop(true);

        PlayParticleSystem(particle);
    }

    private void StopParticleSystem(ParticleSystem particle)
    {
        particle.loop = false;
        particle.Stop(true);
    }
}
