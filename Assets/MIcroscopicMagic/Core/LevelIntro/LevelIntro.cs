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
    
    private float _timePassed = 0f;
    private static readonly int Opacity = Shader.PropertyToID("_Opacity");
    private static readonly int Property = Shader.PropertyToID("Background Color");

    private float _startTime;

    // Anim
    private float _timeElapsed;
    private float _lerpDuration = 3;
    private float _startValue=1.0f;
    private float _endValue=0.0f;
    private float _opacityVal;

    private void Start()
    {
        _startTime = Time.time;
        
        levelName.color = txtColor;
        
        bgMaterial.SetColor(Property, bgColor);
        bgMaterial.SetFloat(Opacity, 1.0f);
    }
    
    private void Update()
    {
        int randomFact = Random.Range(0, 100);
        float deltaTime = randomFact > 98 ? 0.05f : 0.5f; 
        
        _timePassed += Time.deltaTime;
        if(_timePassed > deltaTime)
        {
            levelName.font = levelName.font == firstFont ? secondFont : firstFont;
            _timePassed = 0f;
        }

        if (Time.time > _startTime + 5f && _timeElapsed < _lerpDuration)
        {
            _opacityVal = Mathf.Lerp(_startValue, _endValue, _timeElapsed / _lerpDuration);
            _timeElapsed += Time.deltaTime;
            bgMaterial.SetFloat(Opacity, _opacityVal);
            levelName.alpha = _opacityVal;
        }
    }
}
