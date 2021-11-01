using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

#pragma warning disable IDE0051, IDE0090
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
    private static string currentScene;
    private static string battleScene;
    private static GameObject[] mapSceneObjects;

    // Variables used by the battle system
    private bool canAct;
    private WindowButton caughtButton = WindowButton.None;
    private int selectedItem = -2;
    private int selectedEnemy = -2;
    private int selectedSpecial = -2;
    private bool combatActive = true;
    public static bool? playerWon;
    public static bool allowLose;
    #endregion

    #region Initialization
    /// <summary>
    /// Starts a battle. Waits until the battle is done.
    /// </summary>
    /// <param name="playerUnit">The player unit.</param>
    /// <param name="enemy">The enemy unit.</param>
    /// <param name="battleMusic">The battle music.</param>
    /// <param name="currentSceneName">The name of the scene you are currently in (to return to after the battle).</param>
    /// <param name="currentLocation">The location to return to after the battle.</param>
    /// <param name="battleSceneName">The name of the Battle scene to load</param>
    /// <returns>Whether or not the player won the battle.</returns>
    public static void StartBattle(List<Unit> enemies, AudioClip battleMusic, string scene, bool allowLose = false, string battleSceneName = "CastleBattle") {
        playerWon = null;
        // InputMovement.SaveLocation();

        mapSceneObjects = GameObject.FindGameObjectsWithTag("MapObject");

        foreach (GameObject obj in mapSceneObjects)
            obj.SetActive(false);

        battleScene = battleSceneName;
        SceneManager.LoadScene(battleScene, LoadSceneMode.Additive);

        BattleController.inParameters = true;
        BattleController.inEnemyUnits = enemies;
        BattleController.inMusic = battleMusic;
        BattleController.inScene = scene;
        BattleController.allowLose = allowLose;
    }

	void Start() {
        SetupBattle();
    }
	#endregion

	#region Battle Core
    /// <summary>
    /// Method that sets up the battle and starts it.
    /// </summary>
	private async void SetupBattle() {
        // Setup helpers
        battleSFXHandler.battleController = this;
        uiHandler.battleController = this;
        musicHandler.battleController = this;
        animationHandler.battleController = this;

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

        // Begin combat loop
        combatActive = true;
        CombatLoop();
    }

    /// <summary>
    /// Main loop that keeps combat running until the player
    /// wins or loses.
    /// </summary>
    private async void CombatLoop() {
        while (combatActive) {
            // Player turn
            await PlayerTurn();
            await Task.Delay(waitTimer);
            if (await DidPlayerWin())
                await PlayerWon();

            // Enemy turn
            await EnemyTurn();
            if (await DidPlayerLose())
                await PlayerLost();
        }
    }
	#endregion

	#region Player Turn Handlers
    /// <summary>
    /// Starts the player's turn. Displays player's options
    /// and prompts player to pick one.
    /// </summary>
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

    /// <summary>
    /// Called when player clicks attack button. Handles
    /// selecting an enemy to attack and attacking them.
    /// </summary>
    private async Task PlayerAttack() {
        // Prompt user to select an enemy to attack
        await uiHandler.ShowEnemyTargetingWindow();
        int attackTarget = await CatchSelectedEnemy();
        await uiHandler.HideEnemyTargetingWindow();

        Debug.Assert(canAct);

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

    /// <summary>
    /// Called when player clicks special button. Handles
    /// selecting a special and (if necessary) selecting an
    /// enemy to cast the special.
    /// </summary>
    private async Task PlayerSpecial() {
        // Prompt user to select a special
        await uiHandler.ShowSkillWindow();
        int specialIndex = await CatchSelectedSpecial();
        await uiHandler.HideSkillWindow();

        Debug.Assert(canAct);

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
	
    /// <summary>
    /// Called when player clicks item button. Handles selecting
    /// an item and then using it.
    /// </summary>
    private async Task PlayerItem() {
        InventoryItem<Consumable>[] consumables = Inventory.INSTANCE.GetConsumables();
        await uiHandler.ShowItemsWindow(consumables);
        int itemIndex = await CatchSelectedItem();
        await uiHandler.HideItemsWindow();

        Debug.Assert(canAct);

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
    /// <summary>
    /// Handles enemy turn. Iterates through the list of all enemies
    /// and has each of them perform an action.
    /// </summary>
	private async Task EnemyTurn() {
        // Perform enemy actions in sequence
        foreach (BattleUnit enemy in enemyUnits) {
            TurnStart(enemy);
            await EnemyAction(enemy);
            await Task.Delay(waitTimer);
        }
	}

    /// <summary>
    /// Handles the action of a single enemy. Randomly generates
    /// an action to perform - attack, defend, or special.
    /// </summary>
    /// <param name="enemyUnit">The enemy performing the attack.</param>
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
    /// <summary>
    /// Handles basic attacks, including calculation and animation by calling
    /// a function specific to the damage type of the attacker's weapon.
    /// </summary>
    /// <param name="attacker">The unit attacking.</param>
    /// <param name="target">The unit being attacked.</param>
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

    /// <summary>
    /// Handles basic melee attacks, including calculation and animation.
    /// </summary>
    /// <param name="attacker">The unit attacking.</param>
    /// <param name="target">The unit being attacked.</param>
    private async Task MeleeAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Determine if attack will hit
        bool miss = DetermineMiss(a, t);

        if (miss) {
            // Display output for a miss
            ShowMiss(a, target);
        } else {
            // Determine if crit
            bool crit = DetermineCrit(a, t);

            // Calculate base damage
            int damage = a.effStr + a.weapon.might;
            if (crit) damage -= t.effDef / 2;
            else damage -= t.effDef;

            // Do extra processing to the damage
            damage = ProcessDamage(damage, a, t);

            // Display damage output text and do damage
            await DoDamage(damage, target, a, crit);
        }
    }

    /// <summary>
    /// Handles basic magic attacks, including calculation and animation.
    /// </summary>
    /// <param name="attacker">The unit attacking.</param>
    /// <param name="target">The unit being attacked.</param>
    private async Task MagicAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Determine if attack will hit
        bool miss = DetermineMiss(a, t);

        if (miss) {
            // Display output for a miss
            ShowMiss(a, target);
        } else {
            // Determine if crit
            bool crit = DetermineCrit(a, t);

            // Calculate base damage
            int damage = a.effMag + a.weapon.might;
            if (crit) damage -= t.effRes / 2;
            else damage -= t.effRes;

            // Do extra processing to the damage
            damage = ProcessDamage(damage, a, t);

            // Display damage output text and do damage
            await DoDamage(damage, target, a, crit);
        }
    }

    /// <summary>
    /// Handles basic ranged attacks, including calculation and animation.
    /// </summary>
    /// <param name="attacker">The unit attacking.</param>
    /// <param name="target">The unit being attacked.</param>
    private async Task RangedAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Determine if miss
        bool miss = DetermineMiss(a, t);

        if (miss) {
            // Display output for a miss
            ShowMiss(a, target);
        } else {
            // Calculate crit chance and determine if attack will crit
            bool crit = DetermineCrit(a, t);

            // Calculate damage
            int damage = a.weapon.might;
            if (!crit) damage -= t.effArm;
            else damage -= t.effArm / 2;

            // Do extra processing to the damage
            damage = ProcessDamage(damage, a, t);

            // Display output text, animation, and do damage
            await DoDamage(damage, target, a, crit);
        }
    }

    /// <summary>
    /// Determines if an attack will miss based on the default
    /// accuracy calculation.
    /// </summary>
    /// <param name="a">The unit performing the attack.</param>
    /// <param name="t">The unit being attacked.</param>
    /// <returns>Whether or not the attack will miss.</returns>
    private bool DetermineMiss(Unit a, Unit t) {
        float accuracy = 1 - ((float) (t.effAgi - t.effDex) / a.effDex);
        bool miss = UnityEngine.Random.value > accuracy;
        return miss;
    }

    /// <summary>
    /// Determines if an attack will crit based on the default crit
    /// calculation.
    /// </summary>
    /// <param name="a">The unit performing the attack.</param>
    /// <param name="t">The unit being attacked.</param>
    /// <returns>Whether or not the attack will crit.</returns>
    private bool DetermineCrit(Unit a, Unit t) {
        float critChance = (float) (a.effDex - t.effAgi) / a.effDex;
        float random = UnityEngine.Random.value;
        bool crit = random < critChance;

        return crit;
    }

    /// <summary>
    /// Displays the information for a missed attack.
    /// </summary>
    /// <param name="a">The attacking Unit.</param>
    /// <param name="target">The BattleUnit being attacked.</param>
    private void ShowMiss(Unit a, BattleUnit target) {
        // Display output for a miss
        string message = a.unitName + " missed!";
        uiHandler.DisplayDialogueText(message);
        NumberPopup.DisplayTextPopup("Miss!", NumberType.NONE, target.transform);
    }

    /// <summary>
    /// Performs default extra damage processing. Includes defend check,
    /// weapon damage multiplier, and random 10% offset.
    /// </summary>
    /// <param name="damage">The base damage done by the attack.</param>
    /// <param name="a">The attacking unit.</param>
    /// <param name="t">The target unit.</param>
    /// <returns></returns>
    private int ProcessDamage(int damage, Unit a, Unit t) {
        damage = OffsetValue(damage, 0.1f);
        if (t.isDefending) damage /= 2;
        damage = (int) (a.weapon.attackDamageMultiplier * damage);
        if (damage < 0) damage = 0;

        return damage;
    }
	
    /// <summary>
    /// Does damage and displays information on an enemy, including damage dealt in the
    /// dialogue box and with damage number, the weapon animation, and the flicker animation.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    /// <param name="a"></param>
    /// <param name="damageType"></param>
    /// <param name="crit"></param>
    private async Task DoDamage(int damage, BattleUnit target, Unit a, bool crit = false) {
        // Get convenience reference to target unit
        Unit t = target.unit;

        // Dispay initial popup and damage
        string damageType = a.weapon.atkType.ToString().ToLower();
        NumberPopup popup = NumberPopup.DisplayNumberPopup(damage, NumberType.Damage, target.transform)
                .GetComponent<NumberPopup>();
        battleSFXHandler.PlaySFX(a.weapon.attackSFX);
        target.TakeDamage(damage);
        target.DoAnimation(a.weapon.weaponAnimation);
        _ = animationHandler.FlickerAnimation(target.gameObject);
        int totalDamage = damage;

        // Perform follow up attacks and update number each time
        for (int i = 1; i < a.weapon.hits; i++) {
            target.TakeDamage(damage);
            totalDamage += damage;
            popup.ChangeNumber(totalDamage);
            await Task.Delay(100);
        }

        // Display dialogue message
        string message = a.unitName + " did " + totalDamage + " " + (crit ? "critical " : "")
            + damageType + " damage to " + t.unitName + ".";
        uiHandler.DisplayDialogueText(message);
    }
    #endregion

	#region Other Action Handlers
    /// <summary>
    /// Performs a skill.
    /// </summary>
    /// <param name="skill">The skill to perform.</param>
    /// <param name="user">The unit using the skill.</param>
    /// <param name="target">
    /// The target of the skill. May be null for self healing skills,
    /// AoE skills, and scripted skills that do not need a target.
    /// </param>
    private async Task Skill(Skill skill, BattleUnit user, BattleUnit target = null) {
        _ = user.ConsumeSP(skill.costSP);
        string message = user.unit.unitName + " uses " + skill.skillName + "!";

        if (skill.useScriptedSkillEffects) {
            message += "\n" + await skill.DoScriptedSkillEffect(user, target, battleSFXHandler);
            uiHandler.DisplayDialogueText(message);
        } else {
            message = user.unit.unitName + " uses " + skill.skillName + "!";
            switch (skill.targetType) {
                case TargetType.SELF:
                    // Determine amount to heal and heal
                    int heal = skill.GetValue(user.unit);
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
                    int damage = skill.GetValue(user.unit, target.unit);

                    // Perform extra processing to the damage
                    if (!skill.ignoreDefend && target.unit.isDefending) damage /= 2;
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
                            damage = skill.GetValue(user.unit, enemy.unit);

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
	
    /// <summary>
    /// Performs a defend action.
    /// </summary>
    /// <param name="user">The unit performing the defend animation.</param>
    private void Defend(BattleUnit user) {
        user.unit.isDefending = true;
        string message = user.unit.unitName + " defends!";
        uiHandler.DisplayDialogueText(message);
	}

    /// <summary>
    /// Uses an item.
    /// </summary>
    /// <param name="item">The item being used.</param>
    /// <param name="user">The unit using the item.</param>
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
    /// <summary>
    /// Checks to see if the player won. Despawns any
    /// defeated enemies as well.
    /// </summary>
    /// <returns>Whether or not the player won.</returns>
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

    /// <summary>
    /// Checks to see if the player lost. Despawns player if they
    /// have been defeated.
    /// </summary>
    /// <returns>Whether or not the player lost.</returns>
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
    /// <summary>
    /// Actions that occur at the end of the battle if player won. Handles victory
    /// fanfare, dialogue text, and returning to map.
    /// </summary>
    private async Task PlayerWon() {
        combatActive = false;
        uiHandler.DisplayDialogueText("You win!");
        musicHandler.PlayVictoryFanfare();

        await Utilities.WaitUntil(() => Input.GetButton("Interact"));

        AsyncOperation op = SceneManager.UnloadSceneAsync(battleScene);
        await Utilities.WaitUntil(() => op.isDone);

        foreach (GameObject obj in mapSceneObjects)
            obj.SetActive(true);

        playerWon = true;
    }

    /// <summary>
    /// Actions that occur at the end of battle if player lost. Displays
    /// game over text.
    /// </summary>
    private async Task PlayerLost() {
        combatActive = false;
        uiHandler.DisplayDialogueText("You lose...");

        await Utilities.WaitUntil(() => Input.GetButton("Interact"));

        if (allowLose) {
            AsyncOperation op = SceneManager.UnloadSceneAsync(battleScene);
            await Utilities.WaitUntil(() => op.isDone);
            
            foreach (GameObject obj in mapSceneObjects)
                obj.SetActive(true);

            playerWon = false;
        } else {
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene("GameOver");
		}
    }

	#endregion

	#region Button Events
    /// <summary>
    /// Triggered on enemy target button press.
    /// Stores data about an enemy targeting button pressed.
    /// </summary>
    /// <param name="i">The button pressed. -1 means the back button was pressed.</param>
	public void OnEnemyButton(int i) { selectedEnemy = i; }

    /// <summary>
    /// Triggered on skill button press.
    /// Stores data about a skill button pressed.
    /// </summary>
    /// <param name="i">The button pressed. -1 means the back button was pressed.</param>
    public void OnSkillButton(int i) { selectedSpecial = i; }

    /// <summary>
    /// Triggered on item button press.
    /// Stores data about an item button pressed.
    /// </summary>
    /// <param name="i">The button pressed. -1 means the back button was pressed.</param>
    public void OnItemButton(int i) { selectedItem = i; }

    /// <summary>
    /// Triggered on the player action menu button press.
    /// Stores data about the button the player pressed.
    /// </summary>
    /// <param name="buttonPressed">The button pressed.</param>
	public void OnBasicButton(int buttonPressed) { caughtButton = (WindowButton) buttonPressed; }
	#endregion

	#region Catchers
    /// <summary>
    /// Waits for the player to pick a WindowButton. Returns the
    /// WindowButton selected when the player presses one.
    /// </summary>
    /// <returns>The pressed WindowButton.</returns>
	private async Task<WindowButton> CatchPressedButton() {
        while (caughtButton < 0) await Task.Delay(50);
        WindowButton button = caughtButton;
        caughtButton = WindowButton.None;
        return button;
    }

    /// <summary>
    /// Waits for the player to pick an enemy or go back. Returns the
    /// index when the player makes a selection.
    /// </summary>
    /// <returns>-1 if the player clicked the back button. Otherwise the enemy index.</returns>
    private async Task<int> CatchSelectedEnemy() {
        while (selectedEnemy < -1) await Task.Delay(50);
        int enemy = selectedEnemy;
        selectedEnemy = -2;
        return enemy;
    }

    /// <summary>
    /// Waits for the player to pick an item or go back. Returns the
    /// index when the player makes a selection.
    /// </summary>
    /// <returns>-1 if the player clicked the back button. Otherwise the item index.</returns>
    private async Task<int> CatchSelectedItem() {
        while (selectedItem < -1) await Task.Delay(50);
        int item = selectedItem;
        selectedItem = -2;
        return item;
    }

    /// <summary>
    /// Waits for the player to pick a special or go back. Returns the
    /// index when the player makes a selection.
    /// </summary>
    /// <returns>-1 if the player clicked the back button. Otherwise the special index.</returns>
    private async Task<int> CatchSelectedSpecial() {
        while (selectedSpecial < -1) await Task.Delay(50);
        int special = selectedSpecial;
        selectedSpecial = -2;
        return special;
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Actions to perform at the start of a unit's turn.
    /// </summary>
    /// <param name="unit">The BattleUnit starting their turn.</param>
    public void TurnStart(BattleUnit unit) {
        unit.unit.isDefending = false;
	}

    /// <summary>
    /// Displays a unit's floating HP bar for the enemyBarDuration.
    /// Hides the bar afterwards.
    /// </summary>
    /// <param name="unit">The BattleUnit to show the HP bar for.</param>
    private async void ShowHPBarAsync(BattleUnit unit) {
        unit.unitHUD.ShowHPBar();
        await Utilities.DoAfter(enemyBarDuration, () => unit.unitHUD.HideHPBar());
    }

    /// <summary>
    /// Displays a unit's floating SP bar for the enemyBarDuration.
    /// Hides the bar afterwards.
    /// </summary>
    /// <param name="unit">The BattleUnit to show the SP bar for.</param>
    private async void ShowSPBarAsync(BattleUnit unit) {
        unit.unitHUD.ShowSPBar();
        await Utilities.DoAfter(enemyBarDuration, () => unit.unitHUD.HideSPBar());
    }

    /// <summary>
    /// Spawns in the player's BattleUnit.
    /// </summary>
    /// <param name="unit">The PlayerUnit corresponding to the player.</param>
    /// <param name="location">The location to spawn the player.</param>
    /// <returns>The instantiated BattleUnit for the player.</returns>
    private BattleUnit SpawnPlayerUnit(PlayerUnit unit, Transform location) {
        BattleUnit battleUnit = Instantiate(unit.unitPrefab, location).GetComponent<BattleUnit>();
        battleUnit.unit = unit;
        battleUnit.unit.weapon = Instantiate(battleUnit.unit.weapon);
        return battleUnit;
    }

    /// <summary>
    /// Spawns in an enemy's BattleUnit.
    /// </summary>
    /// <param name="unit">The Unit corresponding to the enemy.</param>
    /// <param name="location">The location to spawn the enemy.</param>
    /// <returns>The instantiated BattleUnit for the enemy.</returns>
    private BattleUnit SpawnEnemyUnit(Unit unit, Transform location) {
        BattleUnit battleUnit = Instantiate(unit.unitPrefab, location).GetComponent<BattleUnit>();
        battleUnit.unit = Instantiate(unit);
        battleUnit.unit.weapon = Instantiate(battleUnit.unit.weapon);
        return battleUnit;
    }

    /// <summary>
    /// Spawns in all the enemies. Amount of enemies spawned will never exceed the enemies
    /// provided by the list nor will they exceed the amount of enemy spawn locations provided.
    /// </summary>
    /// <param name="units">The Units corresponding to the enemies.</param>
    /// <param name="locations">The locations the enemies can spawn on the map.</param>
    /// <returns>A list of the instantiated enemy BattleUnits.</returns>
    private List<BattleUnit> SpawnEnemies(List<Unit> units, List<Transform> locations) {
        List<BattleUnit> battleUnits = new List<BattleUnit>();

        for (int i = 0; i < units.Count && i < locations.Count; i++)
            battleUnits.Add(SpawnEnemyUnit(units[i], locations[i]));

        return battleUnits;
    }

    /// <summary>
    /// Randomly applies an offset to a value. The offset is given as a relative
    /// float, meaning that a value of 0.1 means a 10% offset.
    /// </summary>
    /// <param name="value">The value to offset.</param>
    /// <param name="offset">The actual offset itself.</param>
    /// <returns>The offset value.</returns>
    private int OffsetValue(int value, float offset) {
        // Randomly offsets an integer in the range of the offset
        float multiplier = 1 + UnityEngine.Random.Range(-offset, offset);

        return (int) (multiplier * value);
    }
    #endregion
}
