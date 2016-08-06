using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour
{

    public Transform playerTransform;

    public List<List<Vector3>> squadMembers;

    public float distanceBetweenMembers = 3f;
    public int membersInMinimalCircle = 6;

    private float dPhi;
    private static Mutex mutex;

    private List<List<Vector3>> circleProperPositions;

    // Use this for initialization
    void Awake()
    {
        dPhi = 360f / membersInMinimalCircle;

        squadMembers = new List<List<Vector3>>();
        circleProperPositions = new List<List<Vector3>>();

        AddCircle();
    }

    // Update is called once per frame

    public void AddSquadMember()
    {
        int circles = squadMembers.Count;

        if (squadMembers[circles - 1].Count >= circles * membersInMinimalCircle)
        {
            AddCircle();
            circles = squadMembers.Count;
        }

        Vector3 AddPosition = circleProperPositions[circles - 1][squadMembers[circles - 1].Count];

        squadMembers[circles - 1].Add(AddPosition);
    }

    public void DeleteSquadMember(int circle, int member)
    {
        mutex.WaitOne();

        if (circle < squadMembers.Count)
            squadMembers[circle].RemoveAt(member);

        CompensateCircles();

        mutex.ReleaseMutex();
    }

    public int NumberOfSquadMembers()
    {
        int result = 0;

        foreach (List<Vector3> circle in squadMembers)
        {
            result += circle.Count;
        }

        return result;
    }

    private void AddCircle()
    {
        squadMembers.Add(new List<Vector3>());
        circleProperPositions.Add(new List<Vector3>());

        int circleNr = circleProperPositions.Count - 1;

        for (int i = 0; i < membersInMinimalCircle * (circleNr + 1); i++)
        {
            circleProperPositions[circleNr].Add(GenerateCirclePosition(circleNr, i));
        }
    }

    private Vector3 GenerateCirclePosition(int i, int j)
    {
        float x = distanceBetweenMembers * (i + 1) * Mathf.Cos(dPhi * j / (i + 1));
        float z = distanceBetweenMembers * (i + 1) * Mathf.Sin(dPhi * j / (i + 1));

        return new Vector3(x, 0, z);
    }

    public List<List<Vector3>> CorrectSquadMembersPositions(List<List<Vector3>> toCorrect, float speed, float deltaTime)
    {
        foreach (List<Vector3> circle in toCorrect)
        {
            int ind = toCorrect.IndexOf(circle);

            foreach (Vector3 vector in circle)
            {
                int ind2 = circle.IndexOf(vector);

                toCorrect[ind][ind2] = Vector3.Lerp(vector, circleProperPositions[ind][ind2], speed * deltaTime);
            }
        }

        return toCorrect;
    }

    /// <summary>
    /// 1. Przejdź przez kręgi
    /// 2. Jeśli kręgowi brakuje ludzi, zapamiętaj jego numer
    /// 3. Przenoś ludzi z następnego kręgu do czasu gdy krąg brakujący jest pełny albo krąg następny jest pusty
    /// 4. Jeśli krąg brakujący został zapełniony, zresetuj. Jeśli nie, przejdź dalej.
    /// </summary>

    private void CompensateCircles()
    {
        int minCircleLacking = int.MaxValue;
        int squadMembersLeft = NumberOfSquadMembers();

        for (int i = 0; i < squadMembers.Count - 1; i++)
        {
            if (squadMembers[i].Count < membersInMinimalCircle * (i + 1))
            {
                if (minCircleLacking > i)
                    minCircleLacking = i;

                bool endedWithSuccess = true;

                while (squadMembers[i].Count < membersInMinimalCircle * (i + 1))
                {
                    if (squadMembers[i + 1].Count > 0)
                    {
                        Vector3 temp = squadMembers[i + 1][0];

                        squadMembers[minCircleLacking].Add(temp);
                        squadMembers[i + 1].Remove(temp);
                    }
                    else
                    {
                        endedWithSuccess = false;
                        break;
                    }
                }

                if (endedWithSuccess)
                {
                    minCircleLacking = int.MaxValue;
                }
            }

            foreach (List<Vector3> circle in squadMembers)
            {
                if (circle.Count == 0)
                {
                    int ind = squadMembers.IndexOf(circle);

                    squadMembers.RemoveAt(ind);
                    circleProperPositions.RemoveAt(ind);
                }
            }
        }
    }
}
