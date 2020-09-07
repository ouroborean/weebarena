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

namespace AnimeArena
{
    public partial class Form3 : Form
    {
        public int phys, offeredPhys=0;
        public int spec, offeredSpec=0;
        public int ment, offeredMent=0;
        public int wep, offeredWep=0;
        public int randNeeded;
        public int[] energy = new int[5];

        private void physAdd_Click(object sender, EventArgs e)
        {
            if (phys > 0 && randNeeded > 0)
            {
                phys -= 1;
                offeredPhys += 1;
                randNeeded -= 1;
            }
            physHeld.Text = phys.ToString();
            physOffered.Text = offeredPhys.ToString();
            randomNeeded.Text = randNeeded.ToString();

            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }

        private void specAdd_Click(object sender, EventArgs e)
        {
            if (spec > 0 && randNeeded > 0)
            {
                spec -= 1;
                offeredSpec += 1;
                randNeeded -= 1;
            }
            specHeld.Text = spec.ToString();
            specOffered.Text = offeredSpec.ToString();
            randomNeeded.Text = randNeeded.ToString();
            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }

        private void mentAdd_Click(object sender, EventArgs e)
        {
            if (ment > 0 && randNeeded > 0)
            {
                ment -= 1;
                offeredMent += 1;
                randNeeded -= 1;
            }
            mentHeld.Text = ment.ToString();
            mentOffered.Text = offeredMent.ToString();
            randomNeeded.Text = randNeeded.ToString();
            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }

        private void wepAdd_Click(object sender, EventArgs e)
        {
            if (wep > 0 && randNeeded > 0)
            {
                wep -= 1;
                offeredWep += 1;
                randNeeded -= 1;
            }
            wepHeld.Text = wep.ToString();
            wepOffered.Text = offeredWep.ToString();
            randomNeeded.Text = randNeeded.ToString();
            if (randNeeded == 0)
            {
                turnEndButton.Enabled = true;
            }
        }

        private void physSub_Click(object sender, EventArgs e)
        {
            if (offeredPhys > 0)
            {
                phys += 1;
                offeredPhys -= 1;
                randNeeded += 1;
            }
            physHeld.Text = phys.ToString();
            physOffered.Text = offeredPhys.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }

        private void specSub_Click(object sender, EventArgs e)
        {
            if (offeredSpec > 0)
            {
                phys += 1;
                offeredSpec -= 1;
                randNeeded += 1;
            }
            specHeld.Text = spec.ToString();
            specOffered.Text = offeredSpec.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }

        private void mentSub_Click(object sender, EventArgs e)
        {
            if (offeredMent > 0)
            {
                ment += 1;
                offeredMent -= 1;
                randNeeded += 1;
            }
            mentHeld.Text = ment.ToString();
            mentOffered.Text = offeredMent.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }
        public void refresh(Character c)
        {

            c.targeting = false;
            c.targetable = false;
            c.targeted = false;
            c.currentAbility = null;
            c.chosenAbilitySlot = 7;
            c.firstTargeted = null;
            c.secondTargeted = null;
            c.thirdTargeted = null;
            c.potentialTargets.Clear();
        }
        private void wepSub_Click(object sender, EventArgs e)
        {
            if (offeredWep > 0)
            {
                wep += 1;
                offeredWep -= 1;
                randNeeded += 1;
            }
            wepHeld.Text = wep.ToString();
            wepOffered.Text = offeredWep.ToString();
            randomNeeded.Text = randNeeded.ToString();
        }

        private void turnEndButton_Click(object sender, EventArgs e)
        {
            energy[0] = phys;
            energy[1] = spec;
            energy[2] = ment;
            energy[3] = wep;

            for(int i = 0; i < 3; i++)
            {
                refresh(pTeam[i]);
                refresh(eTeam[i]);
            }

            Form2 frm = new Form2(pTeam, eTeam, energy, energy, false, null);
            frm.Show();
            this.Close();
        }

        Character[] pTeam = new Character[3];
        Character[] eTeam = new Character[3];
        public Form3(Character[] team, Character[] enemyTeam, int randCost, int[] energy)
        {
            InitializeComponent();
            randNeeded = randCost;
            phys = energy[0];
            spec = energy[1];
            ment = energy[2];
            wep = energy[3];
            pTeam = team;
            eTeam = enemyTeam;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            randomNeeded.Text = randNeeded.ToString();
            physHeld.Text = phys.ToString();
            specHeld.Text = spec.ToString();
            mentHeld.Text = ment.ToString();
            wepHeld.Text = wep.ToString();
        }
    }
}
