# Biblioteca API

API para cadastrar livros e controlar empréstimos. Cada livro representa um exemplar. Feita com C#, ASP.NET Core 10, Entity Framework Core e SQLite, somente back-end.

## Executar

Abra Biblioteca.sln no Visual Studio com suporte a .NET 10 e desenvolvimento ASP.NET. Aguarde a restauração dos pacotes e pressione F5. A API fica em http://localhost:5080/api/livros. Também pode executar `dotnet run` na pasta do projeto.

O arquivo biblioteca.db é criado na primeira execução. O projeto usa EnsureCreated para facilitar os testes iniciais, sem migrations. Se mudar o modelo, será necessário usar migrations ou recriar o banco de testes, perdendo seus dados.

## Rotas

| Método | Rota | Uso |
| --- | --- | --- |
| GET | /api/livros | Listar livros |
| GET | /api/livros/{id} | Buscar livro |
| POST | /api/livros | Cadastrar livro |
| PUT | /api/livros/{id} | Atualizar livro |
| DELETE | /api/livros/{id} | Excluir livro sem histórico |
| GET | /api/emprestimos | Listar empréstimos |
| GET | /api/emprestimos/{id} | Buscar empréstimo |
| POST | /api/emprestimos | Registrar empréstimo |
| PUT | /api/emprestimos/{id}/devolucao | Registrar devolução |

## Testar no Postman

Importe Biblioteca.postman_collection.json. No Postman online, selecione o Desktop Agent para acessar a API que está rodando no seu computador. O Cloud Agent não acessa localhost.

Cadastre um livro, copie o id retornado e ajuste a variável livroId da coleção. Depois registre um empréstimo, copie o id e ajuste emprestimoId. Faça a devolução e tente emprestar novamente.

Exemplo de livro:
```json
{"titulo":"Dom Casmurro","autor":"Machado de Assis","ano":1899}
```
Exemplo de empréstimo:
```json
{"livroId":1,"nomeLeitor":"João"}
```

O cadastro retorna 201 e o endereço do recurso no cabeçalho Location. As consultas retornam 200, alterações e exclusões retornam 204. Dados inválidos retornam 400, recursos inexistentes retornam 404 e conflitos retornam 409.

Um livro não pode ter dois empréstimos ativos. A devolução pode ser repetida sem alterar sua data. Livros com histórico não podem ser excluídos. As datas são gravadas em UTC.

## Estrutura

- Models: classes que representam os dados e a entrada de um empréstimo.
- Data/BibliotecaContext.cs: acesso às tabelas e relacionamento entre livro e empréstimo.
- Controllers: rotas HTTP, validações e operações no banco.
- Program.cs: configuração da aplicação e criação inicial do banco.

ASP.NET Core recebe as requisições HTTP e executa os controllers. A API organiza os recursos em rotas, usa métodos HTTP e retorna JSON. O Entity Framework faz o mapeamento das classes para as tabelas e executa as consultas no SQLite.

É uma base de estudo, sem autenticação nem interface. Para ampliar o trabalho, dá para adicionar leitores cadastrados, prazo de devolução e busca por título.

Referências: https://learn.microsoft.com/pt-br/training/modules/build-web-api-minimal-database/ e https://learn.microsoft.com/pt-br/azure/architecture/best-practices/api-design
