using System.Diagnostics;
using System.Text.Json;

namespace WinFormsApp11
{
    public partial class Form1 : Form
    {
        string dosya = "D:\\BP.json";

        class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Number { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        void listele()
        {
            if (File.Exists(dosya))
            {
                var liste = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(dosya));
                dataGridView1.DataSource = liste;
            }
            else
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            var liste = new List<Student>();
            if (File.Exists(dosya))
            {
                liste = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(dosya));
            }

            Student std = new Student();

            if (liste.Count > 0)
            {
                std.Id = File.Exists(dosya)
                    ? liste.Max(x => x.Id) + 1
                    : 1;
            }
            else
            {
                std.Id = 1;
            }

            std.Name = txtAd.Text;
            std.Surname = txtSoyad.Text;
            std.Email = txtEposta.Text;
            std.Number = txtOgrNo.Text;
            liste.Add(std);

            File.WriteAllText(dosya, JsonSerializer.Serialize(liste));
            listele();

            txtAd.Clear();
            txtSoyad.Clear();
            txtEposta.Clear();
            txtOgrNo.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private int secilenId;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            secilenId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            txtAd.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtSoyad.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtEposta.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtOgrNo.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            var liste = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(dosya));

            var yeniListe = liste.Where(x => x.Id != secilenId).ToList();

            File.WriteAllText(dosya, JsonSerializer.Serialize(yeniListe));

            Debug.WriteLine("Silindi: " + secilenId);

            listele();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            var liste = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(dosya));

            var secilenOgr = liste.FirstOrDefault(x => x.Id == secilenId);

            secilenOgr.Name = txtAd.Text;
            secilenOgr.Surname = txtSoyad.Text;
            secilenOgr.Email = txtEposta.Text;
            secilenOgr.Number = txtOgrNo.Text;

            File.WriteAllText(dosya, JsonSerializer.Serialize(liste));

            listele();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var liste = JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(dosya));
            var aranan = liste.Where(x => x.Name == txtAd.Text).ToList();
            dataGridView1.DataSource = aranan;
        }
    }
}
