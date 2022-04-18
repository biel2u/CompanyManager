using CompanyManager.Client.Components;
using CompanyManager.Shared;
using MudBlazor;

namespace CompanyManager.Client.Helpers
{
    public interface IDialogProvider
    {
        IDialogReference ServeDeleteAppointmentDialog(DisplayAppointmentModel appointment);
        IDialogReference ServeEditAppointmentDialog(DisplayAppointmentModel appointment);
    }

    public class DialogProvider : IDialogProvider
    {
        private readonly IDialogService _dialogService;

        public DialogProvider(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public IDialogReference ServeDeleteAppointmentDialog(DisplayAppointmentModel appointment)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();
            var formattedAppointmentDate = appointment.StartDate.ToString("dd/MM HH:mm");
            parameters.Add("DialogContent", $"Czy na pewno chcesz usunąć wizytę z dnia {formattedAppointmentDate} dla {appointment.ClientName}?");
            var dialog = _dialogService.Show<DeleteConfirmationDialog>("Usuń wizytę", parameters, options);

            return dialog;
        }

        public IDialogReference ServeEditAppointmentDialog(DisplayAppointmentModel appointment)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            var parameters = new DialogParameters();
            parameters.Add("SelectedDate", appointment.StartDate.Date);
            parameters.Add("AppointmentIdToEdit", appointment.Id);
            var dialog = _dialogService.Show<EditAppointmentModal>("Edytuj wizytę", parameters, options);

            return dialog;
        }
       
    }
}
