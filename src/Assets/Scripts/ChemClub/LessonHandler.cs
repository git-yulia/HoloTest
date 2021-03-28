using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HoloTest
{
    public class LessonHandler : MonoBehaviour
    {
        public GameObject goalDisplayBoard;
        public Texture goal1, goal2, goal3;
        public GameObject burner;
        private Texture[] goalImages;
        Renderer m_Renderer;
        private int currentGoalNumber;
        private FireHandler fireHandlerScript; 

        private void Start()
        {
            goalImages[0] = goal1;
            goalImages[1] = goal2;
            goalImages[2] = goal3;

            m_Renderer = goalDisplayBoard.GetComponent<Renderer>();
            m_Renderer.material.SetTexture("_MainTex", goal1);
            currentGoalNumber = 0;

            fireHandlerScript = burner.GetComponent<FireHandler>();
        }

        bool isPuzzleSolved()
        {
            bool puzzleStatus = false;
            int fireType = 0;
            float fireHeight = 0.0f;

            fireHandlerScript = burner.GetComponent<FireHandler>();
            fireHandlerScript.GetFireState(out fireType, out fireHeight);

            if (fireType != currentGoalNumber)
            {
                puzzleStatus = false; 
            }
            else
            {
                puzzleStatus = true;
            }

            return puzzleStatus;
        }

        void SetUpNewPuzzle()
        {
            int randomGoal = Random.Range(0, 2);
            currentGoalNumber = randomGoal;
            m_Renderer = goalDisplayBoard.GetComponent<Renderer>();
            m_Renderer.material.SetTexture("_MainTex", goalImages[randomGoal]);
        }

        void Update()
        {
            if (isPuzzleSolved() == true)
            {
                SetUpNewPuzzle();
            }
        }
    }
}
