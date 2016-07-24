using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Three_Layers.Core;
using System.Text.RegularExpressions;

namespace Three_Layers
{
    public partial class ClientsOfBank : Form
    {
        private string datePattern = @"(?([3])3[0-1]|[0-2]?[0-9])\.(?([1])1[0-2]|0?[0-9])\.[1-2][0-9][0-9][0-9]";//@"(?([3])3[0-1]|?2[0-9])\.(?([1])1[0-2]|0?[0-9])\.[12][0-9][0-9][0-9]";
        private string namePattern = @"[а-яА-Я\s]+";
        private string bankNamePattern = @"[а-яА-Я0-9\s]+";
        //
        private UserBLL userBLL;

        private bool IsNullOrEmptyFields ()
        {
            if (_fioEdit.Text == null || _fioEdit.Text == "" || _birthdayEdit.Text == "" || _birthdayEdit.Text == null || _bankNameEdit.Text == null || _bankNameEdit.Text == "")
            {
                return true;
            }
            else
                return false;
        }

        private void ListBoxFill(List<Client> clientList)
        {
            listBox.Items.Clear();
            foreach (var client in clientList)
            {
                listBox.Items.Add(client.Fio);
            }
            _fioEdit.Text = null;
            _birthdayEdit.Text = null;
            _bankNameEdit.Text = null;
        }

        /// <summary>
        /// Конструктор формы.
        /// </summary>
        public ClientsOfBank()
        {
            InitializeComponent();
            userBLL = new UserBLL();
            foreach (var client in userBLL.clients)
            {
                listBox.Items.Add(client.Fio);
            }
        }
        
        /// <summary>
        /// Поиск с примененимем фильтров.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, EventArgs e)
        {   
            if (_familiaFind.Text == "" && _nameFind.Text == "" && _otchestvoFind.Text == "" && _bankNameFind.Text == "")
            {
                MessageBox.Show("Вы не ввели ни одного фильтра!", "Внимание!!!");
            }
            else
            {
                textBox.Text = userBLL.FilterClients(_familiaFind.Text, _nameFind.Text, _otchestvoFind.Text, _bankNameFind.Text);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                _fioEdit.Text = userBLL.clients[listBox.SelectedIndex].Fio;
                try
                {
                    _birthdayEdit.Text = userBLL.clients[listBox.SelectedIndex].Birthday.ToString("dd.MM.yyyy");
                }
                catch
                {
                    MessageBox.Show("Введена неверная дата!", "Внимание!!!");
                }
                _bankNameEdit.Text = userBLL.clients[listBox.SelectedIndex].BankName;
            }
        }

        /// <summary>
        /// Создание нового клиента.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newClientButton_Click(object sender, EventArgs e)
        {
            if (IsNullOrEmptyFields())
            {
                MessageBox.Show("Одно из поле редактирования пустое!", "Внимание!!!");
            }
            else if (!Regex.IsMatch(_birthdayEdit.Text, datePattern))
            {
                MessageBox.Show("Поле даты не соотвествует шаблону!", "Внимание!!!");
            }
            else if (!Regex.IsMatch(_fioEdit.Text, namePattern) || !Regex.IsMatch(_bankNameEdit.Text, bankNamePattern))
            {
                MessageBox.Show("Поле ФИО или Банка не соотвествует шаблону!", "Внимание!!!");
            }
            else
            {
                try
                {
                    userBLL.clients.Add(new Client() { Fio = _fioEdit.Text, Birthday = Convert.ToDateTime(_birthdayEdit.Text), BankName = _bankNameEdit.Text });
                }
                catch
                {
                    MessageBox.Show("Введена неверная дата!", "Внимание!!!");
                }
                userBLL.NewClient(userBLL.clients);
                ListBoxFill(userBLL.clients);
            }
            
        }

        /// <summary>
        /// Редактирование клиента.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editClientButton_Click(object sender, EventArgs e)
        {
            if (IsNullOrEmptyFields())
            {
                MessageBox.Show("Одно из поле редактирования пустое!", "Внимание!!!");
            }
            else if (!Regex.IsMatch(_birthdayEdit.Text, datePattern))
            {
                MessageBox.Show("Поле даты не соотвествует шаблону!", "Внимание!!!");
            }
            else if (!Regex.IsMatch(_fioEdit.Text, namePattern) || !Regex.IsMatch(_bankNameEdit.Text, bankNamePattern))
            {
                MessageBox.Show("Поле ФИО или Банка не соотвествует шаблону!", "Внимание!!!");
            }
            else
            {
                userBLL.clients.RemoveAt(listBox.SelectedIndex);
                userBLL.clients.Insert(listBox.SelectedIndex, new Client() { Fio = _fioEdit.Text, Birthday = Convert.ToDateTime(_birthdayEdit.Text), BankName = _bankNameEdit.Text });
                userBLL.NewClient(userBLL.clients);
                ListBoxFill(userBLL.clients);
            }
        }

        /// <summary>
        /// Удаление клиента.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteClientButton_Click(object sender, EventArgs e)
        {
            if (IsNullOrEmptyFields())
            {
                MessageBox.Show("Одно из поле редактирования пустое!", "Внимание!!!");
            }
            else
            {
                userBLL.clients.RemoveAt(listBox.SelectedIndex);
                userBLL.NewClient(userBLL.clients);
                ListBoxFill(userBLL.clients);
            }
        }

    }
}
