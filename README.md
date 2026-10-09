# 🚗 Revemar - Sistema de Controle de Estoque de Veículos

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle Database](https://img.shields.io/badge/Oracle-Database_21c-F80000?logo=oracle)](https://www.oracle.com/database/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4)](https://docs.microsoft.com/ef/)
[![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)](https://getbootstrap.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Aplicação Web para cadastro e gerenciamento de estoque de veículos novos e seminovos, desenvolvida como solução ao Desafio Técnico da **Revemar**. O sistema permite a manutenção completa (CRUD) de veículos com persistência de dados em banco **Oracle Database** e interface responsiva em **ASP.NET Core MVC**.

---

## 🏛️ Arquitetura da Solução

O projeto foi estruturado seguindo os princípios de **Clean Architecture / Separação de Responsabilidades**, dividindo o código em camadas bem definidas para facilitar manutenibilidade, testabilidade e evolução contínua:

```text
RevemarEstoque/
├── 📁 Revemar.Domain/         # Entidades de negócio, Enums e regras de domínio (sem dependências externas)
├── 📁 Revemar.Infrastructure/ # Contexto do EF Core (AppDbContext), Mapeamentos e Acesso ao Oracle Database
└── 📁 Revemar.Web/            # ASP.NET Core MVC (Controllers, Views Razor, TagHelpers e Assets Bootstrap)
```

---

## 🛠️ Tecnologias e Ferramentas

* **Framework Backend:** .NET 8 (C#)
* **Padrão de Arquitetura:** ASP.NET Core MVC (Model-View-Controller)
* **ORM & Persistência:** Entity Framework Core 8
* **Banco de Dados:** Oracle Database 21c Express Edition (Executado via Docker)
* **Driver do Banco:** `Oracle.EntityFrameworkCore` (Provedor Oficial Oracle)
* **Front-End & UI:** Razor Views (`.cshtml`), Bootstrap 5 e HTML5/CSS3
* **Ambiente & Ferramental:** Visual Studio 2022 / Docker Desktop / Git

---

## ⚙️ Funcionalidades Implementadas

- [x] **Cadastrar Veículo:** Formulário completo com validações de campos obrigatórios, tipos numéricos (Ano e Preço) e seleção de atributos (Marca, Modelo, Cor, Preço, Tipo e Situação).
- [x] **Listar Veículos:** Painel visual com indicação clara da situação no estoque (*Disponível*, *Reservado*, *Vendido*, *Inativo*) via badges coloridas.
- [x] **Consultar Detalhes:** Visualização individual detalhada das especificações do veículo.
- [x] **Editar Veículo:** Atualização de dados cadastrais com preservação da integridade do banco.
- [x] **Excluir Veículo:** Remoção controlada de registros do estoque.

---

## 🚀 Como Executar o Projeto Localmente

### Pré-requisitos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Para execução do Oracle Database)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

---

### 1. Clonar o Repositório

```bash
git clone https://github.com/SEU-USUARIO/Cadastro-e-Controle-de-Estoque-de-Veiulos.git
cd revemar-estoque-veiculos
```

---

### 2. Subir o Banco de Dados Oracle via Docker

Execute o comando a seguir no terminal para inicializar a instância do Oracle Database XE em segundo plano:

```bash
docker run -d --name oracle-xe -p 1521:1521 -e ORACLE_PASSWORD=Herbert_Revemar gvenzl/oracle-xe
```

> ⚠️ **Atenção:** Aguarde cerca de 1 a 2 minutos até que o contêiner finalize a inicialização do banco de dados (status `healthy` no Docker Desktop).

---

### 3. Aplicar as Migrations do Entity Framework Core

Com o contêiner do Oracle ativo, aplique as migrações para criar as tabelas no banco:

#### Via Console do Gerenciador de Pacotes (Visual Studio):
```powershell
Update-Database -Context AppDbContext -StartupProject Revemar.Web
```

#### Via Terminal CLI (.NET Core CLI):
```bash
dotnet ef database update --project Revemar.Infrastructure --startup-project Revemar.Web
```

---

### 🔌 Conectando via Oracle SQL Developer

Para inspecionar as tabelas criadas pelo Entity Framework Core e visualizar os dados cadastrados em tempo real, configure a conexão na sua ferramenta de banco de dados com as seguintes credenciais:

* **Nome da Conexão:** `Revemar_Oracle_Docker` (ou de sua preferência)
* **Tipo de Conexão:** Básico (*Basic*)
* **Nome do Host (*Hostname*):** `localhost`
* **Porta:** `1521`
* **Nome do Serviço (*Service Name*):** `XEPDB1`
* **Usuário:** `SYSTEM`
* **Senha:** `Herbert_Revemar`

> 💡 **Dica:** Ao testar a conexão no Oracle SQL Developer, clique no botão **Testar** (*Test*). Se o status retornar `Sucesso` (*Success*), clique em **Conectar**. A tabela `VEICULOS` estará visível na aba *Tabelas*.

---

### 4. Executar a Aplicação

* **No Visual Studio:** Pressione `F5` ou clique no botão de execução **Revemar.Web**.
* **Via Terminal:**
  ```bash
  dotnet run --project Revemar.Web
  ```

Acesse a aplicação no navegador em: `https://localhost:7214` ou `http://localhost:5214` (A porta pode variar dependendo do ambiente local).

---

## 📄 Licença

Este projeto é um desafio técnico e está sob a licença [MIT](LICENSE).
