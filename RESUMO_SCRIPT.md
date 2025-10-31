# ✅ Script de População - Resumo

## 📁 Arquivos Criados

1. **`populate_database.sql`** - Script SQL completo com todos os dados
2. **`POPULATE_DATABASE_README.md`** - Documentação detalhada de uso
3. **`RESUMO_SCRIPT.md`** - Este arquivo (resumo rápido)

## ⚠️ IMPORTANTE

**Este script assume que já existe um usuário com ID 1** no banco de dados. Os novos registros serão inseridos a partir do ID 2. Se você precisar popular o banco do zero, descomente a linha TRUNCATE no início do arquivo `populate_database.sql`.

## 🎯 O que foi feito

### ✨ Script SQL Realista e Completo

✅ **Análise do Repositório**

- Estrutura do banco de dados analisada
- Todas as entidades mapeadas
- Relacionamentos foreign keys identificados

✅ **Dados Realistas Criados**

- 16 novos usuários (IDs 2-17, professores, alunos, gerentes)
- 8 cursos completos de programação
- 18 atividades com conteúdo educacional
- 23 questões objetivas com múltipla escolha
- 5 exames finais
- 10 badges/conquistas
- Departamentos e equipes organizadas

✅ **Estrutura Educacional Real**

- Cursos: React, Data Science, Segurança, DevOps, JavaScript
- Cronograma e datas consistentes
- Sistema de pontuação funcional
- Feedback e avaliações

## 🚀 Como Usar (3 Passos)

### 1. Certifique-se que o banco está configurado

```bash
# Verificar migrations aplicadas
cd TrainMaster
dotnet ef database update
```

### 2. Execute o script

**Opção A - Via psql:**

```bash
psql "postgresql://postgres.ekxsphpaocqpewmufmow:JptEWOJvDTil3ggj@aws-1-us-east-1.pooler.supabase.com:5432/postgres" -f populate_database.sql
```

**Opção B - Via Supabase Dashboard:**

1. Acesse https://supabase.com/dashboard
2. SQL Editor > New Query
3. Cole o conteúdo de `populate_database.sql`
4. Run

### 3. Verifique os dados

```sql
-- Ver usuários criados
SELECT "Email", "Cpf", "IsActive" FROM "UserEntity";

-- Ver cursos
SELECT "Name", "Author", "StartDate", "EndDate" FROM "CourseEntity" ORDER BY "Id";

-- Ver badges conquistadas
SELECT u."Email", b."Name"
FROM "UserBadgeEntity" ub
JOIN "UserEntity" u ON ub."UserId" = u."Id"
JOIN "BadgeEntity" b ON ub."BadgeId" = b."Id";
```

## 📊 Estatísticas dos Dados

| Categoria              | Quantidade |
| ---------------------- | ---------- |
| **Novos Usuários**     | 16         |
| **Cursos**             | 8          |
| **Atividades**         | 18         |
| **Questões**           | 23         |
| **Opções de questões** | 69         |
| **Exames**             | 5          |
| **Badges**             | 10         |
| **Departamentos**      | 5          |
| **Equipes**            | 8          |

## 🔑 Credenciais para Teste

**Login de Professor:**

```
Email: prof.silva@trainmaster.com
Senha: 123456
```

**Logins de Alunos (exemplos):**

```
Email: maria.silva@trainmaster.com
Senha: 123456

Email: joao.santos@trainmaster.com
Senha: 123456
```

**Login de Gerente:**

```
Email: gerente.rh@trainmaster.com
Senha: 123456
```

**OBSERVAÇÃO**: Todas as senhas são `123456`. Se o hash bcrypt não funcionar, gere um novo hash para esta senha ou atualize no banco após a inserção.

## 📚 Cursos Disponíveis

1. **Desenvolvimento Web Completo com React** - 90 dias
2. **Data Science e Machine Learning** - 90 dias
3. **Segurança Cibernética** - 60 dias
4. **DevOps: CI/CD e Cloud** - 90 dias
5. **JavaScript Avançado e Node.js** - 90 dias
6. **Banco de Dados PostgreSQL** - 60 dias
7. **React Native Mobile** - 90 dias
8. **Cloud Computing AWS** - 90 dias

## ⚙️ Características do Script

✅ **Respeita foreign keys** - Ordem de inserção correta  
✅ **Dados realistas** - Nomes, emails, endereços brasileiros  
✅ **Relacionamentos completos** - Todos os vínculos criados  
✅ **Sistema educacional** - Estrutura de cursos e avaliações  
✅ **Pontuação distribuída** - Sistema de notas coerente  
✅ **Timestamps adequados** - Datas consistentes e lógicas

## 🎨 Dados Exemplo

### Exemplo de Curso:

- **Nome**: Desenvolvimento Web Completo com React
- **Descrição**: Aprenda a criar aplicações web modernas...
- **Atividades**: 6 atividades práticas
- **Questões**: 6 questões objetivas
- **Duração**: 90 dias
- **Instrutor**: João Silva

### Exemplo de Usuário:

- **Nome**: Maria Silva
- **Email**: maria.silva@trainmaster.com
- **Perfil**: Desenvolvedor Junior
- **Skills**: JavaScript, React, Node.js
- **Badges**: 2 conquistas

### Exemplo de Badge:

- **Nome**: Mestre React
- **Descrição**: Domina React e seus conceitos avançados

## 💡 Próximos Passos

Após executar o script, você pode:

1. ✅ Testar login com diferentes usuários
2. ✅ Explorar os cursos e atividades
3. ✅ Verificar o sistema de badges
4. ✅ Testar feedback e avaliações
5. ✅ Navegar pelos departamentos e equipes

## 📖 Documentação Adicional

Para mais detalhes, consulte:

- **`POPULATE_DATABASE_README.md`** - Guia completo
- **`README.md`** - Documentação do projeto

## ⚠️ Importante

- Todos os dados são **fictícios**
- Senha padrão é **`senha123`** para todos
- Execute em ambiente de **desenvolvimento/teste**
- Faça backup antes de truncar tabelas

---

**Pronto para usar!** 🚀

Execute o script e tenha uma base de dados rica para seus estudos e demonstrações.
