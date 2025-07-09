
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Models.Request;
using CludeMedSync.Service.Services;
using Moq;
using CludeMedSync.Service.Mappings;
using Xunit;
using AutoMapper;


namespace CludeMedSync.Tests.Services
{
	public class ProfissionalServiceTests
	{
		private readonly Mock<IProfissionalRepository> _profissionalRepoMock = new();
		private readonly Mock<IConsultaRepository> _consultaRepoMock = new();
		private readonly Mock<IPagedResultRepository<Profissional>> _pagedRepoMock = new();
		private readonly IMapper _mapper;

		private readonly ProfissionalService _service;

		public ProfissionalServiceTests()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MappingProfile());
			});

			_mapper = config.CreateMapper();

			_service = new ProfissionalService(
				_profissionalRepoMock.Object,
				_consultaRepoMock.Object,
				_mapper,
				_pagedRepoMock.Object
			);
		}


		[Fact]
		public async Task CreateAsync_DeveRetornarErro_QuandoProfissionalDuplicado()
		{
			var request = new ProfissionalRequest(
				NomeCompleto: "João",
				Especialidade: "Clínico Geral",
				CRM: "123456",
				Email: "joao@email.com",
				Telefone: "999999999"
			);


			_profissionalRepoMock
				.Setup(r => r.VerificarDuplicidadeProfissionalAsync(
					request.CRM, request.Email, request.Telefone))
				.ReturnsAsync((true, "Profissional já existe"));

			var result = await _service.CreateAsync(request);

			Assert.False(result.Sucesso);
			Assert.Equal("Profissional já existe", result.Mensagem);
		}

		[Fact]
		public async Task CreateAsync_DeveCriarProfissional_QuandoValido()
		{
			var request = new ProfissionalRequest(
				NomeCompleto: "Maria",
				Especialidade: "Cardiologista",
				CRM: "654321",
				Email: "maria@email.com",
				Telefone: "888888888"
			);


			_profissionalRepoMock
				.Setup(r => r.VerificarDuplicidadeProfissionalAsync(
					request.CRM, request.Email, request.Telefone))
				.ReturnsAsync((false, null));

			_profissionalRepoMock
				.Setup(r => r.AddAsync(It.IsAny<Profissional>()))
				.ReturnsAsync(1);

			var result = await _service.CreateAsync(request);

			Assert.True(result.Sucesso);
			Assert.Equal("Profissional cadastrado com sucesso.", result.Mensagem);
			Assert.NotNull(result.Dados);
			Assert.Equal(1, result.Dados?.Id);
		}
	}
}
