using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<T> : MonoBehaviour {
    [Tooltip("Event to register with.")]
    [SerializeField]
    private GameEvent<T> gameEvent;

    [Tooltip("Response to invoke when Event is raised.")]
    [SerializeField]
    private UnityEvent<T> response;

    private void OnEnable() {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable() {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T value) {
        response?.Invoke(value);
    }
}

public class EmptyGameEventListener : GameEventListener<Empty> { }
