using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model1 dbContext; // Контекст базы данных Entity Framework

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new Model1(); // Инициализация контекста базы данных

            // Загрузка всех сотрудников из базы данных в список
            List<Employee> employees = dbContext.Employee.ToList();

            // Установка списка сотрудников в качестве DataContext для привязки данных
            DataContext = employees;
        }

        // Обработчик события нажатия на кнопку "Удалить"
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что DataContext представляет собой список сотрудников
            if (DataContext is List<Employee> employees)
            {
                // Получение выбранных элементов из ListView
                List<Employee> selectedEmployees = list.SelectedItems.Cast<Employee>().ToList();

                // Удаление выбранных сотрудников из базы данных
                foreach (var employee in selectedEmployees)
                {
                    dbContext.Employee.Remove(employee);
                }
                dbContext.SaveChanges(); // Сохранение изменений в базе данных

                // Удаление выбранных сотрудников из списка
                foreach (var employee in selectedEmployees)
                {
                    employees.Remove(employee);
                }

                // Обновление отображения ListView
                list.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select items to delete.", "Delete Items", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обработчик события изменения выбора в ListView
        private void Selector_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            ListView list = sender as ListView;
            Employee selectedEmployee = list?.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                OpenWindow1(selectedEmployee); // Открытие окна редактирования/просмотра данных сотрудника
            }
        }

        // Обработчик события нажатия на кнопку "Добавить"
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow1(null); // Открытие окна для добавления нового сотрудника
        }

        private void OpenWindow1(Employee employee)
        {
            Window1 window1 = new Window1();

            if (employee != null)
            {
                // Заполнение полей данными существующего сотрудника
                window1.NameTextBox.Text = employee.name;
                window1.EmailTextBox.Text = employee.email;
                window1.ageTextBox.Text = Convert.ToString(employee.age);
                window1.LoginTextBox.Text = employee.login;
                window1.PasswordTextBox.Text = employee.password;

                ComboBoxItem selectedItem = window1.ProfessionComboBox.Items.OfType<ComboBoxItem>()
                                                  .FirstOrDefault(item => item.Content.ToString() == employee.role);
                window1.ProfessionComboBox.SelectedItem = selectedItem;

                ComboBoxItem selectedItem2 = window1.StatusComboBox.Items.OfType<ComboBoxItem>()
                                                  .FirstOrDefault(item => item.Content.ToString() == employee.status);
                window1.StatusComboBox.SelectedItem = selectedItem2;
            }

            window1.SaveClicked += (s, args) =>
            {
                // Обработка логики сохранения

                // Проверка ввода данных
                string validationError = ValidateEmployeeInput(window1);
                if (!string.IsNullOrEmpty(validationError))
                {
                    MessageBox.Show(validationError, "Ошибка ввода данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (employee != null)
                {
                    // Обновление существующего сотрудника в базе данных
                    employee.name = window1.NameTextBox.Text;
                    employee.email = window1.EmailTextBox.Text;
                    employee.role = (window1.ProfessionComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    employee.age = Convert.ToInt32(window1.ageTextBox.Text);
                    employee.status = (window1.StatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    employee.login = window1.LoginTextBox.Text;
                    employee.password = window1.PasswordTextBox.Text;

                    dbContext.SaveChanges();
                }
                else
                {
                    // Добавление нового сотрудника в базу данных
                    Employee newEmployee = new Employee
                    {
                        name = window1.NameTextBox.Text,
                        email = window1.EmailTextBox.Text,
                        role = (window1.ProfessionComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                        age = Convert.ToInt16(window1.ageTextBox.Text),
                        status = (window1.StatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                        login = window1.LoginTextBox.Text,
                        password = window1.PasswordTextBox.Text,
                    };

                    dbContext.Employee.Add(newEmployee);
                    dbContext.SaveChanges();
                }

                //Закрытие окна если проверка прошла успешно
                window1.DialogResult = true;

                // Обновление ListView после сохранения
                DataContext = dbContext.Employee.ToList();
            };

            window1.ShowDialog();
        }

        private string ValidateEmployeeInput(Window1 window1)
        {
            // Можно добавить пользовательские правила валидации здесь в соответствии с вашими требованиями
            if (string.IsNullOrEmpty(window1.NameTextBox.Text))
            {
                return "Имя обязательно для заполнения.";
            }

            if (string.IsNullOrEmpty(window1.EmailTextBox.Text) || !IsValidEmail(window1.EmailTextBox.Text))
            {
                return "Недопустимый или пустой адрес электронной почты.";
            }

            if (string.IsNullOrEmpty(window1.ageTextBox.Text) || !int.TryParse(window1.ageTextBox.Text, out _))
            {
                return "Возраст должен быть числом.";
            }

            // Добавьте другие правила валидации при необходимости

            return string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
