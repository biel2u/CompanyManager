using CompanyManager.Client.Models;
using CompanyManager.Shared;

namespace CompanyManager.Client.Helpers
{
    public interface IAppointmentSummarizeService 
    {
        AppointmentSummary UpdateSummarize(AppointmentSummary summary, List<DisplayOfferModel> offers);
        EditAppointmentModel SummarizeAppointment(EditAppointmentModel appointment, DateTime selectedDate, AppointmentSummary summary);
    }

    public class AppointmentSummarizeService : IAppointmentSummarizeService
    {
        public AppointmentSummary UpdateSummarize(AppointmentSummary summary, List<DisplayOfferModel> offers)
        {
            summary.SummarizedCost = 0;
            summary.SummarizedTime = 0;

            foreach (var offer in offers)
            {
                summary.SummarizedTime += offer.TimeInMinutes;
                summary.SummarizedCost += offer.Price;
            }

            return summary;
        }

        public EditAppointmentModel SummarizeAppointment(EditAppointmentModel appointment, DateTime selectedDate, AppointmentSummary summary)
        {
            appointment.StartDate = selectedDate;
            appointment.Time = summary.AppointmentTime;
            var endTime = summary.AppointmentTime.Add(TimeSpan.FromMinutes(summary.SummarizedTime));
            appointment.EndDate = selectedDate + endTime;

            return appointment;
        }
    }
}
