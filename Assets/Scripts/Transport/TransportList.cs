using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportList : MonoBehaviour
{
    [System.Serializable]
    public class TransportInventory
    {
        public string TransportName;
        public GameObject TransportPrefab;
        public Transform TransportSpawn;
        [HideInInspector]
        public GameObject transport;
    }

    public List<TransportInventory> ListOfTransport= new List<TransportInventory>();
    [HideInInspector]
    public GameObject _transportMenu;
    public void Start()
    {
        _transportMenu = CanvasManager.Instance._TransportMenu;

    }
    public void SelectTransport(string name)
    {
        foreach (TransportInventory _transport in ListOfTransport)
        {
            if (_transport.TransportName == name)
            {
                if (_transport.transport) return;
                _transport.transport = Instantiate(_transport.TransportPrefab, _transport.TransportSpawn.position, Quaternion.identity);
                _transportMenu.SetActive(false);
            }
            else
            {
                if (_transport.transport) Destroy(_transport.transport);
            }
        }
    }
}
