using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurante_KiDelicia_Beta1
{


    public partial class Form2 : Form
    {

        decimal troco;
        public string Pagamento {get; set;}

        public decimal ValorRecebido { get; set; }

        private Form1 formulario;
        public Form2(Form1 formulario)
        {
            InitializeComponent();
            this.formulario = formulario;
            textBox1.Visible = false;
            label1.Visible = false;

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string receb = textBox1.Text;
                decimal valor = decimal.Parse(textBox1.Text);
                troco = valor - ValorRecebido;
                if(troco > 0) {
                    label1.Visible = true;
                    label1.Text = $"Troco: {troco:c2}";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Digite somente númeroos. " + ex.Message);
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Visible = true;

            }

            if (!checkBox1.Checked){
                textBox1.Visible = false;
                label1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked) {
                MessageBox.Show("Você precisa selecionar um método de pagamento!");
            }

            else {
                MessageBox.Show("Venda concluída!");
            }

            if (checkBox1.Checked)
            {
                Pagamento = checkBox1.Text;
                Console.WriteLine(Pagamento);
                formulario.Imprimir(Pagamento, troco);
                
                Close();

            } else if (checkBox2.Checked)
            {
                
                Pagamento = checkBox2.Text;
                Console.WriteLine(Pagamento);
                formulario.Imprimir(Pagamento);
                Close();
            }
            else if (checkBox3.Checked)
            {
                
                Pagamento = checkBox3.Text;
                Console.WriteLine(Pagamento);
                formulario.Imprimir(Pagamento);

                Close();
            }

           
        }
    }
}
