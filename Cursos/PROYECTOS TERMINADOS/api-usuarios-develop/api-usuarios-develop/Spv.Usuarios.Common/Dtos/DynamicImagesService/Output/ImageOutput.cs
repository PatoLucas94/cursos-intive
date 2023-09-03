namespace Spv.Usuarios.Common.Dtos.DynamicImagesService.Output
{
    public class ImageOutput
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int Orden { get; set; }

        public byte[] Imagen { get; set; }
    }
}
