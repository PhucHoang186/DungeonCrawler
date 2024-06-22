public enum EntityType
{
    Player, Enemy
}

public enum TurnBaseType
{
    None,
    Player,
    Enemy,
}

public enum CommandStatus
{
    Progressing,
    Done,
}

public enum CommandType
{
    Move,
    Action,
    End_Turn,
    Waiting,
    Waiting_Action,
}

public enum CharacterClass
{
    Warrior,
    Dragon,
}

public enum CamType
{
    Battle_View, Character_View, Full_View
}

public enum highlightType
{
    Move,
    Action,
    ActionRange,
    All,
}

public enum ModifyType
{
    Mana,
    Health,
    Stamina,
    Damage,
}

public enum CharacterEntityState
{
    Live,
    Down,
    Destroy,
}

public enum CastPattern
{
    Around,
    Straight_Line
}

