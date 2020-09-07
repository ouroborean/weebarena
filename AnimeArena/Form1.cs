using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CharacterClass;
using CharacterNaruto;
using AbilityClass;
using CharacterIchigo;
using CharacterNatsu;
using CharacterMidoriya;
using CharacterSaber;
using CharacterSnowWhite;
using CharacterMisaka;
using CharacterTatsumi;
using CharacterTsunayoshi;
using CharacterChachamaru;
using ClassKuroko;
using CharacterAttackertron;
using CharacterDummybot;
using CharacterInvincoBill;
using CharacterToga;
using CharacterItachi;
using CharacterMirio;
using CharacterGunha;
using CharacterGray;
using CharacterMinato;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace AnimeArena
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int pageNum = 1;
        public bool isHost;
        public int[] energy = new int[5] { 0, 0, 0, 0, 0 };
        public int[] enemyEnergy = new int[5] { 0, 0, 0, 0, 0 };
        public Character[] team = new Character[3];
        public Character[] enemyTeam = new Character[3];
        public Dummybot dummybot = new Dummybot();
        public Attackertron attackertron = new Attackertron();
        public InvincoBill invincobill = new InvincoBill();
        public Naruto naruto = new Naruto();
        public Ichigo ichigo = new Ichigo();
        public Midoriya midoriya = new Midoriya();
        public Natsu natsu = new Natsu();
        public Tsunayoshi tsunayoshi = new Tsunayoshi();
        public SnowWhite snowwhite = new SnowWhite();
        public Misaka misaka = new Misaka();
        public Saber saber = new Saber();
        public Tatsumi tatsumi = new Tatsumi();
        public Kuroko kuroko = new Kuroko();
        public Toga toga = new Toga();
        public Itachi itachi = new Itachi();
        public Chachamaru chachamaru = new Chachamaru();
        public Mirio mirio = new Mirio();
        public Gunha gunha = new Gunha();
        public Gray gray = new Gray();
        public Minato minato = new Minato();
        public PictureBox[] selectedDisplay = new PictureBox[3];
        public PictureBox[] energyDisplay = new PictureBox[5];
        public Button[] removeDisplay = new Button[3];
        public string selected;
        
        public void removeSelected(Character[] team, int slot)
        {

            switch (slot)
            {
                case 1:
                    if(team[1] != null)
                    {
                        team[0] = team[1];
                        team[1] = null;
                        if (team[2] != null)
                        {
                            team[1] = team[2];
                            team[2] = null;
                        }
                        break;
                    }
                    else { team[0] = null; break; }
                case 2:
                    if(team[2] != null)
                    {
                        team[1] = team[2];
                        team[2] = null;
                        break;
                    }
                    else { team[1] = null; break; }
                case 3:
                    team[2] = null;
                    break;
            }



        }
        public void showDetailMenu()
        {
            detailAbilityBox1.Visible = true;
            detailAbilityBox2.Visible = true;
            detailAbilityBox3.Visible = true;
            detailAbilityBox4.Visible = true;
            detailBox.Visible = true;
            detailText.Visible = true;
            cooldownText.Visible = true;
            addButton.Visible = true;

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

                    pb[energyCounter+i].Visible = true;
                    pb[energyCounter+i].Image = Image.FromFile(@"..\..\images\specialEnergy.png");
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

                    pb[energyCounter+i].Visible = true;
                    pb[energyCounter+i].Image = Image.FromFile(@"..\..\images\mentalEnergy.png");
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
        public void showDetails(string desc, string profilePath, List<Ability> abilities)
        {

            detailText.Text = desc;

            detailBox.Image = Image.FromFile(@"..\..\images\" + profilePath);
            detailAbilityBox1.Image = Image.FromFile(@"..\..\images\" + abilities[0].imagePath);
            detailAbilityBox2.Image = Image.FromFile(@"..\..\images\" + abilities[1].imagePath);
            detailAbilityBox3.Image = Image.FromFile(@"..\..\images\" + abilities[2].imagePath);
            detailAbilityBox4.Image = Image.FromFile(@"..\..\images\" + abilities[3].imagePath);
        }
        public void showMoveDetails(List<Ability> abilities, int slot)
        {

            detailText.Text = abilities[slot - 1].description;
            showEnergy(abilities[slot - 1], energyDisplay);
            cooldownText.Text = "CD : " + abilities[slot - 1].cooldown.ToString();

        }
        public bool checkDuplicates(Character[] team, Character c)
        {
            bool result = true;

            for (int i = 0; i < team.Length; i++)
            {

                if (team[i] == c)
                {
                    result = false;
                }

            }

            return result;

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            


            energyDisplay[0] = energyBox1;
            energyDisplay[1] = energyBox2;
            energyDisplay[2] = energyBox3;
            energyDisplay[3] = energyBox4;
            energyDisplay[4] = energyBox5;

            selectedDisplay[0] = selectedBox1;
            selectedDisplay[1] = selectedBox2;
            selectedDisplay[2] = selectedBox3;

            removeDisplay[0] = removeButton1;
            removeDisplay[1] = removeButton2;
            removeDisplay[2] = removeButton3;

            narutoBox.Image = Image.FromFile(@"..\..\images\" + naruto.profileImagePath);
            ichigoBox.Image = Image.FromFile(@"..\..\images\" + ichigo.profileImagePath);
            midoriyaBox.Image = Image.FromFile(@"..\..\images\" + midoriya.profileImagePath);
            natsuBox.Image = Image.FromFile(@"..\..\images\" + natsu.profileImagePath);
            misakaBox.Image = Image.FromFile(@"..\..\images\" + misaka.profileImagePath);
            tatsumiBox.Image = Image.FromFile(@"..\..\images\" + tatsumi.profileImagePath);
            saberBox.Image = Image.FromFile(@"..\..\images\" + saber.profileImagePath);
            tsunayoshiBox.Image = Image.FromFile(@"..\..\images\" + tsunayoshi.profileImagePath);
            snowwhiteBox.Image = Image.FromFile(@"..\..\images\" + snowwhite.profileImagePath);




        }

        private void narutoBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "naruto";
                showDetails(naruto.description, naruto.profileImagePath, naruto.abilities);
            }
            if (pageNum == 2)
            {
                selected = "kuroko";
                showDetails(kuroko.description, kuroko.profileImagePath, kuroko.abilities);
            }

        }

        private void detailAbilityBox1_Click(object sender, EventArgs e)
        {

            switch (selected)
            {

                case "naruto":
                    showMoveDetails(naruto.abilities, 1);
                    break;
                case "ichigo":
                    showMoveDetails(ichigo.abilities, 1);
                    break;
                case "midoriya":
                    showMoveDetails(midoriya.abilities, 1);
                    break;
                case "natsu":
                    showMoveDetails(natsu.abilities, 1);
                    break;
                case "tatsumi":
                    showMoveDetails(tatsumi.abilities, 1);
                    break;
                case "misaka":
                    showMoveDetails(misaka.abilities, 1);
                    break;
                case "saber":
                    showMoveDetails(saber.abilities, 1);
                    break;
                case "snowwhite":
                    showMoveDetails(snowwhite.abilities, 1);
                    break;
                case "tsunayoshi":
                    showMoveDetails(tsunayoshi.abilities, 1);
                    break;
                case "kuroko":
                    showMoveDetails(kuroko.abilities, 1);
                    break;
                case "toga":
                    showMoveDetails(toga.abilities, 1);
                    break;
                case "chachamaru":
                    showMoveDetails(chachamaru.abilities, 1);
                    break;
                case "itachi":
                    showMoveDetails(itachi.abilities, 1);
                    break;
                case "mirio":
                    showMoveDetails(mirio.abilities, 1);
                    break;
                case "gunha":
                    showMoveDetails(gunha.abilities, 1);
                    break;
                case "gray":
                    showMoveDetails(gray.abilities, 1);
                    break;
                case "minato":
                    showMoveDetails(minato.abilities, 1);
                    break;
            }
                
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void detailAbilityBox2_Click(object sender, EventArgs e)
        {
            switch (selected)
            {

                case "naruto":

                    showMoveDetails(naruto.abilities, 2);
                    break;
                case "ichigo":
                    showMoveDetails(ichigo.abilities, 2);
                    break;
                case "midoriya":
                    showMoveDetails(midoriya.abilities, 2);
                    break;
                case "natsu":
                    showMoveDetails(natsu.abilities, 2);
                    break;
                case "tatsumi":
                    showMoveDetails(tatsumi.abilities, 2);
                    break;
                case "misaka":
                    showMoveDetails(misaka.abilities, 2);
                    break;
                case "saber":
                    showMoveDetails(saber.abilities, 2);
                    break;
                case "snowwhite":
                    showMoveDetails(snowwhite.abilities, 2);
                    break;
                case "tsunayoshi":
                    showMoveDetails(tsunayoshi.abilities, 2);
                    break;
                case "kuroko":
                    showMoveDetails(kuroko.abilities, 2);
                    break;
                case "toga":
                    showMoveDetails(toga.abilities, 2);
                    break;
                case "chachamaru":
                    showMoveDetails(chachamaru.abilities, 2);
                    break;
                case "itachi":
                    showMoveDetails(itachi.abilities, 2);
                    break;
                case "mirio":
                    showMoveDetails(mirio.abilities, 2);
                    break;
                case "gunha":
                    showMoveDetails(gunha.abilities, 2);
                    break;
                case "gray":
                    showMoveDetails(gray.abilities, 2);
                    break;
                case "minato":
                    showMoveDetails(minato.abilities, 2);
                    break;
            }
        }
        
        private void detailAbilityBox3_Click(object sender, EventArgs e)
        {
            switch (selected)
            {

                case "naruto":
                    showMoveDetails(naruto.abilities, 3);
                    break;
                case "ichigo":
                    showMoveDetails(ichigo.abilities, 3);
                    break;
                case "midoriya":
                    showMoveDetails(midoriya.abilities, 3);
                    break;
                case "natsu":
                    showMoveDetails(natsu.abilities, 3);
                    break;
                case "tatsumi":
                    showMoveDetails(tatsumi.abilities, 3);
                    break;
                case "misaka":
                    showMoveDetails(misaka.abilities, 3);
                    break;
                case "saber":
                    showMoveDetails(saber.abilities, 3);
                    break;
                case "snowwhite":
                    showMoveDetails(snowwhite.abilities, 3);
                    break;
                case "tsunayoshi":
                    showMoveDetails(tsunayoshi.abilities, 3);
                    break;
                case "kuroko":
                    showMoveDetails(kuroko.abilities, 3);
                    break;
                case "toga":
                    showMoveDetails(toga.abilities, 3);
                    break;
                case "chachamaru":
                    showMoveDetails(chachamaru.abilities, 3);
                    break;
                case "itachi":
                    showMoveDetails(itachi.abilities, 3);
                    break;
                case "mirio":
                    showMoveDetails(mirio.abilities, 3);
                    break;
                case "gunha":
                    showMoveDetails(gunha.abilities, 3);
                    break;
                case "gray":
                    showMoveDetails(gray.abilities, 3);
                    break;
                case "minato":
                    showMoveDetails(minato.abilities, 3);
                    break;
            }
        }

        private void detailAbilityBox4_Click(object sender, EventArgs e)
        {
            switch (selected)
            {

                case "naruto":
                    showMoveDetails(naruto.abilities, 4);
                    break;
                case "ichigo":
                    showMoveDetails(ichigo.abilities, 4);
                    break;
                case "midoriya":
                    showMoveDetails(midoriya.abilities, 4);
                    break;
                case "natsu":
                    showMoveDetails(natsu.abilities, 4);
                    break;
                case "tatsumi":
                    showMoveDetails(tatsumi.abilities, 4);
                    break;
                case "misaka":
                    showMoveDetails(misaka.abilities, 4);
                    break;
                case "saber":
                    showMoveDetails(saber.abilities, 4);
                    break;
                case "snowwhite":
                    showMoveDetails(snowwhite.abilities, 4);
                    break;
                case "tsunayoshi":
                    showMoveDetails(tsunayoshi.abilities, 4);
                    break;
                case "kuroko":
                    showMoveDetails(kuroko.abilities, 4);
                    break;
                case "toga":
                    showMoveDetails(toga.abilities, 4);
                    break;
                case "chachamaru":
                    showMoveDetails(chachamaru.abilities, 4);
                    break;
                case "itachi":
                    showMoveDetails(itachi.abilities, 4);
                    break;
                case "mirio":
                    showMoveDetails(mirio.abilities, 4);
                    break;
                case "gunha":
                    showMoveDetails(gunha.abilities, 4);
                    break;
                case "gray":
                    showMoveDetails(gray.abilities, 4);
                    break;
                case "minato":
                    showMoveDetails(minato.abilities, 4);
                    break;
            }
        }

        private void ichigoBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "ichigo";
                showDetails(ichigo.description, ichigo.profileImagePath, ichigo.abilities);
            }
            if (pageNum == 2)
            {
                selected = "toga";
                showDetails(toga.description, toga.profileImagePath, toga.abilities);
            }
        }
        private void midoriyaBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "midoriya";
                showDetails(midoriya.description, midoriya.profileImagePath, midoriya.abilities);
            }
            if (pageNum == 2)
            {
                selected = "chachamaru";
                showDetails(chachamaru.description, chachamaru.profileImagePath, chachamaru.abilities);
            }
        }
        private void natsuBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "natsu";
                showDetails(natsu.description, natsu.profileImagePath, natsu.abilities);
            }
            if (pageNum == 2)
            {
                selected = "itachi";
                showDetails(itachi.description, itachi.profileImagePath, itachi.abilities);
            }
        }
        private void saberBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "saber";
                showDetails(saber.description, saber.profileImagePath, saber.abilities);
            }
            if (pageNum == 2)
            {
                selected = "gunha";
                showDetails(gunha.description, gunha.profileImagePath, gunha.abilities);
            }
            
        }

        private void snowwhiteBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "snowwhite";
                showDetails(snowwhite.description, snowwhite.profileImagePath, snowwhite.abilities);
            }
            if (pageNum == 2)
            {
                selected = "gray";
                showDetails(gray.description, gray.profileImagePath, gray.abilities);
            }
        }
        private void misakaBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "misaka";
                showDetails(misaka.description, misaka.profileImagePath, misaka.abilities);
            }
            if (pageNum == 2)
            {
                selected = "mirio";
                showDetails(mirio.description, mirio.profileImagePath, mirio.abilities);
            }
        }
        private void tsunayoshiBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            if (pageNum == 1)
            {
                selected = "tsunayoshi";
                showDetails(tsunayoshi.description, tsunayoshi.profileImagePath, tsunayoshi.abilities);
            }
            if (pageNum == 2)
            {
                selected = "minato";
                showDetails(minato.description, minato.profileImagePath, minato.abilities);
            }
        }
        private void tatsumiBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < energyDisplay.Length; i++)
            {

                energyDisplay[i].Visible = false;

            }
            showDetailMenu();
            selected = "tatsumi";
            showDetails(tatsumi.description, tatsumi.profileImagePath, tatsumi.abilities);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            
            if (team[0] == null)
            {
                switch (selected)
                {
                    case "naruto":
                        if (checkDuplicates(team, naruto))
                        {
                            team[0] = naruto;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "ichigo":
                        if (checkDuplicates(team, ichigo))
                        {
                            team[0] = ichigo;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "midoriya":
                        if (checkDuplicates(team, midoriya))
                        {
                            team[0] = midoriya;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "tatsumi":
                        if (checkDuplicates(team, tatsumi))
                        {
                            team[0] = tatsumi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "misaka":
                        if (checkDuplicates(team, misaka))
                        {
                            team[0] = misaka;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "saber":
                        if (checkDuplicates(team, saber))
                        {
                            team[0] = saber;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "snowwhite":
                        if (checkDuplicates(team, snowwhite))
                        {
                            team[0] = snowwhite;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "tsunayoshi":
                        if (checkDuplicates(team, tsunayoshi))
                        {
                            team[0] = tsunayoshi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "natsu":
                        if (checkDuplicates(team, natsu))
                        {
                            team[0] = natsu;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "kuroko":
                        if (checkDuplicates(team, kuroko))
                        {
                            team[0] = kuroko;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "toga":
                        if (checkDuplicates(team, toga))
                        {
                            team[0] = toga;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "chachamaru":
                        if (checkDuplicates(team, chachamaru))
                        {
                            team[0] = chachamaru;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "itachi":
                        if (checkDuplicates(team, itachi))
                        {
                            team[0] = itachi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "mirio":
                        if(checkDuplicates(team, mirio))
                        {
                            team[0] = mirio;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "gunha":
                        if(checkDuplicates(team, gunha))
                        {
                            team[0] = gunha;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "gray":
                        if(checkDuplicates(team, gray))
                        {
                            team[0] = gray;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "minato":
                        if(checkDuplicates(team, minato))
                        {
                            team[0] = minato;
                            break;
                        }
                        else
                        {
                            break;
                        }
                }
                if (team[0] != null)
                {
                    selectedBox1.Visible = true;
                    selectedBox1.Image = Image.FromFile(@"..\..\images\" + team[0].profileImagePath);
                    removeButton1.Visible = true;
                }
            }
            else if (team[0] != null && team[1] == null)
            {
                switch (selected)
                {
                    case "naruto":
                        if (checkDuplicates(team, naruto))
                        {
                            team[1] = naruto;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "ichigo":
                        if (checkDuplicates(team, ichigo))
                        {
                            team[1] = ichigo;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "midoriya":
                        if (checkDuplicates(team, midoriya))
                        {
                            team[1] = midoriya;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "tatsumi":
                        if (checkDuplicates(team, tatsumi))
                        {
                            team[1] = tatsumi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "misaka":
                        if (checkDuplicates(team, misaka))
                        {
                            team[1] = misaka;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "saber":
                        if (checkDuplicates(team, saber))
                        {
                            team[1] = saber;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "snowwhite":
                        if (checkDuplicates(team, snowwhite))
                        {
                            team[1] = snowwhite;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "tsunayoshi":
                        if (checkDuplicates(team, tsunayoshi))
                        {
                            team[1] = tsunayoshi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "natsu":
                        if (checkDuplicates(team, natsu))
                        {
                            team[1] = natsu;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "kuroko":
                        if (checkDuplicates(team, kuroko))
                        {
                            team[1] = kuroko;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "toga":
                        if (checkDuplicates(team, toga))
                        {
                            team[1] = toga;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "chachamaru":
                        if (checkDuplicates(team, chachamaru))
                        {
                            team[1] = chachamaru;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "itachi":
                        if (checkDuplicates(team, itachi))
                        {
                            team[1] = itachi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "mirio":
                        if (checkDuplicates(team, mirio))
                        {
                            team[1] = mirio;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "gunha":
                        if (checkDuplicates(team, gunha))
                        {
                            team[1] = gunha;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "gray":
                        if (checkDuplicates(team, gray))
                        {
                            team[1] = gray;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "minato":
                        if (checkDuplicates(team, minato))
                        {
                            team[1] = minato;
                            break;
                        }
                        else
                        {
                            break;
                        }
                }
                if (team[1] != null)
                {
                    selectedBox2.Visible = true;
                    selectedBox2.Image = Image.FromFile(@"..\..\images\" + team[1].profileImagePath);
                    removeButton2.Visible = true;
                }
            }
            else if(team[2] == null)
            {
                switch (selected)
                {
                    case "naruto":
                        if (checkDuplicates(team, naruto))
                        {
                            team[2] = naruto;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "ichigo":
                        if (checkDuplicates(team, ichigo))
                        {
                            team[2] = ichigo;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "midoriya":
                        if (checkDuplicates(team, midoriya))
                        {
                            team[2] = midoriya;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "tatsumi":
                        if (checkDuplicates(team, tatsumi))
                        {
                            team[2] = tatsumi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "misaka":
                        if (checkDuplicates(team, misaka))
                        {
                            team[2] = misaka;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "saber":
                        if (checkDuplicates(team, saber))
                        {
                            team[2] = saber;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "snowwhite":
                        if (checkDuplicates(team, snowwhite))
                        {
                            team[2] = snowwhite;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "tsunayoshi":
                        if (checkDuplicates(team, tsunayoshi))
                        {
                            team[2] = tsunayoshi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "natsu":
                        if (checkDuplicates(team, natsu))
                        {
                            team[2] = natsu;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "kuroko":
                        if (checkDuplicates(team, kuroko))
                        {
                            team[2] = kuroko;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "toga":
                        if (checkDuplicates(team, toga))
                        {
                            team[2] = toga;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "chachamaru":
                        if (checkDuplicates(team, chachamaru))
                        {
                            team[2] = chachamaru;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "itachi":
                        if (checkDuplicates(team, itachi))
                        {
                            team[2] = itachi;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "mirio":
                        if (checkDuplicates(team, mirio))
                        {
                            team[2] = mirio;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "gunha":
                        if (checkDuplicates(team, gunha))
                        {
                            team[2] = gunha;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "gray":
                        if (checkDuplicates(team, gray))
                        {
                            team[2] = gray;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "minato":
                        if (checkDuplicates(team, minato))
                        {
                            team[2] = minato;
                            break;
                        }
                        else
                        {
                            break;
                        }
                }
                if (team[2] != null)
                {
                    selectedBox3.Visible = true;
                    selectedBox3.Image = Image.FromFile(@"..\..\images\" + team[2].profileImagePath);
                    removeButton3.Visible = true;
                    startButton.Visible = true;
                }
            }
            
        }

        private void removeButton1_Click(object sender, EventArgs e)
        {
            removeSelected(team, 1);
            startButton.Visible = false;
            for (int i = 0; i < team.Length; i++)
            {
                if (team[i] != null)
                {
                    selectedDisplay[i].Image = Image.FromFile(@"..\..\images\" + team[i].profileImagePath);
                }
                else
                {
                    selectedDisplay[i].Visible = false;
                    removeDisplay[i].Visible = false;
                }
            }
        }

        private void removeButton2_Click(object sender, EventArgs e)
        {
            removeSelected(team, 2);
            startButton.Visible = false;
            for (int i = 0; i < team.Length; i++)
            {
                if (team[i] != null)
                {
                    selectedDisplay[i].Image = Image.FromFile(@"..\..\images\" + team[i].profileImagePath);
                }
                else
                {
                    selectedDisplay[i].Visible = false;
                    removeDisplay[i].Visible = false;
                }
            }
        }

        private void removeButton3_Click(object sender, EventArgs e)
        {
            removeSelected(team, 3);
            startButton.Visible = false;
            for (int i = 0; i < team.Length; i++)
            {
                if (team[i] != null)
                {
                    selectedDisplay[i].Image = Image.FromFile(@"..\..\images\" + team[i].profileImagePath);
                }
                else
                {
                    selectedDisplay[i].Visible = false;
                    removeDisplay[i].Visible = false;
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            
            string serialString ="";
            if (isHost == false)
            {
                TcpClient connectOut = new TcpClient();
                var result = connectOut.BeginConnect(ipAddress.Text, 5692, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(20));
                if (!success)
                {
                    throw new Exception("Failed to connect.");
                }
                for (int i = 0; i < 3; i++)
                {
                    if (team[i] == naruto)
                    {
                        serialString += "0";
                    }
                    else if (team[i] == ichigo)
                    {
                        serialString += "1";
                    }
                    else if (team[i] == misaka)
                    {
                        serialString += "2";
                    }
                    else if (team[i] == saber)
                    {
                        serialString += "3";
                    }
                    else if (team[i] == tatsumi)
                    {
                        serialString += "4";
                    }
                    else if (team[i] == tsunayoshi)
                    {
                        serialString += "5";
                    }
                    else if (team[i] == midoriya)
                    {
                        serialString += "6";
                    }
                    else if (team[i] == snowwhite)
                    {
                        serialString += "7";
                    }
                    else if (team[i] == natsu)
                    {
                        serialString += "8";
                    }
                    else if (team[i] == kuroko)
                    {
                        serialString += "9";
                    }
                    else if (team[i] == toga)
                    {
                        serialString += "10";
                    }
                    else if (team[i] == chachamaru)
                    {
                        serialString += "11";
                    }
                    else if (team[i] == itachi)
                    {
                        serialString += "12";
                    }
                    else if (team[i] == mirio)
                    {
                        serialString += "13";
                    }
                    else if (team[i] == gunha)
                    {
                        serialString += "14";
                    }
                    else if (team[i] == gray)
                    {
                        serialString += "15";
                    }
                    else if (team[i] == minato)
                    {
                        serialString += "16";
                    }
                    if (i != 2)
                    {
                        serialString += "|";
                    }
                }



                Stream stm = connectOut.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(serialString);
                stm.Write(ba, 0, ba.Length);

                serialString = "";
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                for (int i = 0; i < k; i++)
                {
                    serialString += Convert.ToChar(bb[i]);
                }

                string[] teamCode = new string[3];
                teamCode = serialString.Split('|');

                for (int i = 0; i < 3; i++)
                {
                    switch (teamCode[i])
                    {
                        case "0":
                            enemyTeam[i] = new Naruto();
                            break;
                        case "1":
                            enemyTeam[i] = new Ichigo();
                            break;
                        case "2":
                            enemyTeam[i] = new Misaka();
                            break;
                        case "3":
                            enemyTeam[i] = new Saber();
                            break;
                        case "4":
                            enemyTeam[i] = new Tatsumi();
                            break;
                        case "5":
                            enemyTeam[i] = new Tsunayoshi();
                            break;
                        case "6":
                            enemyTeam[i] = new Midoriya();
                            break;
                        case "7":
                            enemyTeam[i] = new SnowWhite();
                            break;
                        case "8":
                            enemyTeam[i] = new Natsu();
                            break;
                        case "9":
                            enemyTeam[i] = new Kuroko();
                            break;
                        case "10":
                            enemyTeam[i] = new Toga();
                            break;
                        case "11":
                            enemyTeam[i] = new Chachamaru();
                            break;
                        case "12":
                            enemyTeam[i] = new Itachi();
                            break;
                        case "13":
                            enemyTeam[i] = new Mirio();
                            break;
                        case "14":
                            enemyTeam[i] = new Gunha();
                            break;
                        case "15":
                            enemyTeam[i] = new Gray();
                            break;
                        case "16":
                            enemyTeam[i] = new Minato();
                            break;
                    }
                }
                connectOut.Close();
            }
            
            if (isHost == true)
            {
                TcpListener connectIn = new TcpListener(IPAddress.Any, 5692);
                connectIn.Start();
                Socket s = connectIn.AcceptSocket();

                byte[] ba = new byte[100];
                int k = s.Receive(ba);
                for (int i = 0; i < k; i++)
                {
                    serialString += Convert.ToChar(ba[i]);
                }

                string[] teamCode = new string[3];
                teamCode = serialString.Split('|');

                for (int i = 0; i < 3; i++)
                {
                    switch (teamCode[i])
                    {
                        case "0":
                            enemyTeam[i] = new Naruto();
                            break;
                        case "1":
                            enemyTeam[i] = new Ichigo();
                            break;
                        case "2":
                            enemyTeam[i] = new Misaka();
                            break;
                        case "3":
                            enemyTeam[i] = new Saber();
                            break;
                        case "4":
                            enemyTeam[i] = new Tatsumi();
                            break;
                        case "5":
                            enemyTeam[i] = new Tsunayoshi();
                            break;
                        case "6":
                            enemyTeam[i] = new Midoriya();
                            break;
                        case "7":
                            enemyTeam[i] = new SnowWhite();
                            break;
                        case "8":
                            enemyTeam[i] = new Natsu();
                            break;
                        case "9":
                            enemyTeam[i] = new Kuroko();
                            break;
                        case "10":
                            enemyTeam[i] = new Toga();
                            break;
                        case "11":
                            enemyTeam[i] = new Chachamaru();
                            break;
                        case "12":
                            enemyTeam[i] = new Itachi();
                            break;
                        case "13":
                            enemyTeam[i] = new Mirio();
                            break;
                        case "14":
                            enemyTeam[i] = new Gunha();
                            break;
                        case "15":
                            enemyTeam[i] = new Gray();
                            break;
                        case "16":
                            enemyTeam[i] = new Minato();
                            break;
                    }
                }

                serialString = "";

                for (int i = 0; i < 3; i++)
                {
                    if (team[i] == naruto)
                    {
                        serialString += "0";
                    }
                    else if (team[i] == ichigo)
                    {
                        serialString += "1";
                    }
                    else if (team[i] == misaka)
                    {
                        serialString += "2";
                    }
                    else if (team[i] == saber)
                    {
                        serialString += "3";
                    }
                    else if (team[i] == tatsumi)
                    {
                        serialString += "4";
                    }
                    else if (team[i] == tsunayoshi)
                    {
                        serialString += "5";
                    }
                    else if (team[i] == midoriya)
                    {
                        serialString += "6";
                    }
                    else if (team[i] == snowwhite)
                    {
                        serialString += "7";
                    }
                    else if (team[i] == natsu)
                    {
                        serialString += "8";
                    }
                    else if (team[i] == kuroko)
                    {
                        serialString += "9";
                    }
                    else if (team[i] == toga)
                    {
                        serialString += "10";
                    }
                    else if(team[i] == chachamaru)
                    {
                        serialString += "11";
                    }
                    else if (team[i] == itachi)
                    {
                        serialString += "12";
                    }
                    else if (team[i] == mirio)
                    {
                        serialString += "13";
                    }
                    else if (team[i] == gunha)
                    {
                        serialString += "14";
                    }
                    else if (team[i] == gray)
                    {
                        serialString += "15";
                    }
                    else if (team[i] == minato)
                    {
                        serialString += "16";
                    }
                    if (i != 2)
                    {
                        serialString += "|";
                    }
                }

                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes(serialString));
                s.Close();
                connectIn.Stop();
            }
           
            Form2 frm = new Form2(team, enemyTeam, energy, enemyEnergy, isHost, ipAddress.Text);
            frm.Show();
            this.Visible = false;
        }

        private void detailBox_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                ipAddress.Visible = false;
                isHost = true;
            }
            else
            {
                ipAddress.Visible = true;
                isHost = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true){
                ipAddress.Visible = true;
                isHost = false;
            }
            else
            {
                ipAddress.Visible = true;
                isHost = false;
            }
        }

        private void rightArrow_Click(object sender, EventArgs e)
        {
            pageNum += 1;
            if (pageNum == 2)
            {
                leftArrow.Visible = true;
                rightArrow.Visible = false;
                narutoBox.Image = Image.FromFile(@"..\..\images\" + kuroko.profileImagePath);
                ichigoBox.Image = Image.FromFile(@"..\..\images\" + toga.profileImagePath);
                midoriyaBox.Image = Image.FromFile(@"..\..\images\" + chachamaru.profileImagePath);
                natsuBox.Image = Image.FromFile(@"..\..\images\" + itachi.profileImagePath);
                misakaBox.Image = Image.FromFile(@"..\..\images\" + mirio.profileImagePath);
                tatsumiBox.Image = Image.FromFile(@"..\..\images\unknown.png");
                saberBox.Image = Image.FromFile(@"..\..\images\" + gunha.profileImagePath);
                tsunayoshiBox.Image = Image.FromFile(@"..\..\images\" + minato.profileImagePath);
                snowwhiteBox.Image = Image.FromFile(@"..\..\images\" + gray.profileImagePath);
            }
        }

        private void leftArrow_Click(object sender, EventArgs e)
        {
            pageNum -= 1;
            if (pageNum == 1)
            {
                leftArrow.Visible = false;
                rightArrow.Visible = true;
                narutoBox.Image = Image.FromFile(@"..\..\images\" + naruto.profileImagePath);
                ichigoBox.Image = Image.FromFile(@"..\..\images\" + ichigo.profileImagePath);
                midoriyaBox.Image = Image.FromFile(@"..\..\images\" + midoriya.profileImagePath);
                natsuBox.Image = Image.FromFile(@"..\..\images\" + natsu.profileImagePath);
                misakaBox.Image = Image.FromFile(@"..\..\images\" + misaka.profileImagePath);
                tatsumiBox.Image = Image.FromFile(@"..\..\images\" + tatsumi.profileImagePath);
                saberBox.Image = Image.FromFile(@"..\..\images\" + saber.profileImagePath);
                tsunayoshiBox.Image = Image.FromFile(@"..\..\images\" + tsunayoshi.profileImagePath);
                snowwhiteBox.Image = Image.FromFile(@"..\..\images\" + snowwhite.profileImagePath);
            }
        }
    }
}
