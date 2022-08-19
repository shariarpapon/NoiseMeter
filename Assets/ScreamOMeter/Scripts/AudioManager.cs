using System.Collections;
using UnityEngine;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public UnityEngine.UI.Image bar;
    public AudioSource meterSource;
    public AudioSource dingSource;
    public AudioSource bellSource;
    public AnimationCurve pitchCurve;
    public AnimationCurve volumeCurve;
    public TextMeshProUGUI indexMessage;
    public float fadeSpeed = 1;

    public AudioClip[] dingClips;
    private int dingIndex = 0;

    private bool dingPlayed = false;
    private bool canFade = true;
    private Coroutine messageDisplayer;

    private void Awake() 
    {
        dingSource.clip = dingClips[dingIndex];
    }

    private void Update() 
    {

        if (bar.fillAmount < 1 && bar.fillAmount > 0)
        {
            dingPlayed = false;
            meterSource.pitch = pitchCurve.Evaluate(bar.fillAmount);

            if (canFade == false) StartCoroutine(FadeInVolume());
            else meterSource.volume = bar.fillAmount;
        }
        else if (bar.fillAmount >= 1)
        {
            if (bellSource.isPlaying == false) bellSource.Play();

            if (canFade == true) StartCoroutine(FadeOutVolume());

            if (dingPlayed == false)
            {
                dingSource.Play();
                dingPlayed = true;
            }
        }
        else if (bar.fillAmount <= 0) meterSource.volume = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dingIndex++;
            if (dingIndex >= dingClips.Length) dingIndex = 0;
            else if (dingIndex < 0) dingIndex = dingClips.Length - 1;
            UpdateDingClip();
        }
    }


    private void UpdateDingClip() 
    {
        dingSource.clip = dingClips[dingIndex];
        indexMessage.text = "Audio index changed to " + dingIndex;
        indexMessage.transform.parent.gameObject.SetActive(true);

        if (messageDisplayer != null) StopCoroutine(messageDisplayer);
        messageDisplayer = StartCoroutine(HideMessage());
    }

    private IEnumerator HideMessage() 
    {
        yield return new WaitForSeconds(3.0f);
        indexMessage.transform.parent.gameObject.SetActive(false);
        messageDisplayer = null;
    }

    private IEnumerator FadeOutVolume() 
    {
        canFade = false;
        float t = 0;
        while (t <= 1) 
        {
            t += Time.deltaTime * fadeSpeed;
            float v = 1 - volumeCurve.Evaluate(t);
            meterSource.volume = v;
            yield return null;
        }
    }

    private IEnumerator FadeInVolume()
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime * fadeSpeed;
            float v = volumeCurve.Evaluate(t);
            meterSource.volume = v;
            yield return null;
        }
        canFade = true;
    }
}
