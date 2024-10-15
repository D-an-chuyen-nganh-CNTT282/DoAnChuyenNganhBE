using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace DoAnChuyenNganh.Services.Configs
{
    public class LecturerManager
    {
        private readonly DatabaseContext _context;
        public LecturerManager(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<Lecturer?> FindByEmailAsync(string email)
        {
            return await _context.Lecturer.FirstOrDefaultAsync(l => l.LecturerEmail == email);
        }
        public async Task CreateIfNotExistsAsync(string lecturerName, string dayOfBirth, string lecturerGender, string email, string lecturerPhone, string lectuerAddress, string expertise, string? personalWebsiteLink)
        {
            Lecturer? existingLecturer = await FindByEmailAsync(email);
            if (existingLecturer is null)
            {
                Lecturer newLecturer = new Lecturer
                {
                    LecturerName = lecturerName,
                    DayOfBirth = DateTime.Parse(dayOfBirth),
                    LecturerGender = lecturerGender,
                    LecturerEmail = email,
                    LecturerPhone = lecturerPhone,
                    LecturerAddress = lectuerAddress,
                    Expertise = expertise,
                    PersonalWebsiteLink = personalWebsiteLink
                };
                _context.Lecturer.Add(newLecturer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
