using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

    public ParticleSystem bloodSplatter;
    public ParticleSystem sandStepping;

    public AudioClip deathClip;
    public AudioClip hurtClip;
    public AudioClip healClip;

    private AudioSource audioSource;

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

    public void PlayHealClip()
    {
        PlaySoundClip(healClip);
    }

    public void PlayHurtClip()
    {
        PlaySoundClip(hurtClip);
    }

    public void PlayDeathClip()
    {
        PlaySoundClip(deathClip);
    }
    
    private void PlaySoundClip(AudioClip clip)
    {        
        if(audioSource != null && clip != null)
        {
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            //audioSource.clip = clip;

            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayParticleSystem(ParticleSystem particle)
    {
        //if(particle == null) Debug.Log("BloodSplatter");

        //particle.Play();

        

        ParticleSystem.EmissionModule module = particle.emission;

        if (!particle.isPlaying)
        {
            particle.Simulate(0.0f, true, true);
            module.enabled = true;
            particle.Play();
        }
        //else
        {

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
