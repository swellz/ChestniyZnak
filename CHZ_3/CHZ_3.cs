using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace CHZ_3
{
    public partial class CHZ_3 : Form
    {

        Excel.Application excelapp = new Excel.Application();
        Excel.Worksheet excelsheetFile;
        const string English = "qwertyuiop[]\\asdfghjkl;'zxcvbnm,./QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>?`~!@#$%^&*()_+";
        const string Russian = "йцукенгшщзхъ\\фывапролджэячсмитьбю.ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ёЁ!\"№;%:?*()_+";
        const string shortRussian = @"йцукенгшщзхъфывапролджэячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁё";
        string codir = "UTF-8";
        public CHZ_3()
        {
            InitializeComponent();
        }
        private bool InputHasRus(string input)
        {
            foreach (var symbol in input)
            {
                if (shortRussian.IndexOf(symbol) != -1)
                    return true;
            }
            return false;
        }
        static string ConvertRusToEng(string input)
        {
            var result = new StringBuilder(input.Length);
            int index;

            foreach (var symbol in input)
            {
                result.Append((index = Russian.IndexOf(symbol)) != -1 ? English[index] : symbol);
            }



            return result.ToString();
        }
        private void browseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                filePathBox.Text = fileDialog.FileName;
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            Excel.Workbook excelbookFile = excelapp.Workbooks.Add();
            try
            {
                if (File.Exists(filePathBox.Text))
                { 
                    string[] codes = File.ReadAllLines(filePathBox.Text, Encoding.GetEncoding(comboBox1.Text));
                    List<string> codes2 = new List<string>();
                    excelsheetFile = (Excel.Worksheet)excelbookFile.Worksheets.get_Item(1);
                    string code = "";
                    foreach (string s in codes)
                    {
                        code = s;
                        if (code.Contains("<0x1D>"))
                            code = code.Remove(code.IndexOf("<0x1D>"));
                        if (checkBox1.Checked)
                            if (InputHasRus(code))
                                code = ConvertRusToEng(code);
                        codes2.Add(code);
                    }
                    SetCodes(excelsheetFile, filePathBox.Text,codes2);
                    excelbookFile.SaveAs(filePathBox.Text.Remove(filePathBox.Text.IndexOf(".txt")) + ".xlsx");
                    excelbookFile.Close();
                    MessageBox.Show("Готово!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SetCodes(Excel.Worksheet sheet, string filePath, List<string> codes2)
        {
            Excel.Range cell = sheet.get_Range("A1","A1");
            foreach (string code in codes2)
            {
                cell.Value = code;
                cell = cell.Offset[1,0];
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Visible = checkBox1.Checked;
            labelCode.Visible = checkBox1.Checked;
        }

        private void CHZ_3_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
