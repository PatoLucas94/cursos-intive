using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Dtos.Service.DynamicImagesService.Output;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Helpers.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Utils;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Helpers;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class DynamicImagesService : IDynamicImagesService
    {
        private readonly ILogger<DynamicImagesService> _logger;
        private readonly IApiUsuariosRepositoryV2 _apiUsuariosRepositoryV2;
        private readonly IMemoryCache _memoryCache;
        private readonly IApiUsuariosHelper _apiUsuariosHelper;

        public DynamicImagesService(
            ILogger<DynamicImagesService> logger,
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2,
            IApiUsuariosHelper apiUsuariosHelper,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _apiUsuariosRepositoryV2 = apiUsuariosRepositoryV2;
            _apiUsuariosHelper = apiUsuariosHelper;
            _memoryCache = memoryCache;
        }

        public async Task<IResponse<List<ImagenLoginModelOutput>>> ObtenerImagenesLoginAsync()
        {
            try
            {
                _memoryCache.TryGetValue(
                    Cache.DynamicImages.ObtenerImagenesLogin,
                    out List<ImagenLoginModelOutput> imagesOutput
                );

                imagesOutput ??= new List<ImagenLoginModelOutput>();

                if (imagesOutput.Count != 0)
                    return Responses.Ok(imagesOutput);

                var imagenesLogin = await _apiUsuariosRepositoryV2.ObtenerImagenesLoginAsync();
                imagesOutput = await UsuariosHelper.DeserializarImagenLoginAsync(imagenesLogin);

                if (imagesOutput?.Count > 0)
                {
                    _memoryCache.Set(
                        Cache.DynamicImages.ObtenerImagenesLogin,
                        imagesOutput,
                        _apiUsuariosHelper.ExpirationCacheImagenesLogin()
                    );
                }

                return Responses.Ok(imagesOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingObtenerImagenesLoginAsync, ex.Message, ex);
                throw;
            }
        }

        public Task<IResponse<List<ImagenLoginModelOutput>>> ObtenerImagenesLoginAsync(IRequestBody<string> LoginModel)
        {
            return ObtenerImagenesLoginAsync();
        }
    }
}
