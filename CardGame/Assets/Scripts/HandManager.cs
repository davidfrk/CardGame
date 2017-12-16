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

    public Vector3 CardsPositionsOnViewport;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCardsPositions();
    }

    public void UpdateCardsPositions()
    {   
        //Posicionando a mão de acordo com a resolução,  consiste em montar um triÂngulo de acordo com a câmera e escalar ele de modo que um dos lados seja tamanhoDaMão/2
        Vector3 centerOfCViewport = Camera.main.ViewportPointToRay(0.5f * Vector3.right).direction;
        Vector3 rightOfViewport = Camera.main.ViewportPointToRay(Vector3.right).direction;
        float norm = 1/Vector3.Dot(centerOfCViewport, rightOfViewport);
        rightOfViewport = rightOfViewport * norm;
        Vector3 centerToRight = rightOfViewport - centerOfCViewport;
        float handSize = numberOfCards * intervalBetweenCards / 2f;
        float scale = handSize / centerToRight.magnitude;
        transform.position = Camera.main.transform.position + Camera.main.ViewportPointToRay(CardsPositionsOnViewport).direction * scale;

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
