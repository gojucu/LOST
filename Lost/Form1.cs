using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lost
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ProductDataContext db = new ProductDataContext();
        private void button1_Click(object sender, EventArgs e)
        {
            frmMain.Role = "Admin";


            int prodid = int.Parse(textBox1.Text);
            string itemname = textBox2.Text, design = textBox3.Text, price = comboBox1.Text;
            DateTime insertDate = DateTime.Parse(dateTimePicker1.Text);
            var st = new Product
            {
                ID = prodid,
                Name = itemname,
                Description = design,
                Priceab = int.Parse(price),
                InsertDate = insertDate,
                UpdateDate = DateTime.Now,
               
                
            };

            db.Products.InsertOnSubmit(st);
            db.SubmitChanges();
            MessageBox.Show("Success");
            loadData();


        }
        void loadData()
        {
            var st = from s in db.Products select s;
            dataGridView1.DataSource = st;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            loadData();
            FillComboBox();
        }

        private void FillComboBox()
        {
            // Veritabanından verileri çekiyoruz
            var priceList = db.Prices.Select(x => x.ProductPrice).Distinct().ToList();

            // ComboBox'ı dolduruyoruz
            comboBox1.DataSource = priceList;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //Update
            string itemname = textBox2.Text, design = textBox3.Text, price = comboBox1.Text;
            DateTime insertDate = DateTime.Parse(dateTimePicker1.Text);

            var st = db.Products.FirstOrDefault(x => x.ID == int.Parse(textBox1.Text));
            
            Console.WriteLine(st.Name);

            st.Name = itemname;
            st.Description = design;    
            st.Priceab = int.Parse(price);
            st.InsertDate = insertDate;
            st.UpdateDate = DateTime.Now;
            db.SubmitChanges();

            MessageBox.Show("Updated");
            loadData();
        }

        private void getItem_Click(object sender, EventArgs e)
        {
            //Get
            var st = db.Products.FirstOrDefault(x => x.ID == int.Parse(textBox1.Text));

            if (st != null)
            {

                textBox1.Text = st.ID.ToString();
                textBox2.Text = st.Name;
                textBox3.Text = st.Description;
                comboBox1.SelectedItem = st.Priceab.ToString();
                dateTimePicker1.Value = (DateTime)st.InsertDate;

               // var st = from s in db.Products select s;

                var hey =  db.ProductPrices.Where(x => x.ProductID == st.ID);

                //var prices = db.Products
                // .Where(x => x.ID == st.ID) // Ürün ID'sine göre filtreleme
                // .Select(x => x.NewPrices) // Price tablosundan Price değerini alıyoruz
                // .ToList(); // Listeye dönüştür

                var prices = db.ProductPrices
                 .Where(x => x.ProductID == st.ID) // Ürün ID'sine göre filtreleme
                 .Select(x => x.Product.ID) // Price tablosundan Price değerini alıyoruz
                 .ToList(); // Listeye dönüştür

                // var hey2 = db.Products.Where(x => x.ID == st.ID).Select(x => x.Prices.);

                // var test =
                //   from pr in db.Prices select s;
            }
            else
            {
                MessageBox.Show("Ürün bulunamadı");
            }


        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.Yellow;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.White;
        }
    }
}
