using UnityEngine;

public class PlayerMovementStop : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private QuestionUI questionUI;

    public void Update()
    {
        if (this.questionUI.IsVisible())
        {
            this.movement.GetRigidBody().velocity = new Vector2(0.0f, 0.0f);
        }
    }
}
