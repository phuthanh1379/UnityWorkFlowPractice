using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have a boolean argument.
/// Example: An event to toggle a UI interface.
/// </summary>
[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
