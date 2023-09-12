using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[SelectionBase]
public class ResourceBarTracker : MonoBehaviour
{
    [Header("Core Settings")]
    [SerializeField, Required] private Image bar;
    [SerializeField] private int resourceCurrent = 100;
    [SerializeField] private int resourceMax = 100;
    [SerializeField] private int resourceAbsoluteMax = 1000;
    [Space]
    [SerializeField] private bool overkillPossible;
    [Space]
    [SerializeField] private ShapeType shapeOfBar;

    public enum ShapeType
    {
        [InspectorName("Rectangle (Horizontal)")]
        RectangleHorizontal,
        [InspectorName("Rectangle (Vertical)")]
        RectangleVertical,
        [InspectorName("Circle")] 
        Circle,
        Arc
    }
    
    [Header("Arc Settings"), ShowIf("shapeOfBar", ShapeType.Arc)]
    [SerializeField, Range(0, 360)] private int endDegreeValue = 360;
    
    [Header("Animation Speed")]
    [SerializeField, EnumToggleButtons] private AnimationSpeed animationSpeed = AnimationSpeed.Medium;
    private float _animationTime = 0.25f;
    private Coroutine _fillRoutine;

    public enum AnimationSpeed
    {
        [InspectorName("0.125s")] Fast,
        [InspectorName("0.25s")] Medium,
        [InspectorName("0.5s")] Slow,
        None
    }
    
    [Header("Text Settings")]
    [SerializeField] private DisplayType howToDisplayValueText = DisplayType.Percentage;
    [SerializeField, Required] private TMP_Text resourceValueTextField;
    
    public enum DisplayType
    {
        [InspectorName("Long (50|100)")]
        LongValue,
        [InspectorName("Short (50)")]
        ShortValue,
        [InspectorName("Percent (85%)")]
        Percentage,
        None
    }
    
    [Header("Gradient Settings")] 
    [SerializeField] private bool useGradient;
    [SerializeField, ShowIf("useGradient")] private Gradient barGradient;
    
    [Header("Events")]
    [SerializeField] private UnityEvent barIsFilledUp;
    private float _previousFillAmount;
    
    [Header("Test mode")] 
    [SerializeField] private bool enableTesting;
    
    
    private void OnValidate()
    {
        ConfigureBarShapeAndProperties();
    }
    
    private void Start()
    {
        TriggerFillAnimation();
    }
    
    private void ConfigureBarShapeAndProperties()
    {
        switch (shapeOfBar)
        {
            case ShapeType.RectangleHorizontal:
                bar.fillMethod = Image.FillMethod.Horizontal;
                break;
            case ShapeType.RectangleVertical:
                bar.fillMethod = Image.FillMethod.Vertical;
                break;
            case ShapeType.Circle:
            case ShapeType.Arc:
                bar.fillMethod = Image.FillMethod.Radial360;
                //bar.fillOrigin = (int)Image.Origin360.Top;
                break;
        }

        if (!useGradient)
            bar.color = Color.white;
        
        switch (animationSpeed)
        {
            case AnimationSpeed.Fast:
                _animationTime = 0.125f;
                break;
            case AnimationSpeed.Medium:
                _animationTime = 0.25f;
                break;
            case AnimationSpeed.Slow:
                _animationTime = 0.5f;
                break;
            case AnimationSpeed.None:
                _animationTime = 0;
                break;
        }

        UpdateBarAndResourceText();
    }

    private void UpdateBarAndResourceText()
    {
        if (resourceMax <= 0)
        {
            bar.fillAmount = 0;
            SetCurrentResourceValueText();
            return;
        }
        
        float fillAmount;

        if (shapeOfBar == ShapeType.Arc)
            fillAmount = CalculateCircularFillAmount();
        else
            fillAmount = (float)resourceCurrent / resourceMax;
        
        bar.fillAmount = fillAmount;
        SetCurrentResourceValueText();
    }
    
    private float CalculateCircularFillAmount()
    {
        float fraction = (float)resourceCurrent / resourceMax;
        float fillRange = endDegreeValue / 360f;

        return fillRange * fraction;
    }
    
    private void SetCurrentResourceValueText()
    {
        switch (howToDisplayValueText)
        {
            case DisplayType.LongValue:
                resourceValueTextField.SetText($"{resourceCurrent}/{resourceMax}");
                break;
            case DisplayType.ShortValue:
                resourceValueTextField.SetText($"{resourceCurrent}");
                break;
            case DisplayType.Percentage:
                float percentage = ((float)resourceCurrent / resourceMax) * 100;
                resourceValueTextField.SetText($"{Mathf.RoundToInt(percentage)} %");
                break;
            case DisplayType.None:
                resourceValueTextField.SetText(string.Empty);
                break;
        }
    }

    [Title("Testing Area"), ShowIf("enableTesting"), Button]
    public void ChangeResourceByAmountTest(int amount)
    {
        ChangeResourceByAmount(amount);
    }
    public bool ChangeResourceByAmount(int amount)
    {
        if (!overkillPossible && resourceCurrent + amount < 0)
            return false;
        
        resourceCurrent += amount;
        resourceCurrent = Mathf.Clamp(resourceCurrent, 0, resourceMax);
        
        TriggerFillAnimation();
        
        return true;
    }
    
    private void TriggerFillAnimation()
    {
        float targetFill = CalculateTargetFill();

        if (Mathf.Approximately(bar.fillAmount, targetFill))
            return;

        if (_fillRoutine != null) 
            StopCoroutine(_fillRoutine);

        _fillRoutine = StartCoroutine(SmoothlyTransitionToNewValue(targetFill));
        SetCurrentResourceValueText();
    }
    
    private float CalculateTargetFill()
    {
        if (shapeOfBar == ShapeType.Arc)
            return CalculateCircularFillAmount();

        return (float)resourceCurrent / resourceMax;
    }
    
    private IEnumerator SmoothlyTransitionToNewValue(float targetFill)
    {
        float originalFill = bar.fillAmount;
        float elapsedTime = 0.0f;

        while (elapsedTime < _animationTime)
        {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / _animationTime;
            bar.fillAmount = Mathf.Lerp(originalFill, targetFill, time);
            
            UseGradient();
            
            yield return null;
        }

        bar.fillAmount = targetFill;
        
        HandleEvent();
        _previousFillAmount = bar.fillAmount;
    }
    
    private void UseGradient()
    {
        if (!useGradient)
            return;

        if (shapeOfBar == ShapeType.Arc)
        {
            float fillRange = bar.fillAmount / (endDegreeValue / 360f);
            bar.color = barGradient.Evaluate(fillRange);
            return;
        }

        bar.color = barGradient.Evaluate(bar.fillAmount);
    }
    
    
    private void HandleEvent()
    {
        if (_previousFillAmount >= 1)
            return;
        
        if (bar.fillAmount >= 1)
            barIsFilledUp?.Invoke();
    }
    
    [ShowIf("enableTesting"), Button]
    public void ChangeMaxAmountTo(int newMaxAmount)
    {
        newMaxAmount = Mathf.Clamp(newMaxAmount, 0, resourceAbsoluteMax);
        
        resourceMax = newMaxAmount;
        resourceCurrent = Mathf.Clamp(resourceCurrent, 0, resourceMax);
        
        TriggerFillAnimation();
    }
    
    [ShowIf("enableTesting"), Button]
    public void Setup(int resourceCurrent, int resourceMax, int resourceAbsoluteMax, bool overkillPossible, ShapeType shapeOfBar,
                        AnimationSpeed animationSpeed, DisplayType howToDisplayValueText, bool useGradient, Gradient barGradient)
    {
        this.resourceCurrent = resourceCurrent;
        this.resourceMax = resourceMax;
        this.resourceAbsoluteMax = resourceAbsoluteMax;
        this.overkillPossible = overkillPossible;
        this.shapeOfBar = shapeOfBar;
        
        this.animationSpeed = animationSpeed;
        
        this.howToDisplayValueText = howToDisplayValueText;
        
        this.useGradient = useGradient;
        this.barGradient = barGradient;

        
        ConfigureBarShapeAndProperties();
        TriggerFillAnimation();
    }

    [ShowIf("enableTesting"), Button]
    private void TestEvent()
    {
        barIsFilledUp?.Invoke();
    }
}