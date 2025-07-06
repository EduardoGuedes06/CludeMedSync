using CludeMedSync.Domain.Entities.Utils.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Data.Extensions
{
	public class EnumStatusConsultaTypeHandler : SqlMapper.TypeHandler<EnumStatusConsulta>
	{

		public override EnumStatusConsulta Parse(object value)
		{
			if (value == null || value is DBNull)
			{
				throw new ArgumentNullException(nameof(value), "Não é possível converter um valor nulo para EnumStatusConsulta.");
			}

			try
			{
				var intValue = Convert.ToInt32(value);
				return (EnumStatusConsulta)intValue;
			}
			catch (Exception ex)
			{
				throw new InvalidCastException($"Não foi possível converter o valor '{value}' do tipo '{value.GetType()}' para EnumStatusConsulta.", ex);
			}
		}

		public override void SetValue(IDbDataParameter parameter, EnumStatusConsulta value)
		{
			parameter.Value = (int)value;
		}
	}
}
