
[System.Flags]
public enum ItemType
{

	//Null = 0, 
	//Weapon = 1<<0, 
	//Armor = 1<<1, 
	//Artifect = 1<<2,

	//Two_Hand_Sword = 1<<10,
	//Bow = 1<<11,

	//Chest = 1<<20,
	//Pants = 1<<21,
	//helmet = 1<<22,

	//Null = 0,
	//Weapon = 1 << 0,
	//Armor = 1 << 1,
	//Artifect = 1 << 2,
	//Consumbable = 1<<3,
	//ECT = 1<<4,

	//Two_Hand_Sword = 1 << 10,
	//Bow = 1 << 11,

	//Chest = 1 << 20,
	//Pants = 1 << 21,
	//helmet = 1 << 22,


	//trap = 1 << 15,
	//buff = 1 << 16,
	//restore = 1 << 17,
	//aggro = 1 << 18,
	Null = 0,
	Heal = 1 << 0,
	Attraction = 1 << 1,
	Buff = 1 << 2,
	Trap = 1 << 3,
	Debuff = 1 << 4,
    Allways = int.MaxValue,

}

public enum EffectiveType
{
	HP,
	EnemyAttraction,
	Damage,
	Accurate,
	Move,
	Vision,
	Turn,
	Debuff,

}
public enum ItemTarget
{
    Player,
    Enemy,
    Tile,
}

public enum DirEight
{
    T,
    L,
    B,
    R,
    LT,
    LB,
    RB,
    RT
}




public enum OrderItemMethod
{
    Name,
    ID
}

public enum ControlType
{
    FreeMode,
    BattleMode
}

public enum BattleState
{
    Turn_Start,
    Turn_Processing,
    Turn_End,
}

public enum UnitTurnState
{
    WaitTurn, //자기턴을 기다리는 상태
    TurnStart, //턴 시작
    //SetOrder, //명령 설정 (AI 용)
    SetOrder, //명령 대기상태 (플레이어용)
    EndOrder, //
    OrderInitiate, //행동시작
    Processing, //행동중
    EndProcessing, // 행동종료
    TurnEnd, //턴종료
}

public enum OrderType
{
    None,
    Move,
    Attack,
    Skill
}

public enum UnitType
{
    Ally,
    Enemy,


}

public enum AIStateType { 
    Idle,
    Patrol,
    TargetUnitTrack,
    TrackMarkWatch,
    TrackMarkSound,
    BackToOriginPos,
    Attack,
    Skill,
    Dead,
}

public enum TileType
{
    Moveable_Plane,
    Unmoveable,
    Moveable_Slope_LR,
    Moveable_Slope_RL,
    Moveable_Slope_TB,
    Moveable_Slope_BT,
    Moveable_Hill_LR,
    Moveable_Hill_RL,
    Moveable_Hill_TB,
    Moveable_Hill_BT,
}

public enum TileWhoStay { 
    None,
    Enemy,
    Player,
    Trap,
}
//public enum SkillType
//{
//    Strength,
//    Dexterity,
//    Intelligence,
//}
//public enum SKillDetailsType
//{
//    Passive,
//    Active,
//    Super,
//}

public enum SKillID
{
    //Basic Skill
    Combat_Attack = 40,
    Assassin_Attack = 1,
    Shadow_Step = 2,
    Alive_Shadow = 3,
    Dark_Area = 4,
    Shadow_Ability = 5,

    //Combat Skill
	Double_Neck_Slice = 10,
	Sharp_Slice = 11,
	Strong_Push = 12,
	Tornado_Slice = 13,
	Fight_Critical_Hit = 14,
	Strong_Hit = 15,
	Strong_Skin = 16,
	Burserk = 17,
	New_Blood = 18,
	Fuck_Weak = 19,

    //Assassination Skill
    Sudden_Attack = 20,
	Shadow_Kill = 21,
	Destruction = 22,
	All_Direction_Slice = 23,
	Assassin_Critical_Hit = 24,
	Basic_Of_Assassin = 25,
	Shadow_Hit = 26,
	Opportunity_To_Assassin = 27,
	Cad_Attack = 28,
	Spot_Attack = 29,

	//Design Skill
	Setting_Trap = 30,
	Force_Temptation = 31,
	Shadow_Smog = 32,
	Shadow_Tactical_Hit = 33,
	Insane_Dagger = 34,
	Be_Skilled_Trap = 35,
	Sharp_Trap = 36,
	Power_Trap = 37,
	Perfect_Tactical_Hit = 38,
	Buff_Range = 39,
}


public enum AnimeID { 
    None = 0,
    Hit = 1,
    Move = 2,

    Rogue_Volume2_AttackA_01 = 500,
    Ninja_Casting_Start = 1000,
    Ninja_Casting_End = 1001,
    Rogue_Combat_Attack = 501,

    Double_Neck_Slice = 502,
    Ninja_Animset_reversal_finisher_01_1 = 503,

    Anime001_SpeedSlash = 10001,
}

public enum EffectID
{
    None = 0,
    FX_Splash_Portal_Floor = 1000,
    FX_Splash_Reaper_Floor = 1001,
    FX_Splash_Magic_Floor = 1002,
    FX_Splash_Slash_Floor = 1003,
    FX_Splash_Shadow_Air = 1004,
}




public enum SkillCategory   //SkillType
{
    Basic,          //공용스킬
    Combat,         //전투스킬
    Assassination,  //암살스킬
    Design,         //설계스킬
}
public enum SkillType    //SKillDetailsType
{
    Passive,        //패시브
    Active,         //액티브
    Super,          //슈퍼
}

//=====================================


public enum SkillFeature
{
    Usual,          //일반스킬
    Assassination,  //암살스킬
    Combat,         //전투스킬
    Move,           //이동스킬
    Teleport,       //순간이동
}

public enum UnitPositionState
{
    Nomal,          //기본자세
    Guard,          //대비자세
    TacticalHit,    //경계자세
}

public enum AdvType
{
    None,       //0
    Range,      //범위
    Distance,   //사거리
    Sound,      //소리
    Accurate,   //명중률
    Delay,      //재사용대기시간
    Damage,     //공격력(데미지)
    AsMove,     //암살 이동 칸수
    CbMove,     //전투 이동 칸수
    Turn,       //턴 추가
    CC,         //CC확률
    Critical,   //치명타
    Duration,    //지속시간
}
public enum AdvType2
{
    None,       //0
    Range,      //범위
    Distance,   //사거리
    Sound,      //소리
    Accurate,   //명중률
    Delay,      //재사용대기시간
    Damage,     //공격력(데미지)
    Move,       //이동
    Turn,       //턴 추가
}
public enum AdvCondition
{
    Always,         //항상
    Forward,        //전방
    Side,           //측
    Back,           //백
    Stun,           //스턴
    Smog,           //연막
    Shadow,         //그림자타일
    Combat,         //전투페이지
    Assassination,    //암살페이지
}
public enum AdvCondition2
{
    Always,     //항상
    Forward,    //전방
    Side,       //측
    Back,       //백
    Stun,       //스턴
    Smog,       //연막
    Shadow,     //그림자타일
}

public enum PosDirection { 
Forward,
Side,
Back
}

