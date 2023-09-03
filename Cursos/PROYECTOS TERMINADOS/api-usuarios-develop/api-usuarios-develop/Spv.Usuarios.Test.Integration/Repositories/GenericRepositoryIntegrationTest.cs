using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSScriptLib;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;
using static Spv.Usuarios.Common.Testing.Attributes.PriorityAttribute;

namespace Spv.Usuarios.Test.Integration.Repositories
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    [TestCaseOrderer("Spv.Usuarios.Common.Testing.PriorityOrderer", "Spv.Usuarios.Common")]
    public class GenericRepositoryIntegrationTest
    {
        private readonly GenericRepository<Usuario> _genericRepository;

        private int TotalUsuarios => _genericRepository.All().Count();
        private List<int> IdsUsuarios { get; }

        public GenericRepositoryIntegrationTest(ServerFixture server)
        {
            var db = server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContext>();

            _genericRepository = new GenericRepository<Usuario>(db);

            IdsUsuarios = _genericRepository.All().Select(x => x.UserId).ToList();
        }

        [Fact, TestPriority(0)]
        public void All()
        {
            // Act
            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(TotalUsuarios);
        }

        [Fact, TestPriority(1)]
        public void Filter()
        {
            // Act
            var usuarios = _genericRepository.Filter(u => u.UserId == IdsUsuarios.First());
            var noExistenUsuarios = _genericRepository.Filter(u => u.UserId == 0);

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(1);

            noExistenUsuarios.Should().BeEmpty();
            noExistenUsuarios.Count().Should().Be(0);

            var usuario = usuarios.First();

            usuario.UserId.Should().Be(IdsUsuarios.First());
        }

        [Fact, TestPriority(2)]
        public void Filter_Paged()
        {
            // Arrange

            // Act
            var usuarios = _genericRepository.Filter(u => u.UserId > 0, out int total, 0, 3);

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(3);

            usuarios.AsEnumerable().ForEach(u => u.UserId.Should().BeOneOf(IdsUsuarios.GetRange(0, 3)));
            total.Should().Be(3);
        }

        [Fact, TestPriority(3)]
        public void Contains()
        {
            // Act
            var contains = _genericRepository.Contains(u => u.UserId == IdsUsuarios.First());
            var notContains = _genericRepository.Contains(u => u.UserId == 0);

            // Assert
            contains.Should().BeTrue();
            notContains.Should().BeFalse();
        }

        [Fact, TestPriority(4)]
        public void Find_ByPrimaryKey()
        {
            // Act
            var usuario = _genericRepository.Find(IdsUsuarios.First());
            var usuarioInexistente = _genericRepository.Find(0);

            // Assert
            usuario.Should().NotBeNull();
            usuario.UserId.Should().Be(IdsUsuarios.First());

            usuarioInexistente.Should().BeNull();
        }

        [Fact, TestPriority(5)]
        public void Find()
        {
            // Act
            var usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.First());
            var usuarioInexistente = _genericRepository.Find(u => u.UserId == 0);

            // Assert
            usuario.Should().NotBeNull();
            usuario.UserId.Should().Be(IdsUsuarios.First());

            usuarioInexistente.Should().BeNull();
        }

        [Fact, TestPriority(6)]
        public void Find_Tracking()
        {
            // Act
            var usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.First(), true);
            var usuarioInexistente = _genericRepository.Find(u => u.UserId == 0);

            // Assert
            usuario.Should().NotBeNull();
            usuario.UserId.Should().Be(IdsUsuarios.First());

            usuarioInexistente.Should().BeNull();

            var nombreOriginal = usuario.UserName;
            usuario.UserName = "Nombre_no_trackeado";

            _genericRepository.SaveChanges();

            usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.First());

            usuario.Should().NotBeNull();
            usuario.UserName.Should().NotBe("Nombre_no_trackeado");
            usuario.UserName.Should().Be(nombreOriginal);
        }

        [Fact, TestPriority(7)]
        public void Add()
        {
            // Arrange
            var totalUsuarios = TotalUsuarios;
            var nuevoUsuario = new Usuario { UserName = "UsuarioTest" + IdsUsuarios.Max(), Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", UserStatusId = (byte)UserStatus.Active };

            // Act
            _genericRepository.Add(nuevoUsuario);

            _genericRepository.SaveChanges();

            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(totalUsuarios + 1);
        }

        [Fact, TestPriority(8)]
        public void AddObject()
        {
            // Arrange
            var totalUsuarios = TotalUsuarios;
            var nuevoUsuario = new Usuario { UserName = "UsuarioTest" + IdsUsuarios.Max(), Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", UserStatusId = (byte)UserStatus.Active };

            // Act
            _genericRepository.AddObject(nuevoUsuario);

            _genericRepository.SaveChanges();

            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(totalUsuarios + 1);
        }

        [Fact, TestPriority(9)]
        public void Update()
        {
            // Arrange
            var usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.Max());

            // Act
            usuario.UserName = "NombreModificado";

            _genericRepository.Update(usuario);

            _genericRepository.SaveChanges();

            usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.Max());

            // Assert
            usuario.Should().NotBeNull();
            usuario.UserName.Should().Be("NombreModificado");
        }

        [Fact, TestPriority(10)]
        public void UpdateObject()
        {
            // Arrange
            var usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.Max());

            // Act
            usuario.UserName = "NombreModificado2";

            _genericRepository.Update(usuario);

            _genericRepository.SaveChanges();

            usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.Max());

            // Assert
            usuario.Should().NotBeNull();
            usuario.UserName.Should().Be("NombreModificado2");
        }

        [Fact, TestPriority(11)]
        public void Delete()
        {
            // Arrange
            var totalUsuarios = TotalUsuarios;
            var usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.Max());

            // Act
            _genericRepository.Delete(usuario);

            _genericRepository.SaveChanges();

            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(totalUsuarios - 1);

            var ids = IdsUsuarios.GetRange(0, TotalUsuarios);
            ids.Add(IdsUsuarios.Max() + 2);

            usuarios.AsEnumerable().ForEach(u => u.UserId.Should().BeOneOf(ids));
        }

        [Fact, TestPriority(12)]
        public void Delete_Expression()
        {
            // Arrange
            _genericRepository.Find(u => u.UserId == IdsUsuarios.Max() + 2);

            // Act
            _genericRepository.Delete(u => u.UserId == IdsUsuarios.Max() + 2);

            _genericRepository.SaveChanges();

            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(TotalUsuarios);

            usuarios.AsEnumerable().ForEach(u => u.UserId.Should().BeOneOf(IdsUsuarios.GetRange(0, TotalUsuarios)));
        }

        [Fact, TestPriority(13)]
        public void Get()
        {
            // Act
            var usuarios = _genericRepository.Get(u => u.UserId >= 1, o => o.OrderByDescending(u => u.UserId)).ToList();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count.Should().Be(TotalUsuarios);

            var userid = TotalUsuarios;
            var size = usuarios.Count;
            
            for (var i = 0; i < size; i++)
            {
                var usuario = usuarios.ElementAt(i);
                usuario.UserId.Should().Be(userid--);
            }
        }

        [Fact, TestPriority(14)]
        public async Task AddAsync()
        {
            // Arrange
            var totalUsuarios = TotalUsuarios;
            var nuevoUsuario01 = new Usuario { UserName = "UsuarioTest" + (IdsUsuarios.Max() + 2), Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", UserStatusId = (byte)UserStatus.Active };
            var nuevoUsuario02 = new Usuario { UserName = "UsuarioTest" + (IdsUsuarios.Max() + 3), Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", UserStatusId = (byte)UserStatus.Active };

            // Act
            await _genericRepository.AddAsync(nuevoUsuario01);
            await _genericRepository.AddAsync(nuevoUsuario02);

            await _genericRepository.SaveChangesAsync();

            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(totalUsuarios + 2);
        }

        [Fact, TestPriority(15)]
        public async Task DeleteAsync()
        {
            // Arrange
            var totalUsuarios = TotalUsuarios;
            var usuario = _genericRepository.Find(u => u.UserId == IdsUsuarios.Max());

            // Act
            await _genericRepository.DeleteAsync(usuario);

            await _genericRepository.SaveChangesAsync();

            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(totalUsuarios - 1);

            var ids = IdsUsuarios.GetRange(0, TotalUsuarios);
            ids.Add(IdsUsuarios.Max() + 4);

            usuarios.AsEnumerable().ForEach(u => u.UserId.Should().BeOneOf(ids));
        }

        [Fact, TestPriority(16)]
        public async Task DeleteAsync_Expression()
        {
            // Act
            await _genericRepository.DeleteAsync(u => u.UserId == IdsUsuarios.Max() + 4);

            await _genericRepository.SaveChangesAsync();

            var usuarios = _genericRepository.All();

            // Assert
            usuarios.Should().NotBeEmpty();
            usuarios.Count().Should().Be(TotalUsuarios);

            usuarios.AsEnumerable().ForEach(u => u.UserId.Should().BeOneOf(IdsUsuarios.GetRange(0, TotalUsuarios)));
        }

        [Fact, TestPriority(17)]
        public void Count()
        {
            // Act
            var count = _genericRepository.Count;

            // Assert
            count.Should().BePositive();
            count.Should().BeOfType(typeof(int));
            count.Should().Be(TotalUsuarios);
        }
    }
}
