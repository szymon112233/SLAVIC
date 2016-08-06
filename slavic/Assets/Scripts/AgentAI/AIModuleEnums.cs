/**
 * Zawiera używane w skryptach patrolowania typy Enum.
 * */
namespace AIModuleEnums
{
    /**
     * Sposoby obsługi celów.
     * */
    public enum TargetResolvePolicy
    {
        CHASE, DEFEND, HOLD_POSITION, IGNORE
    }

    /**
     * Możliwe zakończenia cyklu patrolowania.
     * */
    public enum PatrolEndingPolicy
    {
        IDLE, REVERSE, CYCLE
    }

    /**
     * Stany sztucznej inteligencji realizującej patrolowanie.
     * */
    public enum PatrolAI_States
    {
        DETERMINE_NEXT_POINT, MOVE_TO_POINT, RESOLVE_TARGET, STAND_BY, RETURN_TO_PATH
    }

    /**
     * Stany obsługi celu.
     * */
    public enum TargetResolveEnum
    {
        CLOSE_IN_ON_TARGET, TARGET_INTERACTION, SEARCH_TARGET, DONE
    }
}