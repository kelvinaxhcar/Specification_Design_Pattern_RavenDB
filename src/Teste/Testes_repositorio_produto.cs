using Raven.TestDriver;
using Specification_Design_Pattern_RavenDB.Entidades;
using Specification_Design_Pattern_RavenDB.Especificacoes;
using Specification_Design_Pattern_RavenDB.Filters;
using Specification_Design_Pattern_RavenDB.Querys;

namespace Teste
{
    public class Testes_repositorio_produto : RavenTestDriver
    {
        private readonly ServicoSpecification _servicoSpecification = new ServicoSpecification();
        public Testes_repositorio_produto()
        {
            
        }

        [Fact]
        public void deve_ober_por_quary_completa()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Teste", Marca = "Marca A", Preco = 1, Data = new DateTime(2024, 03, 07) , Tipo = Tipo.Producao});
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var filtros = new FiltroObterTodosExemplo("?tipo=%3D1&nome=%3DTeste&data=2024-03-01...2024-03-08");
                    var especificacao = _servicoSpecification.ObterQyerys<Produto>(filtros.FilterQueries);

                    var query = Querys<Produto>.Filtrar(session, especificacao);

                    var querystring = query.ToString();

                    var produtosFiltrados = query.ToList();

                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Teste", produtosFiltrados[0].Nome);
                    Assert.Equal("from 'Produtos' where (Tipo = $p0) and (Nome = $p1) and (Data between $p2 and $p3)", querystring);
                }
            }
        }

        [Fact]
        public void deve_ober_por_nome_ao_executar_EspecificacaoEquals()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Teste", Marca = "Marca A", Preco = 1, Data = new DateTime(2024, 03, 07) });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var filtros = new FiltroObterTodosExemplo("?nome=%3DTeste");
                    var especificacao = _servicoSpecification.ObterQyerys<Produto>(filtros.FilterQueries);

                    var query = Querys<Produto>.Filtrar(session, especificacao);

                    var produtosFiltrados = query.ToList();

                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Teste", produtosFiltrados[0].Nome);
                }
            }
        }

        [Fact]
        public void deve_ober_por_data_ao_executar_EspecificacaoBetween()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Teste", Marca = "Marca A", Preco = 1, Data = new DateTime(2024, 03, 07) });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var filtros = new FiltroObterTodosExemplo("?data=2024-03-01...2024-03-08");
                    var especificacao = _servicoSpecification.ObterQyerys<Produto>(filtros.FilterQueries);

                    var query = Querys<Produto>.Filtrar(session, especificacao);

                    var produtosFiltrados = query.ToList();

                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Teste", produtosFiltrados[0].Nome);
                }
            }
        }

        [Fact]
        public void deve_ober_por_enum_ao_executar_EspecificacaoEquals()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Teste", Marca = "Marca A", Preco = 1, Data = new DateTime(2024, 03, 07), Tipo = Tipo.Producao });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var filtros = new FiltroObterTodosExemplo("?tipo=%3D1");
                    var especificacao = _servicoSpecification.ObterQyerys<Produto>(filtros.FilterQueries);

                    var query = Querys<Produto>.Filtrar(session, especificacao);

                    var produtosFiltrados = query.ToList();

                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Teste", produtosFiltrados[0].Nome);
                }
            }
        }

        [Fact]
        public void deve_ober_por_enum_ao_executar_EspecificacaoGreatThen()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Teste", Marca = "Marca A", Preco = 1, Data = new DateTime(2024, 03, 07), Tipo = Tipo.Producao });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" , Preco = 2 });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" ,Preco = 3 });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var filtros = new FiltroObterTodosExemplo("preco=<2");
                    var especificacao = _servicoSpecification.ObterQyerys<Produto>(filtros.FilterQueries);

                    var query = Querys<Produto>.Filtrar(session, especificacao);

                    var produtosFiltrados = query.ToList();

                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Teste", produtosFiltrados[0].Nome);
                }
            }
        }

        [Fact]
        public void deve_ober_por_nome_StartsWith()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A", Preco = 1 });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Teste 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var propriedade = typeof(Produto)
                        .GetProperties()
                        .FirstOrDefault(x => x.Name.Equals("nome", StringComparison.CurrentCultureIgnoreCase));

                    var especificacao = new EspecificacaoStartsWith<Produto>(propriedade.Name, "Te");
                    var query = Querys<Produto>.Filtrar(session, especificacao);

                    var produtosFiltrados = query.ToList();

                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Teste 3", produtosFiltrados[0].Nome);
                }
            }
        }

        [Fact]
        public void deve_ober_por_marca()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A" });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var especificacao = new EspecificacaoEquals<Produto>("Marca", "marca B");
                    var query = Querys<Produto>.Filtrar(session, especificacao);

                    var produtosFiltrados = query.ToList();
                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Produto 2", produtosFiltrados[0].Nome);
                }
            }
        }


        [Fact]
        public void deve_ober_por_nome_e_marca()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A" });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var especificacaoNome = new EspecificacaoEquals<Produto>("Nome", "produto 1");
                    var especificacaoMarca = new EspecificacaoEquals<Produto>("Marca", "marca A");

                    var especificacaoCombinada = new EspecificacaoE<Produto>(especificacaoNome, especificacaoMarca);
                    var query = Querys<Produto>.Filtrar(session, especificacaoCombinada);

                    var produtosFiltrados = query.ToList();
                    // Assert
                    Assert.Single(produtosFiltrados);
                    Assert.Equal("Produto 1", produtosFiltrados[0].Nome);
                }
            }
        }

        [Fact]
        public void deve_ober_por_nome_ou_marca()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A" });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var especificacaoNome = new EspecificacaoEquals<Produto>("Nome", "produto 1")
                        .Ou(new EspecificacaoEquals<Produto>("Marca", "marca B"));

                    var query = Querys<Produto>.Filtrar(session, especificacaoNome);

                    var produtosFiltrados = query.ToList();
                    // Assert
                    Assert.Equal(2, produtosFiltrados.Count);
                    Assert.Equal("Produto 1", produtosFiltrados[0].Nome);
                    Assert.Equal("Produto 2", produtosFiltrados[1].Nome);
                }
            }
        }

        [Fact]
        public void deve_ober_por_nome_ou_marca_e_id()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A" });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C" });
                    session.Store(new Produto { Id = "4", Nome = "Produto 3", Marca = "Marca D" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var especificacaoNomeOU = new EspecificacaoEquals<Produto>("Nome", "produto 1")
                        .Ou(new EspecificacaoEquals<Produto>("Marca", "marca B"))
                        .E(new EspecificacaoEquals<Produto>("Id", "1"));

                    var query = Querys<Produto>.Filtrar(session, especificacaoNomeOU);

                    var produtosFiltrados = query.ToList();
                    // Assert
                    Assert.Equal(1, produtosFiltrados.Count);
                }
            }
        }

        [Fact]
        public void deve_ober_por_nome_ou_marca_ou_id()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A", Cidade = "A1" });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B", Cidade = "A2" });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C", Cidade = "A3" });
                    session.Store(new Produto { Id = "4", Nome = "Produto 3", Marca = "Marca D", Cidade = "A4" });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var especificacaoNomeOU = new EspecificacaoEquals<Produto>("Nome", "produto 1");

                    var especificacaoMarcaOU = new EspecificacaoEquals<Produto>("Marca", "marca B");

                    var especificacaoMarcaOUCidade = new EspecificacaoEquals<Produto>("Cidade", "A1");

                    var especificacaoMarcaID = new EspecificacaoEquals<Produto>("Id", "3");

                    var especificacaoCombinadaOU = new EspecificacaoOu<Produto>(especificacaoNomeOU, especificacaoMarcaOU, especificacaoMarcaOUCidade, especificacaoMarcaID);
                    var query = Querys<Produto>.Filtrar(session, especificacaoCombinadaOU);

                    var produtosFiltrados = query.ToList();
                    // Assert
                    Assert.Equal(3, produtosFiltrados.Count);
                }
            }
        }

        [Fact]
        public void deve_ober_por_maior_que()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A", Cidade = "A1", Preco = 1 });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B", Cidade = "A2", Preco = 2 });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C", Cidade = "A3", Preco = 3 });
                    session.Store(new Produto { Id = "4", Nome = "Produto 3", Marca = "Marca D", Cidade = "A4", Preco = 4 });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var especificacaoMaior = new EspecificacaoGreatThen<Produto>("Preco", "2");

                    var query = Querys<Produto>.Filtrar(session, especificacaoMaior);

                    var produtosFiltrados = query.ToList();
                    // Assert
                    Assert.Equal(2, produtosFiltrados.Count);
                }
            }
        }

        [Fact]
        public void deve_ober_por_menor_que()
        {
            using (var store = GetDocumentStore())
            {
                // Arrange
                using (var session = store.OpenSession())
                {
                    session.Store(new Produto { Id = "1", Nome = "Produto 1", Marca = "Marca A", Cidade = "A1", Preco = 1 });
                    session.Store(new Produto { Id = "2", Nome = "Produto 2", Marca = "Marca B", Cidade = "A2", Preco = 2 });
                    session.Store(new Produto { Id = "3", Nome = "Produto 3", Marca = "Marca C", Cidade = "A3", Preco = 3 });
                    session.Store(new Produto { Id = "4", Nome = "Produto 3", Marca = "Marca D", Cidade = "A4", Preco = 4 });
                    session.SaveChanges();
                }

                // Act
                using (var session = store.OpenSession())
                {
                    var especificacaoMenor = new EspecificacaoLessThen<Produto>("Preco", "2");

                    var query = Querys<Produto>.Filtrar(session, especificacaoMenor);

                    var produtosFiltrados = query.ToList();
                    // Assert
                    Assert.Equal(1, produtosFiltrados.Count);
                }
            }
        }

    }
}