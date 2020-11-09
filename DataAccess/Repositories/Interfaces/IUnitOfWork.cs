using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IUserRepository UserRepository { get; }
		
	}
}
