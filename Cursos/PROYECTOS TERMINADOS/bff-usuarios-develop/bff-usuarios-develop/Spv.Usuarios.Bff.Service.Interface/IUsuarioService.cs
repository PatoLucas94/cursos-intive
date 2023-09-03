using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Interface
{
    public interface IUsuarioService
    {
        Task<IResponse<PerfilModelOutput>> ObtenerPerfilAsync(IRequestBody<PerfilModelInput> perfilModel);

        Task<IResponse<PerfilModelOutputV2>> ObtenerPerfilAsync(IRequestBody<PerfilModelInputV2> perfilModel);

        Task<IResponse<ValidacionExistenciaModelOutput>> ValidarExistenciaAsync(
            IRequestBody<ValidacionExistenciaModelInput> validacionExistenciaModel
        );

        Task<IResponse<ValidacionExistenciaHbiModelOutput>> ValidarExistenciaHbiAsync(
            IRequestBody<ValidacionExistenciaHbiModelInput> validacionExistenciaHbiModel
        );

        Task<IResponse<RegistracionModelOutput>> RegistrarAsync(IRequestBody<RegistracionModelInput> registracionModel);

        Task<IResponse<CambioDeCredencialesModelOutput>> ModificarCredencialesAsync(
            IRequestBody<CambioDeCredencialesModelInput> cambioDeCredencialesModel
        );

        Task<IResponse<ValidacionExistenciaModelOutput>> RecuperarUsuarioAsync(
            IRequestBody<RecuperarUsuarioModelInput> recuperarUsuario
        );

        Task<IResponse<CambioDeClaveModelOutput>> ModificarClaveAsync(
            IRequestBody<CambioDeClaveModelInput> cambioDeClaveModel
        );

        Task<IResponse<MigracionModelOutput>> MigrarAsync(IRequestBody<MigracionModelInput> migracionModel);
    }
}
