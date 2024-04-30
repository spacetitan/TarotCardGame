#region Card
public enum CardType { NONE, FIGHTER, RANGER, MAGE, ITEM, ENEMY}
public enum CardTag {   NONE, ATTACK, SKILL, POWER}
public enum Target{ NONE, SELF, SINGLE, ALL, EVERYONE}
public enum CardStates { NONE, DEFAULT, CLICKED, DRAGGING, AIMING, RELEASED}
#endregion

#region Player
public enum PlayerClass { NONE, FIGHTER, RANGER, MAGE}
#endregion

#region Enemy
public enum EnemyActionType{NONE, CONDITIIONAL, CHANCE}
#endregion
