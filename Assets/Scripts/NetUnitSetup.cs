using UnityEngine;
using UnityEngine.Networking;


public class NetUnitSetup : NetworkBehaviour
{
    [SerializeField] private MonoBehaviour[] _disableBehaviours;

    private void Awake()
    {
        // проверка, кому принадлежит объект, если не нам, то выключаем все поведенческие скрипты
        for (int i = 0; i < _disableBehaviours.Length; i++)
        {
            _disableBehaviours[i].enabled = false;
        }
    }

    // проверка, кому принадлежит объект, если нам, то включаем все поведенческие скрипты
    // данную проверку можно не проводить.
    public override void OnStartAuthority()
    {
        for (int i = 0; i < _disableBehaviours.Length; i++)
        {
            _disableBehaviours[i].enabled = true;
        }
    }
}