using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CharacterIchigo;
using CharacterMidoriya;
using CharacterMisaka;
using CharacterNaruto;
using CharacterNatsu;
using CharacterSaber;
using CharacterSnowWhite;
using CharacterTsunayoshi;
using CharacterTatsumi;
using ClassKuroko;
using CharacterChachamaru;
using CharacterToga;
using CharacterMirio;
using CharacterGunha;
using CharacterGray;
using CharacterMinato;
using CharacterClass;
using AbilityClass;
using EffectClass;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;
using CharacterItachi;

namespace AnimeArena
{
    public partial class Form2 : Form
    {

        #region Field of fields
        public static System.Windows.Forms.Timer turnTimer = new System.Windows.Forms.Timer();
        public static System.Windows.Forms.Timer timerTimer = new System.Windows.Forms.Timer();
        public bool waiting = false;
        public Socket s;
        public Stream strm;
        public bool isHost;
        public int offeredPhys = 0;
        public int offeredSpec = 0;
        public int offeredMent = 0;
        public int offeredWep = 0;
        public int randNeeded;
        int[] enemyTeamEnergy = new int[5] { 0, 0, 0, 0, 0 };
        int[] playerTeamEnergy = new int[5] { 0, 0, 0, 0, 0 };
        int[] playerTeamTempEnergy = new int[5] { 0, 0, 0, 0, 0 };
        int[] swapEnergy = new int[5] { 0, 0, 0, 0, 0 };
        public int playerTogaSlot = 15;
        public int enemyTogaSlot = 15;
        public bool allyTogaChanged = false;
        public bool enemyTogaChanged = false;

        Character[] playerTeam = new Character[3];
        Character[] enemyTeam = new Character[3];
        Character[] allCharacters = new Character[6];
        PictureBox[] player1Effects = new PictureBox[16];
        PictureBox[] player2Effects = new PictureBox[16];
        PictureBox[] player3Effects = new PictureBox[16];
        PictureBox[] enemy1Effects = new PictureBox[16];
        PictureBox[] enemy2Effects = new PictureBox[16];
        PictureBox[] enemy3Effects = new PictureBox[16];
        PictureBox[][] allEffects = new PictureBox[6][];
        Ability targetingAbility;
        PictureBox[] player1Targeted = new PictureBox[3];
        PictureBox[] player2Targeted = new PictureBox[3];
        PictureBox[] player3Targeted = new PictureBox[3];
        PictureBox[] enemy1Targeted = new PictureBox[3];
        PictureBox[] enemy2Targeted = new PictureBox[3];
        PictureBox[] enemy3Targeted = new PictureBox[3];
        PictureBox[][] allTargeted = new PictureBox[6][];
        List<int> player1PossibleTargets = new List<int>();
        List<int> player2PossibleTargets = new List<int>();
        List<int> player3PossibleTargets = new List<int>();
        List<int> player1Targets = new List<int>();
        List<int> player2Targets = new List<int>();
        List<int> player3Targets = new List<int>();
        int chosenAbilitySlot1;
        int chosenAbilitySlot2;
        int chosenAbilitySlot3;
        Label[] allHP = new Label[6];
        int targetingAbilitySlot;
        PictureBox[] enemyTeamDisplay = new PictureBox[3];
        PictureBox[] playerTeamDisplay = new PictureBox[3];
        PictureBox[] character1Moves = new PictureBox[4];
        PictureBox[] character2Moves = new PictureBox[4];
        PictureBox[] character3Moves = new PictureBox[4];
        PictureBox[] character1Energy = new PictureBox[5];
        PictureBox[] character2Energy = new PictureBox[5];
        PictureBox[] character3Energy = new PictureBox[5];
        PictureBox[][] allPlayerMoves = new PictureBox[3][];
        PictureBox[][] allCharacterEnergy = new PictureBox[3][];
        Label[] characterCooldowns = new Label[3];
        TextBox[] detailBoxes = new TextBox[3];
        List<int> potentialTargets = new List<int>();
        List<int> executionOrder = new List<int>();
        int targetingCharacterSlot;
        int primarySlot;
        int turnRandCost;
        bool randPaid = false;
        public Size hpBar1Size;
        public Size hpBar2Size;
        public Size hpBar3Size;
        public Size hpBar4Size;
        public Size hpBar5Size;
        public Size hpBar6Size;
        public List<Effect> displayEffects = new List<Effect>();
        public string hostIPAddress;
        public string serialString ="";
        #endregion
        #region Field of Effects
        public Effect rasenganEff = new Effect("rasenganEff", "naruto2.png", "Rasengan", "This character is stunned.");
        public Effect shadowClonesEff = new Effect("shadowClonesEff", "naruto1.png", "Shadow Clones", "Naruto has 10 points of damage reduction", "Rasengan will target all enemies.", "Naruto Taijutsu is replaced with Uzumaki Barrage.", "Shadow Clones is replaced with Sage Mode.");
        public Effect substitutionEff = new Effect("substitutionEff", "naruto4.png", "Substitution", "Naruto is invulnerable.");
        public Effect sageModeEff = new Effect("sageModeEff", "naruto1-2.png", "Sage Mode", "Naruto is invulnerable.", "Rasengan deals double damage and will stun for 2 turns.", "Uzumaki Barrage is replaced by Toad Taijutsu.");
        public Effect uzumakiBarrageSelfEff = new Effect("uzumakiBarrageSelfEff", "naruto 3-2.png", "Uzumaki Barrage", "Uzumaki Barrage will stun its target.");
        public Effect uzumakiBarrageEff = new Effect("uzumakiBarrageEff", "naruto 3-2.png", "Uzumaki Barrage", "This character is stunned.");
        public Effect toadTaijutsuEff = new Effect("toadTaijutsuEff", "naruto 3-3.png", "Toad Taijutsu", "This character's abilities costs are increased by 2 random energy.");
        public Effect zangetsuBlockEff = new Effect("zangetsuBlockEff", "ichigo4.png", "Zangetsu Block", "Ichigo is invulnerable.");
        public Effect zangetsuStrikeEff = new Effect("zangetsuStrikeEff", "ichigo3.png", "Zangetsu Strike", "Zangetsu Strike does 5 more damage for each stack.");
        public Effect tensaZangetsuEff = new Effect("tensaZangetsuEff", "ichigo2.png", "Tensa Zangetsu", "Ichigo will gain one weapon energy.", "Ichigo is invulnerable.");
        public Effect tensaZangetsuBonusEff = new Effect("tensaZangetsuBonusEff", "ichigo2.png", "Tensa Zangetsu", "Getsuga Tenshou and Zangetsu Strike are improved.");
        public Effect hearDistressAllyEff = new Effect("hearDistressAllyEff", "snowwhite2.png", "Hear Distress", "This character has 25 points of damage reduction.","This character will gain one random energy if a new harmful move is used on them.");
        public Effect hearDistressEnemyEff = new Effect("hearDistressEnemyEff", "snowwhite2.png", "Hear Distress", "The first harmful ability used by this character will be countered and this character will lose one random energy.");
        public Effect luckyRabbitsFootEff = new Effect("luckyRabbitsFootEff", "snowwhite3.png", "Lucky Rabbit's Foot", "If this character is killed, they will be returned to 35 health.");
        public Effect hearDistressEndEff = new Effect("hearDistressEndEff", "snowwhite2.png", "Hear Distress", "Hear Distress has ended.");
        public Effect luckyRabbitsFootEndEff = new Effect("luckyRabbitsFootEndEff", "snowwhite3.png", "Lucky Rabbit's Foot", "Lucky Rabbit's Foot has ended.");
        public Effect snowWhiteLeapEff = new Effect("snowWhiteLeapEff", "snowwhite4.png", "Leap", "Snow White is invulnerable.");
        public Effect hearDistressSuccessEff = new Effect("hearDistressSuccessEff", "snowwhite2.png", "Hear Distress", "This character has been countered by Snow White.");
        public Effect windBladeCombatSelfEff = new Effect("windBladeCombatSelfEff", "saber2.png", "Wind Blade Combat", "Saber cannot be stunned.");
        public Effect windBladeCombatEnemyEff = new Effect("windBladeCombatEnemyEff", "saber2.png", "Wind Blade Combat", "This character will take 10 damage.");
        public Effect avalonEff = new Effect("avalonEff", "saber3.png", "Avalon", "This character will heal for 10 health.");
        public Effect saberParryEff = new Effect("saberParryEff", "saber4.png", "Saber Parry", "Saber is invulnerable.");
        public Effect ironSandEff = new Effect("ironSandEff", "misaka1.png", "Iron Sand", "This character has 35 points of destructible defense.");
        public Effect electricRageEff = new Effect("electricRageEff", "misaka3.png", "Electric Rage", "Misaka will gain one special energy if she takes new damage.", "Misaka cannot be killed.");
        public Effect electricDeflectionEff = new Effect("electricDeflectionEff", "misaka4.png", "Electric Deflection", "Misaka is invulnerable.");
        public Effect fireDragonRoarEff = new Effect("fireDragonRoarEff", "natsu1.png", "Fire Dragon's Roar", "This character will take 10 affliction damage at the end of the\nnext turn.");
        public Effect fireDragonSwordHornEff = new Effect("fireDragonSwordHornEff", "natsu3.png", "Fire Dragon's Sword Horn", "This character will take 5 affliction damage.");
        public Effect natsuDodgeEff = new Effect("natsuDodgeEff", "natsu4.png", "Natsu Dodge", "Natsu is invulnerable.");
        public Effect incursioEff = new Effect("incursioEff", "tatsumi2.png", "Incursio", "This character has 25 points of desctructible defense.", "Neuntote is usable.");
        public Effect neuntoteEff = new Effect("neuntoteEff", "tatsumi3.png", "Neuntote", "This character receives double damage from Neuntote.");
        public Effect invisibilityEff = new Effect("invisibilityEff", "tatsumi4.png", "Invisibility", "Tatsumi is invulnerable.");
        public Effect zeroPointBreakthroughEff = new Effect("zeroPointBreakthroughEff", "tsunayoshi2.png", "Zero Point Breakthrough", "Tsuna will counter the first enemy ability used on him.");
        public Effect burningAxleEff = new Effect("burningAxleEff", "tsunayoshi3.png", "Zero Point Breakthrough", "If this character takes new damage, they will take 15 damage and be stunned for one turn.");
        public Effect burningAxleStunEff = new Effect("burningAxleStunEff", "tsunayoshi3.png", "Burning Axle", "This character is stunned.");
        public Effect flareBurstEff = new Effect("flareBurstEff", "tsunayoshi4.png", "Flare Burst", "Tsuna is invulnerable.");
        public Effect zeroPointBreakthroughSuccessEff = new Effect("zeroPointBreakthroughSuccessEff", "tsunayoshi2.png", "Zero Point Breakthrough", "This character has been countered by Tsuna.");
        public Effect zeroPointBreakthroughStunEff = new Effect("zeroPointBreakthroughStunEff", "tsunayoshi2.png", "Zero Point Breakthrough", "This character is stunned.");
        public Effect zeroPointBreakthroughBoostEff = new Effect("zeroPointBreakthroughBoostEff", "tsunayoshi2.png", "Zero Point Breakthrough", "X-Burner will deal 10 more damage to all targets.");
        public Effect zeroPointBreakthroughEndEff = new Effect("zeroPointBreakthroughEndEff", "tsunayoshi2.png", "Zero Point Breakthrough", "Zero Point Breakthrough has ended.");
        public Effect smashEff = new Effect("smashEff", "midoriya1.png", "SMASH!", "Midoriya is stunned.");
        public Effect airForceGlovesEff = new Effect("airForceGlovesEff", "midoriya2.png", "Air Force Gloves", "If this character uses a move, its cooldown will be increased by one.");
        public Effect shootStyleEff = new Effect("shootStyleEff", "midoriya3.png", "One For All - Shoot Style", "Midoriya will counter the first harmful ability used on him.");
        public Effect shootStyleSuccessEff = new Effect("shootStyleSuccessEff", "midoriya3.png", "One For All - Shoot Style", "This character has been countered by Midoriya.");
        public Effect shootStyleEndEff = new Effect("shootStyleEndEff", "midoriya3.png", "One For All - Shoot Style", "One For All - Shoot Style has ended.");
        public Effect enhancedLeapEff = new Effect("enhancedLeapEff", "midoriya4.png", "Enhanced Leap", "Midoriya is invulnerable.");
        public Effect teleportingStrikeEff = new Effect("teleportingStrikeEff", "kuroko2.png", "Teleporting Strike", "Kuroko is invulnerable.", "Needle Pin will ignore invulnerability.", "Judgement Throw will have double effect.");
        public Effect needlePinEff = new Effect("needlePinEff", "kuroko3.png", "Needle Pin", "Teleporting Strike will have no cooldown.", "Judgement Throw cannot be countered or reflected.");
        public Effect needlePinEnemyEff = new Effect("needlePinEnemyEff", "kuroko3.png", "Needle Pin", "This character cannot reduce damage or become invulnerable.");
        public Effect needlePinStunEff = new Effect("needlePinStunEff", "kuroko3.png", "Needle Pin", "This character is stunned.");
        public Effect judgementThrowEff = new Effect("judgementThrowEff", "kuroko1.png", "Judgement Throw", "Teleporting Strike will deal 15 extra damage.", "Needle Pin will stun its target for one turn.");
        public Effect judgementThrowEnemyEff = new Effect("judgementThrowEnemyEff", "kuroko1.png", "Judgement Throw", "This character will deal 10 less damage with abilities.");
        public Effect judgementThrowDoubleEnemyEff = new Effect("judgementThrowDoubleEnemyEff", "kuroko1.png", "Judgement Throw", "This character will deal 20 less damage with abilities.");
        public Effect kurokoDodgeEff = new Effect("kurokoDodgeEff", "kuroko4.png", "Kuroko Dodge", "Kuroko is invulnerable.");
        public Effect thirstingKnifeEff = new Effect("thirstingKnifeEff", "toga1.png", "Thirsting Knife", "Toga drank this character's blood.");
        public Effect vacuumSyringeDamageEff = new Effect("vacuumSyringeDamageEff", "toga2.png", "Vacuum Syringe", "This character will take 10 affliction damage.");
        public Effect vacuumSyringeEff = new Effect("vacuumSyringeEff", "toga2.png", "Vacuum Syringe", "Toga drank this character's blood.");
        public Effect quirkTransformEff = new Effect("quirkTransformEff", "toga3.png", "Quirk - Transform", "Toga has transformed into an enemy.");
        public Effect togaDodgeEff = new Effect("togaDodgeEff", "toga4.png", "Toga Dodge", "Toga is invulnerable.");
        public Effect targetLockEff = new Effect("targetLockEff", "chachamaru1.png", "Target Lock", "This character is marked for Orbital Satellite Cannon.");
        public Effect activeCombatModeSelfEff = new Effect("activeCombatModeSelfEff", "chachamaru3.png", "Active Combat Mode", "Chachamaru will gain 15 destructible defense.", "Chachamaru cannot use Target Lock or Orbital Satellite Cannon.");
        public Effect activeCombatModeEnemyEff = new Effect("activeCombatModeEnemyEff", "chachamaru3.png","Active Combat Mode", "This character will take 10 damage.");
        public Effect takeFlightEff = new Effect("takeFlightEff", "chachamaru4.png", "Take Flight", "Chachamaru is invulnerable.");
        public Effect amaterasuEff = new Effect("amaterasuEff", "itachi1-1.png", "Amaterasu", "This character will take 10 affliction damage.");
        public Effect tsukuyomiEff = new Effect("tsukuyomiEff", "itachi2-1.png", "Tsukuyomi", "This character is stunned.", "This effect will end if an ally uses a helpful skill on this character.");
        public Effect susanooEff = new Effect("susanooEff", "itachi3.png", "Susano'o", "Itachi has 45 destructible defense.", "Itachi will lose 10 health.", "Amaterasu has been replaced by Totsuka Blade.", "Tsukuyomi has been replaced by Yata Mirror.");
        public Effect totsukaBladeEff = new Effect("totsukaBladeEff", "itachi1-2.png", "Totsuka Blade", "This character is stunned.");
        public Effect crowGenjutsuEff = new Effect("crowGenjutsuEff", "itachi4.png", "Crow Genjutsu", "Itachi is invulnerable.");
        public Effect quirkPermeationEff = new Effect("quirkPermeationEff", "mirio1.png", "Quirk - Permeation", "Mirio is ignoring all incoming harmful effects.");
        public Effect quirkPermeationEndEff = new Effect("quirkPermeationEndEff", "mirio1.png", "Quirk - Permeation", "Quirk - Permeation has ended.");
        public Effect phantomMenaceEff = new Effect("phantomMenaceEff", "mirio2.png", "Phantom Menace", "This character is marked for Phantom Menace.");
        public Effect protectAllyEff = new Effect("protectAllyEff", "mirio3.png", "Protect Ally", "This character is ignoring all incoming harmful effects.");
        public Effect protectAllyEndEff = new Effect("protectAllyEndEff", "mirio3.png", "Protect Ally", "Protect Ally has ended.");
        public Effect mirioDodgeEff = new Effect("mirioDodgeEff", "mirio4.png", "Mirio Dodge", "Mirio is invulnerable.");
        public Effect superAwesomePunchEff = new Effect("superAwesomePunchEff", "sogiita2.png", "Super Awesome Punch", "This character is stunned.");
        public Effect overwhelmingSuppressionEff = new Effect("overwhelmingSuppressionEff", "sogiita3.png", "Overwhelming Suppression", "This character deals 5 less damage.");
        public Effect overwhelmingSuppressionDoubleEff = new Effect("overwhelmingSuppressionDoubleEff", "sogiita3.png", "Overwhelming Suppression", "This character deals 10 less damage.");
        public Effect gutsEff = new Effect("gutsEff", "sogiita1.png", "Guts", "Gunha got mad guts lol");
        public Effect iceMakeEff = new Effect("iceMakeEff", "gray1-1.png", "Ice, Make...", "Gray is ready to use his abilities.");
        public Effect iceMakeUnlimitedEnemyEff = new Effect("iceMakeUnlimitedEnemyEff", "gray1-2.png", "Ice, Make Unlimited", "This character will take 10 damage.");
        public Effect iceMakeUnlimitedAllyEff = new Effect("iceMakeUnlimitedAllyEff", "gray1-2.png", "Ice, Make Unlimited", "This character will gain 5 destructible defense.", "This character has destructible defense.");
        public Effect iceMakeFreezeLancerEff = new Effect("iceMakeFreezeLancerEff", "gray2.png", "Ice, Make Freeze Lancer", "This character will take 15 damage.");
        public Effect iceMakeHammerEff = new Effect("iceMakeHammerEff", "gray3.png", "Ice, Make Hammer", "This character is stunned.");
        public Effect iceMakeShieldEff = new Effect("iceMakeShieldEff", "gray4.png", "Ice, Make Shield", "Gray is invulnerable.");
        public Effect flyingRaijinEff = new Effect("flyingRaijinEff", "minato1.png", "Flying Raijin", "Minato is invulnerable.");
        public Effect markedKunaiEff = new Effect("markedKunaiEff", "minato2.png", "Marked Kunai", "This character is marked for Flying Raijin.");
        public Effect partialShikiFuujinEff = new Effect("partialShikiFuujinEff", "minato3.png", "Partial Shiki Fuujin", "This character's, cooldowns are increased by one.", "This character's abilities cost one additional random energy.");
        public Effect minatoParryEff = new Effect("minatoParryEff", "minato4.png", "Minato Parry", "Minato is invulnerable.");
        #endregion
        public Form2(Character[] team1, Character[] eTeam, int[] energy, int[] enemyEnergy, bool host, string hostIP)
        {
            InitializeComponent();
            isHost = host;
            hostIPAddress = hostIP;
            playerTeam = team1;
            enemyTeam = eTeam;
            playerTeamEnergy = energy;
            enemyTeamEnergy = enemyEnergy;
            foreach (Character c in team1)
            {
                c.targeted = false;

            }
            foreach (Character c in eTeam)
            {
                c.targeted = false;
            }
        }
        public void returnEnergy(Ability abi)
        {
            playerTeamTempEnergy[0] += abi.physCost;
            playerTeamTempEnergy[1] += abi.specCost;
            playerTeamTempEnergy[2] += abi.mentCost;
            playerTeamTempEnergy[3] += abi.wepCost;

            playerTeamTempEnergy[4] += abi.randCost + abi.physCost + abi.specCost + abi.mentCost + abi.wepCost;

            turnRandCost -= abi.randCost;
            physCounterLabel.Text = "x " + playerTeamTempEnergy[0].ToString();
            specCounterLabel.Text = "x " + playerTeamTempEnergy[1].ToString();
            mentCounterLabel.Text = "x " + playerTeamTempEnergy[2].ToString();
            wepCounterLabel.Text = "x " + playerTeamTempEnergy[3].ToString();



            totalCounterLabel.Text = "T : " + playerTeamTempEnergy[4].ToString();
            displayAvailability();
        }

        public void timesUpsUp(object obj, EventArgs e)
        {
            
            
        }
        public void timesUp(object obj, EventArgs e)
        {
            serialString = "";
            while (waiting == true)
            {
                timerTimer.Start();
                if (isHost == true)
                {
                    byte[] ba = new byte[500];

                    s.ReceiveTimeout = 100;
                    try
                    {
                        s.Receive(ba);
                        
                        
                    }
                    catch (SocketException t)
                    {
                        break;
                    }
                    for (int i = 0; i < ba.Length; i++)
                    {
                        serialString += Convert.ToChar(ba[i]);
                    }
                    if (serialString != "")
                    {
                        deSerializeGameState(serialString);
                        waiting = false;
                        nextTurnButton.Enabled = true;
                        turnStart(playerTeam, enemyTeam);
                    }
                }

                if (isHost == false)
                {
                    byte[] bb = new byte[200];

                    //var kx = strm.BeginRead(bb, 0, 200, null, null);

                    //int k = (int)kx.AsyncState;
                    strm.ReadTimeout = 100;
                    try
                    {
                        strm.Read(bb, 0, 200);
                        
                        
                    }
                    catch (IOException t)
                    {
                        break;
                    }
                    for (int i = 0; i < bb.Length; i++)
                    {
                        serialString += Convert.ToChar(bb[i]);
                    }
                    if (serialString != "")
                    {
                        deSerializeGameState(serialString);
                        waiting = false;
                        nextTurnButton.Enabled = true;
                        turnStart(playerTeam, enemyTeam);
                    }
                }
            }

            

        }
        public void removeEffect(Effect eff)
        {
            eff.duration = eff.maxDuration;
            eff.target.effects.Remove(eff);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    allEffects[i][j].Visible = false;
                }
            }
            for (int i = 0; i < allCharacters.Length; i++)
            {

                if (allCharacters[i].dead != true)
                {

                    //populate the list of VISIBLE effects

                    for (int j = 0; j < allCharacters[i].effects.Count; j++)
                    {
                        if (allCharacters[i].effects[j].invisible != true || isAllied(allCharacters[i].effects[j]))
                        {
                            displayEffects.Add(allCharacters[i].effects[j]);
                        }
                    }

                    for (int j = 0; j < displayEffects.Count; j++)
                    {

                        allEffects[i][j].Visible = true;
                        allEffects[i][j].Image = Image.FromFile(@"..\..\images\mini" + displayEffects[j].imgPath);

                    }
                    displayEffects.Clear();
                }

            }
        }
        public void removeTarget(Character[] team, int charSlot, int effSlot)
        {
            Ability rA = new Ability(0,0,0,0,0,0,"","");
            
            switch (effSlot)
            {
                case 0:
                    rA = team[charSlot].firstTargeted;
                    playerTeam[team[charSlot].firstTargeter].currentTargets.Clear();
                    break;
                case 1:
                    rA = team[charSlot].secondTargeted;
                    playerTeam[team[charSlot].secondTargeter].currentTargets.Clear();
                    break;
                case 2:
                    rA = team[charSlot].thirdTargeted;
                    playerTeam[team[charSlot].thirdTargeter].currentTargets.Clear();
                    break;
            }
            returnEnergy(rA);
            for (int i = 0; i < 3; i++)
            {
                executionOrder.Remove(i);
                if (playerTeam[i].firstTargeted == rA)
                {
                    playerTeam[playerTeam[i].firstTargeter].abilities[playerTeam[playerTeam[i].firstTargeter].chosenAbilitySlot].cooldownRemaining = 0;
                    playerTeam[playerTeam[i].firstTargeter].targeted = false;
                    allTargeted[i][0].Visible = false;
                    
                    playerTeam[i].firstTargeted = null;
                    playerTeam[i].firstTargeter = 5;
                }
                if (playerTeam[i].secondTargeted == rA)
                {
                    playerTeam[playerTeam[i].secondTargeter].abilities[playerTeam[playerTeam[i].secondTargeter].chosenAbilitySlot].cooldownRemaining = 0;
                    playerTeam[playerTeam[i].secondTargeter].targeted = false;
                    allTargeted[i][1].Visible = false;
                    
                    playerTeam[i].secondTargeted = null;
                    playerTeam[i].secondTargeter = 5;
                }
                if (playerTeam[i].thirdTargeted == rA)
                {
                    playerTeam[playerTeam[i].thirdTargeter].targeted = false;
                    playerTeam[playerTeam[i].thirdTargeter].abilities[playerTeam[playerTeam[i].thirdTargeter].chosenAbilitySlot].cooldownRemaining = 0;
                    allTargeted[i][2].Visible = false;
                    
                    playerTeam[i].thirdTargeted = null;
                    playerTeam[i].thirdTargeter = 5;
                }
                if (enemyTeam[i].firstTargeted == rA)
                {
                    playerTeam[enemyTeam[i].firstTargeter].targeted = false;
                    playerTeam[enemyTeam[i].firstTargeter].abilities[playerTeam[enemyTeam[i].firstTargeter].chosenAbilitySlot].cooldownRemaining = 0;
                    allTargeted[i+3][0].Visible = false;
                   
                    enemyTeam[i].firstTargeted = null;
                    enemyTeam[i].firstTargeter = 5;
                }
                if (enemyTeam[i].secondTargeted == rA)
                {
                    playerTeam[enemyTeam[i].secondTargeter].targeted = false;
                    playerTeam[enemyTeam[i].secondTargeter].abilities[playerTeam[enemyTeam[i].secondTargeter].chosenAbilitySlot].cooldownRemaining = 0;
                    allTargeted[i + 3][1].Visible = false;
                    
                    enemyTeam[i].secondTargeted = null;
                    enemyTeam[i].secondTargeter = 5;
                }
                if (enemyTeam[i].thirdTargeted == rA)
                {
                    playerTeam[enemyTeam[i].thirdTargeter].targeted = false;
                    playerTeam[enemyTeam[i].thirdTargeter].abilities[playerTeam[enemyTeam[i].thirdTargeter].chosenAbilitySlot].cooldownRemaining = 0;
                    allTargeted[i + 3][2].Visible = false;
                    
                    enemyTeam[i].thirdTargeted = null;
                    enemyTeam[i].thirdTargeter = 5;
                }
            }
            displayAvailability();
        }
        public void setTargeter(Character target, int targeterSlot)
        {

            if (target.firstTargeter == 5)
            {

                target.firstTargeter = targeterSlot;

            }
            else if (target.secondTargeter == 5)
            {
                target.secondTargeter = targeterSlot;
            }
            else { target.thirdTargeter = targeterSlot; }

        }
        public bool checkEnergyAvailability(int[] energy, Ability abi)
        {
            bool available = true;
            int randTracker = 0;
            if (abi.physCost > 0)
            {

                if (energy[0] < abi.physCost || energy[4] < abi.totCost)
                {
                    available = false;
                }
                randTracker += abi.physCost;
            }
            if (abi.specCost > 0)
            {

                if (energy[1] < abi.specCost || energy[4] < abi.totCost)
                {
                    available = false;
                }
                randTracker += abi.specCost;
            }
            if (abi.mentCost > 0)
            {

                if (energy[2] < abi.mentCost || energy[4] < abi.totCost)
                {
                    available = false;
                }
                randTracker += abi.mentCost;
            }
            if (abi.wepCost > 0)
            {

                if (energy[3] < abi.wepCost || energy[4] < abi.totCost)
                {
                    available = false;
                }
                randTracker += abi.wepCost;
            }
            if (abi.randCost > 0)
            {
                if((energy[4] - randTracker) < abi.randCost || energy[4] < abi.totCost )
                {
                    available = false;
                }
            }
            return available;
        }
        public bool isAllied(Effect eff)
        {

            bool check = false;

            for (int i = 0; i < 3; i++)
            {
                if (eff.user == playerTeam[i])
                {
                    check = true;
                }
            }
            return check;

        }
        public Effect findEffect(string name, Character c)
        {
            Effect eff = new Effect();

            foreach (Effect e in c.effects)
            {
                if (e.name == name)
                {
                    eff = e;
                }
            }

            return eff;

        }
        public void checkEffect(Effect eff)
        {
            
            #region Invulnerability Effects
            if (eff.name == "natsuDodgeEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "invisibilityEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "flareBurstEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "enhancedLeapEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "zangetsuBlockEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "tensaZangetsuEff")
            {
                eff.target.invulnerable = true;
                
            }
            if (eff.name == "saberParryEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "electricDeflectionEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "snowWhiteLeapEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "substitutionEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "teleportingStrikeEff")
            {
                eff.user.invulnerable = true;
            }
            if (eff.name == "kurokoDodgeEff")
            {
                eff.user.invulnerable = true;
            }
            if (eff.name == "togaDodgeEff")
            {
                eff.user.invulnerable = true;
            }
            if (eff.name == "mirioDodgeEff")
            {
                eff.user.invulnerable = true;
            }
            if (eff.name == "quirkPermeationEff")
            {
                eff.user.ignoring = true;
            }
            if (eff.name == "protectAllyEff")
            {
                eff.target.ignoring = true;
            }
            if (eff.name == "iceMakeShieldEff")
            {
                eff.target.invulnerable = true;
            }
            if (eff.name == "flyingRaijinEff")
            {
                eff.user.invulnerable = true;
            }
            if (eff.name == "minatoParryEff")
            {
                eff.user.invulnerable = true;
            }
            #endregion
            #region Cooldown Effects
            if (eff.name == "airForceGlovesEff")
            {
                eff.target.cooldownMod += 1;
            }
            if (eff.name == "needlePinEff")
            {
                eff.user.abilities[0].cooldown = 0;
            }
            if (eff.name == "partialShikiFuujinEff")
            {
                eff.target.randomMod += 1;
                eff.target.abilities[0].randCost += 1;
                eff.target.abilities[1].randCost += 1;
                eff.target.abilities[2].randCost += 1;
                eff.target.abilities[3].randCost += 1;
                eff.target.cooldownMod += 1;
            }
            #endregion
            #region Stun Effects
            if (eff.name == "smashEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "zeroPointBreakthroughStunEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "burningAxleStunEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "rasenganEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "uzumakiBarrageEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "needlePinStunEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "tsukuyomiEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "totsukaBladeEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "superAwesomePunchEff")
            {
                eff.target.stunned = true;
            }
            if (eff.name == "iceMakeHammerEff")
            {
                eff.target.stunned = true;
            }
            #endregion
            if (eff.name == "electricRageEff")
            {
                if (isAllied(eff))
                {
                    playerTeamEnergy[1] += eff.counters;
                    eff.counters = 0;
                }
            }
            if (eff.name == "incursioEff")
            {
                eff.description[0] = "Tatsumi has " + eff.ddRemaining + " destructible defense.";
                eff.target.abilities[2].enabled = true;
            }
            if (eff.name == "activeCombatEnemyEff")
            {
                if (eff.user.dead == true)
                {
                    eff.target.effects.Remove(findEffect("activeCombatEnemyEff", eff.target));
                }
            }
            if (eff.name == "windBladeCombatEnemyEff")
            {
                if (eff.user.dead == true)
                {
                    eff.target.effects.Remove(findEffect("windBladeCombatEnemyEff", eff.target));
                }
            }
            if (eff.name == "activeCombatModeSelfEff")
            {
                if (eff.description.Count == 2)
                {
                    eff.description.Add("Chachamaru has " + eff.ddRemaining + " destructible defense.");
                }
                else
                {
                    eff.description[2] = "Chachamaru has " + eff.ddRemaining + " destructible defense.";
                }
                eff.target.abilities[0].enabled = false;
                eff.target.abilities[1].enabled = false;
            }
            if (eff.name == "zeroPointBreakthroughEff")
            {
                
                if (eff.user.counterTriggered == true)
                {
                    endEffect(eff);
                    removeEffect(eff);
                }
            }
            if (eff.name == "shootStyleEff")
            {
                if (eff.user.counterTriggered == true)
                {
                    endEffect(eff);
                    removeEffect(eff);
                }
            }
            if (eff.name == "quirkTransformEff")
            {
                
                int togaHP = eff.user.hp;
                List<Effect> togaEffects = eff.user.effects;
                Character togaUser = eff.user;

                if (isAllied(eff))
                {
                    if (allyTogaChanged == false)
                    {
                        switch (eff.counters)
                        {
                            case 0:
                                playerTeam[playerTogaSlot] = new Ichigo(togaHP, togaEffects);
                                break;
                            case 1:
                                playerTeam[playerTogaSlot] = new Midoriya(togaHP, togaEffects);
                                break;
                            case 2:
                                playerTeam[playerTogaSlot] = new Misaka(togaHP, togaEffects);
                                break;
                            case 3:
                                playerTeam[playerTogaSlot] = new Naruto(togaHP, togaEffects);
                                break;
                            case 4:
                                playerTeam[playerTogaSlot] = new Natsu(togaHP, togaEffects);
                                break;
                            case 5:
                                playerTeam[playerTogaSlot] = new Saber(togaHP, togaEffects);
                                break;
                            case 6:
                                playerTeam[playerTogaSlot] = new SnowWhite(togaHP, togaEffects);
                                break;
                            case 7:
                                playerTeam[playerTogaSlot] = new Tatsumi(togaHP, togaEffects);
                                break;
                            case 8:
                                playerTeam[playerTogaSlot] = new Tsunayoshi(togaHP, togaEffects);
                                break;
                            case 9:
                                playerTeam[playerTogaSlot] = new Kuroko(togaHP, togaEffects);
                                break;
                            case 11:
                                playerTeam[playerTogaSlot] = new Chachamaru(togaHP, togaEffects);
                                break;
                            case 12:
                                playerTeam[playerTogaSlot] = new Itachi(togaHP, togaEffects);
                                break;
                            case 13:
                                playerTeam[playerTogaSlot] = new Mirio(togaHP, togaEffects);
                                break;
                            case 14:
                                playerTeam[playerTogaSlot] = new Gunha(togaHP, togaEffects);
                                break;
                            case 15:
                                playerTeam[playerTogaSlot] = new Gray(togaHP, togaEffects);
                                break;
                            case 16:
                                playerTeam[playerTogaSlot] = new Minato(togaHP, togaEffects);
                                break;
                        }
                        allCharacters[playerTogaSlot] = playerTeam[playerTogaSlot];
                        allCharacters[playerTogaSlot].startHP = allCharacters[playerTogaSlot].hp;
                        playerTeam[playerTogaSlot].startHP = playerTeam[playerTogaSlot].hp;
                        eff.user = playerTeam[playerTogaSlot];
                        adjustEffectUser(playerTeam[playerTogaSlot], togaUser);
                        allyTogaChanged = true;
                    }
                }

                if (!isAllied(eff))
                {
                    if (enemyTogaChanged == false)
                    {
                        switch (eff.counters)
                        {
                            case 0:
                                enemyTeam[enemyTogaSlot] = new Ichigo(togaHP, togaEffects);
                                break;
                            case 1:
                                enemyTeam[enemyTogaSlot] = new Midoriya(togaHP, togaEffects);
                                break;
                            case 2:
                                enemyTeam[enemyTogaSlot] = new Misaka(togaHP, togaEffects);
                                break;
                            case 3:
                                enemyTeam[enemyTogaSlot] = new Naruto(togaHP, togaEffects);
                                break;
                            case 4:
                                enemyTeam[enemyTogaSlot] = new Natsu(togaHP, togaEffects);
                                break;
                            case 5:
                                enemyTeam[enemyTogaSlot] = new Saber(togaHP, togaEffects);
                                break;
                            case 6:
                                enemyTeam[enemyTogaSlot] = new SnowWhite(togaHP, togaEffects);
                                break;
                            case 7:
                                enemyTeam[enemyTogaSlot] = new Tatsumi(togaHP, togaEffects);
                                break;
                            case 8:
                                enemyTeam[enemyTogaSlot] = new Tsunayoshi(togaHP, togaEffects);
                                break;
                            case 9:
                                enemyTeam[enemyTogaSlot] = new Kuroko(togaHP, togaEffects);
                                break;
                            case 11:
                                enemyTeam[enemyTogaSlot] = new Chachamaru(togaHP, togaEffects);
                                break;
                            case 12:
                                enemyTeam[enemyTogaSlot] = new Itachi(togaHP, togaEffects);
                                break;
                            case 13:
                                enemyTeam[enemyTogaSlot] = new Mirio(togaHP, togaEffects);
                                break;
                            case 14:
                                enemyTeam[enemyTogaSlot] = new Gunha(togaHP, togaEffects);
                                break;
                            case 15:
                                enemyTeam[enemyTogaSlot] = new Gray(togaHP, togaEffects);
                                break;
                            case 16:
                                enemyTeam[enemyTogaSlot] = new Minato(togaHP, togaEffects);
                                break;
                        }
                        allCharacters[enemyTogaSlot + 3] = enemyTeam[enemyTogaSlot];
                        allCharacters[enemyTogaSlot].startHP = allCharacters[enemyTogaSlot].hp;
                        enemyTeam[enemyTogaSlot].startHP = enemyTeam[enemyTogaSlot].hp;
                        adjustEffectUser(enemyTeam[enemyTogaSlot], togaUser);
                        eff.user = enemyTeam[enemyTogaSlot];
                        enemyTogaChanged = true;
                    }
                }

            }
            if (eff.name == "susanooEff")
            {
                eff.description[0] = "Itachi has " + eff.ddRemaining + " destructible defense.";
                if (eff.user.id == "12")
                {
                    eff.user.abilities[0] = eff.user.alt1;
                    eff.user.abilities[1] = eff.user.alt2;
                    eff.user.abilities[2].enabled = false;
                }
                if (eff.ddRemaining <= 0 || eff.user.hp <=20)
                {
                    endEffect(eff);
                    eff.user.removeEffect(eff);
                }
            }
            if (eff.name == "shadowClonesEff")
            {
                if (eff.user.id == "3")
                {
                    eff.user.abilities[0] = eff.user.alt1;
                    eff.user.abilities[0].cooldownRemaining = 0;
                    eff.user.abilities[2] = eff.user.alt2;
                    eff.user.damageReductionFlat += 10;
                }
                eff.user.damageReductionFlat += 10;
                eff.user.abilities[1].multiETarget = true;
            }
            if (eff.name == "gutsEff")
            {
                eff.user.abilities[0].enabled = true;
                eff.user.abilities[1].enabled = true;
                eff.user.abilities[2].enabled = true;
                if (eff.counters == 1)
                {
                    eff.description[0] = "Gunha has 1 stack of Guts.";
                }
                else
                {
                    eff.description[0] = "Gunha has " + eff.counters.ToString() + " stacks of Guts.";
                }
            }
            if (eff.name == "iceMakeUnlimitedAllyEff")
            {
                eff.description[1] = "This character has " + eff.ddRemaining + " destructible defense.";
            }
            if (eff.name == "sageModeEff")
            {
                if (eff.user.id == "3")
                {
                    if (eff.user.contains("shadowClonesEff"))
                    {
                        endEffect(findEffect("shadowClonesEff", eff.user));
                        removeEffect(findEffect("shadowClonesEff", eff.user));

                    }
                    eff.user.abilities[2] = eff.user.alt3;
                    eff.user.abilities[0].enabled = false;
                }
                eff.user.invulnerable = true;
            }
            if (eff.name == "toadTaijutsuEff")
            {
                eff.target.randomMod += 2;
                eff.target.abilities[0].randCost += 2;
                eff.target.abilities[1].randCost += 2;
                eff.target.abilities[2].randCost += 2;
                eff.target.abilities[3].randCost += 2;
            }
            if (eff.name == "iceMakeEff")
            {
                if (eff.user.id == "15")
                {
                    eff.user.abilities[0] = eff.user.alt1;
                    
                    eff.user.abilities[1].enabled = true;
                    eff.user.abilities[2].enabled = true;
                    eff.user.abilities[3].enabled = true;
                }
            }
            #region Damage Reduction Effects
            if (eff.name == "hearDistressAllyEff")
            {
                eff.target.energyContribution += eff.counters;
                eff.target.damageReductionFlat += 25;
            }
            #endregion

            #region Control ability checks
            if (eff.name == "windBladeCombatSelfEff")
            {
                if (eff.user.dead == true)
                {
                    eff.user.removeEffect(eff);
                }
            }
            if (eff.name == "windBladeCombatEnemyEff")
            {
                if (eff.user.dead == true)
                {
                    eff.target.removeEffect(eff);
                }
            }
            #endregion
            #region Attack Reduction Effects
            if (eff.name == "judgementThrowEnemyEff")
            {
                eff.target.damageBoostFlat -= 10;
            }
            if (eff.name == "judgementThrowDoubleEnemyEff")
            {
                eff.target.damageBoostFlat -= 20;
            }
            if (eff.name == "overwhelmingSuppressionEff")
            {
                eff.target.damageBoostFlat -= 5;
            }
            if (eff.name == "overwhelmingSuppressionDoubleEff")
            {
                eff.target.damageBoostFlat -= 10;
            }
            #endregion
            

        }
        public void adjustEffectUser(Character c, Character user)
        {
            for (int i = 0; i < 6; i++)
            {

                for (int j = 0; j < allCharacters[i].effects.Count; j++)
                {
                    if (allCharacters[i].effects[j].user == user)
                    {
                        allCharacters[i].effects[j].user = c;
                    }
                }

            }
        }
        public void checkNegationEffect(Effect eff)
        {
            #region Defense Negation Effects
            if (eff.name == "needlePinEnemyEff")
            {
                eff.target.damageReductionFlat = 0;
                eff.target.invulnerable = false;
            }
            if (eff.name == "overwhelmingSuppressionDoubleEff")
            {
                if (eff.counters == 1)
                {
                    eff.target.damageReductionFlat = 0;
                    eff.target.invulnerable = false;
                }
            }
            #endregion
            #region Stun Negation Effects
            if (eff.name == "windBladeCombatSelfEff")
            {
                eff.user.stunned = false;
            }
            #endregion
        }
        public bool isDamaging(Effect eff)
        {
            bool check = false;
            if (eff.damaging == true)
            {
                check = true;
            }
            return check;
        }
        public void tickDuration(Effect eff)
        {

            
            eff.duration -= 1;
            
            if (eff.duration <= 0)
            {
                endEffect(eff);
            }


        }
        public void effectAfflictionDamage(Effect eff, int dmg)
        {
            
            eff.target.hp -= dmg;
            if (eff.target.hp <= 0)
            {
                if (eff.target.contains("electricRageEff"))
                {
                    eff.target.hp = 1;
                }
                else if (eff.target.contains("luckyRabbitsFootEff"))
                {
                    eff.target.hp = 35;
                }
                else
                {
                    eff.target.hp = 0;
                    eff.target.dead = true;
                }
            }
        }
        public void effectDamage(Effect eff, int dmg)
        {
            int modDmg;
            modDmg = dmg - eff.target.damageReductionFlat;
            if (modDmg < 0) { modDmg = 0; }

            for (int i = 0; i < eff.target.destDef.Count; i++)
            {
                if ((modDmg - eff.target.destDef[i].ddRemaining) < 0)
                {
                    eff.target.destDef[i].ddRemaining = (modDmg - eff.target.destDef[i].ddRemaining) * -1;
                    modDmg = 0;
                }
                else
                {
                    modDmg -= eff.target.destDef[i].ddRemaining;
                    eff.target.destDef[i].ddRemaining = 0;
                }
            }

            eff.target.hp -= modDmg;
            if (eff.target.hp <= 0)
            {
                if (eff.target.contains("electricRageEff"))
                {
                    eff.target.hp = 1;
                }
                else if (eff.target.contains("luckyRabbitsFootEff"))
                {
                    eff.target.hp = 35;
                }
                else
                {
                    eff.target.hp = 0;
                    eff.target.dead = true;
                }
            }

        }
        public void endEffect(Effect eff)
        {
            #region Invulnerability effects
            if (eff.name == "natsuDodgeEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "invisibilityEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "flareBurstEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "enhancedLeapEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "zangetsuBlockEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "tensaZangetsuEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "saberParryEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "electricDeflectionEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "snowWhiteLeapEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "substitutionEff")
            {
                eff.target.invulnerable = false;
            }
            if (eff.name == "kurokoDodgeEff")
            {
                eff.user.invulnerable = false;
            }
            if (eff.name == "teleportingStrikeEFf")
            {
                eff.user.invulnerable = false;
            }
            if (eff.name == "togaDodgeEff")
            {
                eff.user.invulnerable = false;
            }
            #endregion

            #region Damage Reduction Effects
            if (eff.name == "hearDistressAllyEff")
            {
                eff.target.damageReductionFlat -= 25;
            }
            #endregion
            #region Cooldown Effects
            if (eff.name == "airForceGlovesEff")
            {
                eff.target.cooldownMod -= 1;
            }
            if (eff.name == "needlePinEff")
            {
                eff.user.abilities[0].cooldown = 2;
            }
            #endregion
            #region Stun Effects    
            if (eff.name == "smashEff")
            {
                eff.user.stunned = false;
            }
            if (eff.name == "zeroPointBreakthroughStunEff")
            {
                eff.target.stunned = false;
            }
            if (eff.name == "burningAxleStunEff")
            {
                eff.target.stunned = false;
            }
            if (eff.name == "rasenganEff")
            {
                eff.target.stunned = false;
            }
            if (eff.name == "uzumakiBarrageEff")
            {
                eff.target.stunned = false;
            }
            if (eff.name == "needlePinStunEff")
            {
                eff.target.stunned = false;
            }
            #endregion
            if (eff.name == "toadTaijutsuEff")
            {

                eff.target.randomMod -= 2;

            }
            if (eff.name == "shadowClonesEff")
            {

                eff.user.damageReductionFlat -= 10;
                eff.user.abilities[0] = eff.user.main1;
                eff.user.abilities[2] = eff.user.main3;
                eff.user.abilities[1].multiETarget = false;

            }
            if (eff.name == "sageModeEff")
            {
                eff.user.abilities[2] = eff.user.main3;
                eff.user.abilities[0] = eff.user.main1;
                eff.user.abilities[0].enabled = true;
                eff.user.invulnerable = false;
            }
            if (eff.name == "quirkTransformEff")
            {
                int togaHP = eff.user.hp;
                List<Effect> togaEffects = new List<Effect>();

                Character togaUser = eff.user;

                togaEffects = eff.user.effects;

                if (!isAllied(eff))
                {
                    enemyTeam[enemyTogaSlot] = new Toga(togaHP, togaEffects);
                    allCharacters[enemyTogaSlot + 3] = enemyTeam[enemyTogaSlot];
                    adjustEffectUser(enemyTeam[enemyTogaSlot], togaUser);
                    enemyTogaChanged = false;
                }
                

            }
            if (eff.name == "susanooEff")
            {
                if (eff.user.id == "12")
                {
                    eff.user.abilities[0] = eff.user.main1;
                    eff.user.abilities[1] = eff.user.main2;
                }
            }
            
            #region Invisible effect ending
            if (eff.name == "hearDistressAllyEff")
            {
                eff.user.effects.Add(new Effect(2, "hearDistressAllyEndEff", "snowwhite2.png", eff.user, eff.user, "Hear Distress", "Hear Distress has ended."));
                eff.target.damageReductionFlat -= 25;
            }
            if (eff.name == "luckyRabbitsFootEff")
            {
                eff.user.effects.Add(new Effect(2, "luckyRabbitsFootEndEff", "snowwhite3.png", eff.user, eff.target, "Lucky Rabbit's Foot", "Lucky Rabbit's Foot has ended."));
            }
            if (eff.name == "shootStyleEff")
            {

                eff.user.effects.Add(new Effect(2, "shootStyleEndEff", "midoriya3.png", eff.user, eff.user, "One For All - Shoot Style", "One For All - Shoot Style has ended."));

            }
            if (eff.name == "hearDistressEnemyEff")
            {

                eff.user.effects.Add(new Effect(2, "hearDistressEnemyEndEff", "snowwhite2.png", eff.user, eff.user, "Hear Distress", "Hear Distress has ended."));

            }
            if (eff.name == "zeroPointBreakthroughEff")
            {

                eff.user.effects.Add(new Effect(2, "zeroPointBreakthroughEndEff", "tsunayoshi2.png", eff.user, eff.user, "Zero Point Breakthrough", "Zero Point Breakthrough has ended."));

            }
            if (eff.name == "protectAllyEff")
            {
                eff.user.effects.Add(new Effect(2, "protectAllyEndEff", "mirio3.png", eff.user, eff.target, "Protect Ally", "Protect Ally has ended."));
            }
            if (eff.name == "quirkPermeationEff")
            {
                eff.user.effects.Add(new Effect(2, "quirkPermeationEndEff", "mirio1.png", eff.user, eff.target, "Quirk - Permeation", "Quirk - Permeation has ended."));
            }
            #endregion
            #region Destructible Defense Effects
            if (eff.name == "ironSandEff")
            {
                eff.target.destDef.RemoveAt(eff.ddIndex);
            }
            if (eff.name == "incursioEff")
            {
                eff.target.destDef.RemoveAt(eff.ddIndex);
                eff.target.abilities[2].enabled = false;
            }
            #endregion
        }

        public Character findAllyByID(int n)
        {
            Character c = new Character();

            for (int i = 0; i < 3; i++)
            {

                if (playerTeam[i].id == n.ToString())
                {
                    c = playerTeam[i];
                }

            }

            return c;
        }

        public Character findEnemyByID(int n)
        {
            Character c = new Character();
            for (int i = 0; i < 3; i++)
            {
                if (enemyTeam[i].id == n.ToString())
                {
                    c = enemyTeam[i];
                }
            }
            return c;
        }

        public int slotByCharacter(Character c)
        {
            int slot = 15;
            for (int i = 0; i < allCharacters.Length; i++)
            {
                if (allCharacters[i] == c)
                {
                    slot = i;
                }
            }
            return slot;
        }
        public string serializeGameState()
        {
            string serial="";
            string targetChar = "";
            string userChar = "";

            #region HP Strings
            serial += "HP";

            for (int i = 0; i < 3; i++)
            {
                serial += "|" + playerTeam[i].hp.ToString();
            }
            for (int i = 0; i < 3; i++)
            {
                serial += "|" + enemyTeam[i].hp.ToString();
            }
            serial += "|";
            #endregion
            #region Effect Strings
            //      |Effect ID/Duration/Target/User/&DDremaining/#DDIndex|
            for (int i = 0; i < 3; i++)
            {

                serial += "!EFFP" + i.ToString() + "-";

                for (int j = 0; j < playerTeam[i].effects.Count; j++)
                {
                    
                    switch (playerTeam[i].effects[j].name)
                    {
                        
                        case "shadowClonesEff":
                            serial += "0/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "sageModeEff":
                            serial += "1/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "rasenganEff":
                            serial += "2/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "uzumakiBarrageEff":
                            serial += "3/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "uzumakiBarrageSelfEff":
                            serial += "4/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "toadTaijutsuEff":
                            serial += "5/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "substitutionEff":
                            serial += "6/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "tensaZangetsuEff":
                            serial += "7/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "tensaZangetsuBonusEff":
                            serial += "8/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zangetsuStrikeEff":
                            serial += "9/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "zangetsuBlockEff":
                            serial += "10/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "hearDistressAllyEff":
                            serial += "11/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "hearDistressEnemyEff":
                            serial += "12/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "hearDistressSuccessEff":
                            serial += "13/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "hearDistressEndEff":
                            serial += "14/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "luckyRabbitsFootEff":
                            serial += "15/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "luckyRabbitsFootEndEff":
                            serial += "16/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "snowWhiteLeapEff":
                            serial += "17/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "windBladeCombatEnemyEff":
                            serial += "18/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "windBladeCombatSelfEff":
                            serial += "19/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "avalonEff":
                            serial += "20/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "saberParryEff":
                            serial += "21/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "electricRageEff":
                            serial += "22/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "ironSandEff":
                            serial += "23/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].ddRemaining + "/" + playerTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "electricDeflectionEff":
                            serial += "24/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "fireDragonRoarEff":
                            serial += "25/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "fireDragonSwordHornEff":
                            serial += "26/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "natsuDodgeEff":
                            serial += "27/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "incursioEff":
                            serial += "28/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].ddRemaining + "/" + playerTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "neuntoteEff":
                            serial += "29/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "invisibilityEff":
                            serial += "30/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughEff":
                            serial += "31/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughStunEff":
                            serial += "32/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughSuccessEff":
                            serial += "33/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughBoostEff":
                            serial += "34/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughEndEff":
                            serial += "35/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "burningAxleEff":
                            serial += "36/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "burningAxleStunEff":
                            serial += "37/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "smashEff":
                            serial += "38/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "airForceGlovesEff":
                            serial += "39/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "shootStyleEff":
                            serial += "40/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "shootStyleSuccessEff":
                            serial += "41/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "shootStyleEndEff":
                            serial += "42/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "enhancedLeapEff":
                            serial += "43/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "flareBurstEff":
                            serial += "44/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "teleportingStrikeEff":
                            serial += "45/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "needlePinEff":
                            serial += "46/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "needlePinEnemyEff":
                            serial += "47/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "needlePinStunEff":
                            serial += "48/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "judgementThrowEff":
                            serial += "49/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "judgementThrowEnemyEff":
                            serial += "50/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "judgementThrowDoubleEnemyEff":
                            serial += "51/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "kurokoDodgeEff":
                            serial += "52/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "thirstingKnifeEff":
                            serial += "53/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "vacuumSyringeDamageEff":
                            serial += "54/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "vacuumSyringeEff":
                            serial += "55/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "quirkTransformEff":
                            serial += "56/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "togaDodgeEff":
                            serial += "57/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "targetLockEff":
                            serial += "58/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "activeCombatModeSelfEff":
                            serial += "59/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].ddRemaining + "/" + playerTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "activeCombatModeEnemyEff":
                            serial += "60/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "takeFlightEff":
                            serial += "61/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "amaterasuEff":
                            serial += "62/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "tsukuyomiEff":
                            serial += "63/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "susanooEff":
                            serial += "64/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].ddRemaining + "/" + playerTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "totsukaBladeEff":
                            serial += "65/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "crowGenjutsuEff":
                            serial += "66/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "quirkPermeationEff":
                            serial += "67/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "quirkPermeationEndEff":
                            serial += "68/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "protectAllyEff":
                            serial += "69/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "protectAllyEndEff":
                            serial += "70/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "phantomMenaceEff":
                            serial += "71/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "mirioDodgeEff":
                            serial += "72/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "superAwesomePunchEff":
                            serial += "73/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "overwhelmingSuppressionEff":
                            serial += "74/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "overwhelmingSuppressionDoubleEff":
                            serial += "75/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "gutsEff":
                            serial += "76/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].counters + "]";
                            break;
                        case "iceMakeEff":
                            serial += "77/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeUnlimitedEnemyEff":
                            serial += "78/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeUnlimitedAllyEff":
                            serial += "79/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "/" + playerTeam[i].effects[j].ddRemaining + "/" + playerTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "iceMakeFreezeLancerEff":
                            serial += "80/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeHammerEff":
                            serial += "81/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeShieldEff":
                            serial += "82/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "flyingRaijinEff":
                            serial += "83/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "markedKunaiEff":
                            serial += "84/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "partialShikiFuujinEff":
                            serial += "85/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "minatoParryEff":
                            serial += "86/" + playerTeam[i].effects[j].duration + "/" + slotByCharacter(playerTeam[i].effects[j].user).ToString() + "]";
                            break;
                    }
                }
                
            }
            for (int i = 0; i < 3; i++)
            {
                serial += "!EFFE" + i.ToString() + "-";
                for (int j = 0; j < enemyTeam[i].effects.Count; j++)
                {
                 
                    switch (enemyTeam[i].effects[j].name)
                    {


                        case "shadowClonesEff":
                            serial += "0/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "sageModeEff":
                            serial += "1/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "rasenganEff":
                            serial += "2/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "uzumakiBarrageEff":
                            serial += "3/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "uzumakiBarrageSelfEff":
                            serial += "4/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "toadTaijutsuEff":
                            serial += "5/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "substitutionEff":
                            serial += "6/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "tensaZangetsuEff":
                            serial += "7/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "tensaZangetsuBonusEff":
                            serial += "8/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zangetsuStrikeEff":
                            serial += "9/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "zangetsuBlockEff":
                            serial += "10/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "hearDistressAllyEff":
                            serial += "11/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "hearDistressEnemyEff":
                            serial += "12/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "hearDistressSuccessEff":
                            serial += "13/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "hearDistressEndEff":
                            serial += "14/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "luckyRabbitsFootEff":
                            serial += "15/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "luckyRabbitsFootEndEff":
                            serial += "16/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "snowWhiteLeapEff":
                            serial += "17/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "windBladeCombatEnemyEff":
                            serial += "18/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "windBladeCombatSelfEff":
                            serial += "19/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "avalonEff":
                            serial += "20/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "saberParryEff":
                            serial += "21/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "electricRageEff":
                            serial += "22/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "ironSandEff":
                            serial += "23/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].ddRemaining + "/" + enemyTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "electricDeflectionEff":
                            serial += "24/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "fireDragonRoarEff":
                            serial += "25/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "fireDragonSwordHornEff":
                            serial += "26/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "natsuDodgeEff":
                            serial += "27/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "incursioEff":
                            serial += "28/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].ddRemaining + "/" + enemyTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "neuntoteEff":
                            serial += "29/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "invisibilityEff":
                            serial += "30/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughEff":
                            serial += "31/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughStunEff":
                            serial += "32/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughSuccessEff":
                            serial += "33/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughBoostEff":
                            serial += "34/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "zeroPointBreakthroughEndEff":
                            serial += "35/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "burningAxleEff":
                            serial += "36/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "burningAxleStunEff":
                            serial += "37/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "smashEff":
                            serial += "38/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "airForceGlovesEff":
                            serial += "39/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "shootStyleEff":
                            serial += "40/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "shootStyleSuccessEff":
                            serial += "41/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "shootStyleEndEff":
                            serial += "42/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "enhancedLeapEff":
                            serial += "43/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "flareBurstEff":
                            serial += "44/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "teleportingStrikeEff":
                            serial += "45/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "needlePinEff":
                            serial += "46/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "needlePinEnemyEff":
                            serial += "47/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "needlePinStunEff":
                            serial += "48/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "judgementThrowEff":
                            serial += "49/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "judgementThrowEnemyEff":
                            serial += "50/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "judgementThrowDoubleEnemyEff":
                            serial += "51/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "kurokoDodgeEff":
                            serial += "52/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "thirstingKnifeEff":
                            serial += "53/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "vacuumSyringeDamageEff":
                            serial += "54/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "vacuumSyringeEff":
                            serial += "55/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "quirkTransformEff":
                            serial += "56/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "togaDodgeEff":
                            serial += "57/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "targetLockEff":
                            serial += "58/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "activeCombatModeSelfEff":
                            serial += "59/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].ddRemaining + "/" + enemyTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "activeCombatModeEnemyEff":
                            serial += "60/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "takeFlightEff":
                            serial += "61/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "amaterasuEff":
                            serial += "62/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "tsukuyomiEff":
                            serial += "63/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "susanooEff":
                            serial += "64/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].ddRemaining + "/" + enemyTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "totsukaBladeEff":
                            serial += "65/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "crowGenjutsuEff":
                            serial += "66/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "quirkPermeationEff":
                            serial += "67/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "quirkPermeationEndEff":
                            serial += "68/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "protectAllyEff":
                            serial += "69/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "protectAllyEndEff":
                            serial += "70/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "phantomMenaceEff":
                            serial += "71/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "mirioDodgeEff":
                            serial += "72/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "superAwesomePunchEff":
                            serial += "73/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "overwhelmingSuppressionEff":
                            serial += "74/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "overwhelmingSuppressionDoubleEff":
                            serial += "75/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "gutsEff":
                            serial += "76/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].counters + "]";
                            break;
                        case "iceMakeEff":
                            serial += "77/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeUnlimitedEnemyEff":
                            serial += "78/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeUnlimitedAllyEff":
                            serial += "79/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "/" + enemyTeam[i].effects[j].ddRemaining + "/" + enemyTeam[i].effects[j].ddIndex + "]";
                            break;
                        case "iceMakeFreezeLancerEff":
                            serial += "80/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeHammerEff":
                            serial += "81/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "iceMakeShieldEff":
                            serial += "82/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "flyingRaijinEff":
                            serial += "83/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "markedKunaiEff":
                            serial += "84/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "partialShikiFuujinEff":
                            serial += "85/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                        case "minatoParryEff":
                            serial += "86/" + enemyTeam[i].effects[j].duration + "/" + slotByCharacter(enemyTeam[i].effects[j].user).ToString() + "]";
                            break;
                    }
                }

            }

            #endregion
           
            serial += "!";

            return serial;
        }

        public void deSerializeGameState(string s)
        {
            s = s.Remove(0, 3);
            string workingString;
            string[] eA = new string[16];
            #region HP Deserialization
            for (int i = 0; i < 3; i++)
            {

                enemyTeam[i].hp = int.Parse(s.Substring(0, s.IndexOf('|')));
                s = s.Remove(0, s.IndexOf('|') + 1);
            }
            for (int i = 0; i < 3; i++)
            {
                playerTeam[i].hp = int.Parse(s.Substring(0, s.IndexOf('|')));
                s = s.Remove(0, s.IndexOf('|') + 1);
            }
            #endregion
            #region Effect Deserialization
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < enemyTeam.Length; j++)
                {
                    enemyTeam[i].effects.Clear();
                    enemyTeam[i].destDef.Clear();
                }
                s = s.Remove(0, s.IndexOf('-') + 1);
                if (s[0] != '!' && s[0] != '#') 
                { 
                    workingString = s.Substring(0, s.IndexOf('!')-1);
                    eA = workingString.Split(']');

                    for (int j = 0; j < eA.Length; j++)
                    {
                        enemyTeam[i].effects.Add(getEffectByID(eA[j], enemyTeam[i]));
                        
                    }
                }   

            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < playerTeam.Length; j++)
                {
                    playerTeam[i].effects.Clear();
                    playerTeam[i].destDef.Clear();
                }
                s = s.Remove(0, s.IndexOf('-') + 1);
                if (s[0] != '!' && s[0] != '#')
                {
                    workingString = s.Substring(0, s.IndexOf('!')-1);
                    eA = workingString.Split(']');

                    for (int j = 0; j < eA.Length; j++)
                    {
                        playerTeam[i].effects.Add(getEffectByID(eA[j], playerTeam[i]));
                        
                    }

                }
            }
            #endregion
        }
        public Effect makeUniqueEffect(Effect eff)
        {
            Effect outputEffect = new Effect();

            outputEffect.name = eff.name;
            outputEffect.imgPath = eff.imgPath;
            outputEffect.displayName = eff.displayName;
            for (int i = 0; i < eff.description.Count; i++)
            {
                outputEffect.description.Add(eff.description[i]);
            }


            return outputEffect;
        }
        public Effect getEffectByID(string s, Character c)
        {
            Effect eff = new Effect();
            string[] effStr = new string[5];

            effStr = s.Split('/');

            switch (effStr[0])
            {
                case "0":
                    eff = makeUniqueEffect(shadowClonesEff);
                    break;
                case "1":
                    eff = makeUniqueEffect(sageModeEff);
                    break;
                case "2":
                    eff = makeUniqueEffect(rasenganEff);
                    break;
                case "3":
                    eff = makeUniqueEffect(uzumakiBarrageEff);
                    break;
                case "4":
                    eff = makeUniqueEffect(uzumakiBarrageSelfEff);
                    break;
                case "5":
                    eff = makeUniqueEffect(toadTaijutsuEff);
                    break;
                case "6":
                    eff = makeUniqueEffect(substitutionEff);
                    break;
                case "7":
                    eff = makeUniqueEffect(tensaZangetsuEff);
                    break;
                case "8":
                    eff = makeUniqueEffect(tensaZangetsuBonusEff);
                    break;
                case "9":
                    eff = makeUniqueEffect(zangetsuStrikeEff);
                    eff.counters = int.Parse(effStr[3]);
                    if (eff.counters == 1)
                    {
                        eff.description.Add("Ichigo has 1 stack of Zangetsu Strike.");
                    }
                    else if (eff.counters > 1)
                    {
                        eff.description.Add("Ichigo has " + eff.counters.ToString() + " stacks of Zangetsu Strike.");
                    }
                    break;
                case "10":
                    eff = makeUniqueEffect(zangetsuBlockEff);
                    break;
                case "11":
                    eff = makeUniqueEffect(hearDistressAllyEff);
                    eff.invisible = true;
                    eff.counters = int.Parse(effStr[3]);
                    break;
                case "12":
                    eff = makeUniqueEffect(hearDistressEnemyEff);
                    eff.invisible = true;
                    break;
                case "13":
                    eff = makeUniqueEffect(hearDistressSuccessEff);
                    break;
                case "14": 
                    eff = makeUniqueEffect(hearDistressEndEff);
                    break;
                case "15":
                    eff = makeUniqueEffect(luckyRabbitsFootEff);
                    eff.invisible = true;
                    break;
                case "16":
                    eff = makeUniqueEffect(luckyRabbitsFootEndEff);
                    break;
                case "17":
                    eff = makeUniqueEffect(snowWhiteLeapEff);
                    break;
                case "18":
                    eff = makeUniqueEffect(windBladeCombatEnemyEff);
                    break;
                case "19":
                    eff = makeUniqueEffect(windBladeCombatSelfEff);
                    break;
                case "20":
                    eff = makeUniqueEffect(avalonEff);
                    break;
                case "21":
                    eff = makeUniqueEffect(saberParryEff);
                    break;
                case "22":
                    eff = makeUniqueEffect(electricRageEff);
                    eff.counters = int.Parse(effStr[3]);
                    break;
                case "23":
                    eff = makeUniqueEffect(ironSandEff);
                    eff.ddRemaining = int.Parse(effStr[3]);
                    eff.ddIndex = int.Parse(effStr[4]);
                    c.destDef.Add(eff);
                    break;
                case "24":
                    eff = makeUniqueEffect(electricDeflectionEff);
                    break;
                case "25":
                    eff = makeUniqueEffect(fireDragonRoarEff);
                    break;
                case "26":
                    eff = makeUniqueEffect(fireDragonSwordHornEff);
                    break;
                case "27":
                    eff = makeUniqueEffect(natsuDodgeEff);
                    break;
                case "28":
                    eff = makeUniqueEffect(incursioEff);
                    eff.ddRemaining = int.Parse(effStr[3]);
                    eff.ddIndex = int.Parse(effStr[4]);
                    c.destDef.Add(eff);
                    break;
                case "29":
                    eff = makeUniqueEffect(neuntoteEff);
                    break;
                case "30":
                    eff = makeUniqueEffect(invisibilityEff);
                    break;
                case "31":
                    eff = makeUniqueEffect(zeroPointBreakthroughEff);
                    eff.invisible = true;
                    break;
                case "32":
                    eff = makeUniqueEffect(zeroPointBreakthroughStunEff);
                    break;
                case "33":
                    eff = makeUniqueEffect(zeroPointBreakthroughSuccessEff);
                    break;
                case "34":
                    eff = makeUniqueEffect(zeroPointBreakthroughBoostEff);
                    break;
                case "35":
                    eff = makeUniqueEffect(zeroPointBreakthroughEndEff);
                    break;
                case "36":
                    eff = makeUniqueEffect(burningAxleEff);
                    break;
                case "37":
                    eff = makeUniqueEffect(burningAxleStunEff);
                    break;
                case "38":
                    eff = makeUniqueEffect(smashEff);
                    break;
                case "39":
                    eff = makeUniqueEffect(airForceGlovesEff);
                    break;
                case "40":
                    eff = makeUniqueEffect(shootStyleEff);
                    eff.invisible = true;
                    break;
                case "41":
                    eff = makeUniqueEffect(shootStyleSuccessEff);
                    break;
                case "42":
                    eff = makeUniqueEffect(shootStyleEndEff);
                    break;
                case "43":
                    eff = makeUniqueEffect(enhancedLeapEff);
                    break;
                case "44":
                    eff = makeUniqueEffect(flareBurstEff);
                    break;
                case "45":
                    eff = makeUniqueEffect(teleportingStrikeEff);
                    break;
                case "46":
                    eff = makeUniqueEffect(needlePinEff);
                    break;
                case "47":
                    eff = makeUniqueEffect(needlePinEnemyEff);
                    break;
                case "48":
                    eff = makeUniqueEffect(needlePinStunEff);
                    break;
                case "49":
                    eff = makeUniqueEffect(judgementThrowEff);
                    break;
                case "50":
                    eff = makeUniqueEffect(judgementThrowEnemyEff);
                    break;
                case "51":
                    eff = makeUniqueEffect(judgementThrowDoubleEnemyEff);
                    break;
                case "52":
                    eff = makeUniqueEffect(kurokoDodgeEff);
                    break;
                case "53":
                    eff = makeUniqueEffect(thirstingKnifeEff);
                    eff.counters = int.Parse(effStr[3]);
                    if (eff.counters == 1)
                    {
                        eff.description.Add("Toga has drunk this character's blood 1 time.");
                    }
                    else
                    {
                        eff.description.Add("Toga has drunk this character's blood " + eff.counters.ToString() + " times.");
                    }
                    break;
                case "54":
                    eff = makeUniqueEffect(vacuumSyringeDamageEff);
                    break;
                case "55":
                    eff = makeUniqueEffect(vacuumSyringeEff);
                    eff.counters = int.Parse(effStr[3]);
                    if (eff.counters == 1)
                    {
                        eff.description.Add("Toga has drunk this character's blood 1 time.");
                    }
                    else
                    {
                        eff.description.Add("Toga has drunk this character's blood " + eff.counters.ToString() + " times.");
                    }
                    break;
                case "56":
                    eff = makeUniqueEffect(quirkTransformEff);
                    eff.counters = int.Parse(effStr[3]);
                    break;
                case "57":
                    eff = makeUniqueEffect(togaDodgeEff);
                    break;
                case "58":
                    eff = makeUniqueEffect(targetLockEff);
                    break;
                case "59":
                    eff = makeUniqueEffect(activeCombatModeSelfEff);
                    eff.ddRemaining = int.Parse(effStr[3]);
                    eff.ddIndex = int.Parse(effStr[4]);
                    c.destDef.Add(eff);
                    break;
                case "60":
                    eff = makeUniqueEffect(activeCombatModeEnemyEff);
                    break;
                case "61":
                    eff = makeUniqueEffect(takeFlightEff);
                    break;
                case "62":
                    eff = makeUniqueEffect(amaterasuEff);
                    break;
                case "63":
                    eff = makeUniqueEffect(tsukuyomiEff);
                    break;
                case "64":
                    eff = makeUniqueEffect(susanooEff);
                    eff.ddRemaining = int.Parse(effStr[3]);
                    eff.ddIndex = int.Parse(effStr[4]);
                    c.destDef.Add(eff);
                    break;
                case "65":
                    eff = makeUniqueEffect(totsukaBladeEff);
                    break;
                case "66":
                    eff = makeUniqueEffect(crowGenjutsuEff);
                    break;
                case "67":
                    eff = makeUniqueEffect(quirkPermeationEff);
                    eff.invisible = true;
                    break;
                case "68":
                    eff = makeUniqueEffect(quirkPermeationEndEff);
                    break;
                case "69":
                    eff = makeUniqueEffect(protectAllyEff);
                    eff.invisible = true;
                    break;
                case "70":
                    eff = makeUniqueEffect(protectAllyEndEff);
                    break;
                case "71":
                    eff = makeUniqueEffect(phantomMenaceEff);
                    break;
                case "72":
                    eff = makeUniqueEffect(mirioDodgeEff);
                    break;
                case "73":
                    eff = makeUniqueEffect(superAwesomePunchEff);
                    break;
                case "74":
                    eff = makeUniqueEffect(overwhelmingSuppressionEff);
                    break;
                case "75":
                    eff = makeUniqueEffect(overwhelmingSuppressionDoubleEff);
                    eff.counters = int.Parse(effStr[3]);
                    if (eff.counters == 1)
                    {
                        eff.description.Add("This character cannot reduce damage or become invulnerable.");
                    }
                    break;
                case "76":
                    eff = makeUniqueEffect(gutsEff);
                    eff.counters = int.Parse(effStr[3]);
                    if (eff.counters == 1)
                    {
                        eff.description[0] = "Gunha has 1 stack of Guts.";
                    }
                    else
                    {
                        eff.description[0] = "Gunha has " + eff.counters.ToString() + " stacks of Guts.";
                    }
                    break;
                case "77":
                    eff = makeUniqueEffect(iceMakeEff);
                    break;
                case "78":
                    eff = makeUniqueEffect(iceMakeUnlimitedEnemyEff);
                    break;
                case "79":
                    eff = makeUniqueEffect(iceMakeUnlimitedAllyEff);
                    eff.ddRemaining = int.Parse(effStr[3]);
                    eff.ddIndex = int.Parse(effStr[4]);
                    c.destDef.Add(eff);
                    eff.description[1] = "This character has " + eff.ddRemaining.ToString() + " destructible defense.";
                    break;
                case "80":
                    eff = makeUniqueEffect(iceMakeFreezeLancerEff);
                    break;
                case "81":
                    eff = makeUniqueEffect(iceMakeHammerEff);
                    break;
                case "82":
                    eff = makeUniqueEffect(iceMakeShieldEff);
                    break;
                case "83":
                    eff = makeUniqueEffect(flyingRaijinEff);
                    break;
                case "84":
                    eff = makeUniqueEffect(markedKunaiEff);
                    break;
                case "85":
                    eff = makeUniqueEffect(partialShikiFuujinEff);
                    break;
                case "86":
                    eff = makeUniqueEffect(minatoParryEff);
                    break;

            }
            eff.duration = int.Parse(effStr[1]);
            eff.user = allCharacters[slotReversal(int.Parse(effStr[2]))];
            eff.target = c;
            
            return eff;
        }

        public int slotReversal(int i)
        {
            if (i > 2)
            {
                i = i - 3;
            }
            else
            {
                i = i + 3;
            }
            return i;
        }

        public void resolveEffect(Effect eff)
        {
            if (eff.name == "fireDragonSwordHornEff")
            {
                effectAfflictionDamage(eff, 5);
            }
            if (eff.name == "fireDragonRoarEff")
            {

                effectAfflictionDamage(eff, 10);

                endEffect(eff);
                
            }
            if (eff.name == "windBladeCombatEnemyEff")
            {
                effectDamage(eff, 10);
            }
            if (eff.name == "activeCombatModeEnemyEff")
            {
                effectDamage(eff, 10);
            }
            if (eff.name == "iceMakeFreezeLancerEff")
            {
                effectDamage(eff, 15);
            }
            if (eff.name == "iceMakeUnlimitedEnemyEff")
            {
                effectDamage(eff, 5);
            }
            if (eff.name == "iceMakeUnlimitedAllyEff")
            {
                eff.target.destDef[eff.target.findEffect("iceMakeUnlimitedAllyEff").ddIndex].ddRemaining += 5;
            }
            if (eff.name == "activeCombatModeSelfEff") 
            {
                eff.target.destDef[eff.target.findEffect("activeCombatModeSelfEff").ddIndex].ddRemaining += 15;
            }
            if (eff.name == "avalonEff")
            {
                eff.target.hp += 10;
                if (eff.target.hp > 100)
                {
                    eff.target.hp = 100;
                }
            }
            if (eff.name == "vacuumSyringeDamageEff")
            {
                effectAfflictionDamage(eff, 10);
                eff.target.findEffect("vacuumSyringeEff").counters += 1;
            }
            if (eff.name == "amaterasuEff")
            {
                effectAfflictionDamage(eff, 10);
            }
            if (eff.name == "susanooEff")
            {
                eff.user.hp -= 10;
            }
            if (eff.name == "tensaZangetsuEff")
            {
                playerTeamTempEnergy[3] += 1;
            }
        }
        public void executeTurn()
        {
            for (int i = 0; i < 6; i++)
            {
                if (i < 3)
                {
                    if (playerTeam[i].effects.Count > 0)
                    {
                        for (int j = 0; j < playerTeam[i].effects.Count; j++)
                        {
                            int num = 3;
                            num += i * 16;
                            num += j;
                            executionOrder.Add(num);
                        }
                    }
                }
                if (i > 2)
                {
                    if (enemyTeam[i-3].effects.Count > 0)
                    {
                        for (int j = 0; j < enemyTeam[i-3].effects.Count; j++)
                        {
                            int num = 3;
                            num += i * 16;
                            num += j;
                            executionOrder.Add(num);
                        }
                    }
                }

              
            }
            for (int i = 0; i < executionOrder.Count; i++)
            {
                switch (executionOrder[i])
                {
                    #region Player Ability Execution
                    case 0:
                        switch (playerTeam[0].chosenAbilitySlot)
                        {
                            case 0:
                                playerTeam[0].executeAbility1(playerTeam, enemyTeam);
                                playerTeam[0].abilities[0].cooldownRemaining = playerTeam[0].abilities[0].cooldown + 1 + playerTeam[0].cooldownMod;
                                break;
                            case 1:
                                playerTeam[0].executeAbility2(playerTeam, enemyTeam);
                                playerTeam[0].abilities[1].cooldownRemaining = playerTeam[0].abilities[1].cooldown + 1 + playerTeam[0].cooldownMod;
                                break;
                            case 2:
                                playerTeam[0].executeAbility3(playerTeam, enemyTeam);
                                playerTeam[0].abilities[2].cooldownRemaining = playerTeam[0].abilities[2].cooldown + 1 + playerTeam[0].cooldownMod;
                                break;
                            case 3:
                                playerTeam[0].executeAbility4(playerTeam, enemyTeam);
                                playerTeam[0].abilities[3].cooldownRemaining = playerTeam[0].abilities[3].cooldown + 1 + playerTeam[0].cooldownMod;
                                break;
                        }
                        break;
                    case 1:
                        switch (playerTeam[1].chosenAbilitySlot)
                        {
                            case 0:
                                playerTeam[1].executeAbility1(playerTeam, enemyTeam);
                                playerTeam[1].abilities[0].cooldownRemaining = playerTeam[1].abilities[0].cooldown + 1 + playerTeam[1].cooldownMod;
                                break;
                            case 1:
                                playerTeam[1].executeAbility2(playerTeam, enemyTeam);
                                playerTeam[1].abilities[1].cooldownRemaining = playerTeam[1].abilities[1].cooldown + 1 + playerTeam[1].cooldownMod;
                                break;
                            case 2:
                                playerTeam[1].executeAbility3(playerTeam, enemyTeam);
                                playerTeam[1].abilities[2].cooldownRemaining = playerTeam[1].abilities[2].cooldown + 1 + playerTeam[1].cooldownMod;
                                break;
                            case 3:
                                playerTeam[1].executeAbility4(playerTeam, enemyTeam);
                                playerTeam[1].abilities[3].cooldownRemaining = playerTeam[1].abilities[3].cooldown + 1 + playerTeam[1].cooldownMod;
                                break;
                        }
                        break;
                    case 2:
                        switch (playerTeam[2].chosenAbilitySlot)
                        {
                            case 0:
                                playerTeam[2].executeAbility1(playerTeam, enemyTeam);
                                playerTeam[2].abilities[0].cooldownRemaining = playerTeam[2].abilities[0].cooldown + 1 + playerTeam[2].cooldownMod;
                                break;
                            case 1:
                                playerTeam[2].executeAbility2(playerTeam, enemyTeam);
                                playerTeam[2].abilities[1].cooldownRemaining = playerTeam[2].abilities[1].cooldown + 1 + playerTeam[2].cooldownMod;
                                break;
                            case 2:
                                playerTeam[2].executeAbility3(playerTeam, enemyTeam);
                                playerTeam[2].abilities[2].cooldownRemaining = playerTeam[2].abilities[2].cooldown + 1 + playerTeam[2].cooldownMod;
                                break;
                            case 3:
                                playerTeam[2].executeAbility4(playerTeam, enemyTeam);
                                playerTeam[2].abilities[3].cooldownRemaining = playerTeam[2].abilities[3].cooldown + 1 + playerTeam[2].cooldownMod;
                                break;
                        }
                        break;
                    #endregion
                    #region Player 1 Effect execution
                    case 3:
                        if ((playerTeam[0].effects.Count > 0) && isAllied(playerTeam[0].effects[0]) && playerTeam[0].effects[0].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[0]);
                        }

                        break;
                    case 4:
                        if ((playerTeam[0].effects.Count > 1) && isAllied(playerTeam[0].effects[1]) && playerTeam[0].effects[1].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[1]);
                        }

                        break;
                    case 5:
                        if ((playerTeam[0].effects.Count > 2) && isAllied(playerTeam[0].effects[2]) && playerTeam[0].effects[2].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[2]);
                        }

                        break;
                    case 6:
                        if ((playerTeam[0].effects.Count > 3) && isAllied(playerTeam[0].effects[3]) && playerTeam[0].effects[3].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[3]);
                        }

                        break;
                    case 7:
                        if ((playerTeam[0].effects.Count > 4) && isAllied(playerTeam[0].effects[4]) && playerTeam[0].effects[4].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[4]);
                        }

                        break;
                    case 8:
                        if ((playerTeam[0].effects.Count > 5) && isAllied(playerTeam[0].effects[5]) && playerTeam[0].effects[5].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[5]);
                        }

                        break;
                    case 9:
                        if ((playerTeam[0].effects.Count > 6) && isAllied(playerTeam[0].effects[6]) && playerTeam[0].effects[6].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[6]);
                        }

                        break;
                    case 10:
                        if ((playerTeam[0].effects.Count > 7) && isAllied(playerTeam[0].effects[7]) && playerTeam[0].effects[7].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[7]);
                        }

                        break;
                    case 11:
                        if ((playerTeam[0].effects.Count > 8) && isAllied(playerTeam[0].effects[8]) && playerTeam[0].effects[8].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[8]);
                        }

                        break;
                    case 12:
                        if ((playerTeam[0].effects.Count > 9) && isAllied(playerTeam[0].effects[9]) && playerTeam[0].effects[9].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[9]);
                        }

                        break;
                    case 13:
                        if ((playerTeam[0].effects.Count >10) && isAllied(playerTeam[0].effects[10]) && playerTeam[0].effects[10].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[10]);
                        }

                        break;
                    case 14:
                        if ((playerTeam[0].effects.Count >11) && isAllied(playerTeam[0].effects[11]) && playerTeam[0].effects[11].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[11]);
                        }

                        break;
                    case 15:
                        if ((playerTeam[0].effects.Count > 12) && isAllied(playerTeam[0].effects[12]) && playerTeam[0].effects[12].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[12]);
                        }

                        break;
                    case 16:
                        if ((playerTeam[0].effects.Count > 13) && isAllied(playerTeam[0].effects[13]) && playerTeam[0].effects[13].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[13]);
                        }

                        break;
                    case 17:
                        if ((playerTeam[0].effects.Count > 14) && isAllied(playerTeam[0].effects[14]) && playerTeam[0].effects[14].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[14]);
                        }

                        break;
                    case 18:
                        if ((playerTeam[0].effects.Count > 15) && isAllied(playerTeam[0].effects[15]) && playerTeam[0].effects[15].resolve == true)
                        {
                            resolveEffect(playerTeam[0].effects[15]);
                        }

                        break;
                    #endregion
                    #region Player 2 Effect execution
                    case 19:
                        if ((playerTeam[1].effects.Count > 0) && isAllied(playerTeam[1].effects[0]) && playerTeam[1].effects[0].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[0]);
                        }

                        break;
                    case 20:
                        if ((playerTeam[1].effects.Count > 1) && isAllied(playerTeam[1].effects[1]) && playerTeam[1].effects[1].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[1]);
                        }

                        break;
                    case 21:
                        if ((playerTeam[1].effects.Count > 2) && isAllied(playerTeam[1].effects[2]) && playerTeam[1].effects[2].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[2]);
                        }

                        break;
                    case 22:
                        if ((playerTeam[1].effects.Count > 3) && isAllied(playerTeam[1].effects[3]) && playerTeam[1].effects[3].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[3]);
                        }

                        break;
                    case 23:
                        if ((playerTeam[1].effects.Count > 4) && isAllied(playerTeam[1].effects[4]) && playerTeam[1].effects[4].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[4]);
                        }

                        break;
                    case 24:
                        if ((playerTeam[1].effects.Count > 5) && isAllied(playerTeam[1].effects[5]) && playerTeam[1].effects[5].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[5]);
                        }

                        break;
                    case 25:
                        if ((playerTeam[1].effects.Count > 6) && isAllied(playerTeam[1].effects[6]) && playerTeam[1].effects[6].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[6]);
                        }

                        break;
                    case 26:
                        if ((playerTeam[1].effects.Count > 7) && isAllied(playerTeam[1].effects[7]) && playerTeam[1].effects[7].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[7]);
                        }

                        break;
                    case 27:
                        if ((playerTeam[1].effects.Count > 8) && isAllied(playerTeam[1].effects[8]) && playerTeam[1].effects[8].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[8]);
                        }

                        break;
                    case 28:
                        if ((playerTeam[1].effects.Count > 9) && isAllied(playerTeam[1].effects[9]) && playerTeam[1].effects[9].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[9]);
                        }

                        break;
                    case 29:
                        if ((playerTeam[1].effects.Count > 10) && isAllied(playerTeam[1].effects[10]) && playerTeam[1].effects[10].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[10]);
                        }

                        break;
                    case 30:
                        if ((playerTeam[1].effects.Count > 11) && isAllied(playerTeam[1].effects[11]) && playerTeam[1].effects[11].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[11]);
                        }

                        break;
                    case 31:
                        if ((playerTeam[1].effects.Count > 12) && isAllied(playerTeam[1].effects[12]) && playerTeam[1].effects[12].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[12]);
                        }

                        break;
                    case 32:
                        if ((playerTeam[1].effects.Count > 13) && isAllied(playerTeam[1].effects[13]) && playerTeam[1].effects[13].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[13]);
                        }

                        break;
                    case 33:
                        if ((playerTeam[1].effects.Count > 14) && isAllied(playerTeam[1].effects[14]) && playerTeam[1].effects[14].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[14]);
                        }

                        break;
                    case 34:
                        if ((playerTeam[1].effects.Count > 15) && isAllied(playerTeam[1].effects[15]) && playerTeam[1].effects[15].resolve == true)
                        {
                            resolveEffect(playerTeam[1].effects[15]);
                        }

                        break;
                    #endregion
                    #region Player 3 Effect execution
                    case 35:
                        if ((playerTeam[2].effects.Count > 0) && isAllied(playerTeam[2].effects[0]) && playerTeam[2].effects[0].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[0]);
                        }

                        break;
                    case 36:
                        if ((playerTeam[2].effects.Count > 1) && isAllied(playerTeam[2].effects[1]) && playerTeam[2].effects[1].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[1]);
                        }

                        break;
                    case 37:
                        if ((playerTeam[2].effects.Count > 2) && isAllied(playerTeam[2].effects[2]) && playerTeam[2].effects[2].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[2]);
                        }

                        break;
                    case 38:
                        if ((playerTeam[2].effects.Count > 3) && isAllied(playerTeam[2].effects[3]) && playerTeam[2].effects[3].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[3]);
                        }

                        break;
                    case 39:
                        if ((playerTeam[2].effects.Count > 4) && isAllied(playerTeam[2].effects[4]) && playerTeam[2].effects[4].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[4]);
                        }

                        break;
                    case 40:
                        if ((playerTeam[2].effects.Count > 5) && isAllied(playerTeam[2].effects[5]) && playerTeam[2].effects[5].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[5]);
                        }

                        break;
                    case 41:
                        if ((playerTeam[2].effects.Count > 6) && isAllied(playerTeam[2].effects[6]) && playerTeam[2].effects[6].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[6]);
                        }

                        break;
                    case 42:
                        if ((playerTeam[2].effects.Count > 7) && isAllied(playerTeam[2].effects[7]) && playerTeam[2].effects[7].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[7]);
                        }

                        break;
                    case 43:
                        if ((playerTeam[2].effects.Count > 8) && isAllied(playerTeam[2].effects[8]) && playerTeam[2].effects[8].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[8]);
                        }

                        break;
                    case 44:
                        if ((playerTeam[2].effects.Count > 9) && isAllied(playerTeam[2].effects[9]) && playerTeam[2].effects[9].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[9]);
                        }

                        break;
                    case 45:
                        if ((playerTeam[2].effects.Count > 10) && isAllied(playerTeam[2].effects[10]) && playerTeam[2].effects[10].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[10]);
                        }

                        break;
                    case 46:
                        if ((playerTeam[2].effects.Count > 11) && isAllied(playerTeam[2].effects[11]) && playerTeam[2].effects[11].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[11]);
                        }

                        break;
                    case 47:
                        if ((playerTeam[2].effects.Count > 12) && isAllied(playerTeam[2].effects[12]) && playerTeam[2].effects[12].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[12]);
                        }

                        break;
                    case 48:
                        if ((playerTeam[2].effects.Count > 13) && isAllied(playerTeam[2].effects[13]) && playerTeam[2].effects[13].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[13]);
                        }

                        break;
                    case 49:
                        if ((playerTeam[2].effects.Count > 14) && isAllied(playerTeam[2].effects[14]) && playerTeam[2].effects[14].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[14]);
                        }

                        break;
                    case 50:
                        if ((playerTeam[2].effects.Count > 15) && isAllied(playerTeam[2].effects[15]) && playerTeam[2].effects[15].resolve == true)
                        {
                            resolveEffect(playerTeam[2].effects[15]);
                        }

                        break;
                    #endregion
                    #region Enemy 1 Effect Execution
                    case 51:
                        if ((enemyTeam[0].effects.Count > 0) && isAllied(enemyTeam[0].effects[0]) && enemyTeam[0].effects[0].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[0]);
                        }

                        break;
                    case 52:
                        if ((enemyTeam[0].effects.Count > 1) && isAllied(enemyTeam[0].effects[1]) && enemyTeam[0].effects[1].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[1]);
                        }

                        break;
                    case 53:
                        if ((enemyTeam[0].effects.Count > 2) && isAllied(enemyTeam[0].effects[2]) && enemyTeam[0].effects[2].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[2]);
                        }

                        break;
                    case 54:
                        if ((enemyTeam[0].effects.Count > 3) && isAllied(enemyTeam[0].effects[3]) && enemyTeam[0].effects[3].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[3]);
                        }

                        break;
                    case 55:
                        if ((enemyTeam[0].effects.Count > 4) && isAllied(enemyTeam[0].effects[4]) && enemyTeam[0].effects[4].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[4]);
                        }

                        break;
                    case 56:
                        if ((enemyTeam[0].effects.Count > 5) && isAllied(enemyTeam[0].effects[5]) && enemyTeam[0].effects[5].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[5]);
                        }

                        break;
                    case 57:
                        if ((enemyTeam[0].effects.Count > 6) && isAllied(enemyTeam[0].effects[6]) && enemyTeam[0].effects[6].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[6]);
                        }

                        break;
                    case 58:
                        if ((enemyTeam[0].effects.Count > 7) && isAllied(enemyTeam[0].effects[7]) && enemyTeam[0].effects[7].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[7]);
                        }

                        break;
                    case 59:
                        if ((enemyTeam[0].effects.Count > 8) && isAllied(enemyTeam[0].effects[8]) && enemyTeam[0].effects[8].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[8]);
                        }

                        break;
                    case 60:
                        if ((enemyTeam[0].effects.Count > 9) && isAllied(enemyTeam[0].effects[9]) && enemyTeam[0].effects[9].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[9]);
                        }

                        break;
                    case 61:
                        if ((enemyTeam[0].effects.Count > 10) && isAllied(enemyTeam[0].effects[10]) && enemyTeam[0].effects[10].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[10]);
                        }

                        break;
                    case 62:
                        if ((enemyTeam[0].effects.Count > 11) && isAllied(enemyTeam[0].effects[11]) && enemyTeam[0].effects[11].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[11]);
                        }

                        break;
                    case 63:
                        if ((enemyTeam[0].effects.Count > 12) && isAllied(enemyTeam[0].effects[12]) && enemyTeam[0].effects[12].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[12]);
                        }

                        break;
                    case 64:
                        if ((enemyTeam[0].effects.Count > 13) && isAllied(enemyTeam[0].effects[13]) && enemyTeam[0].effects[13].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[13]);
                        }

                        break;
                    case 65:
                        if ((enemyTeam[0].effects.Count > 14) && isAllied(enemyTeam[0].effects[14]) && enemyTeam[0].effects[14].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[14]);
                        }

                        break;
                    case 66:
                        if ((enemyTeam[0].effects.Count > 15) && isAllied(enemyTeam[0].effects[15]) && enemyTeam[0].effects[15].resolve == true)
                        {
                            resolveEffect(enemyTeam[0].effects[15]);
                        }

                        break;
                    #endregion
                    #region Enemy 2 Effect Execution
                    case 67:
                        if ((enemyTeam[1].effects.Count > 0) && isAllied(enemyTeam[1].effects[0]) && enemyTeam[1].effects[0].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[0]);
                        }

                        break;
                    case 68:
                        if ((enemyTeam[1].effects.Count > 1) && isAllied(enemyTeam[1].effects[1]) && enemyTeam[1].effects[1].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[1]);
                        }

                        break;
                    case 69:
                        if ((enemyTeam[1].effects.Count > 2) && isAllied(enemyTeam[1].effects[2]) && enemyTeam[1].effects[2].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[2]);
                        }

                        break;
                    case 70:
                        if ((enemyTeam[1].effects.Count > 3) && isAllied(enemyTeam[1].effects[3]) && enemyTeam[1].effects[3].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[3]);
                        }

                        break;
                    case 71:
                        if ((enemyTeam[1].effects.Count > 4) && isAllied(enemyTeam[1].effects[4]) && enemyTeam[1].effects[4].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[4]);
                        }

                        break;
                    case 72:
                        if ((enemyTeam[1].effects.Count > 5) && isAllied(enemyTeam[1].effects[5]) && enemyTeam[1].effects[5].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[5]);
                        }

                        break;
                    case 73:
                        if ((enemyTeam[1].effects.Count > 6) && isAllied(enemyTeam[1].effects[6]) && enemyTeam[1].effects[6].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[6]);
                        }

                        break;
                    case 74:
                        if ((enemyTeam[1].effects.Count > 7) && isAllied(enemyTeam[1].effects[7]) && enemyTeam[1].effects[7].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[7]);
                        }

                        break;
                    case 75:
                        if ((enemyTeam[1].effects.Count > 8) && isAllied(enemyTeam[1].effects[8]) && enemyTeam[1].effects[8].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[8]);
                        }

                        break;
                    case 76:
                        if ((enemyTeam[1].effects.Count > 9) && isAllied(enemyTeam[1].effects[9]) && enemyTeam[1].effects[9].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[9]);
                        }

                        break;
                    case 77:
                        if ((enemyTeam[1].effects.Count > 10) && isAllied(enemyTeam[0].effects[10]) && enemyTeam[1].effects[10].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[10]);
                        }

                        break;
                    case 78:
                        if ((enemyTeam[1].effects.Count > 11) && isAllied(enemyTeam[1].effects[11]) && enemyTeam[1].effects[11].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[11]);
                        }

                        break;
                    case 79:
                        if ((enemyTeam[1].effects.Count > 12) && isAllied(enemyTeam[1].effects[12]) && enemyTeam[1].effects[12].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[12]);
                        }

                        break;
                    case 80:
                        if ((enemyTeam[1].effects.Count > 13) && isAllied(enemyTeam[1].effects[13]) && enemyTeam[1].effects[13].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[13]);
                        }

                        break;
                    case 81:
                        if ((enemyTeam[1].effects.Count > 14) && isAllied(enemyTeam[0].effects[14]) && enemyTeam[1].effects[14].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[14]);
                        }

                        break;
                    case 82:
                        if ((enemyTeam[1].effects.Count > 15) && isAllied(enemyTeam[1].effects[15]) && enemyTeam[1].effects[15].resolve == true)
                        {
                            resolveEffect(enemyTeam[1].effects[15]);
                        }

                        break;
                    #endregion
                    #region Enemy 3 Effect Execution
                    case 83:
                        if ((enemyTeam[2].effects.Count > 0) && isAllied(enemyTeam[2].effects[0]) && enemyTeam[2].effects[0].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[0]);
                        }

                        break;
                    case 84:
                        if ((enemyTeam[2].effects.Count > 1) && isAllied(enemyTeam[2].effects[1]) && enemyTeam[2].effects[1].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[1]);
                        }

                        break;
                    case 85:
                        if ((enemyTeam[2].effects.Count > 2) && isAllied(enemyTeam[2].effects[2]) && enemyTeam[2].effects[2].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[2]);
                        }

                        break;
                    case 86:
                        if ((enemyTeam[2].effects.Count > 3) && isAllied(enemyTeam[2].effects[3]) && enemyTeam[2].effects[3].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[3]);
                        }

                        break;
                    case 87:
                        if ((enemyTeam[2].effects.Count > 4) && isAllied(enemyTeam[2].effects[4]) && enemyTeam[2].effects[4].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[4]);
                        }

                        break;
                    case 88:
                        if ((enemyTeam[2].effects.Count > 5) && isAllied(enemyTeam[2].effects[5]) && enemyTeam[2].effects[5].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[5]);
                        }

                        break;
                    case 89:
                        if ((enemyTeam[2].effects.Count > 6) && isAllied(enemyTeam[2].effects[6]) && enemyTeam[2].effects[6].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[6]);
                        }

                        break;
                    case 90:
                        if ((enemyTeam[2].effects.Count > 7) && isAllied(enemyTeam[2].effects[7]) && enemyTeam[2].effects[7].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[7]);
                        }

                        break;
                    case 91:
                        if ((enemyTeam[2].effects.Count > 8) && isAllied(enemyTeam[2].effects[8]) && enemyTeam[2].effects[8].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[8]);
                        }

                        break;
                    case 92:
                        if ((enemyTeam[2].effects.Count > 9) && isAllied(enemyTeam[2].effects[9]) && enemyTeam[2].effects[9].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[9]);
                        }

                        break;
                    case 93:
                        if ((enemyTeam[2].effects.Count > 10) && isAllied(enemyTeam[2].effects[10]) && enemyTeam[2].effects[10].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[10]);
                        }

                        break;
                    case 94:
                        if ((enemyTeam[2].effects.Count > 11) && isAllied(enemyTeam[2].effects[11]) && enemyTeam[2].effects[11].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[11]);
                        }

                        break;
                    case 95:
                        if ((enemyTeam[2].effects.Count > 12) && isAllied(enemyTeam[2].effects[12]) && enemyTeam[2].effects[12].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[12]);
                        }

                        break;
                    case 96:
                        if ((enemyTeam[2].effects.Count > 13) && isAllied(enemyTeam[2].effects[13]) && enemyTeam[2].effects[13].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[13]);
                        }

                        break;
                    case 97:
                        if ((enemyTeam[2].effects.Count > 14) && isAllied(enemyTeam[2].effects[14]) && enemyTeam[2].effects[14].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[14]);
                        }

                        break;
                    case 98:
                        if ((enemyTeam[2].effects.Count > 15) && isAllied(enemyTeam[2].effects[15]) && enemyTeam[2].effects[15].resolve == true)
                        {
                            resolveEffect(enemyTeam[2].effects[15]);
                        }

                        break;
                        #endregion
                }
                
            }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    allEffects[i][j].Visible = false;
                }
            }
            for (int i = 0; i < allCharacters.Length; i++)
            {

                if (allCharacters[i].dead != true)
                {

                    //populate the list of VISIBLE effects

                    for (int j = 0; j < allCharacters[i].effects.Count; j++)
                    {
                        if (allCharacters[i].effects[j].invisible != true || isAllied(allCharacters[i].effects[j]))
                        {
                            displayEffects.Add(allCharacters[i].effects[j]);
                        }
                    }

                    for (int j = 0; j < displayEffects.Count; j++)
                    {

                        allEffects[i][j].Visible = true;
                        allEffects[i][j].Image = Image.FromFile(@"..\..\images\mini" + displayEffects[j].imgPath);

                    }
                    displayEffects.Clear();
                }

            }
            executionOrder.Clear();

            while (playerTeam[1].startHP != playerTeam[1].hp ||
                   playerTeam[0].startHP != playerTeam[0].hp ||
                   playerTeam[2].startHP != playerTeam[2].hp ||
                   enemyTeam[0].startHP != enemyTeam[0].hp ||
                   enemyTeam[1].startHP != enemyTeam[1].hp ||
                   enemyTeam[2].startHP != enemyTeam[2].hp)
            {
                if (playerTeam[0].startHP != playerTeam[0].hp)
                {
                    if (playerTeam[0].startHP > playerTeam[0].hp)
                    {
                        playerTeam[0].startHP -= 1;
                        p1hpback.Refresh();
                        p1hpfront.Height -= 1;
                        p1hpfront.Top += 1;
                    }
                    else
                    {
                        playerTeam[0].startHP += 1;
                        p1hpback.Refresh();
                        p1hpfront.Height += 1;
                        p1hpfront.Top -= 1;
                    }
                }
                if (playerTeam[1].startHP != playerTeam[1].hp)
                {
                    if (playerTeam[1].startHP > playerTeam[1].hp)
                    {
                        playerTeam[1].startHP -= 1;
                        p2hpback.Refresh();
                        p2hpfront.Height -= 1;
                        p2hpfront.Top += 1;
                    }
                    else
                    {
                        playerTeam[1].startHP += 1;
                        p2hpback.Refresh();
                        p2hpfront.Height += 1;
                        p2hpfront.Top -= 1;
                    }
                }
                if (playerTeam[2].startHP != playerTeam[2].hp)
                {
                    if (playerTeam[2].startHP > playerTeam[2].hp)
                    {
                        playerTeam[2].startHP -= 1;
                        p3hpback.Refresh();
                        p3hpfront.Height -= 1;
                        p3hpfront.Top += 1;
                    }
                    else
                    {
                        playerTeam[2].startHP += 1;
                        p3hpback.Refresh();
                        p3hpfront.Height += 1;
                        p3hpfront.Top -= 1;
                    }
                }
                if (enemyTeam[0].startHP != enemyTeam[0].hp)
                {
                    if (enemyTeam[0].startHP > enemyTeam[0].hp)
                    {
                        enemyTeam[0].startHP -= 1;
                        e1hpback.Refresh();
                        e1hpfront.Height -= 1;
                        e1hpfront.Top += 1;
                    }
                    else
                    {
                        enemyTeam[0].startHP += 1;
                        e1hpback.Refresh();
                        e1hpfront.Height += 1;
                        e1hpfront.Top -= 1;
                    }
                }
                if (enemyTeam[1].startHP != enemyTeam[1].hp)
                {
                    if (enemyTeam[1].startHP > enemyTeam[1].hp)
                    {
                        enemyTeam[1].startHP -= 1;
                        e2hpback.Refresh();
                        e2hpfront.Height -= 1;
                        e2hpfront.Top += 1;
                    }
                    else
                    {
                        enemyTeam[1].startHP += 1;
                        e2hpback.Refresh();
                        e2hpfront.Height += 1;
                        e2hpfront.Top -= 1;
                    }
                }
                if (enemyTeam[2].startHP != enemyTeam[2].hp)
                {
                    if (enemyTeam[2].startHP > enemyTeam[2].hp)
                    {
                        enemyTeam[2].startHP -= 1;
                        e3hpback.Refresh();
                        e3hpfront.Height -= 1;
                        e3hpfront.Top += 1;
                    }
                    else
                    {
                        enemyTeam[2].startHP += 1;
                        e3hpback.Refresh();
                        e3hpfront.Height += 1;
                        e3hpfront.Top -= 1;
                    }
                }

                Thread.Sleep(15);

            }
            

            int debug = 40;

        }

        public void turnStart(Character[] pTeam, Character[] eTeam)
        {
            int debug = 0;
            serialString = "";
            displayAvailability();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    allEffects[i][j].Visible = false;
                }
                allCharacters[i].invulnerable = false;
                if (allCharacters[i].hp <= 0)
                {
                    allCharacters[i].dead = true;
                    allCharacters[i].effects.Clear();
                    allCharacters[i].energyContribution = 0;

                }
            }

            #region Special Toga Shit ffs

            if (playerTogaSlot != 15)
            {
                if (playerTeam[playerTogaSlot].isToga != true && playerTeam[playerTogaSlot].contains("quirkTransformEff") != true)
                {

                    int togaHP = playerTeam[playerTogaSlot].hp;
                    List<Effect> togaEffects = new List<Effect>();

                    

                    Character togaUser = playerTeam[playerTogaSlot];

                    togaEffects = playerTeam[playerTogaSlot].effects;

                    playerTeam[playerTogaSlot] = new Toga(togaHP, togaEffects);
                    allCharacters[playerTogaSlot] = playerTeam[playerTogaSlot];
                    allyTogaChanged = false;
                    adjustEffectUser(playerTeam[playerTogaSlot], togaUser);

                    if (playerTeam[playerTogaSlot].hp <= 0)
                    {
                        playerTeam[playerTogaSlot].dead = true;
                    }
                }
            }

            #endregion
            #region Special Gray Shit
            for (int i =0; i < playerTeam.Length; i++)
            {
                if (playerTeam[i].id == "15")
                {
                    if (!playerTeam[i].contains("iceMakeEff"))
                    {
                        playerTeam[i].abilities[0] = playerTeam[i].main1;
                        playerTeam[i].abilities[1].enabled = false;
                        playerTeam[i].abilities[2].enabled = false;
                        playerTeam[i].abilities[3].enabled = false;
                    }
                }
            }
            #endregion


            p1m1c.Visible = false;
            p1m2c.Visible = false;
            p1m3c.Visible = false;
            p1m4c.Visible = false;
            p2m1c.Visible = false;
            p2m2c.Visible = false;
            p2m3c.Visible = false;
            p2m4c.Visible = false;
            p3m1c.Visible = false;
            p3m2c.Visible = false;
            p3m3c.Visible = false;
            p3m4c.Visible = false;
            debug = 15;
            playerTeam = pTeam;
            enemyTeam = eTeam;
            #region Collection assignments
            enemyTeamDisplay[0] = enemyBox1;
            enemyTeamDisplay[1] = enemyBox2;
            enemyTeamDisplay[2] = enemyBox3;

            characterCooldowns[0] = char1Cooldown;
            characterCooldowns[1] = char2Cooldown;
            characterCooldowns[2] = char3Cooldown;

            player1Effects[0] = p1e1;
            player1Effects[1] = p1e2;
            player1Effects[2] = p1e3;
            player1Effects[3] = p1e4;
            player1Effects[4] = p1e5;
            player1Effects[5] = p1e6;
            player1Effects[6] = p1e7;
            player1Effects[7] = p1e8;
            player1Effects[8] = p1e9;
            player1Effects[9] = p1e10;
            player1Effects[10] = p1e11;
            player1Effects[11] = p1e12;
            player1Effects[12] = p1e13;
            player1Effects[13] = p1e14;
            player1Effects[14] = p1e15;
            player1Effects[15] = p1e16;

            player2Effects[0] = p2e1;
            player2Effects[1] = p2e2;
            player2Effects[2] = p2e3;
            player2Effects[3] = p2e4;
            player2Effects[4] = p2e5;
            player2Effects[5] = p2e6;
            player2Effects[6] = p2e7;
            player2Effects[7] = p2e8;
            player2Effects[8] = p2e9;
            player2Effects[9] = p2e10;
            player2Effects[10] = p2e11;
            player2Effects[11] = p2e12;
            player2Effects[12] = p2e13;
            player2Effects[13] = p2e14;
            player2Effects[14] = p2e15;
            player2Effects[15] = p2e16;

            player3Effects[0] = p3e1;
            player3Effects[1] = p3e2;
            player3Effects[2] = p3e3;
            player3Effects[3] = p3e4;
            player3Effects[4] = p3e5;
            player3Effects[5] = p3e6;
            player3Effects[6] = p3e7;
            player3Effects[7] = p3e8;
            player3Effects[8] = p3e9;
            player3Effects[9] = p3e10;
            player3Effects[10] = p3e11;
            player3Effects[11] = p3e12;
            player3Effects[12] = p3e13;
            player3Effects[13] = p3e14;
            player3Effects[14] = p3e15;
            player3Effects[15] = p3e16;

            enemy1Effects[0] = e1e1;
            enemy1Effects[1] = e1e2;
            enemy1Effects[2] = e1e3;
            enemy1Effects[3] = e1e4;
            enemy1Effects[4] = e1e5;
            enemy1Effects[5] = e1e6;
            enemy1Effects[6] = e1e7;
            enemy1Effects[7] = e1e8;
            enemy1Effects[8] = e1e9;
            enemy1Effects[9] = e1e10;
            enemy1Effects[10] = e1e11;
            enemy1Effects[11] = e1e12;
            enemy1Effects[12] = e1e13;
            enemy1Effects[13] = e1e14;
            enemy1Effects[14] = e1e15;
            enemy1Effects[15] = e1e16;

            enemy2Effects[0] = e2e1;
            enemy2Effects[1] = e2e2;
            enemy2Effects[2] = e2e3;
            enemy2Effects[3] = e2e4;
            enemy2Effects[4] = e2e5;
            enemy2Effects[5] = e2e6;
            enemy2Effects[6] = e2e7;
            enemy2Effects[7] = e2e8;
            enemy2Effects[8] = e2e9;
            enemy2Effects[9] = e2e10;
            enemy2Effects[10] = e2e11;
            enemy2Effects[11] = e2e12;
            enemy2Effects[12] = e2e13;
            enemy2Effects[13] = e2e14;
            enemy2Effects[14] = e2e15;
            enemy2Effects[15] = e2e16;

            enemy3Effects[0] = e3e1;
            enemy3Effects[1] = e3e2;
            enemy3Effects[2] = e3e3;
            enemy3Effects[3] = e3e4;
            enemy3Effects[4] = e3e5;
            enemy3Effects[5] = e3e6;
            enemy3Effects[6] = e3e7;
            enemy3Effects[7] = e3e8;
            enemy3Effects[8] = e3e9;
            enemy3Effects[9] = e3e10;
            enemy3Effects[10] = e3e11;
            enemy3Effects[11] = e3e12;
            enemy3Effects[12] = e3e13;
            enemy3Effects[13] = e3e14;
            enemy3Effects[14] = e3e15;
            enemy3Effects[15] = e3e16;

            allEffects[0] = player1Effects;
            allEffects[1] = player2Effects;
            allEffects[2] = player3Effects;
            allEffects[3] = enemy1Effects;
            allEffects[4] = enemy2Effects;
            allEffects[5] = enemy3Effects;

            playerTeamDisplay[0] = profBox1;
            playerTeamDisplay[1] = profBox2;
            playerTeamDisplay[2] = profBox3;

            detailBoxes[0] = char1detail;
            detailBoxes[1] = char2detail;
            detailBoxes[2] = char3detail;

            character1Moves[0] = char1move1;
            character1Moves[1] = char1move2;
            character1Moves[2] = char1move3;
            character1Moves[3] = char1move4;

            character2Moves[0] = char2move1;
            character2Moves[1] = char2move2;
            character2Moves[2] = char2move3;
            character2Moves[3] = char2move4;

            character3Moves[0] = char3move1;
            character3Moves[1] = char3move2;
            character3Moves[2] = char3move3;
            character3Moves[3] = char3move4;

            character1Energy[0] = char1energyBox1;
            character1Energy[1] = char1energyBox2;
            character1Energy[2] = char1energyBox3;
            character1Energy[3] = char1energyBox4;
            character1Energy[4] = char1energyBox5;

            character2Energy[0] = char2energyBox1;
            character2Energy[1] = char2energyBox2;
            character2Energy[2] = char2energyBox3;
            character2Energy[3] = char2energyBox4;
            character2Energy[4] = char2energyBox5;

            character3Energy[0] = char3energyBox1;
            character3Energy[1] = char3energyBox2;
            character3Energy[2] = char3energyBox3;
            character3Energy[3] = char3energyBox4;
            character3Energy[4] = char3energyBox5;

            allCharacterEnergy[0] = character1Energy;
            allCharacterEnergy[1] = character2Energy;
            allCharacterEnergy[2] = character3Energy;

            allPlayerMoves[0] = character1Moves;
            allPlayerMoves[1] = character2Moves;
            allPlayerMoves[2] = character3Moves;

            player1Targeted[0] = p1t1;
            player1Targeted[1] = p1t2;
            player1Targeted[2] = p1t3;
            player2Targeted[0] = p2t1;
            player2Targeted[1] = p2t2;
            player2Targeted[2] = p2t3;
            player3Targeted[0] = p3t1;
            player3Targeted[1] = p3t2;
            player3Targeted[2] = p3t3;

            enemy1Targeted[0] = e1t1;
            enemy1Targeted[1] = e1t2;
            enemy1Targeted[2] = e1t3;
            enemy2Targeted[0] = e2t1;
            enemy2Targeted[1] = e2t2;
            enemy2Targeted[2] = e2t3;
            enemy3Targeted[0] = e3t1;
            enemy3Targeted[1] = e3t2;
            enemy3Targeted[2] = e3t3;

            allTargeted[0] = player1Targeted;
            allTargeted[1] = player2Targeted;
            allTargeted[2] = player3Targeted;
            allTargeted[3] = enemy1Targeted;
            allTargeted[4] = enemy2Targeted;
            allTargeted[5] = enemy3Targeted;

            allCharacters[0] = playerTeam[0];
            allCharacters[1] = playerTeam[1];
            allCharacters[2] = playerTeam[2];
            allCharacters[3] = enemyTeam[0];
            allCharacters[4] = enemyTeam[1];
            allCharacters[5] = enemyTeam[2];
            #endregion

            foreach (Character c in allCharacters)
            {
                refresh(c);
                c.startHP = c.hp;
            }
            #region HP dimension reset
            p1hpfront.Height = 100;
            p1hpfront.Top = 50;
            p2hpfront.Height = 100;
            p2hpfront.Top = 277;
            p3hpfront.Height = 100;
            p3hpfront.Top = 506;
            e1hpfront.Height = 100;
            e1hpfront.Top = 50;
            e2hpfront.Height = 100;
            e2hpfront.Top = 277;
            e3hpfront.Height = 100;
            e3hpfront.Top = 506;
            #endregion

            p1hpfront.Top += 100 - playerTeam[0].hp;
            p1hpfront.Height -= 100 - playerTeam[0].hp;
            p2hpfront.Top += 100 - playerTeam[1].hp;
            p2hpfront.Height -= 100 - playerTeam[1].hp;
            p3hpfront.Top += 100 - playerTeam[2].hp;
            p3hpfront.Height -= 100 - playerTeam[2].hp;
            e1hpfront.Top += 100 - enemyTeam[0].hp;
            e1hpfront.Height -= 100 - enemyTeam[0].hp;
            e2hpfront.Top += 100 - enemyTeam[1].hp;
            e2hpfront.Height -= 100 - enemyTeam[1].hp;
            e3hpfront.Top += 100 - enemyTeam[2].hp;
            e3hpfront.Height -= 100 - enemyTeam[2].hp;
            debug = 45;
            p1hpback.Refresh();
            p2hpback.Refresh();
            p3hpback.Refresh();
            e1hpback.Refresh();
            e2hpback.Refresh();
            e3hpback.Refresh();

            





            for (int i = 0; i < allCharacters.Length; i++)
            {

                if (allCharacters[i].dead != true)
                {

                    //populate the list of VISIBLE effects

                    for (int j = 0; j < allCharacters[i].effects.Count; j++)
                    {
                        if (allCharacters[i].effects[j].invisible != true || isAllied(allCharacters[i].effects[j]))
                        {
                            displayEffects.Add(allCharacters[i].effects[j]);
                        }
                    }

                    for (int j = 0; j < displayEffects.Count; j++)
                    {

                        allEffects[i][j].Visible = true;
                        allEffects[i][j].Image = Image.FromFile(@"..\..\images\mini" + displayEffects[j].imgPath);

                    }
                    displayEffects.Clear();
                }

            }
            
            for (int i = 0; i < 3; i++)
            {
                if (playerTeam[i].id == "0" && playerTeam[i].contains("tensaZangetsuEff"))
                {
                    playerTeamEnergy[3] += 1;
                }
                if (playerTeam[i].id == "3" && playerTeam[i].contains("sageModeEff"))
                {
                    playerTeamEnergy[1] += 1;
                }
                for (int j = 0; j < playerTeam[i].effects.Count; j++)
                {
                    checkEffect(playerTeam[i].effects[j]);
                    
                }
                for (int j = 0; j < enemyTeam[i].effects.Count; j++)
                {
                    checkEffect(enemyTeam[i].effects[j]);
                }
                for (int j = 0; j < playerTeam[i].effects.Count; j++)
                {
                    checkNegationEffect(playerTeam[i].effects[j]);
                }
                for (int j = 0; j < enemyTeam[i].effects.Count; j++)
                {
                    checkNegationEffect(enemyTeam[i].effects[j]);
                }
            }
            displayAvailability();
            int turnRoll = playerTeam[0].energyContribution + playerTeam[1].energyContribution + playerTeam[2].energyContribution;
            if (turnRoll > 0)
            {
                Random energyRoll = new Random();
                for (int i = 0; i < turnRoll; i++)
                {
                    
                    playerTeamEnergy[energyRoll.Next(4)] += 1;

                }
            }

            if (turnRoll < 0)
            {
                turnRoll *= -1;
                Random energyRoll = new Random();
                for (int i = 0; i < turnRoll; i++)
                {
                    
                    int checkLoss = energyRoll.Next(4);

                    if (playerTeamEnergy[0] == 0 && playerTeamEnergy[1] == 0 && playerTeamEnergy[2] == 0 && playerTeamEnergy[3] == 0)
                    {
                        break;
                    }
                    else if (playerTeamEnergy[checkLoss] > 0)
                    {
                        playerTeamEnergy[checkLoss] -= 1;
                    }
                    else
                    {
                        i = i - 1;
                    }

                }


            }

            for (int i = 0; i < 3; i++)
            {
                playerTeam[i].energyContribution = 1;
            }

            playerTeamEnergy[4] = playerTeamEnergy[0] + playerTeamEnergy[1] + playerTeamEnergy[2] + playerTeamEnergy[3];





            playerTeamTempEnergy = playerTeamEnergy;

            physCounterLabel.Text = "x " + playerTeamTempEnergy[0].ToString();
            specCounterLabel.Text = "x " + playerTeamTempEnergy[1].ToString();
            mentCounterLabel.Text = "x " + playerTeamTempEnergy[2].ToString();
            wepCounterLabel.Text = "x " + playerTeamTempEnergy[3].ToString();
            totalCounterLabel.Text = "T : " + playerTeamTempEnergy[4].ToString();

            for (int i = 0; i < 3; i++)
            {


                playerTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + playerTeam[i].profileImagePath);
                detailBoxes[i].Text = playerTeam[i].description;
                enemyTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + enemyTeam[i].profileImagePath);
            }



            displayAvailability();

        }
        public void showHoverText(int player, int effectSlot)
        {

            effectHoverPanel.Visible = true;
            string text = allCharacters[player].effects[effectSlot].displayName;
            if (player < 3)
            {
                if (allCharacters[player].effects[effectSlot].duration > 100000)
                {
                    text += "   Infinite\n\n";
                }
                else if (allCharacters[player].effects[effectSlot].duration/2 > 1)
                {
                    text += "   " + (allCharacters[player].effects[effectSlot].duration/2).ToString() + " turns remaining\n\n";

                }
                else if (allCharacters[player].effects[effectSlot].duration/2 == 1)
                {
                    text += "   " + (allCharacters[player].effects[effectSlot].duration/2).ToString() + " turn remaining\n\n";
                }
                else
                {
                    text += "   Ends this turn\n\n";
                }
            }
            if (player > 2)
            {
                if (allCharacters[player].effects[effectSlot].duration > 100000)
                {
                    text += "   Infinite\n\n";
                }
                else if (allCharacters[player].effects[effectSlot].duration/2> 1)
                {
                    text += "   " + (allCharacters[player].effects[effectSlot].duration/2).ToString() + " turns remaining\n\n";

                }
                else if (allCharacters[player].effects[effectSlot].duration/2== 1)
                {
                    text += "   " + (allCharacters[player].effects[effectSlot].duration/2).ToString() + " turn remaining\n\n";
                }
                else
                {
                    text += "   Ends this turn\n\n";
                }
            }

            for (int i = 0; i < allCharacters[player].effects[effectSlot].description.Count; i++)
            {
                text += allCharacters[player].effects[effectSlot].description[i] + "\n\n";
                effectHoverPanel.Height += 15;
            }

            effectHoverName.Text = text;

            effectHoverPanel.Top = allEffects[player][effectSlot].Top - effectHoverPanel.Height;
            if (player < 3)
            {
                effectHoverPanel.Left = allEffects[player][effectSlot].Left + 25;
            }
            else
            {
                effectHoverPanel.Left = allEffects[player][effectSlot].Left - effectHoverPanel.Width;
            }
           
        }
        public void displayAvailability()
        {
            
            for(int i = 0; i < 3; i++)
            {
                playerTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + playerTeam[i].profileImagePath);
                enemyTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + enemyTeam[i].profileImagePath);
                for (int j = 0; j < 4; j++)
                {

                    if (checkEnergyAvailability(playerTeamTempEnergy, playerTeam[i].abilities[j]) && playerTeam[i].stunned != true && playerTeam[i].targeted != true && playerTeam[i].abilities[j].cooldownRemaining == 0 && playerTeam[i].dead != true && playerTeam[i].abilities[j].enabled == true && waiting == false)
                    {

                        allPlayerMoves[i][j].Image = Image.FromFile(@"..\..\images\" + playerTeam[i].abilities[j].imagePath);

                    }
                    else
                    {
                        allPlayerMoves[i][j].Image = Image.FromFile(@"..\..\images\null" + playerTeam[i].abilities[j].imagePath);
                        
                    }
                    if (playerTeam[i].abilities[j].cooldownRemaining > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                switch (j)
                                {
                                    case 0:
                                        p1m1c.Visible = true;
                                        p1m1c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 1:
                                        p1m2c.Visible = true;
                                        p1m2c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 2:
                                        p1m3c.Visible = true;
                                        p1m3c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 3:
                                        p1m4c.Visible = true;
                                        p1m4c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                }
                                break;
                            case 1:
                                switch (j)
                                {
                                    case 0:
                                        p2m1c.Visible = true;
                                        p2m1c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 1:
                                        p2m2c.Visible = true;
                                        p2m2c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 2:
                                        p2m3c.Visible = true;
                                        p2m3c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 3:
                                        p2m4c.Visible = true;
                                        p2m4c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                }
                                break;
                            case 2:
                                switch (j)
                                {
                                    case 0:
                                        p3m1c.Visible = true;
                                        p3m1c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 1:
                                        p3m2c.Visible = true;
                                        p3m2c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 2:
                                        p3m3c.Visible = true;
                                        p3m3c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                    case 3:
                                        p3m4c.Visible = true;
                                        p3m4c.Text = playerTeam[i].abilities[j].cooldownRemaining.ToString();
                                        break;
                                }
                                break;
                        }
                    }


                }
                

            }
            




        }
        public void addTargetEffect(PictureBox[] targetEffects, Character user)
        {

            for (int i = 0; i < 3; i++)
            {
                if (targetEffects[i].Visible == false)
                {

                    targetEffects[i].Visible = true;
                    targetEffects[i].Image = Image.FromFile(@"..\..\images\mini" + targetingAbility.imagePath);
                    break;

                }
            }

        }
        public void finishTargeting(Character user)
        {
            user.targeted = true;
            for (int i =0; i < 3; i++)
            {
                playerTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + playerTeam[i].profileImagePath);
                enemyTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + enemyTeam[i].profileImagePath);
            }

            user.chosenAbilitySlot = targetingAbilitySlot;
            
            

            playerTeamTempEnergy[0] -= targetingAbility.physCost;
            playerTeamTempEnergy[1] -= targetingAbility.specCost;
            playerTeamTempEnergy[2] -= targetingAbility.mentCost;
            playerTeamTempEnergy[3] -= targetingAbility.wepCost;
            playerTeamTempEnergy[4] = playerTeamTempEnergy[0] + playerTeamTempEnergy[1] + playerTeamTempEnergy[2] + playerTeamTempEnergy[3];

            

            turnRandCost += targetingAbility.randCost;

            playerTeamTempEnergy[4] -= turnRandCost;

            physCounterLabel.Text = "x " + playerTeamTempEnergy[0].ToString();
            specCounterLabel.Text = "x " + playerTeamTempEnergy[1].ToString();
            mentCounterLabel.Text = "x " + playerTeamTempEnergy[2].ToString();
            wepCounterLabel.Text = "x " + playerTeamTempEnergy[3].ToString();

            

            totalCounterLabel.Text = "T : " + playerTeamTempEnergy[4].ToString();
            displayAvailability();
        }
        public void setTargetingAbility(Character target)
        {
            if (target.firstTargeted == null)
            {

                target.firstTargeted = targetingAbility;

            }
            else if (target.secondTargeted == null)
            {
                target.secondTargeted = targetingAbility;
            }
            else if (target.thirdTargeted == null)
            {
                target.thirdTargeted = targetingAbility;
            }
        }
        public void refresh(Character c)
        {
            c.ignoring = false;
            c.targeting = false;
            c.targetable = false;
            c.targeted = false;
            c.currentAbility = null;
            c.chosenAbilitySlot=0;
            c.firstTargeted=null;
            c.secondTargeted=null;
            c.thirdTargeted=null;
            c.firstTargeter = 5;
            c.secondTargeter = 5;
            c.thirdTargeter = 5;
            c.potentialTargets.Clear();
            c.currentTargets.Clear();
            c.harmed = false;
            c.damaged = 0;
            c.harmedCount = 0;
            c.countered = false;
            c.damageReductionFlat = 0;
            c.damageBoostFlat = 0;
            c.energyContribution = 1;
            c.cooldownMod = 0;
            c.damageBoostFlat = 0;
            c.abilities[0].randCost -= c.randomMod;
            c.abilities[1].randCost -= c.randomMod;
            c.abilities[2].randCost -= c.randomMod;
            c.abilities[3].randCost -= c.randomMod;
            
            c.abilities[0] = c.main1;
            c.abilities[1] = c.main2;
            c.abilities[2] = c.main3;
            c.abilities[3] = c.main4;

            c.randomMod = 0;
            c.stunned = false;
    }
        public void beginTargeting(Character target, int slot)
        {
            
            
            if (target.targetable == true)
            {
                
                if (targetingAbility.multiATarget == true)
                {
                    for (int i = 0; i < potentialTargets.Count; i++)
                    {
                        if (potentialTargets[i] < 3)
                        {
                            if (playerTeam[0].targeting == true)
                            {
                                
                                playerTeam[0].currentTargets.Add(potentialTargets[i]);
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[0]);
                                setTargetingAbility(playerTeam[potentialTargets[i]]);
                                setTargeter(playerTeam[potentialTargets[i]], 0);
                            }
                            if (playerTeam[1].targeting == true)
                            {
                                playerTeam[1].currentTargets.Add(potentialTargets[i]);
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[1]);
                                setTargetingAbility(playerTeam[potentialTargets[i]]);
                                setTargeter(playerTeam[potentialTargets[i]], 1);
                            }
                            if (playerTeam[2].targeting == true)
                            {
                                playerTeam[2].currentTargets.Add(potentialTargets[i]);
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[2]);
                                setTargetingAbility(playerTeam[potentialTargets[i]]);
                                setTargeter(playerTeam[potentialTargets[i]], 2);
                            }
                        }
                    }

                    if (playerTeam[0].targeting == true)
                    {
                        finishTargeting(playerTeam[0]);
                        executionOrder.Add(0);
                        
                    }
                    if (playerTeam[1].targeting == true)
                    {
                        finishTargeting(playerTeam[1]);
                        executionOrder.Add(1);
                    }
                    if (playerTeam[2].targeting == true)
                    {
                        finishTargeting(playerTeam[2]);
                        executionOrder.Add(2);
                    }
                }
                else if (targetingAbility.multiETarget == true)
                {

                    for (int i = 0; i < potentialTargets.Count; i++)
                    {
                        if (potentialTargets[i] > 2)
                        {
                            if(playerTeam[0].targeting == true)
                            {
                                playerTeam[0].currentTargets.Add(potentialTargets[i]);
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[0]);
                                setTargetingAbility(enemyTeam[potentialTargets[i]-3]);
                                setTargeter(enemyTeam[potentialTargets[i]-3], 0);
                            }
                            if (playerTeam[1].targeting == true)
                            {
                                playerTeam[1].currentTargets.Add(potentialTargets[i]);
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[1]);
                                setTargetingAbility(enemyTeam[potentialTargets[i]-3]);
                                setTargeter(enemyTeam[potentialTargets[i] - 3], 1);
                            }
                            if (playerTeam[2].targeting == true)
                            {
                                playerTeam[2].currentTargets.Add(potentialTargets[i]);
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[2]);
                                setTargetingAbility(enemyTeam[potentialTargets[i]-3]);
                                setTargeter(enemyTeam[potentialTargets[i] - 3], 2);
                            }
                        }
                    }
                    if (playerTeam[0].targeting == true)
                    {
                        finishTargeting(playerTeam[0]);
                        executionOrder.Add(0);
                    }
                    if (playerTeam[1].targeting == true)
                    {
                        finishTargeting(playerTeam[1]);
                        executionOrder.Add(1);
                    }
                    if (playerTeam[2].targeting == true)
                    {
                        finishTargeting(playerTeam[2]);
                        executionOrder.Add(2);
                    }

                }
                else if (targetingAbility.allTarget == true)
                {
                    for (int i = 0; i < potentialTargets.Count; i++)
                    {
                        
                        if (playerTeam[0].targeting == true)
                        {
                            playerTeam[0].currentTargets.Add(potentialTargets[i]);
                            if (potentialTargets[i] < 3)
                            {
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[0]);
                                setTargeter(playerTeam[potentialTargets[i]], 0);
                                setTargetingAbility(playerTeam[potentialTargets[i]]);
                            }
                            if (potentialTargets[i] > 2)
                            {
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[0]);
                                setTargetingAbility(enemyTeam[potentialTargets[i] - 3]);
                                setTargeter(enemyTeam[potentialTargets[i] - 3], 0);
                            }
                        }
                        if (playerTeam[1].targeting == true)
                        {
                            playerTeam[1].currentTargets.Add(potentialTargets[i]);
                            if (potentialTargets[i] < 3)
                            {
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[1]);
                                setTargetingAbility(playerTeam[potentialTargets[i]]);
                                setTargeter(playerTeam[potentialTargets[i]], 1);
                            }
                            if (potentialTargets[i] > 2)
                            {
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[1]);
                                setTargetingAbility(enemyTeam[potentialTargets[i] - 3]);
                                setTargeter(enemyTeam[potentialTargets[i] - 3], 1);
                            }
                        }
                        if (playerTeam[2].targeting == true)
                        {
                            playerTeam[2].currentTargets.Add(potentialTargets[i]);
                            if (potentialTargets[i] < 3)
                            {
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[2]);
                                setTargetingAbility(playerTeam[potentialTargets[i]]);
                                setTargeter(playerTeam[potentialTargets[i]], 2);
                            }
                            if (potentialTargets[i] > 2)
                            {
                                addTargetEffect(allTargeted[potentialTargets[i]], playerTeam[2]);
                                setTargetingAbility(enemyTeam[potentialTargets[i] - 3]);
                                setTargeter(enemyTeam[potentialTargets[i] - 3], 2);
                            }
                        }
                        
                    }
                    if (playerTeam[0].targeting == true)
                    {
                        finishTargeting(playerTeam[0]);
                        executionOrder.Add(0);
                    }
                    if (playerTeam[1].targeting == true)
                    {
                        finishTargeting(playerTeam[1]);
                        executionOrder.Add(1);
                    }
                    if (playerTeam[2].targeting == true)
                    {
                        finishTargeting(playerTeam[2]);
                        executionOrder.Add(2);
                    }

                }
                else
                {

                    if (playerTeam[0].targeting == true)
                    {
                        playerTeam[0].currentTargets.Add(slot);
                        addTargetEffect(allTargeted[slot], playerTeam[0]);
                        finishTargeting(playerTeam[0]);
                        setTargeter(target, 0);
                        setTargetingAbility(target);
                        executionOrder.Add(0);
                    }
                    if (playerTeam[1].targeting == true)
                    {
                        playerTeam[1].currentTargets.Add(slot);
                        addTargetEffect(allTargeted[slot], playerTeam[1]);
                        finishTargeting(playerTeam[1]);
                        setTargeter(target, 1);
                        setTargetingAbility(target);
                        executionOrder.Add(1);
                    }
                    if (playerTeam[2].targeting == true)
                    {
                        playerTeam[2].currentTargets.Add(slot);
                        addTargetEffect(allTargeted[slot], playerTeam[2]);
                        finishTargeting(playerTeam[2]);
                        setTargeter(target, 2);
                        setTargetingAbility(target);
                        executionOrder.Add(2);
                    }
                }
                playerTeam[0].targeting = false;
                playerTeam[1].targeting = false;
                playerTeam[2].targeting = false;

                
            }
        }
        public void showEnergy(Ability a, PictureBox[] pb)
        {
            int energyCounter = 0;
            for (int i = 0; i < pb.Length; i++)
            {

                pb[i].Visible = false;

            }

            if (a.physCost > 0)
            {

                for (int i = energyCounter; i < a.physCost; i++)
                {

                    pb[i].Visible = true;
                    pb[i].Image = Image.FromFile(@"..\..\images\physicalEnergy.png");
                    energyCounter += 1;
                    if (a.physCost > 1)
                    {
                        energyCounter -= 1;
                    }
                }

            }
            if (a.specCost > 0)
            {

                for (int i = 0; i < a.specCost; i++)
                {

                    pb[energyCounter + i].Visible = true;
                    pb[energyCounter + i].Image = Image.FromFile(@"..\..\images\specialEnergy.png");
                    energyCounter += 1;
                    if (a.specCost > 1)
                    {
                        energyCounter -= 1;
                    }
                }

            }
            if (a.mentCost > 0)
            {

                for (int i = 0; i < a.mentCost; i++)
                {

                    pb[energyCounter + i].Visible = true;
                    pb[energyCounter + i].Image = Image.FromFile(@"..\..\images\mentalEnergy.png");
                    energyCounter += 1;
                    if (a.mentCost > 1)
                    {
                        energyCounter -= 1;
                    }
                }

            }
            if (a.wepCost > 0)
            {

                for (int i = 0; i < a.wepCost; i++)
                {

                    pb[energyCounter + i].Visible = true;
                    pb[energyCounter + i].Image = Image.FromFile(@"..\..\images\weaponEnergy.png");
                    energyCounter += 1;
                    if (a.wepCost > 1)
                    {
                        energyCounter -= 1;
                    }
                }

            }
            if (a.randCost > 0)
            {

                for (int i = 0; i < a.randCost; i++)
                {

                    pb[energyCounter + i].Visible = true;
                    pb[energyCounter + i].Image = Image.FromFile(@"..\..\images\randomEnergy.png");
                    energyCounter += 1;
                    if (a.randCost > 1)
                    {
                        energyCounter -= 1;
                    }
                }

            }

        }
        public void showMoveDetails(int slot, int c)
        {
            enemyTeam[0].targetable = false;
            enemyTeam[1].targetable = false;
            enemyTeam[2].targetable = false;
            playerTeam[0].targetable = false;
            playerTeam[1].targetable = false;
            playerTeam[2].targetable = false;
            detailBoxes[c].Text = playerTeam[c].abilities[slot].description;
            showEnergy(playerTeam[c].abilities[slot], allCharacterEnergy[c]);
            characterCooldowns[c].Text = "CD : " + playerTeam[c].abilities[slot].cooldown.ToString();
            //turn all portraits non-selected
            for (int i = 0; i < 3; i++)
            {
                playerTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + playerTeam[i].profileImagePath);
                enemyTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + enemyTeam[i].profileImagePath);
            }
            //Reset all active player moves to non-selected
            targetingAbility = null;
            displayAvailability();

            if (checkEnergyAvailability(playerTeamTempEnergy, playerTeam[c].abilities[slot]) && playerTeam[c].stunned != true && playerTeam[c].targeted != true && playerTeam[c].abilities[slot].cooldownRemaining < 1 && playerTeam[c].abilities[slot].enabled == true && waiting == false && playerTeam[c].dead != true) { 
            //turn selected move to selected
                allPlayerMoves[c][slot].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[c].abilities[slot].imagePath);
            
                

                //reset all targeting/targetable protocols
                playerTeam[0].targeting = false;
                playerTeam[1].targeting = false;
                playerTeam[2].targeting = false;

                //turn targeting protocol on
                targetingCharacterSlot = c;
                playerTeam[c].targeting = true;
                switch (slot)
                {
                    case 0:
                        targetingAbility = playerTeam[c].abilities[0];
                        playerTeam[c].chosenAbilitySlot = 0;
                        targetingAbilitySlot = 0;
                        potentialTargets = playerTeam[c].targetAbility1(playerTeam, enemyTeam);
                        if (potentialTargets.Count > 0)
                        {
                            if (potentialTargets[0] == 6)
                            {
                                potentialTargets[0] = c;
                            }
                        }
                        for (int i = 0; i < playerTeam[c].targetAbility1(playerTeam, enemyTeam).Count; i++)
                        {

                            switch (playerTeam[c].targetAbility1(playerTeam, enemyTeam)[i])
                            {
                                case 0:
                                    playerTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[0].profileImagePath);
                                    playerTeam[0].targetable = true;
                                    break;
                                case 1:
                                    playerTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[1].profileImagePath);
                                    playerTeam[1].targetable = true;
                                    break;
                                case 2:
                                    playerTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[2].profileImagePath);
                                    playerTeam[2].targetable = true;
                                    break;
                                case 3:
                                    enemyTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[0].profileImagePath);
                                    enemyTeam[0].targetable = true;
                                    break;
                                case 4:
                                    enemyTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[1].profileImagePath);
                                    enemyTeam[1].targetable = true;
                                    break;
                                case 5:
                                    enemyTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[2].profileImagePath);
                                    enemyTeam[2].targetable = true;
                                    break;
                                case 6:
                                    playerTeamDisplay[c].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[c].profileImagePath);
                                    playerTeam[c].targetable = true;
                                    break;
                            }

                        }

                        break;
                    case 1:
                        targetingAbility = playerTeam[c].abilities[1];
                        targetingAbilitySlot = 1;
                        playerTeam[c].chosenAbilitySlot = 1;
                        
                        potentialTargets = playerTeam[c].targetAbility2(playerTeam, enemyTeam);
                        if (potentialTargets.Count > 0)
                        {
                            if (potentialTargets[0] == 6)
                            {
                                potentialTargets[0] = c;
                            }
                        }
                        for (int i = 0; i < playerTeam[c].targetAbility2(playerTeam, enemyTeam).Count; i++)
                        {

                            switch (playerTeam[c].targetAbility2(playerTeam, enemyTeam)[i])
                            {
                                case 0:
                                    playerTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[0].profileImagePath);
                                    playerTeam[0].targetable = true;
                                    break;
                                case 1:
                                    playerTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[1].profileImagePath);
                                    playerTeam[1].targetable = true;
                                    break;
                                case 2:
                                    playerTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[2].profileImagePath);
                                    playerTeam[2].targetable = true;
                                    break;
                                case 3:
                                    enemyTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[0].profileImagePath);
                                    enemyTeam[0].targetable = true;
                                    break;
                                case 4:
                                    enemyTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[1].profileImagePath);
                                    enemyTeam[1].targetable = true;
                                    break;
                                case 5:
                                    enemyTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[2].profileImagePath);
                                    enemyTeam[2].targetable = true;
                                    break;
                                case 6:
                                    playerTeamDisplay[c].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[c].profileImagePath);
                                    playerTeam[c].targetable = true;
                                    break;
                            }

                        }

                        break;
                    case 2:
                        targetingAbility = playerTeam[c].abilities[2];
                        targetingAbilitySlot = 2;
                        playerTeam[c].chosenAbilitySlot = 2;
                        potentialTargets = playerTeam[c].targetAbility3(playerTeam, enemyTeam);
                        if (potentialTargets.Count > 0)
                        {
                            if (potentialTargets[0] == 6)
                            {
                                potentialTargets[0] = c;
                            }
                        }
                        for (int i = 0; i < playerTeam[c].targetAbility3(playerTeam, enemyTeam).Count; i++)
                        {

                            switch (playerTeam[c].targetAbility3(playerTeam, enemyTeam)[i])
                            {
                                case 0:
                                    playerTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[0].profileImagePath);
                                    playerTeam[0].targetable = true;
                                    break;
                                case 1:
                                    playerTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[1].profileImagePath);
                                    playerTeam[1].targetable = true;
                                    break;
                                case 2:
                                    playerTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[2].profileImagePath);
                                    playerTeam[2].targetable = true;
                                    break;
                                case 3:
                                    enemyTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[0].profileImagePath);
                                    enemyTeam[0].targetable = true;
                                    break;
                                case 4:
                                    enemyTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[1].profileImagePath);
                                    enemyTeam[1].targetable = true;
                                    break;
                                case 5:
                                    enemyTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[2].profileImagePath);
                                    enemyTeam[2].targetable = true;
                                    break;
                                case 6:
                                    playerTeamDisplay[c].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[c].profileImagePath);
                                    playerTeam[c].targetable = true;
                                    break;
                            }

                        }

                        break;
                    case 3:
                        targetingAbility = playerTeam[c].abilities[3];
                        targetingAbilitySlot = 3;
                        playerTeam[c].chosenAbilitySlot = 3;
                        potentialTargets = playerTeam[c].targetAbility4(playerTeam, enemyTeam);
                        if (potentialTargets.Count > 0)
                        {
                            if (potentialTargets[0] == 6)
                            {
                                potentialTargets[0] = c;
                            }
                        }
                        for (int i = 0; i < playerTeam[c].targetAbility4(playerTeam, enemyTeam).Count; i++)
                        {

                            switch (playerTeam[c].targetAbility4(playerTeam, enemyTeam)[i])
                            {
                                case 0:
                                    playerTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[0].profileImagePath);
                                    playerTeam[0].targetable = true;
                                    break;
                                case 1:
                                    playerTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[1].profileImagePath);
                                    playerTeam[1].targetable = true;
                                    break;
                                case 2:
                                    playerTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[2].profileImagePath);
                                    playerTeam[2].targetable = true;
                                    break;
                                case 3:
                                    enemyTeamDisplay[0].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[0].profileImagePath);
                                    enemyTeam[0].targetable = true;
                                    break;
                                case 4:
                                    enemyTeamDisplay[1].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[1].profileImagePath);
                                    enemyTeam[1].targetable = true;
                                    break;
                                case 5:
                                    enemyTeamDisplay[2].Image = Image.FromFile(@"..\..\images\selected" + enemyTeam[2].profileImagePath);
                                    enemyTeam[2].targetable = true;
                                    break;
                                case 6:
                                    playerTeamDisplay[c].Image = Image.FromFile(@"..\..\images\selected" + playerTeam[c].profileImagePath);
                                    playerTeam[c].targetable = true;
                                    break;
                            }

                        }

                        break;
                }
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            
            for (int i = 0; i < 3; i++)
            {
                if (playerTeam[i].isToga == true)
                {
                    playerTogaSlot = i;
                }
                if (enemyTeam[i].isToga == true)
                {
                    enemyTogaSlot = i;
                }
            }

            enemyTeamDisplay[0] = enemyBox1;
            enemyTeamDisplay[1] = enemyBox2;
            enemyTeamDisplay[2] = enemyBox3;

            characterCooldowns[0] = char1Cooldown;
            characterCooldowns[1] = char2Cooldown;
            characterCooldowns[2] = char3Cooldown;

            player1Effects[0] = p1e1;
            player1Effects[1] = p1e2;
            player1Effects[2] = p1e3;
            player1Effects[3] = p1e4;
            player1Effects[4] = p1e5;
            player1Effects[5] = p1e6;
            player1Effects[6] = p1e7;
            player1Effects[7] = p1e8;
            player1Effects[8] = p1e9;
            player1Effects[9] = p1e10;
            player1Effects[10] = p1e11;
            player1Effects[11] = p1e12;
            player1Effects[12] = p1e13;
            player1Effects[13] = p1e14;
            player1Effects[14] = p1e15;
            player1Effects[15] = p1e16;

            player2Effects[0] = p2e1;
            player2Effects[1] = p2e2;
            player2Effects[2] = p2e3;
            player2Effects[3] = p2e4;
            player2Effects[4] = p2e5;
            player2Effects[5] = p2e6;
            player2Effects[6] = p2e7;
            player2Effects[7] = p2e8;
            player2Effects[8] = p2e9;
            player2Effects[9] = p2e10;
            player2Effects[10] = p2e11;
            player2Effects[11] = p2e12;
            player2Effects[12] = p2e13;
            player2Effects[13] = p2e14;
            player2Effects[14] = p2e15;
            player2Effects[15] = p2e16;

            player3Effects[0] = p3e1;
            player3Effects[1] = p3e2;
            player3Effects[2] = p3e3;
            player3Effects[3] = p3e4;
            player3Effects[4] = p3e5;
            player3Effects[5] = p3e6;
            player3Effects[6] = p3e7;
            player3Effects[7] = p3e8;
            player3Effects[8] = p3e9;
            player3Effects[9] = p3e10;
            player3Effects[10] = p3e11;
            player3Effects[11] = p3e12;
            player3Effects[12] = p3e13;
            player3Effects[13] = p3e14;
            player3Effects[14] = p3e15;
            player3Effects[15] = p3e16;

            enemy1Effects[0] = e1e1;
            enemy1Effects[1] = e1e2;
            enemy1Effects[2] = e1e3;
            enemy1Effects[3] = e1e4;
            enemy1Effects[4] = e1e5;
            enemy1Effects[5] = e1e6;
            enemy1Effects[6] = e1e7;
            enemy1Effects[7] = e1e8;
            enemy1Effects[8] = e1e9;
            enemy1Effects[9] = e1e10;
            enemy1Effects[10] = e1e11;
            enemy1Effects[11] = e1e12;
            enemy1Effects[12] = e1e13;
            enemy1Effects[13] = e1e14;
            enemy1Effects[14] = e1e15;
            enemy1Effects[15] = e1e16;

            enemy2Effects[0] = e2e1;
            enemy2Effects[1] = e2e2;
            enemy2Effects[2] = e2e3;
            enemy2Effects[3] = e2e4;
            enemy2Effects[4] = e2e5;
            enemy2Effects[5] = e2e6;
            enemy2Effects[6] = e2e7;
            enemy2Effects[7] = e2e8;
            enemy2Effects[8] = e2e9;
            enemy2Effects[9] = e2e10;
            enemy2Effects[10] = e2e11;
            enemy2Effects[11] = e2e12;
            enemy2Effects[12] = e2e13;
            enemy2Effects[13] = e2e14;
            enemy2Effects[14] = e2e15;
            enemy2Effects[15] = e2e16;

            enemy3Effects[0] = e3e1;
            enemy3Effects[1] = e3e2;
            enemy3Effects[2] = e3e3;
            enemy3Effects[3] = e3e4;
            enemy3Effects[4] = e3e5;
            enemy3Effects[5] = e3e6;
            enemy3Effects[6] = e3e7;
            enemy3Effects[7] = e3e8;
            enemy3Effects[8] = e3e9;
            enemy3Effects[9] = e3e10;
            enemy3Effects[10] = e3e11;
            enemy3Effects[11] = e3e12;
            enemy3Effects[12] = e3e13;
            enemy3Effects[13] = e3e14;
            enemy3Effects[14] = e3e15;
            enemy3Effects[15] = e3e16;

            allEffects[0] = player1Effects;
            allEffects[1] = player2Effects;
            allEffects[2] = player3Effects;
            allEffects[3] = enemy1Effects;
            allEffects[4] = enemy2Effects;
            allEffects[5] = enemy3Effects;

            playerTeamDisplay[0] = profBox1;
            playerTeamDisplay[1] = profBox2;
            playerTeamDisplay[2] = profBox3;

            detailBoxes[0] = char1detail;
            detailBoxes[1] = char2detail;
            detailBoxes[2] = char3detail;

            character1Moves[0] = char1move1;
            character1Moves[1] = char1move2;
            character1Moves[2] = char1move3;
            character1Moves[3] = char1move4;

            character2Moves[0] = char2move1;
            character2Moves[1] = char2move2;
            character2Moves[2] = char2move3;
            character2Moves[3] = char2move4;

            character3Moves[0] = char3move1;
            character3Moves[1] = char3move2;
            character3Moves[2] = char3move3;
            character3Moves[3] = char3move4;

            character1Energy[0] = char1energyBox1;
            character1Energy[1] = char1energyBox2;
            character1Energy[2] = char1energyBox3;
            character1Energy[3] = char1energyBox4;
            character1Energy[4] = char1energyBox5;

            character2Energy[0] = char2energyBox1;
            character2Energy[1] = char2energyBox2;
            character2Energy[2] = char2energyBox3;
            character2Energy[3] = char2energyBox4;
            character2Energy[4] = char2energyBox5;

            character3Energy[0] = char3energyBox1;
            character3Energy[1] = char3energyBox2;
            character3Energy[2] = char3energyBox3;
            character3Energy[3] = char3energyBox4;
            character3Energy[4] = char3energyBox5;

            allCharacterEnergy[0] = character1Energy;
            allCharacterEnergy[1] = character2Energy;
            allCharacterEnergy[2] = character3Energy;

            allPlayerMoves[0] = character1Moves;
            allPlayerMoves[1] = character2Moves;
            allPlayerMoves[2] = character3Moves;

            player1Targeted[0] = p1t1;
            player1Targeted[1] = p1t2;
            player1Targeted[2] = p1t3;
            player2Targeted[0] = p2t1;
            player2Targeted[1] = p2t2;
            player2Targeted[2] = p2t3;
            player3Targeted[0] = p3t1;
            player3Targeted[1] = p3t2;
            player3Targeted[2] = p3t3;

            enemy1Targeted[0] = e1t1;
            enemy1Targeted[1] = e1t2;
            enemy1Targeted[2] = e1t3;
            enemy2Targeted[0] = e2t1;
            enemy2Targeted[1] = e2t2;
            enemy2Targeted[2] = e2t3;
            enemy3Targeted[0] = e3t1;
            enemy3Targeted[1] = e3t2;
            enemy3Targeted[2] = e3t3;

            allTargeted[0] = player1Targeted;
            allTargeted[1] = player2Targeted;
            allTargeted[2] = player3Targeted;
            allTargeted[3] = enemy1Targeted;
            allTargeted[4] = enemy2Targeted;
            allTargeted[5] = enemy3Targeted;

            allCharacters[0] = playerTeam[0];
            allCharacters[1] = playerTeam[1];
            allCharacters[2] = playerTeam[2];
            allCharacters[3] = enemyTeam[0];
            allCharacters[4] = enemyTeam[1];
            allCharacters[5] = enemyTeam[2];

            turnTimer.Tick += new EventHandler(timesUp);
            turnTimer.Enabled = true;
            turnTimer.Interval = 1000;
            turnTimer.Start();
            timerTimer.Tick += new EventHandler(timesUpsUp);
            timerTimer.Enabled = true;
            timerTimer.Interval = 100;

            playerTeam[0].startHP = playerTeam[0].hp;
            p1hpback.Refresh();
            playerTeam[1].startHP = playerTeam[1].hp;
            p1hpback.Refresh();
            playerTeam[2].startHP = playerTeam[2].hp;
            p1hpback.Refresh();

            enemyTeam[0].startHP = enemyTeam[0].hp;
            e1hpback.Refresh(); 
            enemyTeam[1].startHP = enemyTeam[1].hp;
            e1hpback.Refresh(); 
            enemyTeam[2].startHP = enemyTeam[2].hp;
            e1hpback.Refresh();

            if (isHost == true)
            {
                waiting = false;
                TcpListener connectIn = new TcpListener(IPAddress.Any, 5692);
                connectIn.Start();
                s = connectIn.AcceptSocket();
                turnStart(playerTeam, enemyTeam);
            }
            if (isHost == false)
            {

                waiting = true;
                nextTurnButton.Enabled = false;
                TcpClient connectOut = new TcpClient();
                var result = connectOut.BeginConnect(hostIPAddress, 5692, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(20));
                if (!success)
                {
                    throw new Exception("Failed to connect.");
                }
                strm = connectOut.GetStream();

            }
            for (int i = 0; i < 3; i++)
            {
                playerTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + playerTeam[i].profileImagePath);
                detailBoxes[i].Text = playerTeam[i].description;
                enemyTeamDisplay[i].Image = Image.FromFile(@"..\..\images\" + enemyTeam[i].profileImagePath);

                for (int j = 0; j < 4; j++)
                {

                    allPlayerMoves[i][j].Image = Image.FromFile(@"..\..\images\null" + playerTeam[i].abilities[j].imagePath);
                    
                }

            }
            displayAvailability();

            



            
            
        }
        #region Asset click methods
        private void char1move1_Click(object sender, EventArgs e)
        {
            showMoveDetails(0, 0);
            

        }
        private void char1detail_TextChanged(object sender, EventArgs e)
        {

        }
        private void char1move2_Click(object sender, EventArgs e)
        {
            showMoveDetails(1, 0);
            
        }
        private void char1move3_Click(object sender, EventArgs e)
        {
            showMoveDetails(2, 0);

        }
        private void char1move4_Click(object sender, EventArgs e)
        {
            showMoveDetails(3, 0);
            
        }
        private void char2move1_Click(object sender, EventArgs e)
        {
            showMoveDetails(0, 1);
        }
        private void char2move2_Click(object sender, EventArgs e)
        {
            showMoveDetails(1, 1);
        }
        private void char2move3_Click(object sender, EventArgs e)
        {
            showMoveDetails(2, 1);
        }
        private void char2move4_Click(object sender, EventArgs e)
        {
            showMoveDetails(3, 1);
        }
        private void char3move1_Click(object sender, EventArgs e)
        {
            showMoveDetails(0, 2);
        }
        private void char3move2_Click(object sender, EventArgs e)
        {
            showMoveDetails(1, 2);
        }
        private void char3move3_Click(object sender, EventArgs e)
        {
            showMoveDetails(2, 2);
        }
        private void char3move4_Click(object sender, EventArgs e)
        {
            showMoveDetails(3, 2);
        }
        private void profBox1_Click(object sender, EventArgs e)
        {
            beginTargeting(playerTeam[0], 0);
            playerTeam[targetingCharacterSlot].primaryTargetSlot = 0;
        }
        private void profBox2_Click(object sender, EventArgs e)
        {
            beginTargeting(playerTeam[1], 1);
            playerTeam[targetingCharacterSlot].primaryTargetSlot = 1;
        }
        private void profBox3_Click(object sender, EventArgs e)
        {
            beginTargeting(playerTeam[2], 2);
            playerTeam[targetingCharacterSlot].primaryTargetSlot = 2;
        }
        private void enemyBox1_Click(object sender, EventArgs e)
        {
            beginTargeting(enemyTeam[0], 3);
            playerTeam[targetingCharacterSlot].primaryTargetSlot = 3;
        }
        private void enemyBox2_Click(object sender, EventArgs e)
        {
            beginTargeting(enemyTeam[1], 4);
            playerTeam[targetingCharacterSlot].primaryTargetSlot = 4;
        }
        private void enemyBox3_Click(object sender, EventArgs e)
        {
            beginTargeting(enemyTeam[2], 5);
            playerTeam[targetingCharacterSlot].primaryTargetSlot = 5;
        }
        #endregion

        

        private void nextTurnButton_Click(object sender, EventArgs e)
        {



            if (turnRandCost > 0)
            {
                randNeeded = turnRandCost;
                randomAssignmentPanel.Visible = true;
                randomNeeded.Text = turnRandCost.ToString();
                offeredMent = 0;
                offeredPhys = 0;
                offeredSpec = 0;
                offeredWep = 0;
                physHeld.Text = playerTeamTempEnergy[0].ToString();
                specHeld.Text = playerTeamTempEnergy[1].ToString();
                mentHeld.Text = playerTeamTempEnergy[2].ToString();
                wepHeld.Text = playerTeamTempEnergy[3].ToString();
            }
            else if (turnRandCost == 0)
            {
                randPaid = true;
            }
            if (randPaid == true)
            {

                executeTurn();

                for (int i = 0; i < allCharacters.Length; i++)
                {

                    if (allCharacters[i].dead != true)
                    {

                        //populate the list of VISIBLE effects

                        for (int j = 0; j < allCharacters[i].effects.Count; j++)
                        {
                            if (allCharacters[i].effects[j].invisible != true || isAllied(allCharacters[i].effects[j]))
                            {
                                displayEffects.Add(allCharacters[i].effects[j]);
                            }
                        }

                        for (int j = 0; j < displayEffects.Count; j++)
                        {
                            
                            allEffects[i][j].Visible = true;
                            allEffects[i][j].Image = Image.FromFile(@"..\..\images\mini" + displayEffects[j].imgPath);
                            
                        }
                        displayEffects.Clear();
                    }

                }

                

                #region Cooldown Tracking

                for (int i = 0; i < 3; i++)
                {

                    for (int j = 0; j < 4; j++)
                    {
                        playerTeam[i].abilities[j].cooldownRemaining -= 1;
                        if (playerTeam[i].abilities[j].cooldownRemaining <= 0)
                        {
                            switch (i)
                            {
                                case 0:
                                    switch (j)
                                    {
                                        case 0:
                                            p1m1c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 1:
                                            p1m2c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 2:
                                            p1m3c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 3:
                                            p1m4c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                    }
                                    break;
                                case 1:
                                    switch (j)
                                    {
                                        case 0:
                                            p2m1c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 1:
                                            p2m2c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 2:
                                            p2m3c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 3:
                                            p2m4c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                    }
                                    break;
                                case 2:
                                    switch (j)
                                    {
                                        case 0:
                                            p3m1c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 1:
                                            p3m2c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 2:
                                            p3m3c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                        case 3:
                                            p3m4c.Visible = false;
                                            playerTeam[i].abilities[j].cooldownRemaining = 0;
                                            break;
                                    }
                                    break;

                            }
                        }
                    }

                }
                #endregion

                #region Refresh all Characters
                for (int i = 0; i < 3; i++)
                {
                    refresh(playerTeam[i]);
                    refresh(enemyTeam[i]);
                    for (int j = 0; j < 6; j++)
                    {
                        allTargeted[j][i].Visible = false;
                    }
                }
                #endregion
                
                randPaid = false;


                for (int i = 0; i < allCharacters.Length; i++)
                {
                    for (int j = 0; j < allCharacters[i].effects.Count; j++)
                    {
                        checkEffect(allCharacters[i].effects[j]);
                    }
                }
                foreach (Character c in allCharacters)
                {
                    foreach (Effect eff in c.effects)
                    {
                        
                        eff.duration -= 1;
                    }
                }

                for (int i = 0; i < allCharacters.Length; i++)
                {
                    for (int j = allCharacters[i].effects.Count - 1; j >= 0; j--)
                    {
                        if (allCharacters[i].effects[j].duration < 1)
                        {
                            endEffect(allCharacters[i].effects[j]);
                            removeEffect(allCharacters[i].effects[j]);
                        }
                    }
                }


                if (isHost == false)
                {
                    
                    ASCIIEncoding asen = new ASCIIEncoding();

                    byte[] ba = asen.GetBytes(serializeGameState());
                    strm.Write(ba, 0, ba.Length);
                    waiting = true;
                    displayAvailability();
                    nextTurnButton.Enabled = false;
                    
                }
                
                if (isHost == true)
                {
                    

                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes(serializeGameState()));
                    waiting = true;
                    displayAvailability();
                    nextTurnButton.Enabled = false;
                    
                }
                    
                


            }

            
        }
        #region target click methods
        private void p1t1_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 0, 0);
        }
        private void p1t2_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 0, 1);
        }
        private void p1t3_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 0, 2);
        }
        private void p2t1_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 1, 0);
        }
        private void p2t2_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 1, 1);
        }
        private void p2t3_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 1, 2);
        }
        private void p3t1_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 2, 0);
        }
        private void p3t2_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 2, 1);
        }
        private void p3t3_Click(object sender, EventArgs e)
        {
            removeTarget(playerTeam, 2, 2);
        }
        private void e1t1_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 0, 0);
        }
        private void e1t2_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 0, 1);
        }
        private void e1t3_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 0, 2);
        }
        private void e2t1_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 1, 0);
        }
        private void e2t2_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 1, 1);
        }
        private void e2t3_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 1, 2);
        }
        #endregion
        private void specSub_Click(object sender, EventArgs e)
        {
            if (offeredSpec > 0)
            {
                playerTeamTempEnergy[1] += 1;
                offeredSpec -= 1;
                randNeeded += 1;
                turnEndButton.Enabled = false;
            }
            specHeld.Text = playerTeamTempEnergy[1].ToString();
            specOffered.Text = offeredSpec.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }
        private void mentSub_Click(object sender, EventArgs e)
        {
            if (offeredMent > 0)
            {
                playerTeamTempEnergy[2] += 1;
                offeredMent -= 1;
                randNeeded += 1;
                turnEndButton.Enabled = false;
            }
            mentHeld.Text = playerTeamTempEnergy[2].ToString();
            mentOffered.Text = offeredMent.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }
        private void wepSub_Click(object sender, EventArgs e)
        {
            if (offeredWep > 0)
            {
                playerTeamTempEnergy[3] += 1;
                offeredWep -= 1;
                randNeeded += 1;
                turnEndButton.Enabled = false;
            }
            wepHeld.Text = playerTeamTempEnergy[3].ToString();
            wepOffered.Text = offeredWep.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }
        private void physAdd_Click(object sender, EventArgs e)
        {
            if (playerTeamTempEnergy[0] > 0 && randNeeded > 0)
            {
                playerTeamTempEnergy[0] -= 1;
                offeredPhys += 1;
                randNeeded -= 1;
            }
            physHeld.Text = playerTeamTempEnergy[0].ToString();
            physOffered.Text = offeredPhys.ToString();
            randomNeeded.Text = randNeeded.ToString();

            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }
        private void specAdd_Click(object sender, EventArgs e)
        {
            if (playerTeamTempEnergy[1] > 0 && randNeeded > 0)
            {
                playerTeamTempEnergy[1] -= 1;
                offeredSpec += 1;
                randNeeded -= 1;
            }
            specHeld.Text = playerTeamTempEnergy[1].ToString();
            specOffered.Text = offeredSpec.ToString();
            randomNeeded.Text = randNeeded.ToString();
            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }
        private void mentAdd_Click(object sender, EventArgs e)
        {
            if (playerTeamTempEnergy[2] > 0 && randNeeded > 0)
            {
                playerTeamTempEnergy[2] -= 1;
                offeredMent += 1;
                randNeeded -= 1;
            }
            mentHeld.Text = playerTeamTempEnergy[2].ToString();
            mentOffered.Text = offeredMent.ToString();
            randomNeeded.Text = randNeeded.ToString();
            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }
        private void wepAdd_Click(object sender, EventArgs e)
        {
            if (playerTeamTempEnergy[3] > 0 && randNeeded > 0)
            {
                playerTeamTempEnergy[3] -= 1;
                offeredWep += 1;
                randNeeded -= 1;
            }
            wepHeld.Text = playerTeamTempEnergy[3].ToString();
            wepOffered.Text = offeredWep.ToString();
            randomNeeded.Text = randNeeded.ToString();
            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }
        private void e3t1_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 2, 0);
        }
        private void e3t2_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 2, 1);
        }
        private void e3t3_Click(object sender, EventArgs e)
        {
            removeTarget(enemyTeam, 2, 2);
        }

        private void physSub_Click(object sender, EventArgs e)
        {
            if (offeredPhys > 0)
            {
                playerTeamTempEnergy[0] += 1;
                offeredPhys -= 1;
                randNeeded += 1;
                turnEndButton.Enabled = false;
            }
            physHeld.Text = playerTeamTempEnergy[0].ToString();
            physOffered.Text = offeredPhys.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }

        private void turnEndButton_Click(object sender, EventArgs e)
        {
            
            randomAssignmentPanel.Visible = false;
            turnEndButton.Enabled = false;
            turnRandCost = 0;


            executeTurn();

            for (int i = 0; i < allCharacters.Length; i++)
            {

                if (allCharacters[i].dead != true)
                {

                    //populate the list of VISIBLE effects

                    for (int j = 0; j < allCharacters[i].effects.Count; j++)
                    {
                        if (allCharacters[i].effects[j].invisible != true || isAllied(allCharacters[i].effects[j]))
                        {
                            displayEffects.Add(allCharacters[i].effects[j]);
                        }
                    }

                    for (int j = 0; j < displayEffects.Count; j++)
                    {

                        allEffects[i][j].Visible = true;
                        allEffects[i][j].Image = Image.FromFile(@"..\..\images\mini" + displayEffects[j].imgPath);

                    }
                    displayEffects.Clear();
                }

            }

            

            #region Cooldown Tracking
            for (int i = 0; i < 3; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    playerTeam[i].abilities[j].cooldownRemaining -= 1;
                    if (playerTeam[i].abilities[j].cooldownRemaining <= 0)
                    {
                        switch (i)
                        {
                            case 0:
                                switch (j)
                                {
                                    case 0:
                                        p1m1c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 1:
                                        p1m2c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 2:
                                        p1m3c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 3:
                                        p1m4c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                }
                                break;
                            case 1:
                                switch (j)
                                {
                                    case 0:
                                        p2m1c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 1:
                                        p2m2c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 2:
                                        p2m3c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 3:
                                        p2m4c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                }
                                break;
                            case 2:
                                switch (j)
                                {
                                    case 0:
                                        p3m1c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 1:
                                        p3m2c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 2:
                                        p3m3c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                    case 3:
                                        p3m4c.Visible = false;
                                        playerTeam[i].abilities[j].cooldownRemaining = 0;
                                        break;
                                }
                                break;

                        }
                    }
                }

            }
            #endregion

            #region Refresh All Characters
            for (int i = 0; i < 3; i++)
            {
                refresh(playerTeam[i]);
                refresh(enemyTeam[i]);
                for (int j = 0; j < 6; j++)
                {
                    allTargeted[j][i].Visible = false;
                }
            }
            #endregion
            
            randPaid = false;

            for (int i = 0; i < allCharacters.Length; i++)
            {
                for (int j = 0; j < allCharacters[i].effects.Count; j++)
                {
                    checkEffect(allCharacters[i].effects[j]);
                }
            }

            foreach (Character c in allCharacters)
            {
                foreach (Effect eff in c.effects)
                {
                   
                    eff.duration -= 1;
                }
            }

            for (int i = 0; i < allCharacters.Length; i++)
            {
                for (int j = allCharacters[i].effects.Count - 1; j >= 0; j--)
                {
                    if (allCharacters[i].effects[j].duration < 1)
                    {
                        endEffect(allCharacters[i].effects[j]);
                        removeEffect(allCharacters[i].effects[j]);
                    }
                }
            }

            if (isHost == false)
            {

                ASCIIEncoding asen = new ASCIIEncoding();

                byte[] ba = asen.GetBytes(serializeGameState());
                strm.Write(ba, 0, ba.Length);
                waiting = true;
                displayAvailability();
                nextTurnButton.Enabled = false;
                
            }

            if (isHost == true)
            {


                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes(serializeGameState()));
                waiting = true;
                displayAvailability();
                nextTurnButton.Enabled = false;
                
            }








        }

        private void effectHoverPanel_Paint(object sender, PaintEventArgs e)
        {

        }


        #region player 1 effect hover
        private void p1e1_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 0);
        }

        private void p2e1_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 0);
        }

        private void p1e3_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 2);
        }

        private void p1e2_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 1);
        }

        private void p1e4_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 3);
        }

        private void p1e5_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 4);
        }

        private void p1e6_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 5);
        }

        private void p1e7_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 6);
        }

        private void p1e8_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 7);
        }

        private void p1e9_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 8);
        }

        private void p1e10_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 9);
        }

        private void p1e11_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 10);
        }

        private void p1e12_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 11);
        }

        private void p1e13_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 12);
        }

        private void p1e14_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 13);
        }

        private void p1e15_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 14);
        }

        private void p1e16_MouseHover(object sender, EventArgs e)
        {
            showHoverText(0, 15);
        }

        #endregion
        #region player 2 effect hover
        private void p1e1_MouseLeave(object sender, EventArgs e)
        {
            effectHoverPanel.Visible = false;
            effectHoverPanel.Height = 80;
        }

        private void p1e2_MouseLeave(object sender, EventArgs e)
        {
            effectHoverPanel.Visible = false;
            effectHoverPanel.Height = 80;
        }

        private void p2e2_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 1);
        }

        private void p2e3_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 2);
        }

        private void p2e4_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 3);
        }

        private void p2e5_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 4);
        }

        private void p2e6_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 5);
        }

        private void p2e7_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 6);
        }

        private void p2e8_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 7);
        }

        private void p2e9_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 8);
        }

        private void p2e10_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 9);
        }

        private void p2e11_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 10);
        }

        private void p2e12_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 11);
        }

        private void p2e13_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 12);
        }

        private void p2e14_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 13);
        }

        private void p2e15_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 14);
        }

        private void p2e16_MouseHover(object sender, EventArgs e)
        {
            showHoverText(1, 15);
        }
        #endregion

        #region player 3 effect hover
        private void p3e1_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 0);
        }

        private void p3e2_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 1);
        }

        private void p3e3_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 2);
        }

        private void p3e4_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 3);
        }

        private void p3e5_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 4);
        }

        private void p3e6_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 5);
        }

        private void p3e7_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 6);
        }

        private void p3e8_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 7);
        }

        private void p3e9_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 8);
        }

        private void p3e10_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 9);
        }

        private void p3e11_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 10);
        }

        private void p3e12_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 11);
        }

        private void p3e13_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 12);
        }

        private void p3e14_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 13);
        }

        private void p3e15_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 14);
        }

        private void p3e16_MouseHover(object sender, EventArgs e)
        {
            showHoverText(2, 15);
        }


        #endregion
        #region enemy effect hover
        private void e1e1_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 0);
        }

        private void e1e2_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 1);
        }

        private void e1e3_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 2);
        }

        private void e1e4_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 3);
        }

        private void e1e5_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 4);
        }

        private void e1e6_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 5);
        }

        private void e1e7_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 6);
        }

        private void e1e8_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 7);
        }

        private void e1e9_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 8);
        }

        private void e1e10_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 9);
        }

        private void e1e11_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 10);
        }

        private void e1e12_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 11);
        }

        private void e1e13_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 12);
        }

        private void e1e14_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 13);
        }

        private void e1e15_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 14);
        }

        private void e1e16_MouseHover(object sender, EventArgs e)
        {
            showHoverText(3, 15);
        }

        private void e2e1_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 0);
        }

        private void e2e2_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 1);
        }

        private void e2e3_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 2);
        }

        private void e2e4_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 3);
        }

        private void e2e5_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 4);
        }

        private void e2e6_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 5);
        }

        private void e2e7_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 6);
        }

        private void e2e8_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 7);
        }

        private void e2e9_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 8);
        }

        private void e2e10_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 9);
        }

        private void e2e11_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 10);
        }

        private void e2e12_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 11);
        }

        private void e2e13_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 12);
        }

        private void e2e14_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 13);
        }

        private void e2e15_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 14);
        }

        private void e2e16_MouseHover(object sender, EventArgs e)
        {
            showHoverText(4, 15);
        }

        private void e3e1_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 0);
        }

        private void e3e2_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 1);
        }

        private void e3e3_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 2);
        }

        private void e3e4_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 3);
        }

        private void e3e5_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 4);
        }

        private void e3e6_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 5);
        }

        private void e3e7_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 6);
        }

        private void e3e8_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 7);
        }

        private void e3e9_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 8);
        }

        private void e3e10_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 9);
        }

        private void e3e11_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 10);
        }

        private void e3e12_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 11);
        }

        private void e3e13_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 12);
        }

        private void e3e14_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 13);
        }

        private void e3e15_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 14);
        }

        private void e3e16_MouseHover(object sender, EventArgs e)
        {
            showHoverText(5, 15);
        }
        #endregion

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        #region Health Bar Paint Methods
        private void p1hpback_Paint(object sender, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics,
                          playerTeam[0].startHP.ToString(),
                          this.Font,
                          new Point(0, 47),
                          Color.White);
        }

        private void p2hpback_Paint(object sender, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics,
                          playerTeam[1].startHP.ToString(),
                          this.Font,
                          new Point(0, 47),
                          Color.White);
        }

        private void p3hpback_Paint(object sender, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics,
                          playerTeam[2].startHP.ToString(),
                          this.Font,
                          new Point(0, 47),
                          Color.White);
        }

        private void e1hpback_Paint(object sender, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics,
                          enemyTeam[0].startHP.ToString(),
                          this.Font,
                          new Point(19, 47),
                          Color.White);
        }

        private void e2hpback_Paint(object sender, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics,
                          enemyTeam[1].startHP.ToString(),
                          this.Font,
                          new Point(19, 47),
                          Color.White);
        }

        private void e3hpback_Paint(object sender, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics,
                          enemyTeam[2].startHP.ToString(),
                          this.Font,
                          new Point(19, 47),
                          Color.White);
        }
        #endregion
    }
}
