// Copyright takiido. All Rights Reserved.

using UnityEngine;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    public Color bgColor;
    public Color txtColor;

    public Material bgMaterial;
    public TMP_Text[] levelNames;

    public TMP_FontAsset firstFont;
    public TMP_FontAsset altFirstFont;
    public TMP_FontAsset secondFont;
    public TMP_FontAsset altSecondFont;

    public float[] maxOpacities;

    private float timePassed = 0.0f;
    private float startTime;
    private float timeElapsed;
    private float lerpDuration = 3f;

    private float _opacityVal = 1.0f;

    private void Start()
    {
        startTime = Time.time;

        foreach (var levelName in levelNames)
        {
            levelName.color = txtColor;
        }
        
        levelNames[0].alpha = maxOpacities[0];
        levelNames[1].alpha = maxOpacities[1];
        levelNames[2].alpha = maxOpacities[2];
        levelNames[3].alpha = maxOpacities[3];
        levelNames[4].alpha = maxOpacities[4];
        
        bgMaterial.SetColor("_Background Color", bgColor);
        bgMaterial.SetFloat("_Opacity", _opacityVal);
    }

    private void Update()
    {
        float deltaTime = Random.Range(0, 100) > 98 ? 0.05f : 0.5f;
        timePassed += Time.deltaTime;

        if (timePassed > deltaTime)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    levelNames[i].font = levelNames[i].font == firstFont ? secondFont : firstFont;
                }
                else
                {
                    levelNames[i].font = levelNames[i].font == altFirstFont ? altSecondFont : altFirstFont;
                }
            }
            timePassed = 0f;
        }

        if (Time.time > startTime + 5f && timeElapsed < lerpDuration)
        {
            for (int i = 0; i < 5; i++)
            {
                _opacityVal = Mathf.Lerp(maxOpacities[i], 0.0f, timeElapsed / lerpDuration);
                levelNames[i].alpha = _opacityVal;
            }
            timeElapsed += Time.deltaTime;
            bgMaterial.SetFloat("_Opacity", _opacityVal);
        }

        if (_opacityVal < 0.01f)
        {
            foreach (var levelName in levelNames)
            {
                Destroy(levelName.gameObject);
            }
            Destroy(gameObject);
        } 
    }
}