using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace WinFormsApp11
{
    public partial class Form1 : Form
    {
        string dosya = "Data Source=D:\\BP.db";

        SqliteConnection connection;

        class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Number { get; set; }
        }

        static void create_table(SqliteConnection connection)
        {
            const string sql = @"CREATE TABLE IF NOT EXISTS students(
                Id INTEGER PRIMARY KEY,
                Name TEXT NOT NULL,
                Surname TEXT NOT NULL,
                Email TEXT NOT NULL,
                Number TEXT NOT NULL
            )";

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        static void insert_student(SqliteConnection connection, string name, string surname, string email, string number)
        {
            const string sql = @"
                INSERT INTO students (Name, Surname, Email, Number) 
                VALUES (@Name, @Surname, @Email, @Number)
            ";

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Surname", surname);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Number", number);

            command.ExecuteNonQuery();
        }

        static void update_student(SqliteConnection connection, int id, string name, string surname, string email, string number)
        {
            const string sql = @"
                UPDATE students 
                SET Name = @Name, Surname = @Surname, Email = @Email, Number = @Number 
                WHERE id = @id
            ";

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Surname", surname);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Number", number);

            command.ExecuteNonQuery();
        }

        static void delete_student(SqliteConnection connection, int id)
        {
            const string sql = @"
                DELETE FROM students WHERE id = @id
            ";

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        static List<Student> get_students(SqliteConnection connection)
        {
            const string sql = @"
                SELECT * FROM students
            ";

            var student_list = new List<Student>();

            using var command = new SqliteCommand(sql, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string surname = reader.GetString(2);
                string email = reader.GetString(3);
                string number = reader.GetString(4);

                student_list.Add(new Student
                {
                    Id = id,
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Number = number
                });
            }

            return student_list;
        }

        public Form1()
        {
            InitializeComponent();
        }

        void listele()
        {
            var liste = get_students(connection);
            dataGridView1.DataSource = liste;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SqliteConnection(dosya);

            connection.Open();

            create_table(connection);

            listele();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            insert_student(connection, txtAd.Text, txtSoyad.Text, txtEposta.Text, txtOgrNo.Text);

            listele();

            txtAd.Clear();
            txtSoyad.Clear();
            txtEposta.Clear();
            txtOgrNo.Clear();
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
            delete_student(connection, secilenId);

            listele();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            update_student(connection, secilenId, txtAd.Text, txtSoyad.Text, txtEposta.Text, txtOgrNo.Text);

            listele();
        }

        // Improve here
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var liste = get_students(connection);
            var aranan = liste.Where(x => x.Name == txtAd.Text).ToList();
            dataGridView1.DataSource = aranan;
        }

        // REMOVE THE REFERENCES LATER
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

    }
}
