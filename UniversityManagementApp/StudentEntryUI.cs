using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace UniversityManagementApp
{
    public partial class StudentEntryUI : Form
    {
        public StudentEntryUI()
        {
            InitializeComponent();
        }

       private bool isUpdateMode = false;
        int studentId;

        string connectionString = ConfigurationManager.ConnectionStrings["UniversityManagementConString"].ConnectionString;

        private void saveButton_Click(object sender, EventArgs e)
        {
           
                string name = nameTextBox.Text;
                string regNo = regNoTextBox.Text;
                string address = addressTextBox.Text;

                if(isUpdateMode)
                {
                SqlConnection connection = new SqlConnection(connectionString);

                    //insert Query

                    string query = "UPDATE Students SET Name='" + name + "',Address='" + address + "'WHERE ID='"+studentId+"'";


                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    int rowAffected = command.ExecuteNonQuery();
                    connection.Close();

                    //see result

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Update Successfully");

                        saveButton.Text = "Save";
                        studentId = 0;
                        isUpdateMode = false;
                        ShowAllStudents();
                    }
                    else
                    {
                        MessageBox.Show("Update failed");

                    }

                    nameTextBox.Clear();
                    regNoTextBox.Clear();
                    addressTextBox.Clear();
                }
             
        


                else
                {

                    if (IsRegNoExists(regNo))
                    {
                        MessageBox.Show("RegNo Is Already Exist.");
                        return;
                    }

                    //connect to database


                    SqlConnection connection = new SqlConnection(connectionString);

                    //insert Query

                    string query = "INSERT INTO Students VALUES('" + name + "','"+regNo+"','" + address + "'";


                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    int rowAffected = command.ExecuteNonQuery();
                    connection.Close();

                    //see result

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Inserted Successfully");

                       ShowAllStudents();
                    }
                    else
                    {
                        MessageBox.Show("Insertion failed");

                    }

                    nameTextBox.Clear();
                    regNoTextBox.Clear();
                    addressTextBox.Clear();
                }

           }

         

           

        public bool IsRegNoExists(string regNo)
        {


            SqlConnection connection = new SqlConnection(connectionString);

            //insert Query

            string query = "SELECT * FROM Students WHERE RegNo='" + regNo + "'";

            SqlCommand command = new SqlCommand(query, connection);


            bool isRegNoExists = false;

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();



            while (reader.Read())
            {
                isRegNoExists = true;
                break;
            }
            reader.Close();
            connection.Close();
            return isRegNoExists;


        }




        private void LoadStudentListViews(List<Student> students)
        {
            studentListView.Items.Clear();
            foreach (var Student in students)
            {
                ListViewItem item = new ListViewItem(Student.id.ToString());
                item.SubItems.Add(Student.name);
                item.SubItems.Add(Student.regNo);
                item.SubItems.Add(Student.address);

                studentListView.Items.Add(item);



            }


        }

        public void ShowAllStudents()
        {

            SqlConnection connection = new SqlConnection(connectionString);

            //insert Query

            string query = "SELECT * FROM Students ";

            SqlCommand command = new SqlCommand(query, connection);



            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            List<Student> studentList = new List<Student>();

            while (reader.Read())
            {
                Student student = new Student();

                student.id = int.Parse(reader["ID"].ToString());
                student.name = reader["Name"].ToString();
                student.regNo = reader["RegNo"].ToString();
                student.address = reader["Address"].ToString();

                studentList.Add(student);

            }
            reader.Close();
            connection.Close();

            //populate list view with data

            LoadStudentListViews(studentList);
        }

        private void StudentEntryUI_Load(object sender, EventArgs e)
        {
            ShowAllStudents();
        }

        private void studentListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = studentListView.SelectedItems[0];

            int ID = int.Parse(item.Text.ToString());

            Student student = GetStudentByID(ID);

            if (student != null)
            {
                isUpdateMode = true;
                saveButton.Text = "Update";
                studentId = student.id;
               

                nameTextBox.Text = student.name;
                regNoTextBox.Text = student.regNo;
                addressTextBox.Text = student.address;
            }

        }


        public Student GetStudentByID(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            //insert Query

            string query = "SELECT * FROM Students WHERE ID='" + id + "'";

            SqlCommand command = new SqlCommand(query, connection);



            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            List<Student> studentList = new List<Student>();

            while (reader.Read())
            {
                Student student = new Student();

                student.id = int.Parse(reader["ID"].ToString());
                student.name = reader["Name"].ToString();
                student.regNo = reader["RegNo"].ToString();
                student.address = reader["Address"].ToString();

                studentList.Add(student);

            }
            reader.Close();
            connection.Close();
            return studentList.FirstOrDefault();
        }


    }
}

        
         

       




       
      

   




