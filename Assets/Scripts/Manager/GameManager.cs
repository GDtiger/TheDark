using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Town;


public class GameManager : MonoBehaviour
{
	[TabGroup("Ref", "Stage")] public InputManager inputManager;
	[TabGroup("Ref", "Stage")] public GraphicManager graphicManager;
	[TabGroup("Ref", "Stage")] public TurnManager turnManager;
	[TabGroup("Ref", "Stage")] public WaypointManager waypointManager;
	[TabGroup("Ref", "Stage")] public GridManager gridManager;
	[TabGroup("Ref", "Stage")] public PathfindingSystem pathfindingSystem;
	[TabGroup("Ref", "Stage")] public UIManager uIManager;
	[TabGroup("Ref", "Stage")] public InventoryManager inventoryManager;
	[TabGroup("Ref", "Stage")] public GameTutorialManager tutorialManager;

	[TabGroup("Ref", "Town")] public Town_UIManager town_UIManager;


	[TabGroup("Ref", "Need")] public EffectPoolSCO effectPool;
	[TabGroup("Ref", "Need")] public ObjectPoolSCO objectPool;
	[TabGroup("Ref", "Need")] public SoundManager soundManager;
	[TabGroup("Ref", "Need")] public SkillDataBase skillDataBase;

	[TabGroup("Ref", "Need")] public TextDataBase textDataBase;
	[TabGroup("Ref", "Need")] public TextDataBase textDataBaseBattleTitle;
	[TabGroup("Ref", "Need")] public TextDataBase textDataBaseBattleDesc;

	[TabGroup("Ref", "Need")] public ImageDataBase imageDataBase;
	[TabGroup("Ref", "Need")] public ItemDataBase itemDataBase;
	[TabGroup("Ref", "Need")] public UI_LoadingScreen uiLoadScreen;


	[TabGroup("Parameter", "Parameter")] public int currentPlayerID;
	[TabGroup("Parameter", "Parameter")] public int maxItemPocketSize = 3;
	[TabGroup("Parameter", "Parameter")] public int maxQuickSlotSkillSize = 5;
	[TabGroup("Parameter", "Parameter")] public int maxMana = 5;


	[TabGroup("Player", "Info")] public List<PlayerUnitStatus> unitStatus;
	[TabGroup("Player", "Info")] public List<ItemData> inventory = new List<ItemData>();
	[TabGroup("Player", "Info")] public int gold = 9999;
	[TabGroup("Player", "Info")] public int diamond = 0;
	[TabGroup("Player", "Info")] public bool isTutorial = true;





	public bool isPaused;
	public bool debugMode;
	public bool tutorialInagameMode;
	public bool tutorialMode = false;

	#region instance
	static GameManager instance = null;


	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}
	#endregion

	#region SaveLoad Player Data



	//public List<ItemData> pocket = new List<ItemData>();
	#endregion

	private void Awake()
	{
		Debug.Log($"GameManager가 최초로 동작합니다 데이터베이스를 사용가능하게 준비합니다.");
		imageDataBase.Initiate();
		skillDataBase.Initiate();
		textDataBase.Initiate();
		textDataBaseBattleTitle.Initiate();
		textDataBaseBattleDesc.Initiate();
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		soundManager.PlayBGMSound("MainMenu", 1);
		if (debugMode)
		{
			InitiateIngame();
		}
		//
	}

	private void OnEnable()
	{
		//SceneManager.sceneLoaded += OnSceneLoaded;
		//InitiateInTown();
		//InitiateIngame();
	}

	public void GetItemsToPlayer(List<string> itemCodes, Transform target) {
		Debug.Log($"아이템 획득 코루틴 시작 {itemCodes.Count}");
		StartCoroutine(GetItem(itemCodes, target));
	}

	IEnumerator GetItem(List<string> itemCodes, Transform target)
	{
		for (int i = 0; i < itemCodes.Count; i++)
		{
			Debug.Log($"아이템 획득 코루틴 시작 {i}");
			var itemData = itemDataBase.GetItemFromDataBase(itemCodes[i]);
			if (itemData.dropRate > Random.Range(0, 100))
			{
				GetItemToPlayer(itemData, target);
				yield return new WaitForSeconds(0.5f);
			}
		}
		yield return null;
	}


	void GetItemToPlayer(ItemData itemCode, Transform target) {

        Debug.Log($"아이템 획득 {itemCode.name}");

        inventoryManager.AddItemToInventory(itemCode);
		var itemDisplay = objectPool.RequestObject(PrefabID.ItemEarnTexture) as ItemEarnDisplayController;
		itemDisplay.ShowText(itemCode.name, target.position);
    }




 //   void OnSceneLoaded(Scene scene,LoadSceneMode Mode)
	//{
	   

	//	if (!debugMode)
	//	{
	//		if (scene.name == "Stage1" || scene.name == "Stage1 1")
	//		{
	//			InitiateIngame();
	//		}
	//		else if (scene.name == "Town")
	//		{
	//			InitiateInTown();
	//		}
	//		else if (scene.name == "Town 1")
	//		{
	//			InitiateInTown();
	//		}
	//		else if (scene.name == "Town Tutorial")
	//		{
	//			InitiateInTown();
	//		}
	//		//uiLoadScreen.FadeWindow(false);
	//	}
	   
	//}
	private void OnDisable()
	{
		//SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	[Button]
	public void LoadData() 
	{

		imageDataBase.Initiate();
		skillDataBase.Initiate();
		inventory = new List<ItemData>();
		currentPlayerID = unitStatus[0].playerID;
		for (int i = 0; i < unitStatus.Count; i++)
		{
			unitStatus[i].TriggerAfterLoad(skillDataBase);
		}
	}

	[Button]
	public void InitiateIngame()
	{
		Debug.Log("asd");
		if (!Application.isPlaying) return;

		inputManager = InputManager.Instance;
		inputManager.Initiate();

		objectPool.Initiate();

		gridManager = GridManager.Instance;
		gridManager.Initiate();

		waypointManager = WaypointManager.Instance;
		waypointManager.Initiate();

		graphicManager = GraphicManager.Instance;
		graphicManager.Initiate();
		turnManager = TurnManager.Instance;
		turnManager.Initiate();

		for (int i = 0; i < unitStatus.Count; i++)
		{
			unitStatus[i].InitiateGame();
		}
		isPaused = false;


		uIManager = UIManager.Instance;
		uIManager.Initiate();

		effectPool.Initiate();

		inventoryManager = InventoryManager.Instance;
		inventoryManager.Initiate();
		//soundManager = SoundManager.Instance;
		soundManager.PlayBGMSound("Stage1", 1);
		tutorialManager = GameTutorialManager.Instance;
		tutorialManager.Initiate();
	}

	[Button]
	public void InitiateInTown() {
		if (!Application.isPlaying) return;

		Debug.Log("InitiateInTown Start");
		objectPool.Initiate();
		soundManager.PlayBGMSound("Town Tutorial", 1);
		town_UIManager = Town_UIManager.Instance;
		town_UIManager.Initiate();
		Debug.Log("InitiateInTown End");
	}

	public void UseSkill(int playerIndex, PlayerUnitStatus playerUnitStatus, OrderPlayerType skill) {
		playerUnitStatus.UseSkill((int)skill);
		uIManager.DrawSkillIcon(playerIndex);
	}
	public void UseItem(int playerIndex, PlayerUnitStatus playerUnitStatus, OrderPlayerType item)
	{
		playerUnitStatus.UseItem((int)item - 10);
		uIManager.DrawItemIcon();
	}


	public void InitiatePlayerTurn(int playerIndex) {
		unitStatus[playerIndex].InitiateTurn();
		uIManager.DrawSkillIcon(playerIndex);
		uIManager.DrawItemIcon();
		uIManager.SetActionPoint(unitStatus[playerIndex].mana, maxMana);
	}

	public void GetMana(int mana) {
		unitStatus[0].GetMana(mana);
		uIManager.SetActionPoint(unitStatus[0].mana, maxMana);
	}

	public void PlayableUnitDie(int index) {
		uIManager.ShowYouDiedWindow();
	}

	public void BossDied()
	{
		gold += 50;
		diamond += 15;
		uIManager.ShowYouWinWindow();
	}

	public void ReturnToTown() {

		Debug.Log("집으로 ....");
		LoadingSceneController.Instance.LoadScene("Town Tutorial");
		//SceneManager.LoadScene((int)SceneID.Town, LoadSceneMode.Single);
	}




	public SkillData GetSkillDataFromDB(string id)
	{
		return skillDataBase.GetSkillDataFromDataBase(id);
	}

	public SkillData GetSkillDataFromPlayer(int playerID, int skillIndex)
	{
		return skillDataBase.GetSkillDataFromDataBase(GetUnitStatus(playerID).skills[skillIndex]);
	}

	public SkillData GetSkillDataFromItem(int playerID, int skillIndex)
	{
		return skillDataBase.GetItemDataFromDataBase(GetUnitStatus().pocket[skillIndex].ID); 
	}

	public ItemData GetItemDataFromPlayer(int playerID, int skillIndex) {
		return GetUnitStatus().pocket[skillIndex];
	}

	public SkillData GetNormalAttack(int playerID)
	{
		return skillDataBase.GetSkillDataFromDataBase(GetUnitStatus(playerID).normalAttackID);
	}

	public Sprite GetImage(ItemData itemData)
	{
		return imageDataBase.GetImage(itemData.imageID).image;
	}

	public Sprite GetImage(string id)
	{
		return imageDataBase.GetImage(id).image;
		
	}

	public PlayerUnitStatus GetUnitStatus()
	{
		return unitStatus[currentPlayerID];
	}

	public PlayerUnitStatus GetUnitStatus(int index) {
		return unitStatus[index];
	}

	public List<ItemData> GetPocket()
	{
		return GetPocket(currentPlayerID);
	}

	public List<ItemData> GetPocket(int playerID) {
		for (int i = 0; i < unitStatus.Count; i++)
		{
			if (playerID == unitStatus[i].playerID)
			{
				return unitStatus[i].pocket;
			}
		}

		Debug.LogError($"{playerID}는 존재하지않습니다");

		return null;
	}

}
