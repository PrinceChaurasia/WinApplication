using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq.Expressions;
using ClassLibrary1;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        info model = new info();
        public Form1()
        {
            InitializeComponent();
        }

        string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        string phonepattern = "^[ ()+]*([0-9][ ()+]*){10}$";
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        void clear()
        {
            txt_Name.Text = txt_Email.Text = txt_phone.Text = txt_EmpId.Text = "";
            btn_save.Text = "save";
            btn_delete.Enabled=false;
            model.Id = 0;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clear();
            this.ActiveControl = txt_Name;
            LoadData();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
           
            model.Name = txt_Name.Text;
            model.Email = txt_Email.Text;
            model.Phone = txt_phone.Text;
            model.EmpId =Convert.ToInt32( txt_EmpId.Text);

            using (PersonalInfoEntities1 db = new PersonalInfoEntities1())
            {
                if (model.Id == 0)
                {
                    db.infoes.Add(model);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(model).State=EntityState.Modified;
                    db.SaveChanges();
                }
                

            }
            if ((string.IsNullOrEmpty(txt_Name.Text)) && (((TextBox)sender).Text.Length < 7))
            {
                txt_Name.Focus();
                errorProvider1.SetError(this.txt_Name, "PLease provide atleast 7 characters");

            }

            else if (string.IsNullOrEmpty(txt_Email.Text))
            {
                txt_Email.Focus();
                errorProvider2.SetError(this.txt_Email, "!!!Invalid Email!!!");
            }

            else if(string.IsNullOrEmpty(txt_phone.Text))
            {
                txt_phone.Focus();
                errorProvider3.SetError(this.txt_phone, "Please provide 10 digit phone number");
            }
            else
            {
                clear();
                LoadData();
                MessageBox.Show("Submitted successfully");
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to delete this record","Message",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                using (PersonalInfoEntities1 db = new PersonalInfoEntities1())
                {
                    db.infoes.Attach(model);
                    db.infoes.Remove(model);
                    db.SaveChanges();
                    LoadData();
                    clear();
                    MessageBox.Show("Deleted succesfully");
                }
            }
        }

        void LoadData()
        {
            using (PersonalInfoEntities1 db = new PersonalInfoEntities1())
            {
                dgvEmployee.DataSource = db.infoes.ToList < info>();
            }
        }

        private void dgvEmployee_DoubleClick(object sender, EventArgs e)
        {
            if (dgvEmployee.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvEmployee.CurrentRow.Cells["dgId"].Value);
                using (PersonalInfoEntities1 db = new PersonalInfoEntities1())
                {
                    model = db.infoes.Where(x => x.Id == model.Id).FirstOrDefault();
                    txt_Name.Text = model.Name;
                    txt_Email.Text = model.Email;
                    txt_phone.Text = model.Phone;
                    txt_EmpId.Text = Convert.ToString(model.EmpId);
                }

                btn_save.Text = "Update";
                btn_delete.Enabled = true;
            }
        }

        //private void txt_Name_TextChanged(object sender, EventArgs e)
        //{
        //    //if (((TextBox)sender).Text.Length!= 5)
        //    //{
        //    //    MessageBox.Show("You need to write at least 5 characters");
        //    //}

        //    if ((string.IsNullOrEmpty(txt_Name.Text)==false) && (((TextBox)sender).Text.Length<7))
        //    {
        //        txt_Name.Focus();
        //        errorProvider1.SetError(this.txt_Name, "PLease provide atleast 7 characters");
                
        //    }
        //    else
        //    {
        //        errorProvider1.Clear();
        //    }
        //}

        //private void txt_Email_TextChanged(object sender, EventArgs e)
        //{
           
            
           

        //        if (Regex.IsMatch(txt_Email.Text, pattern) == false)
        //        {
        //            txt_Email.Focus();
        //            errorProvider2.SetError(this.txt_Email, "!!!Invalid Email!!!");

                    
        //        }
        //        else
        //        {
        //            errorProvider2.Clear();
        //        }
            
        //}

        //private void txt_phone_TextChanged(object sender, EventArgs e)
        //{
            
           
        //    if(Regex.IsMatch(txt_phone.Text, phonepattern) ==false)
        //    {
                
        //        txt_phone.Focus();
        //        errorProvider3.SetError(this.txt_phone, "Please provide 10 digit phone number");
        //    }
        //    else
        //    {
        //        errorProvider3.Clear();
                
        //    }
        //}

        private void txt_Name_Leave(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(txt_Name.Text) == false) && (((TextBox)sender).Text.Length < 7))
            {
                txt_Name.Focus();
                errorProvider1.SetError(this.txt_Name, "PLease provide atleast 7 characters");

            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void txt_Email_Leave(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txt_Email.Text, pattern) == false)
            {
                txt_Email.Focus();
                errorProvider2.SetError(this.txt_Email, "!!!Invalid Email!!!");


            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void txt_phone_Leave(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txt_phone.Text, phonepattern) == false)
            {

                txt_phone.Focus();
                errorProvider3.SetError(this.txt_phone, "Please provide 10 digit phone number");
            }
            else
            {
                errorProvider3.Clear();

            }
        }

        
    }
}
