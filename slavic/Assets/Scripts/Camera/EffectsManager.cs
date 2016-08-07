using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class EffectsManager : MonoBehaviour
{
    public ScreenOverlay pTransitionPart1;
    public ScreenOverlay pTransitionPart2;

    //Transition
    public float fTransitionLength = 1;
    private float fTransitonMaxIntensity;
    private float fTransitionDelta;
    private bool bTransitionDirection = true;
    private float m_fDelay;

    //Screen Shake
    private Camera pCamera;
    private bool bScreenShake = false;
    private float fScreenShakeTime;
    private float fScreenShakeMagn;
    private float fScreenShakeDampen;

    public bool IsTransitionEnabled()
    { return pTransitionPart1 && pTransitionPart2 && pTransitionPart1.enabled && pTransitionPart2.enabled; }

    public bool IsScreenShakeEnabled() { return bScreenShake; }
    public void SetScreenShake(bool bEnable, float fTime = 0, float fMagnitude = 0, float fDampen = 0) // Magnitude [0:1], Dampen [0:1]
    {
        bScreenShake = bEnable;
        if (bEnable)
        {
            fScreenShakeMagn = fMagnitude;
            fScreenShakeTime = fTime;
            fScreenShakeDampen = 1 - fDampen;
        }
    }

    public void SetTransition(bool bEnable, float fDelay = 0)//true zeby zaciemnic; false ustawia zaciemnienie na 100% i powoli schodzi do 0
    {
        m_fDelay = fDelay;
        bTransitionDirection = bEnable;
        fTransitionDelta = fTransitonMaxIntensity / fTransitionLength;

        if (bEnable)
        {
            pTransitionPart1.enabled = true;
            pTransitionPart2.enabled = true;
            pTransitionPart1.intensity = pTransitionPart2.intensity = 0;
        }
        else
        {
            pTransitionPart1.intensity = pTransitionPart2.intensity = fTransitonMaxIntensity;
        }
    }

	void Start ()
    {
        pCamera = GetComponent<Camera>();
        //Screen transition effect
        if (IsTransitionEnabled())
        {
            fTransitonMaxIntensity = pTransitionPart1.intensity;
            SetTransition(false);
        }

        //SetScreenShake(true, 5, 0.5f, 0.05f);
    }
	

	void Update ()
    {
        //Transition
        if (pTransitionPart1 && pTransitionPart2)
        {
            //Zaciemnianie
            if (bTransitionDirection && pTransitionPart1.intensity != fTransitonMaxIntensity)
            {
                if (m_fDelay > 0)
                    m_fDelay -= Time.deltaTime;
                else
                {
                    float fDeltaIntensity = Time.deltaTime * fTransitionDelta;
                    pTransitionPart1.intensity = pTransitionPart2.intensity = pTransitionPart1.intensity + fDeltaIntensity;

                    if (pTransitionPart1.intensity > fTransitonMaxIntensity)
                    {
                        pTransitionPart1.intensity = pTransitionPart2.intensity = fTransitonMaxIntensity;
                    }
                }
            }
            //Rozjasnianie
            else if (!bTransitionDirection && pTransitionPart1.intensity != 0)
            {
                float fDeltaIntensity = Time.deltaTime * fTransitionDelta;
                pTransitionPart1.intensity = pTransitionPart2.intensity = pTransitionPart1.intensity - fDeltaIntensity;

                if (pTransitionPart1.intensity < 0)
                {
                    pTransitionPart1.intensity = pTransitionPart2.intensity = 0;
                    pTransitionPart1.enabled = false;
                    pTransitionPart2.enabled = false;
                }
            }
        }

        //ScreenShake
        if (bScreenShake && fScreenShakeTime != 0)
        {
            pCamera.transform.localPosition = Random.insideUnitSphere * fScreenShakeMagn;
            fScreenShakeTime -= Time.deltaTime;

            fScreenShakeMagn *= fScreenShakeDampen;

            if (fScreenShakeTime < 0)
            {
                bScreenShake = false;
                pCamera.transform.localPosition = new Vector3(0, 0, 0);
                fScreenShakeTime = 0;
            }
        }
    }
}
