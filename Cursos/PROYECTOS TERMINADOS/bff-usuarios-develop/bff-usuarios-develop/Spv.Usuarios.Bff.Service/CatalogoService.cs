using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class CatalogoService : ICatalogoService
    {
        private readonly ILogger<CatalogoService> _logger;
        private readonly IApiCatalogoRepository _catalogoRepository;

        public CatalogoService(ILogger<CatalogoService> logger, IApiCatalogoRepository catalogoRepository)
        {
            _logger = logger;
            _catalogoRepository = catalogoRepository;
        }

        public async Task<IResponse<List<PaisModelOutput>>> ObtenerPaisesAsync(IRequestBody<PaisesModelInput> paisesModel)
        {
            try
            {
                var paises = await _catalogoRepository.ObtenerPaisesAsync();

                if (paises == null) return Responses.NotFound<List<PaisModelOutput>>(ErrorCode.PaisesInexistentes.ErrorDescription);

                var result = new List<PaisModelOutput>();
                paises.ForEach(pais => {
                    var p = new PaisModelOutput
                    {
                        codigo = pais.codigo,
                        descripcion = pais.descripcion
                    };
                    result.Add(p);
                });

                return Responses.Ok(result.OrderBy(r => r.descripcion).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(CatalogoServiceEvents.ExceptionCallingObtenerPaises, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<List<TipoDocumentoModelOutput>>> ObtenerTiposDocumentoAsync(IRequestBody<TiposDocumentoModelInput> tiposDocumentoModel)
        {
            try
            {
                var tiposDocumento = await _catalogoRepository.ObtenerTiposDocumentoAsync();

                if (tiposDocumento == null) return Responses.NotFound<List<TipoDocumentoModelOutput>>(ErrorCode.TiposDocumentoInexistentes.ErrorDescription);

                var result = new List<TipoDocumentoModelOutput>();
                tiposDocumento.Where(tipoDoc => tipoDoc.tipoPersonaQueAplica.ToUpper() == "F").ToList()
                    .ForEach(tipoDocumento => {
                    var td = new TipoDocumentoModelOutput
                    {
                        codigo = tipoDocumento.codigo,
                        descripcion = tipoDocumento.descripcion
                    };
                    result.Add(td);
                });

                return Responses.Ok(result.OrderBy(r => r.descripcion).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(CatalogoServiceEvents.ExceptionCallingObtenerTiposDocumento, ex.Message, ex);
                throw;
            }
        }
    }
}
