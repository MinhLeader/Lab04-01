using Lab04_01.Model;
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

namespace Lab04_01
{
    public partial class FrmQLSV : Form
    {
        private Model100 db = new Model100();

        public FrmQLSV()
        {
            InitializeComponent();
        }



        private void txtMSSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmQLSV_Load(object sender, EventArgs e)
        {
            try
            {
               

                // Load danh sách sinh viên vào DataGridView
                dataSinhVien.DataSource = db.Students.ToList();

                // Load danh sách Khoa vào ComboBox
                cbbKhoa.DataSource = db.Faculties.ToList();
                cbbKhoa.DisplayMember = "FacultyName";
                cbbKhoa.ValueMember = "FacultyID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataSinhVien.Rows[e.RowIndex];

                txtMSSV.Text = row.Cells["StudentID"].Value.ToString();
                txtName.Text = row.Cells["FullName"].Value.ToString();
                txtScore.Text = row.Cells["AverageScore"].Value.ToString();
                cbbKhoa.SelectedValue = row.Cells["FacultyID"].Value;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = new Student
                {
                    StudentID = int.Parse(txtMSSV.Text),
                    FullName = txtName.Text,
                    AverageScore = decimal.Parse(txtScore.Text),
                    FacultyID = (int)cbbKhoa.SelectedValue
                };

                db.Students.Add(student);
                db.SaveChanges();
                dataSinhVien.DataSource = db.Students.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int studentID = int.Parse(txtMSSV.Text);
                Student student = db.Students.Find(studentID);
                if (student != null)
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.Students.Remove(student);
                        db.SaveChanges();
                        dataSinhVien.DataSource = db.Students.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {

            try
            {
                int studentID = int.Parse(txtMSSV.Text);
                Student student = db.Students.Find(studentID);

                if (student == null)
                {
                    MessageBox.Show("Không tìm thấy mã số sinh viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                student.FullName = txtName.Text;
                student.AverageScore = decimal.Parse(txtScore.Text);
                student.FacultyID = (int)cbbKhoa.SelectedValue;

                db.SaveChanges();
                dataSinhVien.DataSource = db.Students.ToList();

                MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset dữ liệu về giá trị ban đầu
                txtMSSV.Text = "";
                txtName.Text = "";
                txtScore.Text = "";
                cbbKhoa.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
