using CompanyManager.Server.Data;
using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Repositories
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAppointmentsInRange(DateTime startDate, DateTime endDate);
        Task<bool> AddAppointment(Appointment appointment);
    }

    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Appointment>> GetAppointmentsInRange(DateTime startDate, DateTime endDate)
        {
            var appointments = await _dbContext.Appointments.Where(a => (a.StartDate <= startDate && a.EndDate > startDate) || (a.StartDate < endDate && a.EndDate >= endDate)).ToListAsync();
            return appointments;
        }

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            _dbContext.Appointments.Add(appointment);
            var result = await _dbContext.SaveChangesAsync() > 0;

            return result;
        } 
    }
}
