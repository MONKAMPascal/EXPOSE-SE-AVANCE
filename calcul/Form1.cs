using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using PdfSharp.Charting;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms.VisualStyles;

namespace calcul
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lbl_hote.Visible = false;
            txtb_hote.Visible = false;
            lbl_erreur.Visible = false;
            grp_fslm.Visible = false;
            grp_vlsm.Visible = false;
            txtb_ht_par_sr.ReadOnly = true;


        }
        //variables associées à la valeur de l'adressi ip recupérées dans les textbox
        int ad_ip1;
        int ad_ip2;
        int ad_ip3;
        int ad_ip4;

        //variables associées à la valeur du nombre de sous réseaux voulu
        int nb_sr;
        double nbd_sr;
        int compt_sr;

        //variable associées à la valeur du nombre de machines hôtes voulus 
        int nb_hote;

        //variable associées au nombre de bit necessaire pour la partie réseau
        double n_bit;
        //Les sous reseau disponible pour le vlsm
        int nbsr_vlsm;
        int i;
        int j;
        int p;
        int derniere;
        int nbrhote_vlsm;
        int cmpt_hote_vlsm;
        int debut;
        //TEST POUR LE VLSM****************************************************************************************************************************

        private void compteur_plan_sr()
        {
            compt_sr = 0;
            txtb_plan_sr.Text = txtb_plan_sr.Text + Environment.NewLine;
            for (ad_ip4 = 0; (ad_ip4 < 256 && compt_sr <= nb_sr); ad_ip4++)
            {
                if (ad_ip4 % (nb_hote + 2) == 0)
                {
                    compt_sr = compt_sr + 1;
                    txtb_plan_sr.Text = txtb_plan_sr.Text + Environment.NewLine + "sous réseau " + compt_sr + Environment.NewLine;
                }
                //  txtb_plan_sr.Text = txtb_plan_sr.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + ad_ip4.ToString();
            }


        }
        //pour genrer uniquement les hote disponible*****************************************************************************
        private void compteur_plan_hote()
        {
            compt_sr = 0;
            txtb_plan_hote.Text = txtb_plan_hote.Text + Environment.NewLine;
            int counter = 0;
            for (int ad_ip4 = 0; (ad_ip4 < 256 && compt_sr <= nb_sr); ad_ip4++)
            {
                if (ad_ip4 % (nb_hote + 2) == 0)
                {
                    compt_sr = compt_sr + 1;
                    counter = 0; // Réinitialiser le compteur à
                }
                string[] ipParts = (ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + ad_ip4.ToString()).Split('.');

                if (counter == 1) // Afficher uniquement la deuxième
                {
                    txtb_plan_hote.Text = txtb_plan_hote.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + ipParts[ipParts.Length - 1] + Environment.NewLine;
                }

                counter++;
            }

        }
        private void compteur_plan_hote1()
        {
            compt_sr = 0;
            txtb_plan_hote1.Text = txtb_plan_hote1.Text + Environment.NewLine;
            int lastValue = 0;
            int previousValue = 0;

            for (ad_ip4 = 0; (ad_ip4 < 256 && compt_sr <= nb_sr); ad_ip4++)
            {
                if ((ad_ip4 % (nb_hote + 2) == 0) && (lastValue != 0))
                {
                    txtb_plan_hote1.Text = txtb_plan_hote1.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + lastValue.ToString() + Environment.NewLine;
                    lastValue = 0;
                }
                else if (ad_ip4 % (nb_hote + 2) != 0 && ad_ip4 != 0)
                {
                    lastValue = previousValue;
                    previousValue = ad_ip4;
                }
            }

            if (lastValue != 0)
            {
                txtb_plan_hote1.Text = txtb_plan_hote1.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + lastValue.ToString() + Environment.NewLine;
            }
        }
        //pour genrer uniquement les passerel********************************************************************
        private void compteur_plan_pas()
        {
            compt_sr = 0;
            txtb_plan_pas.Text = txtb_plan_pas.Text + Environment.NewLine;
            for (ad_ip4 = 0; (ad_ip4 < 256 && compt_sr <= nb_sr); ad_ip4++)
            {
                if (ad_ip4 % (nb_hote + 2) != 0 && ad_ip4 != 0)
                {
                    continue; // Ignorer le reste des valeurs d'itération
                }

                txtb_plan_pas.Text = txtb_plan_pas.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + ad_ip4.ToString() + Environment.NewLine;
            }
        }
        //pour generer uniquement les diffusion*************************************************************=******************************
        private void compteur_plan_dif()
        {
            compt_sr = 0;
            txtb_plan_dif.Text = txtb_plan_dif.Text + Environment.NewLine;
            int lastValue = 0;

            for (ad_ip4 = 0; (ad_ip4 < 256 && compt_sr <= nb_sr); ad_ip4++)
            {
                if ((ad_ip4 % (nb_hote + 2) == 0) && (lastValue != 0))
                {
                    txtb_plan_dif.Text = txtb_plan_dif.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + lastValue.ToString() + Environment.NewLine;
                    lastValue = 0; // Réinitialiser la dernière valeur                  
                }
                else if (ad_ip4 % (nb_hote + 2) != 0 && ad_ip4 != 0)
                {
                    lastValue = ad_ip4; // Stocker la dernière valeur avant chaque "sous réseau compt_sr"
                }
            }

            if (lastValue != 0)
            {
                txtb_plan_dif.Text = txtb_plan_dif.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + lastValue.ToString() + Environment.NewLine;
            }


        }
        private void compteur_plan_cidr()
        {
            compt_sr = 0;
            txtb_cidr.Text = txtb_cidr.Text + Environment.NewLine;
            for (ad_ip4 = 0; (ad_ip4 < 256 && compt_sr <= nb_sr); ad_ip4++)
            {
                if (ad_ip4 % (nb_hote + 2) == 0)
                {
                    compt_sr = compt_sr + 1;
                    txtb_cidr.Text = txtb_cidr.Text + Environment.NewLine + " /24 " + Environment.NewLine;
                }
                //  txtb_plan_sr.Text = txtb_plan_sr.Text + Environment.NewLine + ad_ip1.ToString() + "." + ad_ip2.ToString() + "." + ad_ip3.ToString() + "." + ad_ip4.ToString();
            }
        }
        //pour gener le plan du vlsmuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu
        private void compteur_plan_vlsm()
        {
            if (int.Parse(txtb_ht_par_sr.Text) != 0 )
            {
                p = p + 1;
                nbrhote_vlsm = int.Parse(txtb_ht_par_sr.Text);
                cmpt_hote_vlsm = cmpt_hote_vlsm + nbrhote_vlsm + 2;
                txtb_plan_sr.Text = txtb_plan_sr.Text + Environment.NewLine + "sous réseau " + p.ToString() + Environment.NewLine;
                // Ne pas afficher les valeurs intermédiaires et afficher seulement la deuxième valeur
                for (j = debut; j <= cmpt_hote_vlsm - 1; j++)
                {
                    if (j == debut + 1)
                    {
                        txtb_plan_hote.Text = txtb_plan_hote.Text + Environment.NewLine + $"{ad_ip1}.{ad_ip2}.{ad_ip3}.{j}" + Environment.NewLine;
                        break; // Sortir de la boucle après avoir affiché la deuxième valeur
                    }
                }
                // Afficher uniquement la première valeur
                txtb_plan_pas.Text = txtb_plan_pas.Text + Environment.NewLine + $"{ad_ip1}.{ad_ip2}.{ad_ip3}.{debut}" + Environment.NewLine;

                // Ne pas afficher les valeurs intermédiaires et afficher seulement la dernière valeur
                for (j = debut; j <= cmpt_hote_vlsm - 1; j++)
                {
                    if (j == cmpt_hote_vlsm - 1)
                    {
                        txtb_plan_dif.Text = txtb_plan_dif.Text + Environment.NewLine + $"{ad_ip1}.{ad_ip2}.{ad_ip3}.{j}" + Environment.NewLine;
                    }
                }
                // Afficher uniquement l'avant-dernière valeur
                for (j = debut; j <= cmpt_hote_vlsm - 1; j++)
                {
                    if (j == cmpt_hote_vlsm - 2)
                    {
                        txtb_plan_hote1.Text = txtb_plan_hote1.Text + Environment.NewLine + $"{ad_ip1}.{ad_ip2}.{ad_ip3}.{j}" + Environment.NewLine;
                        break; // Sortir de la boucle après avoir affiché l'avant-dernière valeur
                    }
                }
                debut = cmpt_hote_vlsm;
                i++;
            }
            else
            {
                lbl_erreur.Visible = true;
                lbl_erreur.Text = "aucun sous reseau ne doit être vide";
                txtb_ht_par_sr.Text = "";
            }
        }

        //pour activer et desactiver la partie dedier a l'hote et le sous reseau***************************************************************
        private void btn_switch_Click(object sender, EventArgs e)
        {
            if (lbl_sr.Visible == true && txtb_sr.Visible == true)
            {
                lbl_hote.Visible = true;
                txtb_hote.Visible = true;
                lbl_sr.Visible = false;
                txtb_sr.Visible = false;
                btn_switch.Text = "Hote Actif";
            }
            else
            {
                lbl_hote.Visible = false;
                txtb_hote.Visible = false;
                lbl_sr.Visible = true;
                txtb_sr.Visible = true;
                btn_switch.Text = "Sous reseau Actif";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Message d'erreur pour le champ de l'adresse ip**********************************************************************µµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµ
            if (string.IsNullOrEmpty(txtb_ip1.Text) || string.IsNullOrEmpty(txtb_ip2.Text) || string.IsNullOrEmpty(txtb_ip3.Text) || string.IsNullOrEmpty(txtb_ip4.Text))
            {
                lbl_erreur.Visible = true;
                lbl_erreur.Text = "Veuillez remplir les champs dedier aux addresse IP!";
            }

            else if (!int.TryParse(txtb_ip1.Text, out int number) || !int.TryParse(txtb_ip2.Text, out int number1) || !int.TryParse(txtb_ip3.Text, out int number2) || !int.TryParse(txtb_ip4.Text, out int number3))
            {
                lbl_erreur.Visible = true;
                lbl_erreur.Text = "Veuillez entrer uniquement des nombre dans le champs dedier a l'adresse ip!";
            }
            else
            {
                ad_ip1 = int.Parse(txtb_ip1.Text);
                ad_ip2 = int.Parse(txtb_ip2.Text);
                ad_ip3 = int.Parse(txtb_ip3.Text);
                ad_ip4 = int.Parse(txtb_ip4.Text);
                if ((ad_ip1 < 192 || ad_ip1 > 223) || (ad_ip2 < 0 || ad_ip2 > 255) || (ad_ip3 < 0 || ad_ip3 > 255) || (ad_ip4 < 0 || ad_ip4 > 255))
                {
                    lbl_erreur.Visible = true;
                    lbl_erreur.Text = "Veuillez entrez un entier entre 192 et 223 pour le 1er octet et entre 0 et 255 pour les autres";
                    txtb_sr.Text = "";
                    txtb_hote.Text = "";

                }
                else if (txtb_sr.Visible == true && string.IsNullOrEmpty(txtb_sr.Text) || (txtb_hote.Visible == true && string.IsNullOrEmpty(txtb_hote.Text)))
                {
                    // message d'erreur pour le zone du nombre de sous réseau et le nombre d'hote*********************************************
                    lbl_erreur.Visible = true;
                    lbl_erreur.Text = "Veuillez remplir les champs dedier au sous reseau ou a l'hote";
                }
                else if (!int.TryParse(txtb_sr.Text, out int number4) && txtb_sr.Visible == true || !int.TryParse(txtb_hote.Text, out int number5) && txtb_hote.Visible == true)
                {

                    lbl_erreur.Visible = true;
                    lbl_erreur.Text = "Veuillez entrer uniquement des nombre dans le champ dedier a l'hote ou au sous reseau !";
                }
                else
                {
                    if (txtb_sr.Visible && lbl_sr.Visible == true)
                    {
                        lbl_erreur.Text = "";
                        nb_sr = int.Parse(txtb_sr.Text);
                        nb_hote = (256 / nb_sr) - 2;
                        lbl_hote.Visible = true;
                        txtb_hote.Visible = true;
                        nbd_sr = nb_sr;
                        n_bit = Math.Ceiling(Math.Log10(nbd_sr) / Math.Log10(2));
                        txtb_hote.Text = nb_hote.ToString();

                    }
                    else
                    {
                        if (txtb_hote.Visible && lbl_hote.Visible == true)
                        {
                            lbl_erreur.Text = "";
                            nb_hote = int.Parse(txtb_hote.Text);
                            nb_sr = 256 / (nb_hote + 2);
                            txtb_sr.Text = nb_sr.ToString();
                            lbl_sr.Visible = true;
                            txtb_sr.Visible = true;
                            txtb_sr.Text = nb_sr.ToString();
                        }
                    }
                    txtb_mk1.Text = 255.ToString();
                    txtb_mk2.Text = 255.ToString();
                    txtb_mk3.Text = 255.ToString();
                    txtb_mk4.Text = 0.ToString();
                    compteur_plan_sr();
                    compteur_plan_pas();
                    compteur_plan_hote();
                    compteur_plan_dif();
                    compteur_plan_hote1();
                    compteur_plan_cidr();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((txtb_plan_dif.Text) != "" || (txtb_plan_sr.Text) != "" || (txtb_plan_pas.Text) != "" || (txtb_plan_hote.Text) != "" || (txtb_plan_hote1.Text) != "")
            {
                txtb_plan_dif.Text = "";
                txtb_plan_sr.Text = "";
                txtb_plan_hote.Text = "";
                txtb_plan_pas.Text = "";
                txtb_plan_hote1.Text = "";
                txtb_cidr.Text = "";
            }
        }

        private void btn_eff_srht_Click(object sender, EventArgs e)
        {
            if ((txtb_hote.Text) != "" || (txtb_sr.Text) != "")
            {
                txtb_sr.Text = "";
                txtb_hote.Text = "";
                txtb_sr.Visible = true;
                lbl_sr.Visible = true;
                txtb_hote.Visible = false;
                lbl_hote.Visible = false;
            }
        }
        //pour activer la section FLSM µµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµ
        private void btn_flsm_Click(object sender, EventArgs e)
        {
            if (grp_fslm.Visible == false)
            {
                grp_fslm.Visible = true;
                btn_flsm.Text = "FLSM Actif";
            }
            else
            {
                grp_fslm.Visible = false;
                btn_flsm.Text = "FLSM Inactif";
            }
        }
        //POUR ACTVER LA SECTION VLSM µµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµ
        private void btn_vlsm_Click(object sender, EventArgs e)
        {
            if (grp_vlsm.Visible == false)
            {
                grp_vlsm.Visible = true;
                btn_vlsm.Text = "VLSM Actif";
            }
            else
            {
                grp_vlsm.Visible = false;
                btn_vlsm.Text = "VLSM Inactif";
            }
        }

        //CALCUL VLSM µµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµµ
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_ip1.Text) || string.IsNullOrEmpty(txtb_ip2.Text) || string.IsNullOrEmpty(txtb_ip3.Text) || string.IsNullOrEmpty(txtb_ip4.Text))
            {
                lbl_erreur.Visible = true;
                lbl_erreur.Text = "Veuillez remplir les champs dedier aux addresse IP!";
            }
            else if (txtb_nbsr_vlsm.Text == "" && button1.Enabled == true)
            {
                lbl_erreur.Visible = true;
                lbl_erreur.Text = "Vous devez entrez un nombre de sous réseaux VLSM";
            }
            else if (!int.TryParse(txtb_nbsr_vlsm.Text, out int number))
            {
                lbl_erreur.Visible = true;
                lbl_erreur.Text = "Veuillez entrer uniquement des nombre dans le champs dédier aux nombres de sous réseau VLSM!";
            }
            else
            {
                lbl_erreur.Text = "";
                nbsr_vlsm = int.Parse(txtb_nbsr_vlsm.Text);
                if ((nbsr_vlsm < 1 || nbsr_vlsm > 85) && button1.Enabled == true)
                {
                    lbl_erreur.Visible = true;
                    lbl_erreur.Text = "Nombre de sous réseau impossible pour la classe C d'adresses";
                }
                else
                {
                   
                    lbl_erreur.Text = "";
                    if (txtb_ht_par_sr.ReadOnly == true && txtb_nbsr_vlsm.ReadOnly == false && button1.Enabled == true)
                    {
                        txtb_ht_par_sr.ReadOnly = false;
                        txtb_nbsr_vlsm.ReadOnly = true;
                        i = i + 1;
                        lbl_srr_vlsm.Text = "sous réseau " + i.ToString() + ".";
                        txtb_ht_par_sr.Focus();
                    }
                    else
                    {
                        if (txtb_ht_par_sr.ReadOnly == false && txtb_nbsr_vlsm.ReadOnly == true && button1.Enabled == true)
                        {
                            lbl_srr_vlsm.Text = "sous réseau " + i.ToString() + ".";
                            ad_ip1 = int.Parse(txtb_ip1.Text);
                            ad_ip2 = int.Parse(txtb_ip2.Text);
                            ad_ip3 = int.Parse(txtb_ip3.Text);
                            ad_ip4 = int.Parse(txtb_ip4.Text);
                            if ((ad_ip1 < 192 || ad_ip1 > 223) || (ad_ip2 < 0 || ad_ip2 > 255) || (ad_ip3 < 0 || ad_ip3 > 255) || (ad_ip4 < 0 || ad_ip4 > 255))
                            {
                                lbl_erreur.Visible = true;
                                lbl_erreur.Text = "Veillez entrez une adresse de classe C";
                            }
                            else
                            {                     
                                txtb_ht_par_sr.ReadOnly = false;
                                // ici c'est la partie ou tout est verifié on peut donc afficher le compteur de plan
                                
                                lbl_srr_vlsm.Text = "sous réseau " + i.ToString() + ".";
                               compteur_plan_vlsm();
                                derniere = derniere + 1;
                            }
                        }
                    }
                }
            }
        }
        
        private void btn_pdt_Click(object sender, EventArgs e)
        {
            // Récupérer les résultats des textboxes
            string result1 = txtb_plan_sr.Text;
            string result2 = txtb_plan_dif.Text;
            string result3 = txtb_plan_pas.Text;
            string result4 = txtb_plan_hote.Text;
            string result5 = txtb_plan_hote1.Text;
            string result6 = txtb_cidr.Text;

            try
            {
                // Créer un nouveau SaveFileDialog
                using (var saveFileDialog = new SaveFileDialog())
                {
                    // Configurer la boîte de dialogue pour enregistrer un fichier PDF
                    saveFileDialog.Filter = "Fichier PDF (*.pdf)|*.pdf";
                    saveFileDialog.FileName = "votre_document.pdf";

                    // Afficher la boîte de dialogue pour enregistrer le fichier
                    DialogResult result = saveFileDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                    {
                        // Obtenir le chemin du fichier sélectionné par l'utilisateur
                        string filePath = saveFileDialog.FileName;

                        // Créer un nouveau document PDF
                        Document document = new Document();

                        // Créer un écrivain PDF pour écrire dans le fichier
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                        // Ouvrir le document pour l'édition
                        document.Open();

                        // Ajouter les résultats des textboxes au document PDF
                        document.Add(new Paragraph("le résultat de vos calcul"));
                        document.Add(new Paragraph(" Sous réseau " + result1 + " Adresse Réseau " + result2 + " 1 adresss hote " + result3 + " Dernier adresse " + result4 +" Diffusion" + result5));
                        document.Close();

                        // Afficher un message de confirmation
                        MessageBox.Show("Les résultats ont été enregistrés au format PDF avec succès !");
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions lors de l'enregistrement du fichier PDF
                MessageBox.Show("Une erreur s'est produite lors de l'enregistrement du fichier PDF : " + ex.Message);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (txtb_ht_par_sr.Text != "" || txtb_nbsr_vlsm.Text !="" ) 
            {
                txtb_nbsr_vlsm.Text = "";
                txtb_ht_par_sr.Text = "";
            }
        }
    }
}
