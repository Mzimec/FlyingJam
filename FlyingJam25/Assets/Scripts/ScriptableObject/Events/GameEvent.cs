using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public readonly struct Empty { };

public class GameEvent<T> : ScriptableObject
{
    private readonly List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();
    
    public void Raise(T value) {
        for (int i = listeners.Count - 1; i >= 0; i--) {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(GameEventListener<T> listener) {
        if (!listeners.Contains(listener)) {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener<T> listener) {
        if (listeners.Contains(listener)) {
            listeners.Remove(listener);
        }
    }
}

