using CompanyManager.Server.Data;
using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Repositories
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAppointmentsInRangeHourlyAccuracy(DateTime startDate, DateTime endDate);
        Task<bool> AddAppointment(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsInRangeDailyAccuracy(DateTime startDate, DateTime endDate);
        Task<bool> DeleteAppointment(int id);
    }

    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Appointment>> GetAppointmentsInRangeHourlyAccuracy(DateTime startDate, DateTime endDate)
        {
            var appointments = await _dbContext.Appointments
                .Where(a => (a.StartDate < startDate && a.EndDate > startDate) || (a.StartDate < endDate && a.EndDate > endDate))
                .ToListAsync();
            return appointments;
        }

        public async Task<List<Appointment>> GetAppointmentsInRangeDailyAccuracy(DateTime startDate, DateTime endDate)
        {
            var appointments = await _dbContext.Appointments
                .Where(a => a.StartDate.Date >= startDate && a.StartDate.Date <= endDate)
                .Include(a => a.Customer)
                .Include(a => a.Offers)
                .ToListAsync();
            return appointments;
        }

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            _dbContext.Appointments.Add(appointment);
            var result = await _dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> DeleteAppointment(int id)
        {
            var appoitment = _dbContext.Appointments.FirstOrDefault(a => a.Id == id);
            if(appoitment == null) return false;

            _dbContext.Appointments.Remove(appoitment);
            var result = await _dbContext.SaveChangesAsync() > 0;

            return result;
        }
    }
}
