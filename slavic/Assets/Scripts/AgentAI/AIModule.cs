using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AIModuleEnums;

/**
 * Klasa opisuje zadanie patrolowania przez aktorów obdarzonych sztuczną inteligencją.
 * */
public class AIModule : MonoBehaviour
{
    public TargetResolvePolicy targetResolvePolicy;			//zachowanie po wykryciu celu
    public PatrolEndingPolicy patrolEndingPolicy;			//zachowanie po zrealizowaniu scieżki patrolowania
    public float standByTime;								//czas oczekiwania po utraceniu celu
    public float searchTime;                                //czas poszukiwania przeciwnika po zgubieniu
    public float defendChaseDistance;                       //odległość na którą gonimy przeciwnika w trybie obrony
    public float destinationTolerence;						//odległość do punktu wymagana by uznać jego zaliczenie
    public List<Vector3> patrolPoints;						//lista z aktualnymi punktami patrolowymi

    private bool isActive;                                  //określa czy moduł AI jest włączony
    private PatrolAI_States aiState;						//aktualny stan algorytmu sztucznej inteligencji
    private Vector3 lastTargetLocation;			    		//ostatnia pozycja celu jako punkt w przestrzeni
    private Vector3 lastAgentLocationOnPath;				//ostatnia pozycja agenta na scieżce (do powracania z obsługi celu)
    private TargetResolveEnum targetResolve;                //aktualny pod-stan stanu obsługi celu

    private float standByTimeElapsed;                       //Licznik czasu poświęconego na oczekiwanie.
    private int patrolPointsIndex;							//indeks na miejsce listy z aktualnym punktem TODO: zamienić na wskaźnik
    private bool rightListMovement;							//kierunek przechodzenia po liście punktów tworzących scieżkę patrolowania

    private OmniSense sense;                                //Zmysł na podstawie którego analizujemy otoczenia.

    /**
     * NavAgent- agent nawigacyjny systemu Pathfinding'u w Unity.
     * Do niego kieruje się zadane położenia aktora (gdzie ma się poruszyć).
     * W przypadku braku NavAgent'a agent nie będzie się poruszał.
     * */
    private NavMeshAgent navAgent;

    /**
     * Zestaw wyposażenia- wyposażenie i/lub uzbrojenie dostępne dla aktora.
     * Na podstawie wykrytych przez zmysły obiektów określa cel dla aktora.
     * W zależności od dostępnego wyposażenia może to być przeciwnik do zaatakowania, sojusznik do wyleczenia lub zasób do podniesienia.
     * W przypadku braku wyposażenia, aktor będzie się tylko poruszał.
     * */
    //-zestaw wyposażenia
    private EquipmentAI equipmentAI;

    /**
     * Inicjalizacja po utworzeniu.
     * */
    void Start()
    {
        rightListMovement = true;
        targetResolve = TargetResolveEnum.DONE;

        lastTargetLocation = new Vector3(0, 0, 0);
        lastAgentLocationOnPath = new Vector3(0, 0, 0);

        if (patrolPoints == null)
        {
            patrolPoints = new List<Vector3>();
        }
        patrolPointsIndex = 0;

        navAgent = GetComponent<NavMeshAgent>();
        sense = GetComponent<OmniSense>();
        equipmentAI = GetComponent<EquipmentAI>();
        isActive = false;
    }

    /**
     * Uaktualnienie wywoływane w każdej klatce.
     * */
    void Update()
    {
        if (isActive)
        {
            AgentBehaviourUpdate();
        }
    }

    /**
     * Określa zachowanie agenta na ten cykl.
     * Uaktualnia zachowanie zmysłów, ekwipunku i zachowania patrolowania.
     * */
    private void AgentBehaviourUpdate()
    {
        SenseUpdate();
        EquipmentUpdate();
        PatrolBehaviourUpdate();
    }

    /**
     * Uaktualnia wykryte przez zmysł obiekty.
     * */
    private void SenseUpdate()
    {
        if (sense != null)
        {
            sense.DetectGameObjects();
        }
    }

    /**
     * Przeprowadza uaktualnienie informacji z wyposażenia.
     * */
    private void EquipmentUpdate()
    {
        if (equipmentAI != null)
        {
            equipmentAI.determineNewFavourites();
        }
    }

    /**
     * Przeprowadza uaktualnienie maszyny stanów realizującej zadanie patrolowania.
     * Zachowania:
     * 1.Określanie następnego punktu do którego należy się poruszyć
     * 2.Przechodzenie do następnego punktu
     * 3.Obsługa celu
     * 4.Odczekanie
     * 5.Powrót na scieżkę
     * */
    private void PatrolBehaviourUpdate()
    {
        switch (aiState)
        {
            /**
             * 1.Określanie następnego punktu do którego należy się poruszyć
             * */
            case PatrolAI_States.DETERMINE_NEXT_POINT:
                DetermineNextPoint();
                break;

            /**
             * 2.Przechodzenie do następnego punktu
             * */
            case PatrolAI_States.MOVE_TO_POINT:
                MoveToNextPoint();
                break;

            /**
             * 3.Obsługa celu
             * */
            case PatrolAI_States.RESOLVE_TARGET:
                TargetResolve();
                break;

            /**
             * 4.Odczekanie
             * */
            case PatrolAI_States.STAND_BY:
                StandBy();
                break;

            /**
             * 5.Powrót na scieżkę
             * */
            case PatrolAI_States.RETURN_TO_PATH:
                ReturnToPatrolPath();
                break;

            /**
             * Nigdy nie powinno mieć miejsca.
             * */
            default:
                //Assert(true, "Default call in PatrolBehaviourUpdate()");
                break;
        }
    }

    /**
     * 1.Określanie następnego punktu do którego należy się poruszyć
     * -Na podstawie dostarczonej listy punktów do przejścia i obecnej pozycji aktora określa się punkt w przestrzeni który należy podać NavAgent'owi
     * do realizacji.
     * -W przypadku gdy zrealizowano ostatni punkt z listy aktor może: przejść w bezczynność, powtórzyć scieżkę w odwrotnej kolejności,
     * powtórzyć scieżkę od pierwszego punktu.
     * -W przypadku spostrzeżenia celu następuje jego obsługa.
     * */
    private void DetermineNextPoint()
    {
        if (!isTargetToResolve())
        {
            if (patrolPoints.Count > 0)
            {
                if (patrolPointsIndex < 0)
                {
                    //Przekroczenie listy punktów z lewej strony
                    switch (patrolEndingPolicy)
                    {
                        case PatrolEndingPolicy.IDLE:
                            patrolPoints.Clear();
                            if (navAgent != null)
                            {
                                navAgent.destination = transform.position;
                            }
                            break;

                        case PatrolEndingPolicy.REVERSE:
                            patrolPointsIndex = 0;
                            rightListMovement = !rightListMovement;
                            aiState = PatrolAI_States.MOVE_TO_POINT;
                            break;

                        case PatrolEndingPolicy.CYCLE:
                            patrolPointsIndex = patrolPoints.Count - 1;
                            aiState = PatrolAI_States.MOVE_TO_POINT;
                            break;

                        default:
                            patrolPoints.Clear();
                            if (navAgent != null)
                            {
                                navAgent.destination = transform.position;
                            }
                            break;
                    }
                }
                else if (patrolPointsIndex >= patrolPoints.Count)
                {
                    //Przekroczenie listy punktów z prawej strony
                    switch (patrolEndingPolicy)
                    {
                        case PatrolEndingPolicy.IDLE:
                            patrolPoints.Clear();
                            if (navAgent != null)
                            {
                                navAgent.destination = transform.position;
                            }
                            break;

                        case PatrolEndingPolicy.REVERSE:
                            patrolPointsIndex = patrolPoints.Count - 1;
                            rightListMovement = !rightListMovement;
                            aiState = PatrolAI_States.MOVE_TO_POINT;
                            break;

                        case PatrolEndingPolicy.CYCLE:
                            patrolPointsIndex = 0;
                            aiState = PatrolAI_States.MOVE_TO_POINT;
                            break;

                        default:
                            patrolPoints.Clear();
                            if (navAgent != null)
                            {
                                navAgent.destination = transform.position;
                            }
                            break;
                    }
                }
                else
                {
                    aiState = PatrolAI_States.MOVE_TO_POINT;
                }
            }
        }
        else
        {
            prepareTargetResolve();
        }
    }

    /**
     * 2.Przechodzenie do następnego punktu
     * -NavAgent otrzymuje punkt do zrealizowania i przesuwa aktora w jego stronę.
     * -W przypadku spostrzeżenia celu następuje jego obsługa.
     * */
    private void MoveToNextPoint()
    {
        if (!isTargetToResolve())
        {
            if (Vector3.Distance(transform.position, patrolPoints[patrolPointsIndex]) > destinationTolerence)
            {
                if (navAgent != null)
                {
                    navAgent.destination = patrolPoints[patrolPointsIndex];
                }
            }
            else
            {
                if (rightListMovement)
                {
                    patrolPointsIndex++;
                }
                else
                {
                    patrolPointsIndex--;
                }
                aiState = PatrolAI_States.DETERMINE_NEXT_POINT;
            }
        }
        else
        {
            prepareTargetResolve();
        }
    }

    private void prepareTargetResolve()
    {
        if (aiState == PatrolAI_States.DETERMINE_NEXT_POINT || aiState == PatrolAI_States.MOVE_TO_POINT)
        {
            lastAgentLocationOnPath = transform.position;
        }
        aiState = PatrolAI_States.RESOLVE_TARGET;
    }
    /**
     * 3.Obsługa celu
     * -Następuje gdy jedna z dostępnych broni wykryje poprawny dla niej cel.
     * -Bieżąca pozycja aktora jest zapamiętywana, by można było powrócić do niej celem kontynuowania scieżki patrolu.
     * -Obsługa ma trzy możliwe wersje: pościg(A), obrona(B) i utrzymanie pozycji(C).
     * -Po zakończeniu obsługi następuje odczekanie.
     * */
    private void TargetResolve()
    {
        switch (targetResolvePolicy)
        {
            /**
             * * 3A.Pościg
             * -Rozpoczyna się od zbliżania do celu.
             * -Zbliżanie do celu dpoóki: nie jest w zasięgu i jest widoczny i żyje.
             * -Interakcja z celem dopóki: jest w zasięgu i jest widoczny i żyje.
             * -Poszukiwanie celu dopóki: nie jest widoczny i skończą się poszukiwania.
             * -Zakończenie obsługi gdy: cel nie żyje lub nie odnaleziono celu po zgubieniu.
             * */
            case TargetResolvePolicy.CHASE:
                if (equipmentAI.currentTarget == null)
                {
                    //zgubiono cel
                    targetResolve = TargetResolveEnum.SEARCH_TARGET;
                }
                else
                {
                    standByTimeElapsed = 0;
                    if (Vector3.Distance(transform.position, equipmentAI.currentTarget.transform.position) > equipmentAI.optimalDistanceToTarget)
                    {
                        //trzeba zbliżyć się do celu
                        targetResolve = TargetResolveEnum.CLOSE_IN_ON_TARGET;
                    }
                    else
                    {
                        //jesteśmy dostatecznie blisko celu
                        targetResolve = TargetResolveEnum.TARGET_INTERACTION;
                    }
                }
                break;

            /**
             * * 3B.Obrona
             * -Rozpoczyna się od zbliżania do celu.
             * -Zbliżanie do celu dpoóki: nie jest w zasięgu i jest widoczny i żyje i nie oddalimy się zanadto od scieżki patrolu.
             * -Interakcja z celem dopóki: jest w zasięgu i jest widoczny i żyje.
             * -Poszukiwanie celu dopóki: nie jest widoczny i skończą się poszukiwania i nie oddalimy się zanadto od scieżki patrolu.
             * -Zakończenie obsługi gdy: cel nie żyje lub nie odnaleziono celu po zgubieniu lub oddaliliśmy się zanadto od scieżki patrolu.
             * */
            case TargetResolvePolicy.DEFEND:

                if (equipmentAI.currentTarget == null)
                {
                    //cel niewidoczny
                    if (Vector3.Distance(lastAgentLocationOnPath, lastTargetLocation) <= defendChaseDistance)
                    {
                        //ostatnia pozycja w zasięgu pogoni
                        targetResolve = TargetResolveEnum.SEARCH_TARGET;
                    }
                    else
                    {
                        //ostatnia pozycja poza zasięgiem pogoni
                        targetResolve = TargetResolveEnum.DONE;
                    }
                }
                else
                {
                    //cel widoczny
                    if (Vector3.Distance(lastAgentLocationOnPath, equipmentAI.currentTarget.transform.position) <= defendChaseDistance)
                    {
                        //można się zbliżyć
                        if (Vector3.Distance(transform.position, equipmentAI.currentTarget.transform.position) <= equipmentAI.optimalDistanceToTarget)
                        {
                            //cel w pożądanym zasięgu
                            targetResolve = TargetResolveEnum.TARGET_INTERACTION;
                        }
                        else
                        {
                            //cel poza pożądanym zasięgiem
                            targetResolve = TargetResolveEnum.CLOSE_IN_ON_TARGET;
                        }
                    }
                    else
                    {
                        //nie można się zbliżyć
                        if (Vector3.Distance(transform.position, equipmentAI.currentTarget.transform.position) <= equipmentAI.maximalDistanceToTarget)
                        {
                            //cel w dopuszczalnym zasięgu
                            targetResolve = TargetResolveEnum.TARGET_INTERACTION;
                        }
                        else
                        {
                            //cel poza pożądanym zasięgiem
                            targetResolve = TargetResolveEnum.DONE;
                        }
                    }
                }
                break;

            /**
             * * 3C.Utrzymanie pozycji
             * -Rozpoczyna się od interakcji z celem.
             * -Interakcja z celem dopóki: jest w zasięgu i jest widoczny i żyje.
             * -Zakończenie obsługi gdy: cel nie żyje lub nie jest w zasięgu lub nie jest widoczny.
             * */
            case TargetResolvePolicy.HOLD_POSITION:
                if (equipmentAI.currentTarget == null)
                {
                    //zgubiono cel
                    targetResolve = TargetResolveEnum.DONE;
                }
                else
                {
                    standByTimeElapsed = 0;
                    if (Vector3.Distance(transform.position, equipmentAI.currentTarget.transform.position) > equipmentAI.maximalDistanceToTarget)
                    {
                        //cel poza zasięgiem
                        targetResolve = TargetResolveEnum.DONE;
                    }
                    else
                    {
                        //jesteśmy dostatecznie blisko celu
                        targetResolve = TargetResolveEnum.TARGET_INTERACTION;
                    }
                }
                break;

            /**
             * Nigdy nie powinno mieć miejsca.
             * */
            default:
                //Assert(true, "Default call in TargetResolve()");
                break;
        }

        switch (targetResolve)
        {
            case TargetResolveEnum.CLOSE_IN_ON_TARGET:
                CloseOnTarget();
                break;

            case TargetResolveEnum.TARGET_INTERACTION:
                TargetInteraction();
                break;

            case TargetResolveEnum.SEARCH_TARGET:
                SearchTarget();
                break;

            case TargetResolveEnum.DONE:
                standByTimeElapsed = 0;
                aiState = PatrolAI_States.STAND_BY;
                break;

            default:
                break;
        }

        if (equipmentAI.currentTarget != null)
        {
            lastTargetLocation = equipmentAI.currentTarget.transform.position;
        }
    }

    /**
     * 3.1.Zbliżenie do celu
     * -Agent podaje pozycję celu NavAgent'owi do zrealizowania, dopóki cel nie znajdzie się w zasięgu.
     * */
    private void CloseOnTarget()
    {
        if (navAgent != null)
        {
            navAgent.destination = equipmentAI.currentTarget.transform.position;
        }
    }

    /**
     * 3.2.Interakcja z celem
     * -Interakcja za pomocą odpowiedniego wyposażenia dopóki cel jest w zasięgu.
     * */
    private void TargetInteraction()
    {
        if (navAgent != null)
        {
            navAgent.destination = transform.position;
        }
    }

    /**
     * 3.3.Poszukiwanie celu
     * -Gdy utracimy cel z zasięgu zmysłów, idziemy do jego ostatniej znanej pozycji.
     * -Po dojściu do niej przerywamy poszukiwania.
     * -Można w przyszłości zaimplementować inne rozwiązania
     * */
    private void SearchTarget()
    {
        standByTimeElapsed += Time.deltaTime;
        if (Vector3.Distance(transform.position, lastTargetLocation) > destinationTolerence && standByTimeElapsed < searchTime)
        {
            if (navAgent != null)
            {
                navAgent.destination = lastTargetLocation;
            }
        }
        else
        {
            standByTimeElapsed = 0;
            targetResolve = TargetResolveEnum.DONE;
            aiState = PatrolAI_States.STAND_BY;
        }
    }

    /**
     * 4.Odczekanie
     * -Następuje po obsłudze celu.
     * -Aktor odczekuje kilka sekund.
     * -Jeśli w czasie ich trwania nie wykryte zostaną następne cele, to następuje powrót na scieżkę.
     * -Jeśli zostaną wykryte cele, następuje ich obsługa.
     * */
    private void StandBy()
    {
        if (!isTargetToResolve())
        {
            standByTimeElapsed += Time.deltaTime;
            if (standByTimeElapsed >= standByTime)
            {
                standByTimeElapsed = 0;
                aiState = PatrolAI_States.RETURN_TO_PATH;
            }
        }
        else
        {
            prepareTargetResolve();
        }
    }

    /**
     * 5.Powrót na scieżkę
     * -Następuje po odczekaniu.
     * -Aktor powraca na zapamiętaną pozycję by wznowić patrolowanie.
     * -Jeśli zostaną wykryte cele, następuje ich obsługa.
     * */
    private void ReturnToPatrolPath()
    {
        if (!isTargetToResolve())
        {
            if (Vector3.Distance(transform.position, lastAgentLocationOnPath) > destinationTolerence)
            {
                if (navAgent != null)
                {
                    navAgent.destination = lastAgentLocationOnPath;
                }
            }
            else
            {
                aiState = PatrolAI_States.DETERMINE_NEXT_POINT;
            }
        }
        else
        {
            prepareTargetResolve();
        }
    }

    /**
     * Określa czy ma nastąpić przerwanie patrolowania by obsłużyć nowy cel.
     * True- należy, False- nie należy.
     * */
    private bool isTargetToResolve()
    {
        if (equipmentAI == null || equipmentAI.currentTarget == null)
        {
            return false;
        }
        if (targetResolvePolicy == TargetResolvePolicy.IGNORE ||
            (targetResolvePolicy == TargetResolvePolicy.HOLD_POSITION && Vector3.Distance(transform.position, equipmentAI.currentTarget.transform.position) > equipmentAI.maximalDistanceToTarget) ||
            (targetResolvePolicy == TargetResolvePolicy.DEFEND && Vector3.Distance(transform.position, equipmentAI.currentTarget.transform.position) > equipmentAI.maximalDistanceToTarget)
            )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /**
    * Ustawia typ obsługi celów na jeden z trzech dostępnych: 
    * -Pościg (CHASE)
    * -Obrona (DEFEND)
    * -Utrzymanie Pozycji (HOLD_POSITION)
    * */
    public void SetTargetResolvePolicy(TargetResolvePolicy newTargetResolvePolicy)
    {
        targetResolvePolicy = newTargetResolvePolicy;
    }

    /**
     * Ustawia sposób zakończenia patrolu na jeden z trzech dostępnych:
     * -Bezczynność (IDLE)
     * -Odwrotna kolejność (REVERSE)
     * -Od początku (CYCLE)
     * */
    public void SetPatrolEnding(PatrolEndingPolicy newPatrolEndingPolicy)
    {
        patrolEndingPolicy = newPatrolEndingPolicy;
    }

    /**
     * Ustawia nową scieżkę patrolowania.
     * */
    public void SetPatrolRoute(List<Vector3> newPatrolList)
    {
        if (patrolPoints == null)
        {
            patrolPoints = new List<Vector3>();
        }
        patrolPoints.Clear();
        patrolPoints = newPatrolList;
        RestartState();
    }

    public void SetPatrolRoute(Vector3 newPoint)
    {
        if (patrolPoints == null)
        {
            patrolPoints = new List<Vector3>();
        }
        patrolPoints.Clear();
        patrolPoints.Add(newPoint);
    }

    /**
     * Restartuje zachowanie agenta.
     * */
    private void RestartState()
    {
        patrolPointsIndex = 0;
        aiState = PatrolAI_States.DETERMINE_NEXT_POINT;
        rightListMovement = true;
    }

    public void Activate()
    {
        isActive = true;
        navAgent.enabled = true;
    }

    public void Deactivate()
    {
        isActive = false;
        navAgent.enabled = false;
    }
}