using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

// A class that handles the bulk of combat logic

public class BattleController : MonoBehaviour {
    private enum WindowButton {
        None = -1,
        Attack = 0,
        Special = 1,
        Item = 2,
        Defend = 3
    }

    #region Input

    [HideInInspector]
    public static PlayerUnit inPlayerUnit;
    [HideInInspector]
    public static List<Unit> inEnemyUnits;
    [HideInInspector]
    public static AudioClip inMusic;
    [HideInInspector]
    public static string inScene;
    [HideInInspector]
    public static Transform inLocation;
    [HideInInspector]
    public static bool inParameters;

    #endregion

    #region Fields
    /// <summary>
    /// The prefab for the BattleUnit.
    /// </summary>
    [Header("Player and Enemy References")]
    /// <summary>
    /// The location to place the player
    /// </summary>
    public Transform playerLocation;

    /// <summary>
    /// The locations to place the enemy. Enemies that will
    /// spawn is limited to the number of enemy locations
    /// </summary>
    public List<Transform> enemyLocations;

    /// <summary>
    /// The Unit to spawn for the battle representing the enemy.
    /// A duplicate of the enemy Unit is spawned in.
    /// </summary>
    [SerializeField]
    private List<Unit> enemyUnitObjs;

    /// <summary>
    /// The BattleUnit instantiated from the player Unit
    /// </summary>
    [HideInInspector]
    public BattleUnit playerUnit;

    /// <summary>
    /// The BattleUnit instantiated from the enemy Unit
    /// </summary>
    [HideInInspector]
    public List<BattleUnit> enemyUnits;

    /// <summary>
    /// The BattleMusicHandler for the battle.
    /// </summary>
    [Header("Helper Scripts")]
    public BattleMusicHandler musicHandler;

    /// <summary>
    /// The BattleAnimationHandler for the battle.
    /// </summary>
    public BattleAnimationHandler animationHandler;

    /// <summary>
    /// The DamageCalculationHandler for the battle.
    /// </summary>
    public DamageCalculationHandler damageHandler;

    /// <summary>
    /// The BatlteUIHandler for the battle.
    /// </summary>
    public BattleUIHandler uiHandler;

    /// <summary>
    ///  The BattleSFXHandler for the battle.
    /// </summary>
    public BattleSFXHandler battleSFXHandler;

    /// <summary>
    /// The time in miliseconds to wait after something happens to
    /// let the player process what just happened
    /// </summary>
    public int waitTimer;

    /// <summary>
    /// The time in miliseconds to display enemy bars
    /// </summary>
    public int enemyBarDuration;

    /// <summary>
    /// SFX to play when a unit dies.
    /// </summary>
    [Header("Sound Effects")]
    public AudioClip enemyDefeatSFX;

    // Variables regarding the world screen
    private string currentScene;

    // Variables used by the battle system
    private bool canAct;
    private WindowButton caughtButton = WindowButton.None;
    private int selectedItem = -2;
    private int selectedEnemy = -2;
    private int selectedSpecial = -2;
    private bool isSetup;
    #endregion

    #region Initialization
    /// <summary>
    /// Starts a battle
    /// </summary>
    /// <param name="playerUnit">The player unit.</param>
    /// <param name="enemy">The enemy unit.</param>
    /// <param name="battleMusic">The battle music.</param>
    /// <param name="currentSceneName">The name of the scene you are currently in (to return to after the battle).</param>
    /// <param name="currentLocation">The location to return to after the battle.</param>
    /// <param name="battleSceneName">The name of the Battle scene to load</param>
    public static void StartBattle(List<Unit> enemies, AudioClip battleMusic, string scene, string battleSceneName = "CastleBattle") {
        CharacterMovementController.loadSavedLocation = true;
        CharacterMovementController.savedLocation = GameObject.FindGameObjectWithTag("Player").transform.position;

        SceneManager.LoadScene(battleSceneName);

        BattleController.inParameters = true;
        BattleController.inEnemyUnits = enemies;
        BattleController.inMusic = battleMusic;
        BattleController.inScene = scene;
    }

    private void Update() {
		if (isSetup) {
            playerUnit.UpdateHUD();

            foreach (BattleUnit enemy in enemyUnits)
                enemy.UpdateHUD();
		}
	}

	void Start() {
        SetupBattle();
    }
	#endregion

	#region Battle Core
	private async void SetupBattle() {
        // Setup helpers
        battleSFXHandler.battleController = this;
        uiHandler.battleController = this;
        musicHandler.battleController = this;
        animationHandler.battleController = this;
        damageHandler.battleController = this;

        // Get parameters from other scene if present
        if (inParameters) {
            enemyUnitObjs = inEnemyUnits;
            musicHandler.battleMusic = inMusic;
            currentScene = inScene;
            inParameters = false;
        }

        // Play the battle music
        musicHandler.PlayBattleMusic();

        // Spawn units
        playerUnit = SpawnPlayerUnit(PlayerUnit.INSTANCE, playerLocation);
        enemyUnits = SpawnEnemies(enemyUnitObjs, enemyLocations);

        // Setup UI
        await uiHandler.setupHUD(playerUnit, enemyUnits);

        isSetup = true;
        CombatLoop();
    }

    private async void CombatLoop() {
        // Main loop that keeps combat running until the end
        while (!await DidPlayerWin() && !await DidPlayerLose()) {
            // Player turn
            await PlayerTurn();
            await Task.Delay(waitTimer);
            if (await DidPlayerWin())
                PlayerWon();

            // Enemy turn
            await EnemyTurn();
            if (await DidPlayerLose())
                PlayerLost();
        }
    }
	#endregion

	#region Player Turn Handlers
	private async Task PlayerTurn() {
        // Show the player option window
        await uiHandler.ShowPlayerOptionWindow();

        // Start player turn
        TurnStart(playerUnit);
        canAct = true;

        // Catch the pressed button and hide the menu
        WindowButton selectedButton = await CatchPressedButton();
        battleSFXHandler.PlayConfirmSFX();
        await uiHandler.HidePlayerOptionWindow();

        // Perform the next action based on what's happening
        switch (selectedButton) {
            case WindowButton.Attack:
                await PlayerAttack();
                break;
            case WindowButton.Special:
                await PlayerSpecial();
                break;
            case WindowButton.Item:
                await PlayerItem();
                break;
            case WindowButton.Defend:
                // Perform the defense
                Defend(playerUnit);
                canAct = false;
                break;
        }
    }

    private async Task PlayerAttack() {
        // Prompt user to select an enemy to attack
        await uiHandler.ShowEnemyTargetingWindow();
        int attackTarget = await CatchSelectedEnemy();
        await uiHandler.HideEnemyTargetingWindow();

        if (attackTarget >= 0) {
            // Attack the enemy
            canAct = false;
            BattleUnit target = enemyUnits[attackTarget];
            ShowHPBarAsync(target);
            await Attack(playerUnit, target);
        } else {
            // Player selected back button
            await PlayerTurn();
		}
    }

    private async Task PlayerSpecial() {
        // Prompt user to select a special
        await uiHandler.ShowSkillWindow();
        int specialIndex = await CatchSelectedSpecial();
        await uiHandler.HideSkillWindow();

        if (specialIndex >= 0) {
            // Player selected a special
            Skill special = playerUnit.unit.skills[specialIndex];

            // Select a target if necessary
            BattleUnit target;
            if (special.targetType == TargetType.ENEMY || special.useScriptedSkillEffects && special.needsTarget) {
                await uiHandler.ShowEnemyTargetingWindow();
                int attackTarget = await CatchSelectedEnemy();
                await uiHandler.HideEnemyTargetingWindow();

                if (attackTarget >= 0) {
                    canAct = false;
                    target = enemyUnits[attackTarget];
                    // Perform the skill
                    ShowHPBarAsync(target);
                    await Skill(special, playerUnit, target);

                } else
                    await PlayerSpecial();
            } else {
                // Skill does not need target
                foreach (BattleUnit enemy in enemyUnits)
                    ShowHPBarAsync(enemy);
                await Skill(special, playerUnit);
			}
        } else {
			// Player selected the back button
		    await PlayerTurn();
		}
    }
	
    private async Task PlayerItem() {
        InventoryItem<Consumable>[] consumables = Inventory.INSTANCE.GetConsumables();
        await uiHandler.ShowItemsWindow(consumables);
        int itemIndex = await CatchSelectedItem();
        await uiHandler.HideItemsWindow();
        if (itemIndex >= 0) {
            // Player chose an item
            Consumable _item = consumables[itemIndex].item;
            Inventory.INSTANCE.RemoveConsumable(Atlas.GetID(_item));
            await Item(_item, playerUnit);
		} else {
            // Player chose back button
            await PlayerTurn();
		}
	}
    #endregion

	#region Enemy Turn Handlers
	private async Task EnemyTurn() {
        // Perform enemy actions in sequence
        foreach (BattleUnit enemy in enemyUnits) {
            await EnemyAction(enemy);
            await Task.Delay(waitTimer);
        }
	}

    private async Task EnemyAction(BattleUnit enemyUnit) {
        // Determine what enemy should do
        Skill _skill = null;
        
        // Get a random enemy skill
        if (enemyUnit.unit.skills.Count > 1) {
            int index = Random.Range(0, enemyUnit.unit.skills.Count);
            _skill = enemyUnit.unit.skills[index];
		}

        // Calculate weights for attack, defend, and skill
        int atkWeight = Mathf.Clamp(enemyUnit.unit.skills.Count, 1, 3);
        int defendWeight = Mathf.Clamp(enemyUnit.unit.skills.Count / 2, 0, 2);
        int skillWeight = enemyUnit.unit.skills.Count;

        // Determine what enemy should do
        int totalWeight = atkWeight + defendWeight + skillWeight;

        // Determine if unit should defend
        if (Random.Range(0, totalWeight) < defendWeight) {
            // enemy defends
            Defend(enemyUnit);
        } else {
            totalWeight -= defendWeight;
            if (Random.Range(0, totalWeight) < atkWeight) {
                // enemy attacks
                await Attack(enemyUnit, playerUnit);
            } else {
                // enemy does a special if possible
                if (_skill != null && enemyUnit.unit.cSP >= _skill.costSP) {
                    // enemy does skill
                    ShowSPBarAsync(enemyUnit);
                    await Skill(_skill, enemyUnit, playerUnit);
                } else {
                    // enemy does basic attack
                    await Attack(enemyUnit, playerUnit);
                }
            }
        }
	}
	#endregion

	#region Basic Attack Handlers
	private async Task Attack(BattleUnit attacker, BattleUnit target) {
        // Determine what type of attack to do
        AttackType attackType = attacker.unit.weapon.atkType;

        switch (attackType) {
            case AttackType.Melee:
                await MeleeAttack(attacker, target);
                break;
            case AttackType.Magic:
                await MagicAttack(attacker, target);
                break;
            case AttackType.Ranged:
                await RangedAttack(attacker, target);
                break;
        }
    }

    private async Task MeleeAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Calculate miss chance and determine if attack will miss
        float accuracy = 1 - ((float) (t.effAgi - t.effDex) / a.effDex);
        bool miss = UnityEngine.Random.value > accuracy;

        if (miss) {
            // Display output for a miss
            string message = a.unitName + " missed!";
            uiHandler.DisplayDialogueText(message);
            NumberPopup.DisplayTextPopup("Miss!", NumberType.NONE, target.transform);
        } else {
            // Calculate base damage
            int damage = a.effStr + a.weapon.might - t.effDef;

            // Do extra processing to the damage
            damage = OffsetValue(damage, 0.1f);
            if (t.isDefending) damage /= 2;
            if (damage < 0) damage = 0;

            // Display animation
            target.DoAnimation(a.weapon.weaponAnimation);

            // Display damage output text and do damage
            NumberPopup.DisplayNumberPopup(damage, NumberType.Damage, target.transform);
            battleSFXHandler.PlaySFX(a.weapon.attackSFX);
            target.TakeDamage(damage);
            _ = animationHandler.FlickerAnimation(target.gameObject);
            string message = a.unitName + " did " + damage + " melee damage to " + t.unitName + ".";
            uiHandler.DisplayDialogueText(message);
        }
    }

    private async Task MagicAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Calculate miss chance and determine if attack will miss
        float accuracy = 1 - ((float) (t.effAgi - t.effDex) / a.effDex);
        bool miss = UnityEngine.Random.value > accuracy;

        if (miss) {
            // Display output for a miss
            string message = a.unitName + " missed!";
            uiHandler.DisplayDialogueText(message);
            NumberPopup.DisplayTextPopup("Miss!", NumberType.NONE, target.transform);
        } else {
            // Calculate base damage
            int damage = a.effMag + a.weapon.might - t.effRes;

            // Do extra processing to the damage
            damage = OffsetValue(damage, 0.1f);
            if (t.isDefending) damage /= 2;
            if (damage < 0) damage = 0;

            // Display animation
            target.DoAnimation(a.weapon.weaponAnimation);

            // Display damage output text and do damage
            NumberPopup.DisplayNumberPopup(damage, NumberType.Damage, target.transform);
            battleSFXHandler.PlaySFX(a.weapon.attackSFX);
            target.TakeDamage(damage);
            _ = animationHandler.FlickerAnimation(target.gameObject);
            string message = a.unitName + " did " + damage + " magic damage to " + t.unitName + ".";
            uiHandler.DisplayDialogueText(message);
        }
    }

    private async Task RangedAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Calculate crit chance and determine if attack will crit
        float critChance = (float) (a.effDex - t.effAgi) / a.effDex;
        bool crit = UnityEngine.Random.value < critChance;

        // Calculate damage
        int damage = a.weapon.might;
        if (!crit) damage -= t.effArm;

        // Do extra processing to the damage
        damage = OffsetValue(damage, 0.1f);
        if (t.isDefending) damage /= 2;
        if (damage < 0) damage = 0;

        // Display output text, animation, and do damage
        string message = a.unitName + " did " + damage + (crit ? " critical" : "") + " ranged damage to " + t.unitName + ".";
        uiHandler.DisplayDialogueText(message);
        target.DoAnimation(a.weapon.weaponAnimation);
        battleSFXHandler.PlaySFX(a.weapon.attackSFX);
        target.TakeDamage(damage);
        _ = animationHandler.FlickerAnimation(target.gameObject);
        NumberPopup.DisplayTextPopup(damage + "!", NumberType.Damage, target.transform);
    }
	#endregion

	#region Other Action Handlers
    private async Task Skill(Skill skill, BattleUnit user, BattleUnit target = null) {
        user.unit.cSP -= skill.costSP;
        string message = user.unit.unitName + " uses " + skill.skillName + "!";

        if (skill.useScriptedSkillEffects) {
            message += "\n" + await skill.doScriptedSkillEffect(user, target, battleSFXHandler);
            uiHandler.DisplayDialogueText(message);
        } else {
            message = user.unit.unitName + " uses " + skill.skillName + "!";
            switch (skill.targetType) {
                case TargetType.SELF:
                    // Determine amount to heal and heal
                    int heal = skill.getValue(user.unit);
                    int value = user.Heal(heal);

                    // Display popup and recovery number
                    message += "\n" + user.unit.unitName + " recovered " + value + " HP.";
                    NumberPopup.DisplayNumberPopup(value, NumberType.Heal, user.transform);

                    // Display animation
                    user.DoAnimation(skill.skillAnimation);
                    battleSFXHandler.PlaySFX(skill.skillSFX);
                    uiHandler.DisplayDialogueText(message);
                    break;
                case TargetType.ENEMY:
                    // Determine damage to deal
                    int damage = skill.getValue(user.unit, target.unit);

                    // Perform extra processing to the damage
                    if (!skill.ignoreDefend) damage /= 2;
                    damage = OffsetValue(damage, skill.offsetRange);
                    if (damage < 0) damage = 0;

                    // Deal damage and display animation and play sound effect
                    target.TakeDamage(damage);
                    _ = animationHandler.FlickerAnimation(target.gameObject);
                    target.DoAnimation(skill.skillAnimation);
                    battleSFXHandler.PlaySFX(skill.skillSFX);

                    // Display dialogue text and damage number
                    message += "\n" + target.unit.unitName + " took " + damage + " special damage.";
                    uiHandler.DisplayDialogueText(message);
                    NumberPopup.DisplayNumberPopup(damage, NumberType.Damage, target.transform);
                    break;
                case TargetType.ALLENEMY:
                    int totalDamage = 0;

                    if (user == playerUnit) {
                        foreach (BattleUnit enemy in enemyUnits) {
                            // Get damage to this enemy
                            damage = skill.getValue(user.unit, enemy.unit);

                            // Do extra damage processing
                            if (!skill.ignoreDefend) damage /= 2;
                            damage = OffsetValue(damage, skill.offsetRange);
                            if (damage < 0) damage = 0;

                            totalDamage += damage;

                            // Deal damage and do animation
                            enemy.TakeDamage(damage);
                            enemy.DoAnimation(skill.skillAnimation);

                            // Display damage number
                            NumberPopup.DisplayNumberPopup(damage, NumberType.Damage, enemy.transform);
                            _ = animationHandler.FlickerAnimation(enemy.gameObject);
                            battleSFXHandler.PlaySFX(skill.skillSFX);
                        }

                        // Display dialogue text
                        message += "\nEnemies took a total of " + totalDamage + " special damage!";
                        uiHandler.DisplayDialogueText(message);
					} else goto case TargetType.ENEMY;

                    break;
			}
		}
	}
	
    private void Defend(BattleUnit user) {
        user.unit.isDefending = true;
        string message = user.unit.unitName + " defends!";
        uiHandler.DisplayDialogueText(message);
	}

    private async Task Item(Consumable item, BattleUnit user) {
        string message = user.unit.unitName + " uses a " + item.itemName + ".\n";
        switch (item.type) {
            case ConsumableType.HealthRecover:
                // Perform healing
                int heal = user.Heal(item.recovery);

                // Display visuals
                user.DoAnimation(item.animation);
                battleSFXHandler.PlaySFX(item.soundEffect);
                NumberPopup.DisplayNumberPopup(heal, NumberType.Heal, user.transform);

                // Display dialogue message
                message += user.unit.unitName + " recovered " + heal + " HP.";
                uiHandler.DisplayDialogueText(message);
                break;
            case ConsumableType.SpecialRecover:
                // Perform recovery
                int recover = user.RecoverSP(item.recovery);

                // Display visuals
                user.DoAnimation(item.animation);
                battleSFXHandler.PlaySFX(item.soundEffect);
                NumberPopup.DisplayNumberPopup(recover, NumberType.SpHeal, user.transform);

                // Display dialogue message
                message += user.unit.unitName + " recovered " + recover + " SP.";
                uiHandler.DisplayDialogueText(message);
                break;
            case ConsumableType.StatBuff:
                break;
            case ConsumableType.DebuffInflict:
                break;
            case ConsumableType.DamageDeal:
                int totalDamage = 0;

                foreach (BattleUnit target in enemyUnits) {
                    // Deal damage
                    target.TakeDamage(item.damage);
                    totalDamage += item.damage;

                    // Display visuals
                    target.DoAnimation(item.animation);
                    _ = animationHandler.FlickerAnimation(target.gameObject);
                    NumberPopup.DisplayNumberPopup(item.damage, NumberType.Damage, target.transform);
                    await Task.Delay(200);
				}

                message += "Enemies took a total of " + totalDamage + " item damage!";
                uiHandler.DisplayDialogueText(message);
                break;
		}
	}
    #endregion

	#region Win Condition Checkers
	private async Task<bool> DidPlayerWin() {
        // Remove all dead enemies from the list
        for (int i = 0; i < enemyUnits.Count; i++) {
            if (enemyUnits[i].unit.cHP <= 0) {
                battleSFXHandler.PlaySFX(enemyDefeatSFX);
                Destroy(enemyUnits[i].gameObject);
                await Task.Delay(300);
                enemyUnits.RemoveAt(i);
                i--;
			}
		}

        // Check if any enemies are left
        return enemyUnits.Count == 0;
	}

    private async Task<bool> DidPlayerLose() {
        // Check to see if player died
        if (playerUnit.unit.cHP <= 0) {
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            Destroy(playerUnit.gameObject);
            await Task.Delay(300);
            return true;
        }
        return false;
	}

	#endregion

	#region Battle Finish Handlers
    private async void PlayerWon() {
        uiHandler.DisplayDialogueText("You win!");
        musicHandler.PlayVictoryFanfare();

        await Utilities.WaitUntil(() => Input.GetButtonDown("Interact"));
        
        SceneManager.LoadScene(currentScene);
	}

    private async void PlayerLost() {
        uiHandler.DisplayDialogueText("You lose...");
	}

	#endregion

	#region Button Events
	public void OnEnemyButton(int i) { selectedEnemy = i; }
    public void OnSkillButton(int i) { selectedSpecial = i; }
    public void OnItemButton(int i) { selectedItem = i; }

    public async void ItemBackButton() {
        await uiHandler.HideItemsWindow();
        await uiHandler.ShowPlayerOptionWindow();
	}

	public void OnBasicButton(int buttonPressed) { caughtButton = (WindowButton) buttonPressed; }
	#endregion

	#region Catchers
	private async Task<WindowButton> CatchPressedButton() {
        while (caughtButton < 0) await Task.Delay(50);
        WindowButton button = caughtButton;
        caughtButton = WindowButton.None;
        return button;
    }

    private async Task<int> CatchSelectedEnemy() {
        while (selectedEnemy < -1) await Task.Delay(50);
        int enemy = selectedEnemy;
        selectedEnemy = -2;
        return enemy;
    }

    private async Task<int> CatchSelectedItem() {
        while (selectedItem < -1) await Task.Delay(50);
        int item = selectedItem;
        selectedItem = -2;
        return item;
    }

    private async Task<int> CatchSelectedSpecial() {
        while (selectedSpecial < -1) await Task.Delay(50);
        int special = selectedSpecial;
        selectedSpecial = -2;
        return special;
    }
    #endregion

    #region Helper Methods
    public void TurnStart(BattleUnit unit) {
        unit.unit.isDefending = false;
	}

    private async void ShowHPBarAsync(BattleUnit unit) {
        unit.unitHUD.ShowHPBar();
        await Utilities.DoAfter(enemyBarDuration, () => unit.unitHUD.HideHPBar());
    }

    private async void ShowSPBarAsync(BattleUnit unit) {
        unit.unitHUD.ShowSPBar();
        await Utilities.DoAfter(enemyBarDuration, () => unit.unitHUD.HideSPBar());
    }

    private BattleUnit SpawnPlayerUnit(PlayerUnit unit, Transform location) {
        BattleUnit battleUnit = Instantiate(unit.unitPrefab, location).GetComponent<BattleUnit>();
        battleUnit.unit = unit;
        battleUnit.unit.weapon = Instantiate(battleUnit.unit.weapon);
        return battleUnit;
    }

    private BattleUnit SpawnEnemyUnit(Unit unit, Transform location) {
        BattleUnit battleUnit = Instantiate(unit.unitPrefab, location).GetComponent<BattleUnit>();
        battleUnit.unit = Instantiate(unit);
        battleUnit.unit.weapon = Instantiate(battleUnit.unit.weapon);
        return battleUnit;
    }

    private List<BattleUnit> SpawnEnemies(List<Unit> units, List<Transform> locations) {
        List<BattleUnit> battleUnits = new List<BattleUnit>();

        for (int i = 0; i < units.Count && i < locations.Count; i++)
            battleUnits.Add(SpawnEnemyUnit(units[i], locations[i]));

        return battleUnits;
    }

    private int OffsetValue(int value, float offset) {
        // Randomly offsets an integer in the range of the offset
        float multiplier = 1 + UnityEngine.Random.Range(-offset, offset);

        return (int) (multiplier * value);
    }
    #endregion
}
