using PnPOrganizer.Core.Attributes;
using PnPOrganizer.Core.BattleAssistant;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.Core.Character.StatModifiers;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace PnPOrganizer.Services
{
    public class SkillsService : ISkillsService
    {
        public event EventHandler? ViewRefreshRequested;

        /// <summary>
        /// Contains every skill
        /// </summary>
        public Dictionary<SkillIdentifier, Skill> Registry { get; }

        #region Skill Declarations

        #region Character
        public readonly Skill Sneaking;
        public readonly Skill VirtuallyInvisible;
        public readonly Skill Intimidate;
        public readonly Skill Flirting;
        public readonly Skill NatureStudy;
        public readonly Skill Theft;
        public readonly Skill Lockpicking;
        public readonly Skill Counterfeiting;
        public readonly Skill KnowledgeOfPeople;
        public readonly Skill ActorByBirth;
        public readonly Skill Tracking;
        public readonly Skill PoisonKnowledge;
        public readonly Skill Gambling;
        public readonly Skill SkilledLier;
        public readonly Skill LieDetector;
        public readonly Skill SkilledSpeaker;
        public readonly Skill Climbing;
        public readonly Skill Teacher;
        public readonly Skill Plunge;
        public readonly Skill HardToKill;
        public readonly Skill Sympathic;
        public readonly Skill Alertness;
        public readonly Skill RescueIsNear;
        public readonly Skill Etiquette;
        public readonly Skill Trading;
        public readonly Skill Perseverence;
        public readonly Skill Encouragement;
        public readonly Skill EnergyBoost;
        public readonly Skill Momentum;
        public readonly Skill CommandTone;
        public readonly Skill Luck;
        public readonly Skill Healing;
        public readonly Skill LastBreath;
        public readonly Skill FutureMarket;
        public readonly Skill Avenger;

        public readonly Skill HP;
        public readonly Skill Profession;
        public readonly Skill Stamina2;
        public readonly Skill Stamina4;
        public readonly Skill Stats;
        public readonly Skill Energy3;
        public readonly Skill Energy6;
        public readonly Skill NextLevel;
        public readonly Skill NextElemental;
        public readonly Skill ElementalProfessionGreen;
        public readonly Skill ElementalProfessionYellow;
        public readonly Skill ElementalProfessionRed;
        public readonly Skill FourthElemental;
        #endregion Character

        #region Melee
        public readonly Skill LightBlow;
        public readonly Skill Smithing;
        public readonly Skill RunOver;
        public readonly Skill AimedAttackMelee;
        public readonly Skill WeaponsAndArmor;
        public readonly Skill Kick;
        public readonly Skill Taunt;
        public readonly Skill ArmorBreaker;
        public readonly Skill Assassinate;
        public readonly Skill JumpAttack;
        public readonly Skill OneHandedCombat;
        public readonly Skill SecondHand;
        public readonly Skill Shield;
        public readonly Skill DefensiveFighting;
        public readonly Skill Nimble;
        public readonly Skill AggressiveCombat;
        public readonly Skill Fencing;
        public readonly Skill FullDamage;
        public readonly Skill ShieldBash;
        public readonly Skill Parade;
        public readonly Skill ThereAndAway;
        public readonly Skill RecklessAttack;
        public readonly Skill DuplexFerrum;
        public readonly Skill SomethingWithShield;
        public readonly Skill QuickParade;
        public readonly Skill SkillfulRetreat;
        public readonly Skill Feint;
        public readonly Skill RoundHouseAttack;
        public readonly Skill HeavyParade;
        public readonly Skill PerfectBlock;
        public readonly Skill DevastatingAttack;
        public readonly Skill Cavallery;
        public readonly Skill DefensiveStance;
        public readonly Skill ArmorUp;
        public readonly Skill AccurateMelee;
        public readonly Skill RecognizeStyle;
        public readonly Skill CripplingBlow;
        public readonly Skill HijackerMelee;
        public readonly Skill Armor;
        public readonly Skill Combo;
        public readonly Skill PerfectBlow;
        public readonly Skill EveryBlowAHit;
        public readonly Skill TakeAHit;
        public readonly Skill AttackOfOpportunity;
        public readonly Skill DisarmMelee;
        public readonly Skill KillingSpree;
        public readonly Skill HeavyFighting;
        public readonly Skill Riposte;
        public readonly Skill LoneWarrior;
        public readonly Skill ChainAttack;
        public readonly Skill ShieldBreaker;
        public readonly Skill BladeFan;
        #endregion Melee

        #region Ranged
        public readonly Skill LightShot;
        public readonly Skill Quickdraw;
        public readonly Skill SkilledWithThrowingWeapons;
        public readonly Skill BetterThanThrowing;
        public readonly Skill CalmAiming;
        public readonly Skill DisarmRanged;
        public readonly Skill DualThrow;
        public readonly Skill SlingshotMarksman;
        public readonly Skill ShootFromTheSaddle;
        public readonly Skill BuildArrows;
        public readonly Skill PreciseThrow;
        public readonly Skill AimedAttackRanged;
        public readonly Skill AccurateRanged;
        public readonly Skill HijackerRanged;
        public readonly Skill BowMaking;
        public readonly Skill StrongThrow;
        public readonly Skill Headshot;
        public readonly Skill BackLine;
        public readonly Skill RoutinedWithThrowingWeapons;
        public readonly Skill ProfessionalSlingshotMarksman;
        public readonly Skill PerfectShot;
        public readonly Skill SurpriseAttack;
        public readonly Skill NailDown;
        public readonly Skill CurvedShot;
        public readonly Skill QuickAim;
        public readonly Skill StrongArrows;
        public readonly Skill PiercingArrow;
        public readonly Skill LuckyShot;
        public readonly Skill DoubleShot;
        public readonly Skill MasterfulArcher;
        public readonly Skill Magazine;
        public readonly Skill Oneshot;
        public readonly Skill LastShot;
        public readonly Skill HuntersMark;
        public readonly Skill Readiness;
        public readonly Skill MasterOfThrowingWeapons;
        public readonly Skill Trueshot;
        public readonly Skill Return;
        #endregion Ranged

        #endregion Skill Declarations

        // TODO Skills: implement and/or add every type of skill bonus
        public SkillsService()
        {
            Registry = new();

            var resourceLoader = ResourceLoader.GetForViewIndependentUse();

            #region Skill Definitions
            #region Character
            // Checkpoint 0
            Sneaking = CreateAndRegisterSkill(nameof(Sneaking), resourceLoader.GetString("Skills_SkillSneaking"), SkillCategory.Character, 2, resourceLoader.GetString("Skills_SkillSneakingDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Sneak, Dice.D4) });
            Intimidate = CreateAndRegisterSkill(nameof(Intimidate), resourceLoader.GetString("Skills_SkillIntimidate"), SkillCategory.Character, 2, resourceLoader.GetString("Skills_SkillIntimidateDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Intimidate, Dice.D4) });
            Flirting = CreateAndRegisterSkill(nameof(Flirting), resourceLoader.GetString("Skills_SkillFlirting"), SkillCategory.Character, 2, resourceLoader.GetString("Skills_SkillFlirtingDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Performance, 0, true) }); // Dummy StatModifier to show the skill on the AttributeTestsPage
            NatureStudy = CreateAndRegisterSkill(nameof(NatureStudy), resourceLoader.GetString("Skills_SkillNatureStudy"), SkillCategory.Character, 2, resourceLoader.GetString("Skills_SkillNatureStudyDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Nature, Dice.D4) });
            VirtuallyInvisible = CreateAndRegisterSkill(nameof(VirtuallyInvisible), resourceLoader.GetString("Skills_SkillVirtuallyInvisible"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillVirtuallyInvisibleDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Sneak, 1) },
                new SkillIdentifier[] { Sneaking.Identifier });
            Theft = CreateAndRegisterSkill(nameof(Theft), resourceLoader.GetString("Skills_SkillTheft"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillTheftDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.SleightOfHand, Dice.D4, 0, true) },
                new SkillIdentifier[] { Sneaking.Identifier });
            Lockpicking = CreateAndRegisterSkill(nameof(Lockpicking), resourceLoader.GetString("Skills_SkillLockpicking"), SkillCategory.Character, 1,
                resourceLoader.GetString("Skills_SkillLockpickingDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.SleightOfHand, Dice.D4, 0, true) },
                new SkillIdentifier[] { Sneaking.Identifier });
            Counterfeiting = CreateAndRegisterSkill(nameof(Counterfeiting), resourceLoader.GetString("Skills_SkillCounterfeiting"), SkillCategory.Character, 1,
                resourceLoader.GetString("Skills_SkillCounterfeitingDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.SleightOfHand, Dice.D4, 0, true) },
                new SkillIdentifier[] { Sneaking.Identifier });
            KnowledgeOfPeople = CreateAndRegisterSkill(nameof(KnowledgeOfPeople), resourceLoader.GetString("Skills_SkillKnowledgeOfPeople"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillKnowledgeOfPeopleDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Insight, 1) },
                new SkillIdentifier[] { Intimidate.Identifier, Flirting.Identifier });
            ActorByBirth = CreateAndRegisterSkill(nameof(ActorByBirth), resourceLoader.GetString("Skills_SkillActorByBirth"), SkillCategory.Character, 3,
                resourceLoader.GetString("Skills_SkillActorByBirthDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Performance, Dice.D4, 2) },
                new SkillIdentifier[] { KnowledgeOfPeople.Identifier });
            Tracking = CreateAndRegisterSkill(nameof(Tracking), resourceLoader.GetString("Skills_SkillTracking"), SkillCategory.Character, 1,
                resourceLoader.GetString("Skills_SkillTrackingDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Nature, Dice.D4, 0, true) },
                new SkillIdentifier[] { NatureStudy.Identifier });
            PoisonKnowledge = CreateAndRegisterSkill(nameof(PoisonKnowledge), resourceLoader.GetString("Skills_SkillPoisonKnowledge"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillPoisonKnowledgeDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Nature, Dice.D4, 0, true) },
                new SkillIdentifier[] { NatureStudy.Identifier });
            Gambling = CreateAndRegisterSkill(nameof(Gambling), resourceLoader.GetString("Skills_SkillGambling"), SkillCategory.Character, 1,
                resourceLoader.GetString("Skills_SkillGamblingDescr"),
                new IStatModifier[]
                {
                    new AttributeCheckStatModifier(AttributeCheckType.SleightOfHand, Dice.D4, 0, true),
                    new AttributeCheckStatModifier(AttributeCheckType.Performance, Dice.D4, 0, true)
                },
                new SkillIdentifier[] { Theft.Identifier, Lockpicking.Identifier, Counterfeiting.Identifier });
            SkilledLier = CreateAndRegisterSkill(nameof(SkilledLier), resourceLoader.GetString("Skills_SkillSkilledLier"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillSkilledLierDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Bluff, 1) },
                new SkillIdentifier[] { Theft.Identifier, Lockpicking.Identifier, Counterfeiting.Identifier });
            LieDetector = CreateAndRegisterSkill(nameof(LieDetector), resourceLoader.GetString("Skills_SkillLieDetector"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillLieDetectorDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Insight, Dice.D4, 0, true) },
                new SkillIdentifier[] { SkilledLier.Identifier, KnowledgeOfPeople.Identifier });
            SkilledSpeaker = CreateAndRegisterSkill(nameof(SkilledSpeaker), resourceLoader.GetString("Skills_SkillSkilledSpeaker"), SkillCategory.Character, 3,
                resourceLoader.GetString("Skills_SkillSkilledSpeakerDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Persuade, Dice.D4, 1) },
                new SkillIdentifier[] { KnowledgeOfPeople.Identifier });

            // Checkpoint 1
            Climbing = CreateAndRegisterSkill(nameof(Climbing), resourceLoader.GetString("Skills_SkillClimbing"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillClimbingDescr"),
                new IStatModifier[]
                {
                    new AttributeCheckStatModifier(AttributeCheckType.Athletics, Dice.D4, 0, true),
                    new AttributeCheckStatModifier(AttributeCheckType.Acrobatics, Dice.D4, 0, true)
                },
                new SkillIdentifier[] { Gambling.Identifier, SkilledLier.Identifier, LieDetector.Identifier, SkilledSpeaker.Identifier, ActorByBirth.Identifier, Tracking.Identifier, PoisonKnowledge.Identifier });
            Teacher = CreateAndRegisterSkill(nameof(Teacher), resourceLoader.GetString("Skills_SkillTeacher"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillTeacherDescr"), null,
                new SkillIdentifier[] { Gambling.Identifier, SkilledLier.Identifier, LieDetector.Identifier, SkilledSpeaker.Identifier, ActorByBirth.Identifier, Tracking.Identifier, PoisonKnowledge.Identifier });
            Plunge = CreateAndRegisterSkill(nameof(Plunge), resourceLoader.GetString("Skills_SkillPlunge"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillPlungeDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, 8) },
                new SkillIdentifier[] { Gambling.Identifier, SkilledLier.Identifier, LieDetector.Identifier, SkilledSpeaker.Identifier, ActorByBirth.Identifier, Tracking.Identifier, PoisonKnowledge.Identifier },
                activationType: ActivationType.Active, staminaCost: 3);
            HardToKill = CreateAndRegisterSkill(nameof(HardToKill), resourceLoader.GetString("Skills_SkillHardToKill"), SkillCategory.Character, 5,
                resourceLoader.GetString("Skills_SkillHardToKillDescr"), null,
                new SkillIdentifier[] { Gambling.Identifier, SkilledLier.Identifier, LieDetector.Identifier, SkilledSpeaker.Identifier, ActorByBirth.Identifier, Tracking.Identifier, PoisonKnowledge.Identifier });
            Sympathic = CreateAndRegisterSkill(nameof(Sympathic), resourceLoader.GetString("Skills_SkillSympathic"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillSympathicDescr"), null,
                new SkillIdentifier[] { Gambling.Identifier, SkilledLier.Identifier, LieDetector.Identifier, SkilledSpeaker.Identifier, ActorByBirth.Identifier, Tracking.Identifier, PoisonKnowledge.Identifier });
            Alertness = CreateAndRegisterSkill(nameof(Alertness), resourceLoader.GetString("Skills_SkillAlertness"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillAlertnessDescr"),
                new IStatModifier[]
                {
                    new AttributeCheckStatModifier(AttributeCheckType.Perceive, 1),
                    new AttributeCheckStatModifier(AttributeCheckType.Inspect, 1)
                },
                new SkillIdentifier[] { Gambling.Identifier, SkilledLier.Identifier, LieDetector.Identifier, SkilledSpeaker.Identifier, ActorByBirth.Identifier, Tracking.Identifier, PoisonKnowledge.Identifier });
            RescueIsNear = CreateAndRegisterSkill(nameof(RescueIsNear), resourceLoader.GetString("Skills_SkillRescueIsNear"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillRescueIsNearDescr"), null, new SkillIdentifier[] { Plunge.Identifier }, activationType: ActivationType.Active);
            Etiquette = CreateAndRegisterSkill(nameof(Etiquette), resourceLoader.GetString("Skills_SkillEtiquette"), SkillCategory.Character, 1,
                resourceLoader.GetString("Skills_SkillEtiquetteDescr"), null, new SkillIdentifier[] { Sympathic.Identifier });
            Trading = CreateAndRegisterSkill(nameof(Trading), resourceLoader.GetString("Skills_SkillTrading"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillTradingDescr"), new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Persuade, Dice.D4, 0, true) },
                new SkillIdentifier[] { Sympathic.Identifier });

            // Checkpoint 2
            Perseverence = CreateAndRegisterSkill(nameof(Perseverence), resourceLoader.GetString("Skills_SkillPerseverence"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillPerseverenceDescr"), null,
                new SkillIdentifier[] { Climbing.Identifier, Teacher.Identifier, RescueIsNear.Identifier, HardToKill.Identifier, Etiquette.Identifier, Trading.Identifier, Alertness.Identifier });
            Encouragement = CreateAndRegisterSkill(nameof(Encouragement), resourceLoader.GetString("Skills_SkillEncouragement"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillEncouragementDescr"), null,
                new SkillIdentifier[] { Climbing.Identifier, Teacher.Identifier, RescueIsNear.Identifier, HardToKill.Identifier, Etiquette.Identifier, Trading.Identifier, Alertness.Identifier });
            EnergyBoost = CreateAndRegisterSkill(nameof(EnergyBoost), resourceLoader.GetString("Skills_SkillEnergyBoost"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillEnergyBoostDescr"), null,
                new SkillIdentifier[] { Climbing.Identifier, Teacher.Identifier, RescueIsNear.Identifier, HardToKill.Identifier, Etiquette.Identifier, Trading.Identifier, Alertness.Identifier });
            Momentum = CreateAndRegisterSkill(nameof(Momentum), resourceLoader.GetString("Skills_SkillMomentum"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillMomentumDescr"), null,
                new SkillIdentifier[] { Climbing.Identifier, Teacher.Identifier, RescueIsNear.Identifier, HardToKill.Identifier, Etiquette.Identifier, Trading.Identifier, Alertness.Identifier },
                activationType: ActivationType.Active);
            CommandTone = CreateAndRegisterSkill(nameof(CommandTone), resourceLoader.GetString("Skills_SkillCommandTone"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillCommandToneDescr"), null,
                new SkillIdentifier[] { Climbing.Identifier, Teacher.Identifier, RescueIsNear.Identifier, HardToKill.Identifier, Etiquette.Identifier, Trading.Identifier, Alertness.Identifier });
            Luck = CreateAndRegisterSkill(nameof(Luck), resourceLoader.GetString("Skills_SkillLuck"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillLuckDescr"), null,
                new SkillIdentifier[] { Climbing.Identifier, Teacher.Identifier, RescueIsNear.Identifier, HardToKill.Identifier, Etiquette.Identifier, Trading.Identifier, Alertness.Identifier },
                activationType: ActivationType.Active);
            Healing = CreateAndRegisterSkill(nameof(Healing), resourceLoader.GetString("Skills_SkillHealing"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillHealingDescr"), null,
                new SkillIdentifier[] { Climbing.Identifier, Teacher.Identifier, RescueIsNear.Identifier, HardToKill.Identifier, Etiquette.Identifier, Trading.Identifier, Alertness.Identifier });
            LastBreath = CreateAndRegisterSkill(nameof(LastBreath), resourceLoader.GetString("Skills_SkillLastBreath"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillLastBreathDescr"), null, new SkillIdentifier[] { Perseverence.Identifier }, activationType: ActivationType.Active, usesPerBattle: 1);
            FutureMarket = CreateAndRegisterSkill(nameof(FutureMarket), resourceLoader.GetString("Skills_SkillFutureMarket"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillFutureMarketDescr"), null, new SkillIdentifier[] { Perseverence.Identifier }, activationType: ActivationType.Active, usesPerBattle: 1);
            Avenger = CreateAndRegisterSkill(nameof(Avenger), resourceLoader.GetString("Skills_SkillAvenger"), SkillCategory.Character, 2,
                resourceLoader.GetString("Skills_SkillAvengerDescr"), null, new SkillIdentifier[] { Momentum.Identifier }, activationType: ActivationType.Active);

            // Repeatable Skills
            var hpID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(HP)
            };
            HP = RegisterSkill(new Skill(hpID, resourceLoader.GetString("Skills_SkillHP"), 3, resourceLoader.GetString("Skills_SkillHPDescr"),
                new IStatModifier[] { new CharacterStatModifier(nameof(CharacterPageViewModel.MaxHealthModifierBonus), 6) }).SetRepeatable());
            var professionID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(Profession)
            };
            Profession = RegisterSkill(new Skill(professionID, resourceLoader.GetString("Skills_SkillProfession"), 3, resourceLoader.GetString("Skills_SkillProfessionDescr"), null).SetRepeatable());
            var stamina2ID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(Stamina2)
            };
            Stamina2 = RegisterSkill(new Skill(stamina2ID, resourceLoader.GetString("Skills_SkillStamina2"), 3, resourceLoader.GetString("Skills_SkillStamina2Descr"),
                new IStatModifier[] { new CharacterStatModifier(nameof(CharacterPageViewModel.MaxStaminaModifierBonus), 2) }).SetRepeatable());
            var stamina4ID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(Stamina4)
            };
            Stamina4 = RegisterSkill(new Skill(stamina4ID, resourceLoader.GetString("Skills_SkillStamina4"), 5, resourceLoader.GetString("Skills_SkillStamina4Descr"),
                new IStatModifier[] { new CharacterStatModifier(nameof(CharacterPageViewModel.MaxStaminaModifierBonus), 4) }).SetRepeatable());
            var statsID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(Stats)
            };
            Stats = RegisterSkill(new Skill(statsID, resourceLoader.GetString("Skills_SkillStats"), 5, resourceLoader.GetString("Skills_SkillStatsDescr"), null).SetRepeatable());
            var energy3ID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(Energy3)
            };
            Energy3 = RegisterSkill(new Skill(energy3ID, resourceLoader.GetString("Skills_SkillEnergy3"), 3, resourceLoader.GetString("Skills_SkillEnergy3Descr"),
                new IStatModifier[] { new CharacterStatModifier(nameof(CharacterPageViewModel.MaxEnergyModifierBonus), 3) }).SetRepeatable());
            var energy6ID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(Energy6)
            };
            Energy6 = RegisterSkill(new Skill(energy6ID, resourceLoader.GetString("Skills_SkillEnergy6"), 5, resourceLoader.GetString("Skills_SkillEnergy6Descr"),
                new IStatModifier[] { new CharacterStatModifier(nameof(CharacterPageViewModel.MaxEnergyModifierBonus), 6) }).SetRepeatable());
            var nextLevelID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(NextLevel)
            };
            NextLevel = RegisterSkill(new Skill(nextLevelID, resourceLoader.GetString("Skills_SkillNextLevel"), 4, resourceLoader.GetString("Skills_SkillNextLevelDescr"), null).SetRepeatable());
            var nextElementalID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(NextElemental)
            };
            NextElemental = RegisterSkill(new Skill(nextElementalID, resourceLoader.GetString("Skills_SkillNextElemental"), 5, resourceLoader.GetString("Skills_SkillNextElementalDescr"), null).SetRepeatable());
            var elemProfGreenID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(ElementalProfessionGreen)
            };
            ElementalProfessionGreen = RegisterSkill(new Skill(elemProfGreenID, resourceLoader.GetString("Skills_SkillElementalProfessionGreen"), 1, resourceLoader.GetString("Skills_SkillElementalProfessionGreenDescr"), null).SetRepeatable());
            var elemProfYellowID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(ElementalProfessionYellow)
            };
            ElementalProfessionYellow = RegisterSkill(new Skill(elemProfYellowID, resourceLoader.GetString("Skills_SkillElementalProfessionYellow"), 3, resourceLoader.GetString("Skills_SkillElementalProfessionYellowDescr"), null).SetRepeatable());
            var elemProfRedID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(ElementalProfessionRed)
            };
            ElementalProfessionRed = RegisterSkill(new Skill(elemProfRedID, resourceLoader.GetString("Skills_SkillElementalProfessionRed"), 5, resourceLoader.GetString("Skills_SkillElementalProfessionRedDescr")).SetRepeatable());
            var fourthElementalID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Character,
                Name = nameof(FourthElemental)
            };
            FourthElemental = RegisterSkill(new Skill(fourthElementalID, resourceLoader.GetString("Skills_SkillFourthElemental"), 6, resourceLoader.GetString("Skills_SkillFourthElementalDescr"), null).SetRepeatable());

            #endregion Character

            #region Melee
            // Checkpoint 0
            LightBlow = CreateAndRegisterSkill(nameof(LightBlow), resourceLoader.GetString("Skills_SkillLightBlow"), SkillCategory.Melee, 1, resourceLoader.GetString("Skills_SkillLightBlowDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, Dice.D6, 2.0),
                    new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.EndValue, 0.5, CalculatorBonusType.Multiplicative)
                }, activationType: ActivationType.Active, staminaCost: 1);
            Smithing = CreateAndRegisterSkill(nameof(Smithing), resourceLoader.GetString("Skills_SkillSmithing"), SkillCategory.Melee, 1, resourceLoader.GetString("Skills_SkillSmithingDescr"),
                new IStatModifier[] { new AttributeCheckStatModifier(AttributeCheckType.Performance, 0, true) }); // Dummy StatModifier to show the skill on the AttributeTestsPage
            RunOver = CreateAndRegisterSkill(nameof(RunOver), resourceLoader.GetString("Skills_SkillRunOver"), SkillCategory.Melee, 2, resourceLoader.GetString("Skills_SkillRunOverDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, Dice.D4, 2) },
                activationType: ActivationType.Active, staminaCost: 3);
            AimedAttackMelee = CreateAndRegisterSkill(nameof(AimedAttackMelee), resourceLoader.GetString("Skills_SkillAimedAttackMelee"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillAimedAttackMeleeDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, -5) },
                new SkillIdentifier[] { LightBlow.Identifier }, activationType: ActivationType.Active, staminaCost: 2);
            WeaponsAndArmor = CreateAndRegisterSkill(nameof(WeaponsAndArmor), resourceLoader.GetString("Skills_SkillWeaponsAndArmor"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillWeaponsAndArmorDescr"), null, new SkillIdentifier[] { Smithing.Identifier });
            Kick = CreateAndRegisterSkill(nameof(Kick), resourceLoader.GetString("Skills_SkillKick"), SkillCategory.Melee, 1,
                resourceLoader.GetString("Skills_SkillKickDescr"), null, new SkillIdentifier[] { RunOver.Identifier }, activationType: ActivationType.Active, staminaCost: 1);
            Taunt = CreateAndRegisterSkill(nameof(Taunt), resourceLoader.GetString("Skills_SkillTaunt"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillTauntDescr"), null, new SkillIdentifier[] { RunOver.Identifier });
            ArmorBreaker = CreateAndRegisterSkill(nameof(ArmorBreaker), resourceLoader.GetString("Skills_SkillArmorBreaker"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillArmorBreakerDescr"), null, new SkillIdentifier[] { AimedAttackMelee.Identifier }, activationType: ActivationType.Active,
                staminaCost: 5);
            var assassinateID = new SkillIdentifier
            {
                SkillCategory = SkillCategory.Melee,
                Name = nameof(Assassinate)
            };
            Assassinate = RegisterSkill(new Skill(assassinateID, resourceLoader.GetString("Skills_SkillAssassinate"), 2,
                resourceLoader.GetString("Skills_SkillAssassinateDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.EndValue, 3, CalculatorBonusType.Multiplicative) },
                new SkillIdentifier[] { AimedAttackMelee.Identifier }, activationType: ActivationType.Active, staminaCost: 1)
                .AddForcedDependency(Sneaking.Identifier));
            JumpAttack = CreateAndRegisterSkill(nameof(JumpAttack), resourceLoader.GetString("Skills_SkillJumpAttack"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillJumpAttackDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, Dice.D6, 2) },
                new SkillIdentifier[] { Kick.Identifier }, activationType: ActivationType.Active, staminaCost: 1);

            // Checkpoint 1
            OneHandedCombat = CreateAndRegisterSkill(nameof(OneHandedCombat), resourceLoader.GetString("Skills_SkillOneHandedFighting"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillOneHandedFightingDescr"),
                new IStatModifier[]
                {
                    new CharacterStatModifier(nameof(CharacterPageViewModel.InitiativeModifierBonus), 1),
                    new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, 1),
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 1),
                },
                new SkillIdentifier[] { AimedAttackMelee.Identifier, ArmorBreaker.Identifier, Assassinate.Identifier, WeaponsAndArmor.Identifier, JumpAttack.Identifier, Taunt.Identifier });
            SecondHand = CreateAndRegisterSkill(nameof(SecondHand), resourceLoader.GetString("Skills_SkillSecondHand"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillSecondHandDescr"), null,
                new SkillIdentifier[] { AimedAttackMelee.Identifier, ArmorBreaker.Identifier, Assassinate.Identifier, WeaponsAndArmor.Identifier, JumpAttack.Identifier, Taunt.Identifier });
            Shield = CreateAndRegisterSkill(nameof(Shield), resourceLoader.GetString("Skills_SkillShield"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillShieldDescr"), null,
                new SkillIdentifier[] { AimedAttackMelee.Identifier, ArmorBreaker.Identifier, Assassinate.Identifier, WeaponsAndArmor.Identifier, JumpAttack.Identifier, Taunt.Identifier });
            DefensiveFighting = CreateAndRegisterSkill(nameof(DefensiveFighting), resourceLoader.GetString("Skills_SkillDefensiveFighting"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillDefensiveFightingDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, 2),
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, -2),
                },
                new SkillIdentifier[] { AimedAttackMelee.Identifier, ArmorBreaker.Identifier, Assassinate.Identifier, WeaponsAndArmor.Identifier, JumpAttack.Identifier, Taunt.Identifier },
                activationType: ActivationType.Active);
            Nimble = CreateAndRegisterSkill(nameof(Nimble), resourceLoader.GetString("Skills_SkillNimble"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillNimbleDescr"), new IStatModifier[] { new CharacterStatModifier(nameof(CharacterPageViewModel.InitiativeModifierBonus), 2) },
                new SkillIdentifier[] { AimedAttackMelee.Identifier, ArmorBreaker.Identifier, Assassinate.Identifier, WeaponsAndArmor.Identifier, JumpAttack.Identifier, Taunt.Identifier });
            AggressiveCombat = CreateAndRegisterSkill(nameof(AggressiveCombat), resourceLoader.GetString("Skills_SkillAgressiveFighting"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillAgressiveFightingDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, -2),
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 2),
                },
                new SkillIdentifier[] { AimedAttackMelee.Identifier, ArmorBreaker.Identifier, Assassinate.Identifier, WeaponsAndArmor.Identifier, JumpAttack.Identifier, Taunt.Identifier },
                activationType: ActivationType.Active);
            Fencing = CreateAndRegisterSkill(nameof(Fencing), resourceLoader.GetString("Skills_SkillFencing"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillFencingDescr"), null, new SkillIdentifier[] { OneHandedCombat.Identifier });
            FullDamage = CreateAndRegisterSkill(nameof(FullDamage), resourceLoader.GetString("Skills_SkillFullDamage"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillFullDamageDescr"), null, new SkillIdentifier[] { SecondHand.Identifier });
            ShieldBash = CreateAndRegisterSkill(nameof(ShieldBash), resourceLoader.GetString("Skills_SkillShieldBash"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillShieldBashDescr"), null, new SkillIdentifier[] { Shield.Identifier }, activationType: ActivationType.Active, staminaCost: 2);
            Parade = CreateAndRegisterSkill(nameof(Parade), resourceLoader.GetString("Skills_SkillHeavyParade"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillParadeDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, Dice.D4) },
                new SkillIdentifier[] { DefensiveFighting.Identifier });
            ThereAndAway = CreateAndRegisterSkill(nameof(ThereAndAway), resourceLoader.GetString("Skills_SkillThereAndAway"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillThereAndAwayDescr"), null, new SkillIdentifier[] { Nimble.Identifier });
            RecklessAttack = CreateAndRegisterSkill(nameof(RecklessAttack), resourceLoader.GetString("Skills_SkillRecklessAttack"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillRecklessAttackDescr"), null, new SkillIdentifier[] { AggressiveCombat.Identifier }, activationType: ActivationType.Active,
                staminaCost: 3);
            DuplexFerrum = CreateAndRegisterSkill(nameof(DuplexFerrum), resourceLoader.GetString("Skills_SkillDuplexFerrum"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillDuplexFerrumDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, Dice.D4) },
                new SkillIdentifier[] { FullDamage.Identifier });
            SomethingWithShield = CreateAndRegisterSkill(nameof(SomethingWithShield), resourceLoader.GetString("Skills_SkillSomethingWithShield"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillSomethingWithShieldDescr"), null, new SkillIdentifier[] { ShieldBash.Identifier }, activationType: ActivationType.Active,
                staminaCost: 3);
            QuickParade = CreateAndRegisterSkill(nameof(QuickParade), resourceLoader.GetString("Skills_SkillQuickParade"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillQuickParadeDescr"), null, new SkillIdentifier[] { Parade.Identifier }, activationType: ActivationType.Active,
                staminaCost: 1);
            SkillfulRetreat = CreateAndRegisterSkill(nameof(SkillfulRetreat), resourceLoader.GetString("Skills_SkillSkillfulRetreat"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillSkillfulRetreatDescr"), null, new SkillIdentifier[] { DefensiveFighting.Identifier, ThereAndAway.Identifier }, activationType: ActivationType.Active,
                staminaCost: 1);
            Feint = CreateAndRegisterSkill(nameof(Feint), resourceLoader.GetString("Skills_SkillFeint"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillFeintDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 4) },
                new SkillIdentifier[] { Nimble.Identifier });
            RoundHouseAttack = CreateAndRegisterSkill(nameof(RoundHouseAttack), resourceLoader.GetString("Skills_SkillRoundHouseAttack"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillRoundHouseAttackDescr"), null, new SkillIdentifier[] { RecklessAttack.Identifier }, activationType: ActivationType.Active, staminaCost: 3);
            HeavyParade = CreateAndRegisterSkill(nameof(HeavyParade), resourceLoader.GetString("Skills_SkillHeavyParade"), SkillCategory.Melee, 5,
                resourceLoader.GetString("Skills_SkillHeavyParadeDescr"), null, new SkillIdentifier[] { QuickParade.Identifier }, activationType: ActivationType.Active, staminaCost: 2);
            PerfectBlock = CreateAndRegisterSkill(nameof(PerfectBlock), resourceLoader.GetString("Skills_SkillPerfectBlock"), SkillCategory.Melee, 4,
                resourceLoader.GetString("Skills_SkillPerfectBlockDescr"), null, new SkillIdentifier[] { QuickParade.Identifier }, activationType: ActivationType.Active, staminaCost: 5);
            DevastatingAttack = CreateAndRegisterSkill(nameof(DevastatingAttack), resourceLoader.GetString("Skills_SkillDevastatingAttack"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillDevastatingAttackDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.EndValue, 2, CalculatorBonusType.Multiplicative) },
                new SkillIdentifier[] { RoundHouseAttack.Identifier }, activationType: ActivationType.Active, staminaCost: 3);

            // Checkpoint 2
            Cavallery = CreateAndRegisterSkill(nameof(Cavallery), resourceLoader.GetString("Skills_SkillCavallery"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillCavalleryDescr"), null,
                new SkillIdentifier[] { Fencing.Identifier, DuplexFerrum.Identifier, HeavyParade.Identifier, PerfectBlock.Identifier, SkillfulRetreat.Identifier, Feint.Identifier, DevastatingAttack.Identifier });
            DefensiveStance = CreateAndRegisterSkill(nameof(DefensiveStance), resourceLoader.GetString("Skills_SkillDefensiveStance"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillDefensiveStanceDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Armor, ApplianceMode.EndValue, 2, CalculatorBonusType.Multiplicative),
                    new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, Dice.D6, 2),
                },
                new SkillIdentifier[] { Fencing.Identifier, DuplexFerrum.Identifier, HeavyParade.Identifier, PerfectBlock.Identifier, SkillfulRetreat.Identifier, Feint.Identifier, DevastatingAttack.Identifier },
                activationType: ActivationType.Active, staminaCost: 1);
            ArmorUp = CreateAndRegisterSkill(nameof(ArmorUp), resourceLoader.GetString("Skills_SkillArmorUp"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillArmorUpDescr"), null, new SkillIdentifier[] { DefensiveStance.Identifier }, ActivationType.Active, staminaCost: 1);
            AccurateMelee = CreateAndRegisterSkill(nameof(AccurateMelee), resourceLoader.GetString("Skills_SkillAccurateMelee"), SkillCategory.Melee, 1,
                resourceLoader.GetString("Skills_SkillAccurateMeleeDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, 2) },
                new SkillIdentifier[] { Fencing.Identifier, DuplexFerrum.Identifier, HeavyParade.Identifier, PerfectBlock.Identifier, SkillfulRetreat.Identifier, Feint.Identifier, DevastatingAttack.Identifier });
            RecognizeStyle = CreateAndRegisterSkill(nameof(RecognizeStyle), resourceLoader.GetString("Skills_SkillRecognizeStyle"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillRecognizeStyleDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 1),
                    new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, 1)
                },
                new SkillIdentifier[] { Fencing.Identifier, DuplexFerrum.Identifier, HeavyParade.Identifier, PerfectBlock.Identifier, SkillfulRetreat.Identifier, Feint.Identifier, DevastatingAttack.Identifier });
            CripplingBlow = CreateAndRegisterSkill(nameof(CripplingBlow), resourceLoader.GetString("Skills_SkillCripplingBlow"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillCripplingBlowDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, -1) },
                new SkillIdentifier[] { RecognizeStyle.Identifier }, activationType: ActivationType.Active, staminaCost: 2);
            HijackerMelee = CreateAndRegisterSkill(nameof(HijackerMelee), resourceLoader.GetString("Skills_SkillHijackerMelee"), SkillCategory.Melee, 1,
                resourceLoader.GetString("Skills_SkillHijackerMeleeDescr"), null, new SkillIdentifier[] { Cavallery.Identifier }, activationType: ActivationType.Active,
                staminaCost: 3);
            Armor = CreateAndRegisterSkill(nameof(Armor), resourceLoader.GetString("Skills_SkillArmor"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillArmorDescr"), null, new SkillIdentifier[] { DefensiveStance.Identifier });
            Combo = CreateAndRegisterSkill(nameof(Combo), resourceLoader.GetString("Skills_SkillCombo"), SkillCategory.Melee, 4,
                resourceLoader.GetString("Skills_SkillComboDescr"), null, new SkillIdentifier[] { AccurateMelee.Identifier });
            PerfectBlow = CreateAndRegisterSkill(nameof(PerfectBlow), resourceLoader.GetString("Skills_SkillPerfectBlow"), SkillCategory.Melee, 4,
                resourceLoader.GetString("Skills_SkillPerfectBlowDescr"), null, new SkillIdentifier[] { AccurateMelee.Identifier }, activationType: ActivationType.Active,
                staminaCost: 5).SetOnlySoloUsable();
            EveryBlowAHit = CreateAndRegisterSkill(nameof(EveryBlowAHit), resourceLoader.GetString("Skills_SkillEveryBlowAHit"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillEveryBlowAHitDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 2) },
                new SkillIdentifier[] { AccurateMelee.Identifier });
            TakeAHit = CreateAndRegisterSkill(nameof(TakeAHit), resourceLoader.GetString("Skills_SkillTakeAHit"), SkillCategory.Melee, 1,
                resourceLoader.GetString("Skills_SkillTakeAHitDescr"), null, new SkillIdentifier[] { RecognizeStyle.Identifier }, activationType: ActivationType.Active);

            // Checkpoint 3
            AttackOfOpportunity = CreateAndRegisterSkill(nameof(AttackOfOpportunity), resourceLoader.GetString("Skills_SkillAttackOfOpportunity"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillAttackOfOpportunityDescr"), null,
                new SkillIdentifier[] { HijackerMelee.Identifier, Armor.Identifier, ArmorUp.Identifier, Combo.Identifier, PerfectBlow.Identifier, EveryBlowAHit.Identifier, TakeAHit.Identifier },
                activationType: ActivationType.Active, staminaCost: 1);
            DisarmMelee = CreateAndRegisterSkill(nameof(DisarmMelee), resourceLoader.GetString("Skills_SkillDisarmMelee"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillDisarmMeleeDescr"), null,
                new SkillIdentifier[] { HijackerMelee.Identifier, Armor.Identifier, ArmorUp.Identifier, Combo.Identifier, PerfectBlow.Identifier, EveryBlowAHit.Identifier, TakeAHit.Identifier },
                activationType: ActivationType.Active, staminaCost: 1);
            KillingSpree = CreateAndRegisterSkill(nameof(KillingSpree), resourceLoader.GetString("Skills_SkillKillingSpree"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillKillingSpreeDescr"), null,
                new SkillIdentifier[] { HijackerMelee.Identifier, Armor.Identifier, ArmorUp.Identifier, Combo.Identifier, PerfectBlow.Identifier, EveryBlowAHit.Identifier, TakeAHit.Identifier });
            HeavyFighting = CreateAndRegisterSkill(nameof(HeavyFighting), resourceLoader.GetString("Skills_SkillHeavyFighting"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillHeavyFightingDescr"), null,
                new SkillIdentifier[] { HijackerMelee.Identifier, Armor.Identifier, ArmorUp.Identifier, Combo.Identifier, PerfectBlow.Identifier, EveryBlowAHit.Identifier, TakeAHit.Identifier });
            Riposte = CreateAndRegisterSkill(nameof(Riposte), resourceLoader.GetString("Skills_SkillRiposte"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillRiposteDescr"), null,
                new SkillIdentifier[] { HijackerMelee.Identifier, Armor.Identifier, ArmorUp.Identifier, Combo.Identifier, PerfectBlow.Identifier, EveryBlowAHit.Identifier, TakeAHit.Identifier });
            LoneWarrior = CreateAndRegisterSkill(nameof(LoneWarrior), resourceLoader.GetString("Skills_SkillLoneWarrior"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillLoneWarriorDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.BaseValue, 1),
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 1),
                    new CalculatorStatModifier(CalculatorValueType.Parry, ApplianceMode.BaseValue, 1),
                    new CalculatorStatModifier(CalculatorValueType.Armor, ApplianceMode.BaseValue, 1),
                },
                new SkillIdentifier[] { HijackerMelee.Identifier, Armor.Identifier, ArmorUp.Identifier, Combo.Identifier, PerfectBlow.Identifier, EveryBlowAHit.Identifier, TakeAHit.Identifier },
                activationType: ActivationType.Active);
            ChainAttack = CreateAndRegisterSkill(nameof(ChainAttack), resourceLoader.GetString("Skills_SkillChainAttack"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillChainAttackDescr"), null, new SkillIdentifier[] { DisarmMelee.Identifier },
                activationType: ActivationType.Active, staminaCost: 1);
            ShieldBreaker = CreateAndRegisterSkill(nameof(ShieldBreaker), resourceLoader.GetString("Skills_SkillShieldBreaker"), SkillCategory.Melee, 2,
                resourceLoader.GetString("Skills_SkillShieldBreakerDescr"), null, new SkillIdentifier[] { HeavyFighting.Identifier },
                activationType: ActivationType.Active);
            BladeFan = CreateAndRegisterSkill(nameof(BladeFan), resourceLoader.GetString("Skills_SkillBladeFan"), SkillCategory.Melee, 3,
                resourceLoader.GetString("Skills_SkillBladeFanDescr"), null, new SkillIdentifier[] { Riposte.Identifier },
                activationType: ActivationType.Active, staminaCost: 1, energyCost: 1);

            #endregion Melee

            #region Ranged
            // Checkpoint 0
            LightShot = CreateAndRegisterSkill(nameof(LightShot), resourceLoader.GetString("Skills_SkillLightShot"), SkillCategory.Ranged, 1, resourceLoader.GetString("Skills_SkillLightShotDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, Dice.D6, 2.0),
                    new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.EndValue, 0.5, CalculatorBonusType.Multiplicative)
                }, activationType: ActivationType.Active, staminaCost: 1);
            Quickdraw = CreateAndRegisterSkill(nameof(Quickdraw), resourceLoader.GetString("Skills_SkillQuickdraw"), SkillCategory.Ranged, 1, resourceLoader.GetString("Skills_SkillQuickdrawDescr"), null,
                activationType: ActivationType.Active, staminaCost: 1);
            SkilledWithThrowingWeapons = CreateAndRegisterSkill(nameof(SkilledWithThrowingWeapons), resourceLoader.GetString("Skills_SkillSkilledWithThrowingWeapons"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillSkilledWithThrowingWeaponsDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 1) });
            BetterThanThrowing = CreateAndRegisterSkill(nameof(BetterThanThrowing), resourceLoader.GetString("Skills_SkillBetterThanThrowing"), SkillCategory.Ranged, 1, resourceLoader.GetString("Skills_SkillBetterThanThrowingDescr"));
            CalmAiming = CreateAndRegisterSkill(nameof(CalmAiming), resourceLoader.GetString("Skills_SkillCalmAiming"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillCalmAimingDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 2) },
                new SkillIdentifier[] { LightShot.Identifier }, activationType: ActivationType.Active);
            DisarmRanged = CreateAndRegisterSkill(nameof(DisarmRanged), resourceLoader.GetString("Skills_SkillDisarmRanged"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillDisarmRangedDescr"), null, new SkillIdentifier[] { Quickdraw.Identifier },
                activationType: ActivationType.Active, staminaCost: 2);
            DualThrow = CreateAndRegisterSkill(nameof(DualThrow), resourceLoader.GetString("Skills_SkillDualThrow"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillDualThrowDescr"), null, new SkillIdentifier[] { SkilledWithThrowingWeapons.Identifier });
            SlingshotMarksman = CreateAndRegisterSkill(nameof(SlingshotMarksman), resourceLoader.GetString("Skills_SkillSlingshotMarksman"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillSlingshotMarksmanDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, Dice.D6, 2) },
                new SkillIdentifier[] { BetterThanThrowing.Identifier });

            // Checkpoint 1
            ShootFromTheSaddle = CreateAndRegisterSkill(nameof(ShootFromTheSaddle), resourceLoader.GetString("Skills_SkillShootFromTheSaddle"), SkillCategory.Ranged, 3,
                resourceLoader.GetString("Skills_SkillShootFromTheSaddleDescr"), null, new SkillIdentifier[] { CalmAiming.Identifier, DisarmRanged.Identifier, DualThrow.Identifier, SlingshotMarksman.Identifier });
            BuildArrows = CreateAndRegisterSkill(nameof(BuildArrows), resourceLoader.GetString("Skills_SkillBuildArrows"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillBuildArrowsDescr"), null, new SkillIdentifier[] { CalmAiming.Identifier, DisarmRanged.Identifier, DualThrow.Identifier, SlingshotMarksman.Identifier });
            PreciseThrow = CreateAndRegisterSkill(nameof(PreciseThrow), resourceLoader.GetString("Skills_SkillPreciseThrow"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillPreciseThrowDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, -4) },
                new SkillIdentifier[] { CalmAiming.Identifier, DisarmRanged.Identifier, DualThrow.Identifier, SlingshotMarksman.Identifier },
                activationType: ActivationType.Active, staminaCost: 1);
            AimedAttackRanged = CreateAndRegisterSkill(nameof(AimedAttackRanged), resourceLoader.GetString("Skills_SkillAimedAttackRanged"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillAimedAttackRangedDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, -5) },
                new SkillIdentifier[] { CalmAiming.Identifier, DisarmRanged.Identifier, DualThrow.Identifier, SlingshotMarksman.Identifier },
                activationType: ActivationType.Active, staminaCost: 2);
            AccurateRanged = CreateAndRegisterSkill(nameof(AccurateRanged), resourceLoader.GetString("Skills_SkillAccurateRanged"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillAccurateRangedDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 1) },
                new SkillIdentifier[] { CalmAiming.Identifier, DisarmRanged.Identifier, DualThrow.Identifier, SlingshotMarksman.Identifier });
            HijackerRanged = CreateAndRegisterSkill(nameof(HijackerRanged), resourceLoader.GetString("Skills_SkillHijackerRanged"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillHijackerRangedDescr"), null, new SkillIdentifier[] { ShootFromTheSaddle.Identifier });
            BowMaking = CreateAndRegisterSkill(nameof(BowMaking), resourceLoader.GetString("Skills_SkillBowMaking"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillBowMakingDescr"), null, new SkillIdentifier[] { BuildArrows.Identifier });
            StrongThrow = CreateAndRegisterSkill(nameof(StrongThrow), resourceLoader.GetString("Skills_SkillStrongThrow"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillStrongThrowDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.BaseValue, Dice.D6) },
                new SkillIdentifier[] { PreciseThrow.Identifier }, activationType: ActivationType.Active);
            Headshot = CreateAndRegisterSkill(nameof(Headshot), resourceLoader.GetString("Skills_SkillHeadshot"), SkillCategory.Ranged, 3,
                resourceLoader.GetString("Skills_SkillHeadshotDescr"), null, new SkillIdentifier[] { AimedAttackRanged.Identifier });
            BackLine = CreateAndRegisterSkill(nameof(BackLine), resourceLoader.GetString("Skills_SkillBackLine"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillBackLineDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 1) },
                new SkillIdentifier[] { AccurateRanged.Identifier });
            RoutinedWithThrowingWeapons = CreateAndRegisterSkill(nameof(RoutinedWithThrowingWeapons), resourceLoader.GetString("Skills_SkillRoutinedWithThrowingWeapons"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillRoutinedWithThrowingWeaponsDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 2) },
                new SkillIdentifier[] { StrongThrow.Identifier });
            ProfessionalSlingshotMarksman = CreateAndRegisterSkill(nameof(ProfessionalSlingshotMarksman), resourceLoader.GetString("Skills_SkillProfessionalSlingshotMarksman"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillProfessionalSlingshotMarksmanDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 2),
                    new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.BaseValue, Dice.D6, 2)
                },
                new SkillIdentifier[] { AccurateRanged.Identifier });
            PerfectShot = CreateAndRegisterSkill(nameof(PerfectShot), resourceLoader.GetString("Skills_SkillPerfectShot"), SkillCategory.Ranged, 4,
                resourceLoader.GetString("Skills_SkillPerfectShotDescr"), null, new SkillIdentifier[] { BackLine.Identifier }, activationType: ActivationType.Active,
                staminaCost: 5);

            // Checkpoint 2
            SurpriseAttack = CreateAndRegisterSkill(nameof(SurpriseAttack), resourceLoader.GetString("Skills_SkillSurpriseAttack"), SkillCategory.Ranged, 3,
                resourceLoader.GetString("Skills_SkillSurpriseAttackDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.BaseValue, Dice.D4) },
                new SkillIdentifier[] { HijackerRanged.Identifier, BowMaking.Identifier, RoutinedWithThrowingWeapons.Identifier, Headshot.Identifier, ProfessionalSlingshotMarksman.Identifier, PerfectShot.Identifier },
                activationType: ActivationType.Active);
            NailDown = CreateAndRegisterSkill(nameof(NailDown), resourceLoader.GetString("Skills_SkillNailDown"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillNailDownDescr"), null,
                new SkillIdentifier[] { HijackerRanged.Identifier, BowMaking.Identifier, RoutinedWithThrowingWeapons.Identifier, Headshot.Identifier, ProfessionalSlingshotMarksman.Identifier, PerfectShot.Identifier },
                activationType: ActivationType.Active);
            CurvedShot = CreateAndRegisterSkill(nameof(CurvedShot), resourceLoader.GetString("Skills_SkillCurvedShot"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillCurvedShotDescr"), null,
                new SkillIdentifier[] { HijackerRanged.Identifier, BowMaking.Identifier, RoutinedWithThrowingWeapons.Identifier, Headshot.Identifier, ProfessionalSlingshotMarksman.Identifier, PerfectShot.Identifier },
                activationType: ActivationType.Active);
            QuickAim = CreateAndRegisterSkill(nameof(QuickAim), resourceLoader.GetString("Skills_SkillQuickAim"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillQuickAimDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, -3) },
                new SkillIdentifier[] { HijackerRanged.Identifier, BowMaking.Identifier, RoutinedWithThrowingWeapons.Identifier, Headshot.Identifier, ProfessionalSlingshotMarksman.Identifier, PerfectShot.Identifier },
                activationType: ActivationType.Active);
            StrongArrows = CreateAndRegisterSkill(nameof(StrongArrows), resourceLoader.GetString("Skills_SkillStrongArrows"), SkillCategory.Ranged, 1,
                resourceLoader.GetString("Skills_SkillStrongArrowsDescr"), null,
                new SkillIdentifier[] { HijackerRanged.Identifier, BowMaking.Identifier, RoutinedWithThrowingWeapons.Identifier, Headshot.Identifier, ProfessionalSlingshotMarksman.Identifier, PerfectShot.Identifier });
            PiercingArrow = CreateAndRegisterSkill(nameof(PiercingArrow), resourceLoader.GetString("Skills_SkillPiercingArrow"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillPiercingArrowDescr"), null, new SkillIdentifier[] { NailDown.Identifier }, activationType: ActivationType.Active);
            LuckyShot = CreateAndRegisterSkill(nameof(LuckyShot), resourceLoader.GetString("Skills_SkillLuckyShot"), SkillCategory.Ranged, 3,
                resourceLoader.GetString("Skills_SkillLuckyShotDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, -6) },
                new SkillIdentifier[] { CurvedShot.Identifier }, activationType: ActivationType.Active);
            DoubleShot = CreateAndRegisterSkill(nameof(DoubleShot), resourceLoader.GetString("Skills_SkillDoubleShot"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillDoubleShotDescr"), null, new SkillIdentifier[] { QuickAim.Identifier }, activationType: ActivationType.Active, usesPerBattle: 1);
            MasterfulArcher = CreateAndRegisterSkill(nameof(MasterfulArcher), resourceLoader.GetString("Skills_SkillMasterfulArcher"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillMasterfulArcherDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 2) },
                new SkillIdentifier[] { StrongArrows.Identifier });
            Magazine = CreateAndRegisterSkill(nameof(Magazine), resourceLoader.GetString("Skills_SkillMagazine"), SkillCategory.Ranged, 3,
                resourceLoader.GetString("Skills_SkillMagazineDescr"), null, new SkillIdentifier[] { DoubleShot.Identifier }, activationType: ActivationType.Active);

            // Checkpoint 3
            Oneshot = CreateAndRegisterSkill(nameof(Oneshot), resourceLoader.GetString("Skills_SkillOneshot"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillOneshotDescr"),
                new IStatModifier[] { new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.EndValue, 2, CalculatorBonusType.Multiplicative) },
                new SkillIdentifier[] { SurpriseAttack.Identifier, PiercingArrow.Identifier, LuckyShot.Identifier, Magazine.Identifier, MasterfulArcher.Identifier },
                activationType: ActivationType.Active);
            LastShot = CreateAndRegisterSkill(nameof(LastShot), resourceLoader.GetString("Skills_SkillLastShot"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillLastShotDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, Dice.D6),
                    new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.BaseValue, 0 * 2.5) // TODO Make value for loaded Character Pearls available!!!!
                },
                new SkillIdentifier[] { SurpriseAttack.Identifier, PiercingArrow.Identifier, LuckyShot.Identifier, Magazine.Identifier, MasterfulArcher.Identifier },
                activationType: ActivationType.Active);
            HuntersMark = CreateAndRegisterSkill(nameof(HuntersMark), resourceLoader.GetString("Skills_SkillHuntersMark"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillHuntersMarkDescr"), null,
                new SkillIdentifier[] { SurpriseAttack.Identifier, PiercingArrow.Identifier, LuckyShot.Identifier, Magazine.Identifier, MasterfulArcher.Identifier },
                activationType: ActivationType.Active);
            Readiness = CreateAndRegisterSkill(nameof(Readiness), resourceLoader.GetString("Skills_SkillReadiness"), SkillCategory.Ranged, 3,
                resourceLoader.GetString("Skills_SkillReadinessDescr"), null,
                new SkillIdentifier[] { SurpriseAttack.Identifier, PiercingArrow.Identifier, LuckyShot.Identifier, Magazine.Identifier, MasterfulArcher.Identifier },
                activationType: ActivationType.Active);
            MasterOfThrowingWeapons = CreateAndRegisterSkill(nameof(MasterOfThrowingWeapons), resourceLoader.GetString("Skills_SkillMasterOfThrowingWeapons"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillMasterOfThrowingWeaponsDescr"),
                new IStatModifier[]
                {
                    new CalculatorStatModifier(CalculatorValueType.Damage, ApplianceMode.BaseValue, 1),
                    new CalculatorStatModifier(CalculatorValueType.Hit, ApplianceMode.BaseValue, 2)
                },
                new SkillIdentifier[] { SurpriseAttack.Identifier, PiercingArrow.Identifier, LuckyShot.Identifier, Magazine.Identifier, MasterfulArcher.Identifier });
            Trueshot = CreateAndRegisterSkill(nameof(Trueshot), resourceLoader.GetString("Skills_SkillTrueshot"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillTrueshotDescr"), null, new SkillIdentifier[] { Oneshot.Identifier },
                activationType: ActivationType.Active);
            Return = CreateAndRegisterSkill(nameof(Return), resourceLoader.GetString("Skills_SkillReturn"), SkillCategory.Ranged, 2,
                resourceLoader.GetString("Skills_SkillReturnDescr"), null, new SkillIdentifier[] { MasterOfThrowingWeapons.Identifier },
                activationType: ActivationType.Active);

            #endregion Ranged
            #endregion Skill Definitions

            SetupSkillDependencies();
        }

        public Skill? GetSkill(int index) => Registry.ElementAt(index).Value;

        public int GetSkillIndex(SkillIdentifier identifier) => Registry.Keys.ToList().IndexOf(identifier);

        public Skill? GetSkillFromStatModifier(IStatModifier statModifier) => Registry.FirstOrDefault(entry => entry.Value.StatModifiers!.Contains(statModifier)).Value;

        public IReadOnlyDictionary<SkillIdentifier, Skill> GetSkillsFromStatModifier(IStatModifier statModifier)
        {
            return Registry
                .Where(entry => entry.Value.StatModifiers != null)
                .Where(entry => entry.Value.StatModifiers!.Contains(statModifier))
                .ToImmutableDictionary();
        }

        public Skill CreateAndRegisterSkill(string name, string displayName, SkillCategory skillCategory = SkillCategory.Character,
            int maxSkillPoints = 1, string description = "", IStatModifier[]? skillModifiers = null, SkillIdentifier[]? skillDependencies = null,
            ActivationType activationType = ActivationType.Passive, int energyCost = 0, int staminaCost = 0, int usesPerBattle = -1,
            int skillTreeCheckpoint = 0)
        {
            var identifier = new SkillIdentifier
            {
                SkillCategory = skillCategory,
                Name = name
            };
            var skill = new Skill(identifier, displayName, maxSkillPoints, description, skillModifiers, skillDependencies,
                activationType, energyCost, staminaCost, usesPerBattle, skillTreeCheckpoint);
            return RegisterSkill(skill);
        }

        public Skill RegisterSkill(Skill skill)
        {
            Registry.Add(skill.Identifier, skill);
            return skill;
        }

        private void SetupSkillDependencies()
        {
            foreach (var skill in Registry.Values)
            {
                if (!skill.DependendSkills.Any() && skill.ForcedDependendSkill == null)
                {
                    skill.IsSkillable = true;
                    continue;
                }
                foreach (var dependendSkillId in skill.DependendSkills)
                {
                    Registry[dependendSkillId].PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName is nameof(Skill.IsActive))
                        {
                            var anyDependencyActive = skill.DependendSkills.Where(id => Registry[id].IsActive).Any();
                            skill.IsSkillable = anyDependencyActive;
                        }
                    };
                }
                if (skill.ForcedDependendSkill != null)
                    skill.IsSkillable = Registry[(SkillIdentifier)skill.ForcedDependendSkill].IsActive;
            }
        }

        public void LoadFromCharacter(CharacterData data)
        {
            Registry.Values.ToList().ForEach(skill => skill.SkillPoints = 0);

            var skillSaveData = data.Skills;
            foreach (var saveData in skillSaveData)
            {
                Registry[saveData.Identifier].SkillPoints = saveData.SkillPoints;
                if (Registry[saveData.Identifier].IsRepeatable && saveData.Repetition != null)
                    Registry[saveData.Identifier].Repetition = (int)saveData.Repetition;
            }
            ViewRefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        public void SaveToCharacter(ref CharacterData data)
        {
            var activeSkills = Registry.Values.Where(skill => skill.IsActive);
            var saveData = activeSkills.ToList().ConvertAll(skill => new SkillSaveData()
            {
                Identifier = skill.Identifier,
                SkillPoints = skill.SkillPoints,
                Repetition = skill.IsRepeatable ? skill.Repetition : null
            });
            data.Skills = saveData;
        }

        public void ResetData()
        {
            foreach (var skill in Registry.Values)
            {
                skill.SkillPoints = 0;
                if (skill.IsRepeatable)
                    skill.Repetition = 0;
            }
        }

        public List<Skill> GetFromStatModifierType<TStatModifier>() where TStatModifier : IStatModifier
        {
            var result = Registry.Values.Where(skill =>
            {
                if (skill.StatModifiers != null && skill.StatModifiers.Length > 0)
                    return skill.StatModifiers.Any(statMod => statMod is TStatModifier);
                return false;
            });
            return result.ToList();
        }

        public List<TStatModifier> GetStatModifiers<TStatModifier>() where TStatModifier : IStatModifier
        {
            var statModifierSkills = GetFromStatModifierType<TStatModifier>();
            var result = statModifierSkills.SelectMany(skill => SelectStatModifiers<TStatModifier>(skill));
            return result.ToList();
        }

        public List<TStatModifier> GetActiveStatModifiers<TStatModifier>() where TStatModifier : IStatModifier
        {
            var activeStatModifierSkills = GetFromStatModifierType<TStatModifier>().Where(skill => skill.IsActive);
            var result = activeStatModifierSkills.SelectMany(skill => SelectStatModifiers<TStatModifier>(skill));
            return result.ToList();
        }

        private IEnumerable<TStatModifier> SelectStatModifiers<TStatModifier>(Skill skill) where TStatModifier : IStatModifier
        {
            return skill.StatModifiers!.Where(statMod => statMod is TStatModifier).Cast<TStatModifier>();
        }
    }
}