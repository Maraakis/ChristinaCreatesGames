using UnityEngine;

public class TestSkill : MonoBehaviour
{
    [SerializeField] private int manaValue = 10;
    [SerializeField] private ResourceBarTracker resourceBarTracker;

    [SerializeField] private int increaseManaBy = 1000;
    private bool _buffApplied;
    
    public void ChangeMana()
    {
        bool successfulCast = resourceBarTracker.ChangeResourceByAmount(manaValue);
        
        if (successfulCast)
            Debug.Log("Cast successful");
        else
            Debug.Log("Cast failed due to lack of Mana");
    }

    public void ChangeMaxMana()
    {
        if (!_buffApplied)
        {
            resourceBarTracker.ChangeMaxAmountTo(increaseManaBy);
            _buffApplied = true;
        }
        else
        {
            resourceBarTracker.ChangeMaxAmountTo(100);
            _buffApplied = false;
        }
    }
}
