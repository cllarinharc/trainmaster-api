# 🗄️ Script de População de Dados - TrainMaster

Este documento explica como usar o script `populate_database.sql` para popular o banco de dados com dados de teste realistas.

## 📋 Visão Geral

O script `populate_database.sql` foi criado para popular todas as tabelas do banco de dados TrainMaster com dados fake (mas realistas) para fins de estudo e demonstração.

### ⚠️ IMPORTANTE

**Este script assume que já existe um usuário com ID 1** no banco de dados. Os novos registros serão inseridos a partir do ID 2. Se você precisar popular o banco do zero, descomente a linha TRUNCATE no início do script.

### 📊 Dados Incluídos

| Tabela                        | Quantidade | Descrição                                             |
| ----------------------------- | ---------- | ----------------------------------------------------- |
| **UserEntity**                | 16         | Novos usuários (IDs 2-17, excluindo o admin ID 1)     |
| **PessoalProfileEntity**      | 16         | Perfis pessoais completos                             |
| **ProfessionalProfileEntity** | 16         | Perfis profissionais com cargos e habilidades         |
| **AddressEntity**             | 16         | Endereços de todos os usuários                        |
| **EducationLevelEntity**      | 22         | Histórico educacional (múltiplas formações)           |
| **DepartmentEntity**          | 5          | Departamentos (RH, TI, Vendas, Marketing, Financeiro) |
| **TeamEntity**                | 8          | Equipes organizadas por departamento                  |
| **CourseEntity**              | 8          | Cursos completos de programação                       |
| **CourseActivitieEntity**     | 18         | Atividades práticas dos cursos                        |
| **QuestionEntity**            | 23         | Questões teóricas e práticas                          |
| **QuestionOptionEntity**      | 69         | Opções de múltipla escolha                            |
| **ExamEntity**                | 5          | Exames finais dos cursos                              |
| **ExamQuestionEntity**        | 14         | Questões vinculadas aos exames                        |
| **BadgeEntity**               | 10         | Insígnias/Conquistas                                  |
| **UserBadgeEntity**           | 13         | Insígnias conquistadas por usuários                   |
| **CourseFeedbackEntity**      | 11         | Feedback de alunos sobre cursos                       |
| **CourseAvaliationEntity**    | 6          | Avaliações de cursos                                  |
| **HistoryPasswordEntity**     | 5          | Histórico de senhas alteradas                         |
| **NotificationEntity**        | 8          | Notificações do sistema                               |

## 🚀 Como Executar

### Pré-requisitos

1. Banco de dados PostgreSQL configurado (Supabase ou local)
2. PostgreSQL Client instalado (psql ou interface gráfica)
3. Credenciais de acesso ao banco de dados

### Método 1: Via Terminal (psql)

```bash
# No diretório do projeto
cd trainmaster-api

# Executar o script
psql "postgresql://postgres.ekxsphpaocqpewmufmow:JptEWOJvDTil3ggj@aws-1-us-east-1.pooler.supabase.com:5432/postgres" -f populate_database.sql
```

### Método 2: Via Supabase Dashboard

1. Acesse https://supabase.com/dashboard
2. Faça login no seu projeto
3. Vá em **SQL Editor**
4. Abra o arquivo `populate_database.sql`
5. Copie e cole o conteúdo completo
6. Clique em **Run**

### Método 3: Via DBeaver/outra ferramenta

1. Abra sua ferramenta SQL favorita
2. Conecte ao banco de dados
3. Abra o arquivo `populate_database.sql`
4. Execute o script completo

## 🔍 O que o Script Faz

### 1. Estrutura Hierárquica de Dados

O script segue a seguinte ordem de inserção (respeitando foreign keys):

```
1. Users (17 usuários)
   ↓
2. PessoalProfiles (perfis pessoais)
   ↓
3. ProfessionalProfiles (perfis profissionais)
   ↓
4. Addresses (endereços)
   ↓
5. EducationLevels (formação acadêmica)
   ↓
6. Departments (departamentos)
   ↓
7. Teams (equipes)
   ↓
8. Courses (cursos)
   ↓
9. CourseActivities (atividades)
   ↓
10. Questions (questões)
    ↓
11. QuestionOptions (opções)
    ↓
12. Exams (exames)
    ↓
13. ExamQuestions (questões de exames)
    ↓
14. Badges (insígnias)
    ↓
15. UserBadges (conquistas)
    ↓
16. CourseFeedbacks (feedback)
    ↓
17. CourseAvaliations (avaliações)
    ↓
18. HistoryPasswords (histórico)
    ↓
19. Notifications (notificações)
```

### 2. Tipos de Usuários Criados

- **Administrador**: 1 usuário
- **Professores/Instrutores**: 4 usuários
- **Alunos/Estudantes**: 10 usuários
- **Gerentes de Departamento**: 2 usuários

### 3. Cursos Disponíveis

1. **Desenvolvimento Web Completo com React** (90 dias)
2. **Data Science e Machine Learning com Python** (90 dias)
3. **Segurança Cibernética e Ethical Hacking** (60 dias)
4. **DevOps: Containers, CI/CD e Cloud** (90 dias)
5. **JavaScript Avançado e Node.js** (90 dias)
6. **Banco de Dados com PostgreSQL** (60 dias)
7. **React Native: Desenvolvimento Mobile** (90 dias)
8. **Cloud Computing com AWS** (90 dias)

### 4. Atividades e Questões

Cada curso possui:

- Múltiplas atividades práticas
- Questões objetivas com 4 opções cada
- Sistema de pontuação distribuído
- Temas específicos do curso

### 5. Badges e Conquistas

**Insígnias Disponíveis:**

- 🎓 Desenvolvedor Iniciante
- ⚛️ Mestre React
- 📊 Cientista de Dados
- 🛡️ Guardião da Segurança
- 🚀 DevOps Expert
- 💯 100% Aproveitamento
- 🏆 Top Student
- ✅ Assiduidade
- ⏰ Entrega Pontual
- 👨‍🏫 Mentor

## ⚠️ Importante

### Senha Padrão

Todas as senhas no banco de dados foram hasheadas com bcrypt:

- **Hash usado**: `$2a$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi`
- **Senha original**: `123456` (para testes)

**Para fazer login durante os testes:**

```
Email: prof.silva@trainmaster.com
Senha: 123456
```

**OBSERVAÇÃO**: Se o hash não funcionar, você pode:

1. **Gerar um novo hash** usando bcrypt para "123456"
2. **Atualizar no banco** após a inserção:
   ```sql
   UPDATE "UserEntity"
   SET "Password" = '$2a$10$SEU_NOVO_HASH_AQUI'
   WHERE "Email" = 'prof.silva@trainmaster.com';
   ```

### Como Gerar um Hash Bcrypt

**Opção 1: Em C# (.NET)**

```csharp
using BCrypt.Net;

var hash = BCrypt.Net.BCrypt.HashPassword("123456");
Console.WriteLine(hash);
```

**Opção 2: Online**

- Use um gerador online: https://bcrypt-generator.com/
- Digite "123456" e copie o hash gerado

### Limpar Dados Existentes (Opcional)

Se você quiser limpar os dados existentes antes de inserir novos dados, **descomente** a linha no início do script:

```sql
-- TRUNCATE TABLE "UserBadgeEntity", "BadgeEntity", ...
```

**⚠️ ATENÇÃO**: Isso apagará TODOS os dados existentes!

### Relacionamentos Únicos

O banco possui alguns relacionamentos únicos (um-para-um):

- Um User pode ter apenas um PessoalProfile
- Um User pode ter apenas um ProfessionalProfile
- Um PessoalProfile pode ter apenas um Address
- Um ProfessionalProfile pode ter apenas um EducationLevel

Se você tentar inserir dados duplicados, o script falhará.

## 📈 Verificação dos Dados

Depois de executar o script, você pode verificar os dados inseridos:

```sql
-- Contar usuários
SELECT COUNT(*) FROM "UserEntity";

-- Ver cursos criados
SELECT "Name", "Author", "IsActive" FROM "CourseEntity";

-- Ver badges
SELECT "Name", "Description" FROM "BadgeEntity";

-- Ver relações de badges
SELECT u."Email", b."Name"
FROM "UserBadgeEntity" ub
JOIN "UserEntity" u ON ub."UserId" = u."Id"
JOIN "BadgeEntity" b ON ub."BadgeId" = b."Id";
```

## 🎯 Casos de Uso

Este script é perfeito para:

✅ **Demonstrações** da plataforma para stakeholders  
✅ **Testes** de funcionalidades completas  
✅ **Desenvolvimento** local sem precisar criar dados manualmente  
✅ **Estudos** de estrutura de dados educacionais  
✅ **Apresentações** de portfolios e projetos

## 🔧 Solução de Problemas

### Erro: "duplicate key value violates unique constraint"

Isso significa que já existem dados no banco. Soluções:

1. Limpar os dados existentes (TRUNCATE)
2. Modificar os IDs no script
3. Usar um banco de dados novo

### Erro: "violates foreign key constraint"

O script está tentando inserir dados fora de ordem. Verifique:

1. Se as migrations foram aplicadas corretamente
2. Se não há dados orfãos no banco
3. Execute novamente o TRUNCATE se necessário

### Erro: "value too long for type character varying"

Alguns valores excedem o tamanho máximo. Neste caso:

1. Verifique os tamanhos máximos na migration
2. Reduza os textos nos INSERTs

## 📞 Suporte

Se encontrar problemas:

1. Verifique os logs do banco de dados
2. Confirme que as migrations estão atualizadas
3. Execute `dotnet ef database update` se necessário

## 📝 Notas Finais

- Os dados são **fictícios** mas **realistas**
- Todos os CPFs são válidos (apenas estrutura)
- Os emails seguem padrões da plataforma
- Datas são consistentes e lógicas
- Relacionamentos respeitam as regras de negócio

---

**Autor**: Sistema de Geração de Dados  
**Data**: 2024  
**Versão**: 1.0  
**Projeto**: TrainMaster
