using AutoMapper;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Common.ExternalResponses.Abstract;
using Spv.Usuarios.Bff.Domain.Errors;
using Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Input;
using Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Output;
using Spv.Usuarios.Bff.ViewModels.RhSsoController.CommonRhSso.Output;
using Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output;

namespace Spv.Usuarios.Bff.MappingProfiles
{
    /// <summary>
    /// AppMappingProfile
    /// </summary>
    public class AppMappingProfile : Profile
    {
        /// <summary>
        /// AppMappingProfile
        /// </summary>
        public AppMappingProfile()
        {
            CreateMap<ValidacionExistenciaModelInput, RecuperarUsuarioModelInput>().ReverseMap();
            CreateMap<ValidacionExistenciaModelOutput, RecuperarUsuarioModelResponse>().ReverseMap();
            CreateMap<TokenModelOutput, TokenModelResponse>().ReverseMap();
            CreateMap<AceptadosModelOutput, ApiTyCAceptadosModelOutput>().ReverseMap();
            CreateMap<AceptadosModelOutput, ApiTyCVigenteModelOutput>().ReverseMap();
            CreateMap<ApiTyCAceptadosModelOutput, ApiTyCAceptacionModelOutput>().ReverseMap();

            CreateMap<AceptadosModelOutput, AceptadosModelResponse>()
                .ForMember(dest => dest.VigenciaDesde, opt => opt.MapFrom(src => src.vigencia_desde))
                .ReverseMap();

            CreateMap<AutenticacionModelInput, ApiUsuariosAutenticacionV2ModelInput>()
                .ForMember(dest => dest.clave, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.nro_documento, opt => opt.MapFrom(src => src.DocumentNumber))
                .ForMember(dest => dest.usuario, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();

            CreateMap<ApiGenericError, InternalCodeAndDetailErrors>()
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Detalle))
                .ForMember(dest => dest.InternalCode, opt => opt.MapFrom(src => src.Codigo))
                .ReverseMap();

            CreateMap<BiometriaAutenticacionModelRequest, BiometriaAutenticacionModelInput>();
            CreateMap<DatosBiometricosModelRequest, DatosBiometricosModelInput>();
            CreateMap<BiometriaAutenticacionModelOutput, BiometriaAutenticacionModelResponse>();
            CreateMap<IdentificacionDigitalOutput, IdentificacionDigitalResponse>();
        }
    }
}
