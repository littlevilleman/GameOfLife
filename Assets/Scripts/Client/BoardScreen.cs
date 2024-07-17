using Core;
using System.Collections.Generic;
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

        private void OnStepOn(HashSet<Vector2Int> cells, int step)
        {
            stepsText.text = $"Step: {step}";
            cellsText.text = $"Cells: {cells.Count}";
        }

        private void OnDisable()
        {
            board.OnStepOn -= OnStepOn;
        }
    }
}