using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataModels;

namespace DataAccess.Repositories.Interfaces
{
	public interface IUserRepository
	{
		#region Patient
		Task<List<PatientModel>> GetAllPatient();
		Task<PatientModel> GetPatientById(int Id);
		Task AddPatient(PatientModel model);
		Task EditPatient(PatientModel model);
		//todo
		//Task DeletePatient(int id);
		#endregion

		#region Isolated
		Task<List<IsolatedModel>> GetAllIsolated();
		//todo
		//Task EditPatient(IsolatedModel model);
		
		//Task DeleteIsolated(int id);
	
	
		#endregion

	}
}
