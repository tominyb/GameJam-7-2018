﻿using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SpriteRenderer))]
public class NetworkItem : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.ItemContainer m_items = null;
    [HideInInspector] [SyncVar] public int ItemIndex = -1;
    public int HealthRestoreAmount { get { return m_items.Items[ItemIndex].HealthRestoreAmount; } }

    public override void OnStartClient()
    {
        base.OnStartClient();
        GetComponent<SpriteRenderer>().sprite = m_items.Items[ItemIndex].Sprite;
    }
}
