using System.DirectoryServices;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Student_Form
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //declare the array storage
        string[] names = new string[100];
        string[] addresses = new string[100];
        string[] programs = new string[100];
        string[] yr_levels = new string[100];
        char status = 'A';

        int index = 0;
        int updatedIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string yearlevel = "";
            string name = txtFullname.Text;
            string address = txtAddress.Text; ;
            string program = comboBoxProgram.Text;
            if (rbOne.IsChecked == true) yearlevel = "1";
            else if (rbTwo.IsChecked == true) yearlevel = "2";
            else if (rbThree.IsChecked == true) yearlevel = "3";
            else if (rbFour.IsChecked == true) yearlevel = "4";

            string data = $"{name} - {address} - {program} = {yearlevel}";

            if (name == "" || address == "" || program == "" || yearlevel == "")
            {
                MessageBox.Show("Please fill all fields","Student Data",MessageBoxButton.OK);
                return;
            }
            SaveData(name, address, program, yearlevel);
            ClearData();
        }
        /**
         * Clears the form data after the sava action
         */
        private void ClearData()
        {
            txtFullname.Clear();
            txtAddress.Clear();
            comboBoxProgram.SelectedIndex = -1;
            rbOne.IsChecked = false;
            rbTwo.IsChecked = false;
            rbThree.IsChecked = false;
            rbFour.IsChecked = false;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = dataGrid.SelectedIndex;
            if (index >= 0)
            {
                txtFullname.Text = names[index];
                txtAddress.Text = addresses[index];
                comboBoxProgram.Text = programs[index];
                if (yr_levels[index] == "1") rbOne.IsChecked = true;
                else if (yr_levels[index] == "2") rbTwo.IsChecked = true;
                else if (yr_levels[index] == "3") rbThree.IsChecked = true;
                else if (yr_levels[index] == "4") rbFour.IsChecked = true;
                //make the delete data button enabled
                btnDeleteData.IsEnabled = true;
                //change the status to E or Update
                status = 'E';
                updatedIndex = index;
            }            
        }
        /**
         * Perform the save action. There are two save action here.
         * Add - Add the new data to the array
         * Update - updates the data from the array
         */
        private void SaveData(string n, string a, string p, string y)
        {
            if (status == 'A')
            {
                //save to array
                names[index] = n;
                addresses[index] = a;
                programs[index] = p;
                yr_levels[index] = y;
                //add to datagrid
                dataGrid.Items.Add(new
                {
                    Firstname = names[index],
                    Address = addresses[index],
                    Program = programs[index],
                    YearLevel = yr_levels[index]
                });
                //increment index
                index++;
                //display message
                MessageBox.Show("New data successfully added!","Student Form",MessageBoxButton.OK);
            }
            else if (status == 'E' && updatedIndex >= 0)
            {
                //update data on the given index
                names[updatedIndex] = n;
                addresses[updatedIndex] = a;
                programs[updatedIndex] = p;
                yr_levels[updatedIndex] = y;
                //refresh datagrid
                RefreshGrid();
                //set the status and updatedIndex to default
                status = 'A';
                updatedIndex = -1;
                //display message
                MessageBox.Show("Student data successfully updated!", "Student Form", MessageBoxButton.OK);
            }
        }

        private void btnDeleteData_Click(object sender, RoutedEventArgs e)
        {
            int deleteIndex = dataGrid.SelectedIndex;

            if (deleteIndex == -1)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }
            ShiftElements(deleteIndex);
            //decrement the size
            index--;
            //update the grid
            RefreshGrid();
            //disables the delete button after deleting
            btnDeleteData.IsEnabled = false;
            //clears the data
            ClearData();
            MessageBox.Show("Student data deleted successfully!", "Student Form", MessageBoxButton.OK);
        }

        /**
         * This function refreshes the data grid
         */
        private void RefreshGrid()
        {
            dataGrid.Items.Clear();
            for (int i = 0; i < index; i++)
            {
                dataGrid.Items.Add(new
                {
                    Firstname = names[i],
                    Address = addresses[i],
                    Program = programs[i],
                    YearLevel = yr_levels[i]
                });
            }
        }

        /**
         * This function shifts the elements to the left after performing delete
         */
        private void ShiftElements(int deletedIndex)
        {
            for (int i = deletedIndex; i < index; i++)
            {
                names[i] = names[i + 1];
                addresses[i] = addresses[i + 1];
                programs[i] = programs[i + 1];
                yr_levels[i] = yr_levels[i + 1];
            }
        }
    }
}