using Core;
using TMPro;
using UnityEngine;

namespace Client
{
    public class BoardScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text stepsText;
        [SerializeField] private TMP_Text cellsText;

        [SerializeField] private BoardCustomerWidget customer;
        [SerializeField] private BoardPlayerWidget player; 

        private ICustomizableBoard board;

        public void Display(params object[] parameters)
        {
            board = parameters[1] as ICustomizableBoard;
            board.OnStepOn += OnStepOn;

            customer.Display(parameters);
            player.Display(parameters);
        }

        private void OnStepOn(int step)
        {
            stepsText.text = $"Step: {step}";
            cellsText.text = $"Cells: {board.Cells}";
        }

        private void OnDisable()
        {
            board.OnStepOn -= OnStepOn;
        }
    }
}