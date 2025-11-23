🧩 Sistema de Gerenciamento de Tarefas

Este projeto foi desenvolvido em C# (.NET 8) utilizando Minimal API, Entity Framework Core e SQLite.
O sistema permite cadastrar usuários e gerenciar suas tarefas, com controle de status, data de criação e conclusão.

🚀 Tecnologias Utilizadas

.NET 8 (Minimal API)

Entity Framework Core

SQLite

C#

Swagger (para testes via navegador)

⚙️ Como Executar o Projeto

1️⃣ Clonar o repositório
git clone https://github.com/seunome/sistema-tarefas.git
cd sistema-tarefas

2️⃣ Restaurar dependências
dotnet restore

3️⃣ ⚠️ Executar as migrations (OBRIGATÓRIO)

Antes de rodar o projeto, é necessário criar o banco de dados SQLite e aplicar as migrations.
Esses comandos geram o arquivo app.db com todas as tabelas (Usuario e Tarefa).

Se o dotnet-ef ainda não estiver instalado:

dotnet tool install --global dotnet-ef

Em seguida, execute:

dotnet ef migrations add InitialCreate


dotnet ef database update

4️⃣ Rodar o servidor
dotnet run

📡 Endpoints Disponíveis
👤 Usuário
Método	Endpoint	Descrição
POST	/usuario	Cria um novo usuário
GET	/usuario	Lista todos os usuários
GET	/usuario/{id}	Busca um usuário por ID
PUT	/usuario/{id}	Atualiza dados de um usuário
DELETE	/usuario/{id}	Remove um usuário

✅ Tarefa
Método	Endpoint	Descrição
POST	/tarefa	Cria uma nova tarefa vinculada a um usuário
GET	/tarefa	Lista todas as tarefas com seus respectivos usuários
GET	/tarefa/{id}	Busca uma tarefa específica
PUT	/tarefa/{id}	Atualiza os dados de uma tarefa
DELETE	/tarefa/{id}	Remove uma tarefa
POST	/tarefa/concluir/{id}	Marca uma tarefa como concluída
