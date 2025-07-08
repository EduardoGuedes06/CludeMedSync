using AutoMapper;
using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Entities.Pagination;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Domain.Entities.Utils.Enums;
using CludeMedSync.Domain.Models;
using CludeMedSync.Models.Request;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Models.Request;
using CludeMedSync.Service.Models.Response;

namespace CludeMedSync.Service.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Paciente, PacienteResponse>();
			CreateMap<PacienteRequest, Paciente>();

			CreateMap<Profissional, ProfissionalResponse>();
			CreateMap<ProfissionalRequest, Profissional>();

			CreateMap<Consulta, ConsultaResponse>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((EnumStatusConsulta)src.Status).ToString()));

			CreateMap<AgendarConsultaRequest, Consulta>();

			CreateMap<AtualizarConsultaRequest, Consulta>();

			CreateMap<ConsultaLog, ConsultaLogResponse>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((EnumStatusConsulta)src.Status).ToString()));

			//CreateMap<ConsultaCompleta, ConsultaResponse>()
			//	.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

			//CreateMap<ConsultaLogCompleta, ConsultaLogResponse>()
			//	.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

			CreateMap(typeof(PagedResult<>), typeof(PagedResultResponse<>))
				.ForMember("Items", opt => opt.MapFrom("Items"));
		}

	}
}
