﻿using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class NetUnitSetup : NetworkBehaviour
{
    [SerializeField] private MonoBehaviour[] _disableBehaviours = null;

    private void Awake()
    {
        // проверка, кому принадлежит объект, если не нам, то выключаем все поведенческие скрипты
        if (!hasAuthority)
        {
            for (int i = 0; i < _disableBehaviours.Length; i++)
            {
                _disableBehaviours[i].enabled = false;
            }
        }
    }

    // проверка, кому принадлежит объект, если нам, то включаем все поведенческие скрипты
    // данную проверку можно не проводить.
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        for (int i = 0; i < _disableBehaviours.Length; i++)
        {
            _disableBehaviours[i].enabled = true;
        }
    }
}