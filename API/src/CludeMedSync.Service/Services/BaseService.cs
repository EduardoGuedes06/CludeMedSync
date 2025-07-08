using AutoMapper;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Models.Response;

namespace CludeMedSync.Service.Services
{
	public abstract class BaseService<T> where T : class
	{
		protected readonly IMapper _mapper;
		protected readonly IPagedResultRepository<T> _pagedRepository;

		protected BaseService(IMapper mapper, IPagedResultRepository<T> pagedRepository)
		{
			_mapper = mapper;
			_pagedRepository = pagedRepository;
		}

		public async Task<object> ObterPaginadoGenericoAsync(
			int page,
			int pageSize,
			object? filtros = null,
			string? orderBy = null,
			bool orderByDesc = false,
			bool ativo = true,
			Type tipoDto = null!)
		{

			var dadosPaginados = await _pagedRepository.ObterPaginadoGenericoAsync(page, pageSize, filtros, orderBy, ativo, orderByDesc);

			var dadosPaginadosResponse = _mapper.Map(dadosPaginados, dadosPaginados.GetType(), typeof(PagedResultResponse<>).MakeGenericType(tipoDto));

			return dadosPaginadosResponse;
		}

	}

}
