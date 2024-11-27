using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPart : MonoBehaviour
{
    public enum DungeonPartType
    {
        Room,
        Hallway
    }

    [SerializeField]
    private LayerMask pokojeLayerMask;

    [SerializeField]
    private DungeonPartType dungeonPartType;

    [SerializeField] // Wype³nia pust¹ przestrzeñ po stworzeniu lochu
    private GameObject fillerWall;

    public List<Transform> entrypointsList;

    public new Collider collider;

    public bool CzyMaDostepnyPunktWejscia(out Transform entrypoint)
    {
        Transform resultingEntry = null;
        bool result = false;

        int totalRetries = 100;
        int retryIndex = 0;

        if (entrypointsList.Count == 1)
        {
            Transform entry = entrypointsList[0];
            if (entry.TryGetComponent<EntryPoint>(out EntryPoint res))
            {
                if (res.CzyZajety())
                {
                    result = false;
                    resultingEntry = null;
                }
                else
                {
                    result = true;
                    resultingEntry = entry;
                    res.UstawNaZajety();
                }
                entrypoint = resultingEntry;
                return result;
            }
        }

        while (resultingEntry == null && retryIndex < totalRetries)
        {
            int randomEntryIndex = Random.Range(0, entrypointsList.Count);
            Transform entry = entrypointsList[randomEntryIndex];

            if (entry.TryGetComponent<EntryPoint>(out EntryPoint entryPoint))
            {
                if (!entryPoint.CzyZajety())
                {
                    resultingEntry = entry;
                    result = true;
                    entryPoint.UstawNaZajety();
                    break;
                }
            }
            retryIndex++;
        }
        entrypoint = resultingEntry;
        return result;
    }

    public void NieuzywanyPunktWejscia(Transform entrypoint)
    {
        if (entrypoint.TryGetComponent<EntryPoint>(out EntryPoint res))
        {
            res.UstawNaZajety(false);
        }
    }

    public void UzupelnijPusteDrzwi()
    {
        entrypointsList.ForEach((entry) =>
        {
            if (entry.TryGetComponent(out EntryPoint entryPoint))
            {
                if (!entryPoint.CzyZajety())
                {
                    GameObject wall = Instantiate(fillerWall);
                    wall.transform.position = entry.transform.position;
                    wall.transform.rotation = entry.transform.rotation;
                }
            }
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
