using AutoMapper;
using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Entities.Utils.Enums;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Service.Interfaces;
using CludeMedSync.Service.Mappings;
using CludeMedSync.Service.Models.Request;
using CludeMedSync.Service.Services;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CludeMedSync.Tests.Services
{
	public class ConsultaServiceTests
	{
		private readonly Mock<IConsultaRepository> _consultaRepoMock;
		private readonly Mock<IPacienteRepository> _pacienteRepoMock;
		private readonly Mock<IProfissionalRepository> _profissionalRepoMock;
		private readonly Mock<IConsultaLogRepository> _consultaLogRepoMock;
		private readonly Mock<IPagedResultRepository<Consulta>> _pagedRepoMock;
		private readonly IMapper _mapper;
		private readonly ConsultaService _service;

		public ConsultaServiceTests()
		{
			var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
			_mapper = config.CreateMapper();

			_consultaRepoMock = new Mock<IConsultaRepository>();
			_pacienteRepoMock = new Mock<IPacienteRepository>();
			_profissionalRepoMock = new Mock<IProfissionalRepository>();
			_consultaLogRepoMock = new Mock<IConsultaLogRepository>();
			_pagedRepoMock = new Mock<IPagedResultRepository<Consulta>>();

			_service = new ConsultaService(
				_consultaRepoMock.Object,
				_pacienteRepoMock.Object,
				_profissionalRepoMock.Object,
				_consultaLogRepoMock.Object,
				_mapper,
				_pagedRepoMock.Object
			);
		}

		[Fact]
		public async Task AgendarAsync_DeveAgendarComSucesso_QuandoDadosValidos()
		{
			var request = new AgendarConsultaRequest(
				PacienteId: 1,
				ProfissionalId: 1,
				DataHoraInicio: new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0).AddDays(1),
				Motivo: "Check-up",
				Observacao: null
			);
			var usuarioId = Guid.NewGuid();

			_pacienteRepoMock.Setup(r => r.GetByIdAsync(request.PacienteId)).ReturnsAsync(new Paciente { Id = 1, NomeCompleto = "Paciente Teste" });
			_profissionalRepoMock.Setup(r => r.GetByIdAsync(request.ProfissionalId)).ReturnsAsync(new Profissional { Id = 1, NomeCompleto = "Dr. Teste" });
			_consultaRepoMock.Setup(r => r.ExisteConsultaNoMesmoDiaAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(false);
			_consultaRepoMock.Setup(r => r.ExisteConsultaNoMesmoHorarioAsync(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(false);
			_consultaRepoMock.Setup(r => r.AddAsync(It.IsAny<Consulta>())).ReturnsAsync(100);
			var resultado = await _service.AgendarAsync(request, usuarioId);

			resultado.Sucesso.Should().BeTrue();
			resultado.Mensagem.Should().Be("Consulta agendada com sucesso.");
			resultado.Dados.Should().NotBeNull();
			resultado.Dados?.Id.Should().Be(100);
			_consultaLogRepoMock.Verify(r => r.AddAsync(It.IsAny<ConsultaLog>()), Times.Once);
		}

		[Fact]
		public async Task AgendarAsync_DeveFalhar_QuandoPacienteNaoEncontrado()
		{
			var dataAgendamento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
			var request = new AgendarConsultaRequest(1, 1, dataAgendamento, null, null);
			_pacienteRepoMock.Setup(r => r.GetByIdAsync(request.PacienteId)).ReturnsAsync((Paciente)null);

			var resultado = await _service.AgendarAsync(request, Guid.NewGuid());

			resultado.Sucesso.Should().BeFalse();
			resultado.Mensagem.Should().Be("Paciente não encontrado");
			resultado.Status.Should().Be(404);
		}

		[Fact]
		public async Task ConfirmarAsync_DeveAlterarStatus_QuandoValido()
		{
			var dataAgendamento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
			var consultaId = 1;
			var usuarioId = Guid.NewGuid();
			var consulta = Consulta.Agendar(usuarioId, 1, 1, dataAgendamento, "Motivo", null);
			var paciente = new Paciente { Id = 1, NomeCompleto = "Paciente" };
			var profissional = new Profissional { Id = 1, NomeCompleto = "Profissional" };

			_consultaRepoMock.Setup(r => r.GetByIdComAgregadosAsync(consultaId)).ReturnsAsync((consulta, paciente, profissional));
			_consultaRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Consulta>())).ReturnsAsync(true);

			var resultado = await _service.ConfirmarAsync(consultaId, usuarioId);

			resultado.Sucesso.Should().BeTrue();
			resultado.Mensagem.Should().Be("Status da consulta alterado com sucesso.");
			consulta.Status.Should().Be((int)EnumStatusConsulta.Confirmada);
			_consultaRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Consulta>()), Times.Once);
		}
	}
}