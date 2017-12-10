using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HandManager : MonoBehaviour {

    static public HandManager instance;

    public float intervalBetweenCards = 2;
    public float numberOfCards = 8;
    public GameObject CardSlot;
    public List<Transform> CardsTransforms;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateCardsPositions()
    {
        float offset = (numberOfCards - 1) * intervalBetweenCards / 2f;
        foreach (Transform transform in CardsTransforms)
        {
            DestroyImmediate(transform.gameObject);
        }
        CardsTransforms.Clear();
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject slot = Instantiate(CardSlot, transform.position + transform.right * (intervalBetweenCards * i - offset), transform.rotation, this.transform);
            CardsTransforms.Add(slot.transform);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HandManager))]
public class HandEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HandManager hand = (HandManager)target;
        if (GUILayout.Button("UpdateCardsPositions"))
        {
            hand.UpdateCardsPositions();
        }
    }
}
#endif
