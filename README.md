# TrainMaster

# **Descrição do projeto**

- É uma plataforma web inovadora destinada a gerenciar e otimizar o treinamento de funcionários em empresas. O objetivo é criar um ambiente de aprendizado online, acessível
  a qualquer hora, de qualquer lugar e em qualquer dispositivo, para fornecer aulas, conteúdos e provas de forma eficiente e prática.

---

# **Solução**

## **IDE's Utilizadas**

- Visual Studio 2022
- PostgreSQL

---

## **Recursos do Projeto**

- **Serilog**: Para geração e gerenciamento de logs.
- **FluentValidator**: Para validação de dados e regras de negócios.
- **Entity Framework (ORM)**: Para mapeamento e interação com o banco de dados.
- **Unit of Work**: Padrão de design para gerenciar transações e persistência de dados de forma coesa.
- **Migrations**: Gerenciamento de alterações no banco de dados.
- **Xunit**: Para criação de testes unitários.
- **Razor**: Para criação do front da aplicação.
- **Robot Framework**: Automação de teste de software para testes de aceitação e desenvolvimento orientado a testes de aceitação.

---

## **Como Executar o Projeto**

### **🚀 Método Rápido (Recomendado)**

Use o script automatizado para execução completa:

```bash
# Executar aplicação completa (migrations + execução)
./run-api.sh

# Verificar configurações
./run-api.sh --check-only

# Executar apenas migrations
./run-api.sh --migrate-only

# Ver todas as opções
./run-api.sh --help
```

### **📋 Pré-requisitos**

- .NET 8.0 SDK instalado
- Entity Framework Tools: `dotnet tool install --global dotnet-ef`
- Conexão com Supabase configurada

### **1. Configuração do Banco de Dados (Supabase)**

O projeto está configurado para usar o **Supabase** como banco de dados:

**String de Conexão:**

```
postgresql://postgres.ekxsphpaocqpewmufmow:JptEWOJvDTil3ggj@aws-1-us-east-1.pooler.supabase.com:5432/postgres
```

**Configuração Automática:**

- As migrations são executadas automaticamente na inicialização
- Não é necessário configurar banco local
- Acesso via Supabase Dashboard: https://supabase.com/dashboard

### **2. Executando o Projeto**

#### **Opção A: Script Automatizado (Recomendado)**

```bash
# Na raiz do projeto
./run-api.sh
```

#### **Opção B: Comandos Manuais**

```bash
# Restaurar pacotes
dotnet restore

# Aplicar migrations
cd TrainMaster
dotnet ef database update

# Executar aplicação
dotnet run
```

#### **Opção C: Visual Studio 2022**

1. Abra o projeto no Visual Studio 2022
2. Configure o projeto **TrainMaster** como Startup Project
3. Clique no botão **HTTPS** para iniciar

### **3. Acesso à Aplicação**

Após a execução, a aplicação estará disponível em:

- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger**: http://localhost:5000/swagger

### **3. Banco de Dados**

- **Centralização de Exceções:**  
  Implementada a classe `ExceptionMiddleware` para unificar o tratamento de erros no sistema.
- **Alterações Realizadas:**  
  Ajustadas as classes `Program` e `RepositoryUoW` para integrar o middleware.
- **Mensagens de Erro:**
  - Se o banco de dados não existir, os endpoints retornam:
    ```text
    The database is currently unavailable. Please try again later.
    ```
  - Para erros inesperados na criação do banco, é exibido:
    ```text
    An unexpected error occurred. Please contact support if the problem persists.
    ```

---

### **4. Configuração do Log**

- O sistema gera logs diários com informações sobre os processos executados no projeto.
- O log será salvo no diretório:  
  `C://Users//User//Downloads//logs`.  
  **Nota**: É necessário criar a pasta manualmente nesse caminho ou alterar o diretório no código, caso deseje personalizá-lo.

**Formato do arquivo de log criado**:

- Arquivo diário com informações estruturadas.

---

### **5. Finalização**

- Após seguir as etapas anteriores, o sistema será iniciado, e uma página com a interface **Swagger** será aberta automaticamente no navegador configurado no Visual Studio. Essa página permitirá explorar e testar os endpoints da API.

---

## **Estrutura do Projeto**

Essa estrutura garante organização, modularidade e escalabilidade ao projeto.

### **1. TrainMaster (API)**

Contém os endpoints para acesso e execução das funcionalidades:

1. Organização das pastas:

- **Controllers**: Controladores da aplicação.
- **Views**: Front enn da aplicação.
- **Extensions**:
  - SwaggerDocumentation: Documentação do swagger.
  - ExtensionsLogs: Classe para gerar logs.
  - ExceptionMiddleware classe para tratar erro de conexão com o banco de dados.
  - Extensões para a classe `Program`.
- **Appsettings**: Configurações, incluindo conexão com o banco de dados.
- **Program**: Classe principal para inicialização.

---

### **2. TrainMaster.Application**

Camada intermediária entre os controladores e o banco de dados. Responsável também por funções específicas, como envio de e-mails.

1. Organização das pastas:

- **ExtensionError**: Contém a classe `Result` para controle de erros, usando FluentValidator.
- **Services**: Contém as classes de serviços e interfaces.
- **UnitOfWork**: Implementação do padrão **Unit of Work**, que gerencia transações e persistência de dados.

---

### **3. TrainMaster.Domain**

Camada de domínio, responsável pelos dados principais do sistema.

1. Organização das pastas:

- **Entity**: Contém as entidades do projeto.
- **Enum**: Contém enums utilizados no projeto.
- **General**: Contém classes genéricas, incluindo a `BaseEntity`, com propriedades comuns às entidades.
- **Dto**: Contém objetos de transferência de dados (DTOs), utilizados para transportar informações entre as camadas do sistema sem expor diretamente as entidades do domínio.

---

### **4. TrainMaster.Infrastructure**

Camada responsável pela interação com o banco de dados.

1. Organização das pastas:

- **Connection**: Configuração de conexão e mapeamento das entidades para o Entity Framework.
- **Migrations**: Diretório onde as migrations geradas serão armazenadas.
- **Repository**: Contém repositórios e suas interfaces.

---

### **5. TrainMaster.Shared**

Biblioteca utilizada para validações e compartilhamento de recursos comuns:

1. Organização das pastas:

- **Enums**: Classes de enums para erros.
- **Helpers**: Classe auxiliar para validação de erros.
- **Validator**: Regras de validação para as entidades.

---

### **6. TrainMaster.Tests**

Biblioteca utilizada para crição de testes unitário com Xunit e Moq:

1. Organização das pastas:

- **Entity**: Classes de testes para as entidades.
- **Services**: Classe de testes para o service.

---

### **7. Robot**

Biblioteca utilizada para automação de testes Robot Framework.

1. Organização das pastas:

- **Robot**: Contém o arquivo de teste responsável pela automação do fluxo correto de cadastro de curso, validando que o caminho funciona conforme o esperado.
- **Robot_bug**: Contém o arquivo de teste automatizado com foco na identificação de falhas no cadastro de curso, simulando cenários com comportamentos inesperados ou entradas inválidas, permitindo detectar possíveis bugs no sistema.

---

### **Bibliotecas (packages) para .NET, instaladas via NuGet**

1. coverlet.collector – Biblioteca para cobertura de código em testes unitários no .NET.
2. FluentValidation – Framework para validação de dados no .NET com sintaxe fluente.
3. Microsoft.EntityFrameworkCore – ORM do .NET para acesso a bancos de dados.
4. Microsoft.EntityFrameworkCore.Tools – Ferramentas para migrações e gestão do Entity Framework Core.
5. Microsoft.VisualStudio.Web.CodeGeneration.Design – Geração automática de código para ASP.NET Core (Scaffolding).
6. Npgsql – Provedor de acesso ao banco de dados PostgreSQL para .NET.
7. Npgsql.EntityFrameworkCore.PostgreSQL – Integração do Entity Framework Core com PostgreSQL.
8. Serilog.AspNetCore – Biblioteca para logging estruturado no ASP.NET Core.
9. Swashbuckle.AspNetCore – Ferramenta para documentar APIs ASP.NET Core com Swagger.

---

### **ViaCEP**

Acesso ao serviço da API dos correios, através do ViaCEP (gratuito e amplamente utilizado).

---

## **📚 Documentação Adicional**

### **Scripts e Automação**

- **[SCRIPT_USAGE.md](SCRIPT_USAGE.md)**: Guia completo do script `run-api.sh`
- **[SUPABASE_SETUP.md](SUPABASE_SETUP.md)**: Configuração e uso do Supabase
- **[ENVIRONMENT_SETUP.md](ENVIRONMENT_SETUP.md)**: Configuração segura com variáveis de ambiente

### **Comandos Úteis**

```bash
# Executar aplicação
./run-api.sh

# Verificar configurações
./run-api.sh --check-only

# Aplicar migrations
./run-api.sh --migrate-only

# Compilar projeto
./run-api.sh --build-only

# Ajuda
./run-api.sh --help
```

### **Acesso ao Banco de Dados**

- **Supabase Dashboard**: https://supabase.com/dashboard
- **Cliente PostgreSQL**: Use as credenciais fornecidas
- **Linha de comando**:
  ```bash
  psql "postgresql://postgres.ekxsphpaocqpewmufmow:JptEWOJvDTil3ggj@aws-1-us-east-1.pooler.supabase.com:5432/postgres"
  ```
