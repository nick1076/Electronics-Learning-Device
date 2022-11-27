using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour
{

    public List<LessonSteps> stepSets = new List<LessonSteps>();
    public int lessonID = -1;
    public int stepID = -1;
    public List<GameObject> alternateUI = new List<GameObject>();

    private void Start()
    {
        foreach (LessonSteps set in stepSets)
        {
            foreach (Step step in set.steps)
            {
                foreach(GameObject obj in step.toEnable)
                {
                    obj.SetActive(false);
                }
                foreach (GameObject obj in step.toDisable)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    public void BeginLesson(int nLessonID)
    {
        if (stepSets.Count - 1 <= nLessonID)
        {
            //Can continue
            lessonID = nLessonID;
            HideAllUI();
            SetStep(0);
        }
        else
        {
            Debug.LogError("Lesson ID out of bounds.");
        }
    }

    public void ProgressStep(int dist)
    {
        if (lessonID != -1)
        {
            if (stepSets[lessonID].steps.Count - 1 >= stepID + dist && stepID + dist >= 0)
            {
                SetStep(stepID + dist);
            }
            else
            {
                Debug.LogError("Step ID out of bounds.");
            }
        }
        else
        {
            Debug.LogError("Lesson ID out of bounds.");
        }
    }

    public void SetStep(int step)
    {
        if (lessonID != -1)
        {
            foreach (LessonSteps set in stepSets)
            {
                foreach (Step stepN in set.steps)
                {
                    foreach (GameObject obj in stepN.toEnable)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in stepN.toDisable)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            if (step == -1)
            {
                return;
            }
            if (stepSets[lessonID].steps.Count - 1 >= step)
            {
                foreach (GameObject obj in stepSets[lessonID].steps[step].toEnable)
                {
                    obj.SetActive(true);
                }
                foreach (GameObject obj in stepSets[lessonID].steps[step].toDisable)
                {
                    obj.SetActive(false);
                }
            }
            stepID = step;
        }
        else
        {
            Debug.LogError("Lesson ID out of bounds.");
        }
    }

    public void CompleteLesson()
    {
        SetStep(-1);
        ShowAllUI();
        lessonID = -1;
        stepID = -1;
    }

    public void HideAllUI()
    {
        foreach (GameObject obj in alternateUI)
        {
            obj.SetActive(false);
        }
    }

    public void ShowAllUI()
    {
        foreach (GameObject obj in alternateUI)
        {
            obj.SetActive(true);
        }
    }
}
