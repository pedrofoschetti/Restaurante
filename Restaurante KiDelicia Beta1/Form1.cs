using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Bematech;
using Bematech.MiniImpressoras;
using Bematech.Texto;

namespace Restaurante_KiDelicia_Beta1
{

    public partial class Form1 : Form
    {
        private Dictionary<string, decimal> products;
        public decimal totalPrice { get; set; }
        string product;
        decimal items;
        string quantidade;
        double preço = 0;
        double precinho;
        int quantia;
        string[] produtos = new string[10];
        int clicks = 0;
        public string pagamento { get; set; }
        public object listaform { get; set; }

        public string ListaItems { get; set; }


        public Form1()
        {
            InitializeComponent();
            InitializeProducts();
            PopulateComboBox();
            items = numericUpDown1.Value = 1;
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

        }

        public void InitializeProducts()
        {
            products = new Dictionary<string, decimal>
            {
                { "Self-service", 20.0m },
                { "Self-service c/ churrasco", 26.0m },
                { "Self-service c/ carne e churrasco", 24.0m },
                { "Self-service 1/2 (CRIANÇA)", 10.0m },
                { "Marmitex P tradicional", 12.0m },
                { "Marmitex P c/ churrasco", 15.0m },
                { "Marmitex G tradicional", 17.0m },
                { "Marmitex G c/ churrasco", 20.0m },
                { "Refrigerante 1L", 8.0m },
                { "Refrigerante 2L", 12.0m },
                { "Refrigerante 250ml", 2.5m },
                { "Refrigerante KS", 4.0m },
                { "Refrigerante 600ml", 6.0m },
                { "Refrigerante Lata", 5.0m },
                { "Suco", 7.0m },
                { "Suco lata", 6.0m },
                { "Suco jarra 1L", 14.0m },
                { "Suco jarra 1,5L", 21.0m },
                { "Del vale garrafa", 5.0m },
                { "Energético", 10.0m },
                { "Gatorade", 6.0m },
                { "Kapo", 3.0m },
                { "Trufa", 3.5m },
                { "Água sem gás", 2.0m },
                { "Água c gás", 3.0m },
                { "Marmita com frango" , 3.5m }
            };
        }

        public void PopulateComboBox()
        {
            foreach (string productName in products.Keys)
            {
                comboBox1.Items.Add(productName);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string selectedProduct = comboBox1.SelectedItem?.ToString();
            if (selectedProduct == null || selectedProduct == string.Empty)
            {
                MessageBox.Show("Selecione um produto.");
                return;
            }

            int quantity = (int)numericUpDown1.Value;
            decimal price = products[selectedProduct];
            decimal total = quantity * price;

            string cartItem = $"{quantity}x - {selectedProduct} {price:C2} = {total:C2}";

            lista.Items.Add(cartItem);

            totalPrice += total;
            numericUpDown1.Value = 1;
            UpdateTotalPriceLabel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        public void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex = lista.SelectedIndex;
            if (selectedIndex >= 0)
            {
                string selectedItem = lista.Items[selectedIndex].ToString();
                decimal totalToRemove = GetTotalFromCartItem(selectedItem);
                lista.Items.RemoveAt(selectedIndex);
                totalPrice -= totalToRemove;
                UpdateTotalPriceLabel();
            }
        }

        public void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        public decimal GetTotalFromCartItem(string cartItem)
        {
            string[] parts = cartItem.Split('=');
            if (parts.Length == 2 && decimal.TryParse(parts[1].Trim().Replace("R$", ""), out decimal total))
            {
                return total;
            }
            return 0.0m;
        }
        public void UpdateTotalPriceLabel()
        {
            label2.Text = $"{totalPrice:C2}";
            
        }

        public void SaveReport(string pagamento)
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string reportFilePath = Path.Combine(directoryPath, "relatorio.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(reportFilePath, true))
                {
                    // Escrever o conteúdo do relatório no arquivo
                    Form2 formulario2 = new Form2(this);
                    writer.WriteLine($"Data: {DateTime.Now}");
                    writer.WriteLine("Itens vendidos:");

                    foreach (object item in lista.Items)
                    {
                        writer.WriteLine(item.ToString());
                    }

                    writer.WriteLine($"Valor final: {totalPrice:C2}");
                    writer.WriteLine($"Método de pagamento: {pagamento}");
                    writer.WriteLine("----------");

                    MessageBox.Show("Relatório salvo com sucesso!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o relatório: {ex.Message}");
            }
        }

        public void Imprimir(string pagamento, decimal troco)
        {
            ImpressoraNaoFiscal impressoraNaoFiscal = new ImpressoraNaoFiscal(ModeloImpressoraNaoFiscal.MP4000TH, "COM6");
            TextoFormatado texto = new TextoFormatado();
            impressoraNaoFiscal.Imprimir("                   CUPOM FISCAL \n");
            impressoraNaoFiscal.Imprimir("              RESTAURANTE KIDELICIA \n");
            texto.Alinhamento = TextoFormatado.TipoAlinhamento.Esquerda;
            impressoraNaoFiscal.Imprimir($"Data: {DateTime.Now} \n");
            impressoraNaoFiscal.Imprimir("Itens vendidos: \n");
            foreach (object item in lista.Items)
            {
                impressoraNaoFiscal.Imprimir(item.ToString() + "\n");
            }

            impressoraNaoFiscal.Imprimir($"Valor final: {totalPrice:C2} \n");
            impressoraNaoFiscal.Imprimir($"Método de pagamento: {pagamento}  \n");
            impressoraNaoFiscal.Imprimir($"Troco: {troco} \n");
            impressoraNaoFiscal.CortarPapel(false);
        }

        public void Imprimir(string pagamento) {
            ImpressoraNaoFiscal impressoraNaoFiscal = new ImpressoraNaoFiscal(ModeloImpressoraNaoFiscal.MP4000TH, "COM5");
            TextoFormatado texto = new TextoFormatado();
            impressoraNaoFiscal.Imprimir("                   CUPOM FISCAL \n");
            impressoraNaoFiscal.Imprimir("              RESTAURANTE KIDELICIA \n");
            texto.Alinhamento = TextoFormatado.TipoAlinhamento.Esquerda;
            impressoraNaoFiscal.Imprimir($"Data: {DateTime.Now} \n");
            impressoraNaoFiscal.Imprimir("Itens vendidos: \n");
            foreach (object item in lista.Items)
            {
                impressoraNaoFiscal.Imprimir(item.ToString() + "\n");
            }

            impressoraNaoFiscal.Imprimir($"Valor final: {totalPrice:C2} \n");
            impressoraNaoFiscal.Imprimir($"Método de pagamento: {pagamento}  \n");
            impressoraNaoFiscal.CortarPapel(false);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (lista.Items.Count > 0)
            {
                Form2 form2 = new Form2(this);
                form2.ValorRecebido = totalPrice;
                form2.Show();
                listaform = lista.Items;
            }

            else
            {
                MessageBox.Show("Voce precisa adicionar produtos para vender.");
            }
            
            
        }
    }
}
