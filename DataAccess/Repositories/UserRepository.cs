using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;
		public UserRepository(AppDbContext context)
		{
			_context = context;
		}

		#region Patient
		public async Task<List<PatientModel>> GetAllPatient()
		{
			List<PatientModel> list = await (from u in _context.Patients
										  select new PatientModel
										  {
											  Id = u.Id,
											  Email = u.Email,
											  PhoneNumber = u.PhoneNumber,
											  FirstName = u.FirstName,
											  LastName = u.LastName,
											 
										  }).ToListAsync();
			
			return list;
		}
	
		public async Task<PatientModel>  GetPatientById(int Id)
		{
			PatientModel item = await (from u in _context.Patients
							  where u.Id == Id
							  select new PatientModel
							  {
								  Id = u.Id,
								  FirstName = u.FirstName,
								  LastName = u.LastName,
								  Email = u.Email,
								  PhoneNumber = u.PhoneNumber,
								 
							  }).FirstOrDefaultAsync();
			item.Isolateds = await (from j in _context.Isolateds
							  where j.PatientId == Id
							  select new IsolatedModel
							  {
								  Id = j.Id,
								  FirstName = j.FirstName,
								  LastName = j.LastName,
								  Email = j.Email,
								  PhoneNumber = j.PhoneNumber,

							  }).ToListAsync();
			return item;
		}

		public async Task AddPatient(PatientModel model)
		{
			Patient patient = new Patient();
			patient.LastName = model.LastName;
			patient.FirstName = model.FirstName;
			patient.PhoneNumber = model.PhoneNumber;
			patient.Email = model.Email;
		
			_context.Patients.Add(patient);
			if (model.Isolateds?.Count > 0)
			{
				foreach (var item in model.Isolateds)
				{
					Isolated isolated = new Isolated();
					isolated.Email = item.Email;
					isolated.FirstName = item.FirstName;
					isolated.LastName = item.LastName;
					isolated.PhoneNumber = item.PhoneNumber;
					patient.Isolateds.Add(isolated);
				}

			}
			await _context.SaveChangesAsync();
		}
		public async Task EditPatient(PatientModel model)
		{
			var patient = await (from c in _context.Patients
							 where c.Id == model.Id
							 select c).FirstOrDefaultAsync();
			if (patient != null)
			{
				patient.FirstName  = model.FirstName;
				patient.LastName = model.LastName;
				patient.PhoneNumber = model.PhoneNumber;
				patient.Email = model.Email;
			
				await _context.SaveChangesAsync();
			}
		}
		#endregion

		#region Isolated

		public async Task<List<IsolatedModel>> GetAllIsolated()
		{
			var customerlist = await (from u in _context.Patients
									  select new IsolatedModel
									  {
										  Id = u.Id,
										  Email = u.Email,
										  PhoneNumber = u.PhoneNumber,
										  FirstName = u.FirstName,
										  LastName = u.LastName,

									  }).ToListAsync();

			return customerlist;
		}

		public async Task EditIsolated(IsolatedModel model)
		{
			var isolated = await (from c in _context.Isolateds
								 where c.Id == model.Id
								 select c).FirstOrDefaultAsync();
			if (isolated != null)
			{
				isolated.FirstName = model.FirstName;
				isolated.LastName = model.LastName;
				isolated.PhoneNumber = model.PhoneNumber;
				isolated.Email = model.Email;

				await _context.SaveChangesAsync();
			}
		}
		#endregion



	}
}
