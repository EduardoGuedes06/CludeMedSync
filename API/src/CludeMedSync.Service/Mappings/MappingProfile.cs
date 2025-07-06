using AutoMapper;
using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Entities.Utils.Enums;
using CludeMedSync.Domain.Models;
using CludeMedSync.Service.DTOs;

namespace CludeMedSync.Service.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Paciente, PacienteDto>();
			CreateMap<CreatePacienteDto, Paciente>();

			CreateMap<Profissional, ProfissionalDto>();
			CreateMap<CreateProfissionalDto, Profissional>();

			CreateMap<Consulta, ConsultaDto>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((EnumStatusConsulta)src.Status).ToString()));
			CreateMap<AgendarConsultaDto, Consulta>();
			CreateMap<AtualizarConsultaDto, Consulta>();

			CreateMap<ConsultaLog, ConsultaLogDto>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((EnumStatusConsulta)src.Status).ToString()));
		}
	}
}
