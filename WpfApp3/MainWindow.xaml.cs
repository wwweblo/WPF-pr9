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
                window1.NameTextBox.Text = employee.name;
                window1.EmailTextBox.Text = employee.email;

                // Set the selected item in ComboBox by matching the content
                ComboBoxItem selectedItem = window1.ProfessionComboBox.Items.OfType<ComboBoxItem>()
                                                  .FirstOrDefault(item => item.Content.ToString() == employee.role);
                window1.ProfessionComboBox.SelectedItem = selectedItem;
            }

            window1.SaveClicked += (s, args) =>
            {
                // Handle the save logic here
                if (employee != null)
                {
                    // Update existing employee in the database
                    employee.name = window1.NameTextBox.Text;
                    employee.email = window1.EmailTextBox.Text;

                    // Access the content of the selected ComboBoxItem
                    employee.role = (window1.ProfessionComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

                    dbContext.SaveChanges();
                }
                else
                {
                    // Add new employee to the database
                    Employee newEmployee = new Employee
                    {
                        name = window1.NameTextBox.Text,
                        email = window1.EmailTextBox.Text,

                        // Access the content of the selected ComboBoxItem
                        role = (window1.ProfessionComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString()
                    };

                    dbContext.Employee.Add(newEmployee);
                    dbContext.SaveChanges();
                }

                // Refresh the ListView after saving
                DataContext = dbContext.Employee.ToList();
            };

            window1.ShowDialog();
        }
    }
}
