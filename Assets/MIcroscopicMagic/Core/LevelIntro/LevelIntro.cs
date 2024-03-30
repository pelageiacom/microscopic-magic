// Copyright takiido. All Rights Reserved.

using UnityEngine;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    public Color bgColor;
    public Color txtColor;

    public Material bgMaterial;
    public TMP_Text levelName;

    public TMP_FontAsset firstFont;
    public TMP_FontAsset secondFont;

    private float timePassed = 0.0f;
    private float startTime;
    private float timeElapsed;
    private float lerpDuration = 3f;
    private float startValue = 1.0f;
    private float endValue = 0.0f;

    private void Start()
    {
        startTime = Time.time;
        levelName.color = txtColor;
        bgMaterial.SetColor("_Background Color", bgColor);
        bgMaterial.SetFloat("_Opacity", 1.0f);
    }

    private void Update()
    {
        float deltaTime = Random.Range(0, 100) > 98 ? 0.05f : 0.5f;
        timePassed += Time.deltaTime;

        if (timePassed > deltaTime)
        {
            levelName.font = levelName.font == firstFont ? secondFont : firstFont;
            timePassed = 0f;
        }

        if (Time.time > startTime + 5f && timeElapsed < lerpDuration)
        {
            float opacityVal = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            bgMaterial.SetFloat("_Opacity", opacityVal);
            levelName.alpha = opacityVal;
        }
    }
}