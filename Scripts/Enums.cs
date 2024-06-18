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
public enum EnemyActionType{ NONE, CONDITIIONAL, CHANCE}
#endregion

#region Modifiers
public enum ModifierType{ NONE, DMGDEALT, DMGTAKEN, CARDCOST, SHOPCOST}
public enum ModifierValueType{ NONE, PERCENT, FLAT}
#endregion

#region Status Effects
public enum StatusType { NONE, STARTOFTURN, ENDOFTURN, EVENT}
public enum StatusStackType { NONE, INTENSITY, DURATION}
#endregion