using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Data.Repositories
{
	public class ConsultaLogRepository : Repository<ConsultaLog>, IConsultaLogRepository
	{
		public ConsultaLogRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}
	}
}
