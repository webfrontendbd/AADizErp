using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Pages.HRPages;
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
        private Employee employeeDto = new Employee();

        [ObservableProperty]
        private ObservableCollection<string> genderList = new() {
                "Male",
                "Female",
                "Other"
            };

        public AddEmployeeViewModel(HrService employeeService)
        {
            _employeeService = employeeService;
        }
        // ─────────────────────────────────────────────
        // VALIDATION LOGIC
        // ─────────────────────────────────────────────

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(EmployeeDto.EmployeeName))
            {
                ShowError("Employee name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmployeeDto.Phone))
            {
                ShowError("Phone number is required.");
                return false;
            }

            if (EmployeeDto.Phone.Length < 11)
            {
                ShowError("Enter a valid phone number.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmployeeDto.Gender))
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
            if (IsLoading) return;

            if (!Validate()) return;

            IsLoading = true;

            try
            {
                var newEmployee = new Employee
                {
                    EmployeeName = EmployeeDto.EmployeeName.Trim(),
                    Phone = EmployeeDto.Phone.Trim(),
                    Gender = EmployeeDto.Gender.Trim(),
                    EmployeeType = EmployeeDto.EmployeeType.Trim(),
                    ConfirmationStatus = false  // New employees = unconfirmed
                };

                var result = await _employeeService.SaveUnconfirmedEmployee(newEmployee);

                if (result != null)
                {
                    ClearForm();
                    await Shell.Current.GoToAsync($"{nameof(UnconfirmEmployeeListPage)}");
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
                IsLoading = false;
            }
        }

        private void ClearForm()
        {
            EmployeeDto = new Employee();
        }
    }
}

