using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationManager : MonoBehaviour
{
    private EventSystem eventSystem;

    private void Start()
    {
        // Get the EventSystem component
        eventSystem = GetComponent<EventSystem>();
        GameManager.Instance.onRightHandMove += GameManager_onRightHandMove;
    }

    private void GameManager_onRightHandMove(object sender, GameManager.OnRightHandMoveArgs e)
    {
        AxisEventData data = new AxisEventData(EventSystem.current);

        data.moveDir = GetMoveDirection(e.moveDirection);

        data.selectedObject = EventSystem.current.currentSelectedGameObject;

        ExecuteEvents.Execute(data.selectedObject, data, ExecuteEvents.moveHandler); ;
    }

    MoveDirection GetMoveDirection(Vector2 inputVector)
    {
        if (inputVector.magnitude < 0.1f)
        {
            return MoveDirection.None;
        }

        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            if (inputVector.x > 0)
            {
                return MoveDirection.Left;
            }
            else
            {
                return MoveDirection.Right;
            }
        }
        else
        {
            if (inputVector.y > 0)
            {
                return MoveDirection.Up;
            }
            else
            {
                return MoveDirection.Down;
            }
        }
    }
}
