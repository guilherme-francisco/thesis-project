using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        if(ToolsPanelUI.Instance.GetNavigation() == ToolsPanelUI.Navigation.Inside) {
            GameManager.Instance.MoveGameObject(gameObject, speed);
        }       
    }
}
