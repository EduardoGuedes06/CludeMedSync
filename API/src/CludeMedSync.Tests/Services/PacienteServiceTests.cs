using AutoMapper;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Domain.Models.Utils.Enums;
using CludeMedSync.Models.Request;
using CludeMedSync.Service.Mappings;
using CludeMedSync.Service.Services;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CludeMedSync.Tests.Services
{
	public class PacienteServiceTests
	{
		private readonly Mock<IPacienteRepository> _pacienteRepoMock;
		private readonly Mock<IConsultaRepository> _consultaRepoMock;
		private readonly Mock<IPagedResultRepository<Paciente>> _pagedRepoMock;
		private readonly IMapper _mapper;
		private readonly PacienteService _service;

		public PacienteServiceTests()
		{
			var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
			_mapper = config.CreateMapper();

			_pacienteRepoMock = new Mock<IPacienteRepository>();
			_consultaRepoMock = new Mock<IConsultaRepository>();
			_pagedRepoMock = new Mock<IPagedResultRepository<Paciente>>();

			_service = new PacienteService(
				_pacienteRepoMock.Object,
				_consultaRepoMock.Object,
				_mapper,
				_pagedRepoMock.Object
			);
		}

		[Fact]
		public async Task CreateAsync_DeveCriarPaciente_QuandoDadosNaoDuplicados()
		{

			var data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
			var request = new PacienteRequest("Paciente Novo", data, "12345678901", "paciente@email.com", "999999999");

			_pacienteRepoMock.Setup(r => r.VerificarDuplicidadePacienteAsync(request.CPF, request.Email, request.Telefone))
				.ReturnsAsync((false, null));

			_pacienteRepoMock.Setup(r => r.AddAsync(It.IsAny<Paciente>())).ReturnsAsync(1);

			var resultado = await _service.CreateAsync(request);

			resultado.Sucesso.Should().BeTrue();
			resultado.Dados.Should().NotBeNull();
			resultado.Dados?.NomeCompleto.Should().Be("Paciente Novo");
		}

		[Fact]
		public async Task DeleteAsync_DeveFalhar_QuandoPacienteTemConsultaVinculada()
		{
			var pacienteId = 1;

			_pacienteRepoMock.Setup(r => r.GetByIdAsync(pacienteId)).ReturnsAsync(new Paciente { Id = pacienteId });
			_consultaRepoMock.Setup(r => r.GetByRelationShip(nameof(Consulta.PacienteId), pacienteId.ToString(), It.IsAny<EnumTipoAtributo>()))
				.ReturnsAsync(new Consulta());

			var resultado = await _service.DeleteAsync(pacienteId);
			resultado.Sucesso.Should().BeFalse();
			resultado.Mensagem.Should().Be("Não é possível excluir o paciente. Consulta vinculada encontrada.");
			_pacienteRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
		}
	}
}