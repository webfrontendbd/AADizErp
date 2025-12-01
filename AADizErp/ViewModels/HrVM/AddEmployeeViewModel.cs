using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AADizErp.ViewModels.HrVM
{
    public partial class AddEmployeeViewModel : BaseViewModel
    {
        private readonly HrService _employeeService;

        [ObservableProperty]
        private Employee employee = new Employee();

        public ObservableCollection<string> GenderList { get; }

        [ObservableProperty]
        private bool isBusy;

        public bool IsNotBusy => !IsBusy;
        public AddEmployeeViewModel(HrService employeeService)
        {
            _employeeService = employeeService;
            GenderList = new ObservableCollection<string>
            {
                "Male",
                "Female",
                "Other"
            };
        }
        // ─────────────────────────────────────────────
        // VALIDATION LOGIC
        // ─────────────────────────────────────────────

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Employee.EmployeeName))
            {
                ShowError("Employee name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Employee.Phone))
            {
                ShowError("Phone number is required.");
                return false;
            }

            if (Employee.Phone.Length < 11)
            {
                ShowError("Enter a valid phone number.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Employee.Gender))
            {
                ShowError("Gender is required.");
                return false;
            }

            return true;
        }

        private async void ShowError(string message)
        {
            await Shell.Current.DisplayAlert("Validation Error", message, "OK");
        }


        // ─────────────────────────────────────────────
        // COMMAND – ADD EMPLOYEE
        // ─────────────────────────────────────────────

        [RelayCommand]
        private async Task AddEmployeeAsync()
        {
            if (IsBusy) return;

            if (!Validate()) return;

            IsBusy = true;

            try
            {
                var newEmployee = new Employee
                {
                    EmployeeName = Employee.EmployeeName.Trim(),
                    Phone = Employee.Phone.Trim(),
                    Gender = Employee.Gender.Trim(),
                    EmployeeType = Employee.EmployeeType.Trim(),
                    ConfirmationStatus = false  // New employees = unconfirmed
                };

                var result = await _employeeService.SaveUnconfirmedEmployee(newEmployee);

                if (result != null)
                {
                    await Shell.Current.DisplayAlert("Success", "Employee added successfully.", "OK");
                    ClearForm();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to add employee.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ClearForm()
        {
            Employee = new Employee();
        }
    }
}

