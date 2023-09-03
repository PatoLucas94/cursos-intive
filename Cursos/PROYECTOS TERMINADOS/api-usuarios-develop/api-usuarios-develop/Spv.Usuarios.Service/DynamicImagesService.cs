using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Spv.Usuarios.Common.Dtos.DynamicImagesService.Output;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Service.Interface;

namespace Spv.Usuarios.Service
{
    public class DynamicImagesService : IDynamicImagesService
    {
        private readonly IDynamicImagesLoginRepository _dynamicImagesLoginRepository;
        private readonly IDynamicImagesRepository _dynamicImagesRepository;
        public DynamicImagesService(IDynamicImagesLoginRepository dynamicImagesLoginRepository,
                                    IDynamicImagesRepository dynamicImagesRepository)
        {
            _dynamicImagesLoginRepository = dynamicImagesLoginRepository;
            _dynamicImagesRepository = dynamicImagesRepository;
        }

        public async Task<IResponse<List<ImageOutput>>> ObtenerImagesLoginAsync(IRequestBody<bool> habilitado)
        {
            var imagesOutput = new List<ImageOutput>();
            var imagesLogin = await _dynamicImagesLoginRepository.ObtenerImagesLogin(habilitado.Body);

            foreach (var item in imagesLogin)
            {
                var imagen = await _dynamicImagesRepository.ObtenerImages(item.IdImagen);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<DynamicImages, ImageOutput>());
                var mapper = config.CreateMapper();

                var imagenOutput = mapper.Map <ImageOutput> (imagen);
                imagenOutput.Orden = item.Orden;
                imagenOutput.Nombre = item.Nombre;

                imagesOutput.Add(imagenOutput);
            }

            return Responses.Ok(imagesOutput);
        }
    }
}
