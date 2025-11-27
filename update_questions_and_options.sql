-- ===================================================================
-- Script para Atualizar Perguntas e Opções com Textos que Fazem Sentido
-- ===================================================================
-- Este script atualiza todas as perguntas (QuestionEntity) e opções
-- (QuestionOptionEntity) no banco do Supabase com textos interessantes
-- e coerentes para facilitar testes e demonstrações
-- ===================================================================

-- ===================================================================
-- ATUALIZAÇÃO DAS PERGUNTAS (QuestionEntity)
-- ===================================================================

UPDATE "QuestionEntity" SET "Statement" = 'Qual comando você usa para instalar todas as dependências listadas no package.json de um projeto React?' WHERE "Id" = 1;
UPDATE "QuestionEntity" SET "Statement" = 'Quais são os três arquivos principais de configuração de um projeto React criado com Create React App?' WHERE "Id" = 2;
UPDATE "QuestionEntity" SET "Statement" = 'Em React, o que são Props e para que servem?' WHERE "Id" = 3;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é a sintaxe correta para criar um componente funcional em React usando arrow function?' WHERE "Id" = 4;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é a principal diferença entre componentes funcionais e componentes de classe em React?' WHERE "Id" = 5;
UPDATE "QuestionEntity" SET "Statement" = 'O que faz o hook useState no React?' WHERE "Id" = 6;
UPDATE "QuestionEntity" SET "Statement" = 'Para que serve o hook useEffect em React?' WHERE "Id" = 7;
UPDATE "QuestionEntity" SET "Statement" = 'Como você estruturaria o estado de uma aplicação Todo List em React?' WHERE "Id" = 8;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é a melhor prática para adicionar um novo item a um array de estado no React?' WHERE "Id" = 9;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é a forma correta de remover um item de um array de estado no React sem mutar o estado original?' WHERE "Id" = 10;
UPDATE "QuestionEntity" SET "Statement" = 'Como você implementa rotas em uma aplicação React usando React Router?' WHERE "Id" = 11;
UPDATE "QuestionEntity" SET "Statement" = 'O que faz o componente Link do React Router e qual sua vantagem sobre uma tag <a> tradicional?' WHERE "Id" = 12;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é a sintaxe correta para criar uma lista em Python?' WHERE "Id" = 13;
UPDATE "QuestionEntity" SET "Statement" = 'Como você cria um dicionário em Python com chave "nome" e valor "João"?' WHERE "Id" = 14;
UPDATE "QuestionEntity" SET "Statement" = 'O que são List Comprehensions em Python?' WHERE "Id" = 15;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é o comando correto para ler um arquivo CSV usando a biblioteca Pandas?' WHERE "Id" = 16;
UPDATE "QuestionEntity" SET "Statement" = 'O que faz o método groupby() do Pandas?' WHERE "Id" = 17;
UPDATE "QuestionEntity" SET "Statement" = 'Como você filtra um DataFrame do Pandas para mostrar apenas linhas onde uma coluna tem valor maior que 10?' WHERE "Id" = 18;
UPDATE "QuestionEntity" SET "Statement" = 'O que é SQL Injection e por que é perigoso?' WHERE "Id" = 19;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é a diferença entre autenticação e autorização?' WHERE "Id" = 20;
UPDATE "QuestionEntity" SET "Statement" = 'O que representa o OWASP Top 10?' WHERE "Id" = 21;
UPDATE "QuestionEntity" SET "Statement" = 'O que é um container Docker?' WHERE "Id" = 22;
UPDATE "QuestionEntity" SET "Statement" = 'Qual é a diferença entre uma imagem Docker e um container Docker?' WHERE "Id" = 23;

-- ===================================================================
-- ATUALIZAÇÃO DAS OPÇÕES (QuestionOptionEntity)
-- ===================================================================
-- Vamos atualizar todas as opções usando uma abordagem com CTE e ROW_NUMBER

-- Pergunta 1: Comando npm install
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 1
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'npm install'
        WHEN 2 THEN 'npm update'
        WHEN 3 THEN 'npm create-react-app'
        WHEN 4 THEN 'npm start'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 2: Arquivos de Configuração React
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 2
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'package.json, src/index.js, public/index.html'
        WHEN 2 THEN 'App.js, index.js, README.md'
        WHEN 3 THEN 'package.json, node_modules, .gitignore'
        WHEN 4 THEN 'webpack.config.js, babel.config.js, tsconfig.json'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 3: Props em React
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 3
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Propriedades passadas de um componente pai para um componente filho'
        WHEN 2 THEN 'Propriedades locais do componente que não podem ser alteradas'
        WHEN 3 THEN 'Funções internas do componente que não são exportadas'
        WHEN 4 THEN 'Estados locais do componente que são privados'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 4: Componente Funcional
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 4
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'const Component = () => { return <div>Hello</div>; }'
        WHEN 2 THEN 'function Component() { return <div>Hello</div>; }'
        WHEN 3 THEN 'class Component extends React.Component { render() { return <div>Hello</div>; } }'
        WHEN 4 THEN 'var Component = function() { return <div>Hello</div>; }'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 5: Diferença Componentes
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 5
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Componentes de classe usam this e métodos de ciclo de vida, componentes funcionais usam hooks'
        WHEN 2 THEN 'Componentes funcionais são mais lentos que componentes de classe'
        WHEN 3 THEN 'Não há diferença, são apenas formas diferentes de escrever a mesma coisa'
        WHEN 4 THEN 'Componentes de classe não podem usar estado, apenas funcionais podem'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 6: useState Hook
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 6
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Hook para gerenciar estado local em componentes funcionais'
        WHEN 2 THEN 'Hook para executar efeitos colaterais após renderização'
        WHEN 3 THEN 'Hook para compartilhar estado entre componentes sem props'
        WHEN 4 THEN 'Hook para otimizar performance de renderização'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 7: useEffect Hook
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 7
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Executa efeitos colaterais após renderização, como chamadas de API ou manipulação do DOM'
        WHEN 2 THEN 'Gerencia estado local do componente'
        WHEN 3 THEN 'Cria variáveis constantes que não podem ser alteradas'
        WHEN 4 THEN 'Importa dependências externas para o componente'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 8: Estrutura Estado Todo
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 8
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Um array de objetos, cada um com propriedades como id, text e completed'
        WHEN 2 THEN 'Uma única string contendo todas as tarefas separadas por vírgula'
        WHEN 3 THEN 'Um número que representa a quantidade de tarefas'
        WHEN 4 THEN 'Um boolean que indica se há tarefas ou não'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 9: Adicionar Tarefa
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 9
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Usar spread operator para criar um novo array com o item adicionado: setTodos([...todos, newTodo])'
        WHEN 2 THEN 'Usar push diretamente no array: todos.push(newTodo)'
        WHEN 3 THEN 'Usar splice para inserir no meio do array: todos.splice(0, 0, newTodo)'
        WHEN 4 THEN 'Usar map para transformar cada item: todos.map(item => item)'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 10: Remover Tarefa
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 10
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Usar filter para criar um novo array sem o item: setTodos(todos.filter(todo => todo.id !== id))'
        WHEN 2 THEN 'Usar splice diretamente: todos.splice(index, 1)'
        WHEN 3 THEN 'Usar pop para remover o último item: todos.pop()'
        WHEN 4 THEN 'Usar shift para remover o primeiro item: todos.shift()'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 11: React Router
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 11
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Usar BrowserRouter, Routes e Route do react-router-dom para definir rotas'
        WHEN 2 THEN 'Usar window.location para navegar entre páginas'
        WHEN 3 THEN 'Usar redirects nativos do navegador com window.history'
        WHEN 4 THEN 'Usar links HTML normais <a href> que recarregam a página'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 12: Link Component
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 12
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Componente que permite navegação sem recarregar a página, mantendo o estado da SPA'
        WHEN 2 THEN 'Tag HTML padrão <a> que funciona igual em qualquer contexto'
        WHEN 3 THEN 'Função JavaScript que redireciona para outra página'
        WHEN 4 THEN 'Proprietário do React que criou a biblioteca'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 13: Lista Python
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 13
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'minha_lista = [1, 2, 3, "quatro"]'
        WHEN 2 THEN 'minha_lista = {1, 2, 3, "quatro"}'
        WHEN 3 THEN 'minha_lista = (1, 2, 3, "quatro")'
        WHEN 4 THEN 'minha_lista = 1, 2, 3, "quatro"'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 14: Dicionário Python
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 14
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'pessoa = {"nome": "João", "idade": 30}'
        WHEN 2 THEN 'pessoa = ["nome": "João", "idade": 30]'
        WHEN 3 THEN 'pessoa = ("nome": "João", "idade": 30)'
        WHEN 4 THEN 'pessoa = <"nome": "João", "idade": 30>'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 15: List Comprehensions
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 15
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Forma concisa e elegante de criar listas aplicando uma expressão a cada item de uma sequência'
        WHEN 2 THEN 'Método built-in de uma lista que retorna uma nova lista ordenada'
        WHEN 3 THEN 'Tipo especial de loop que só funciona com números'
        WHEN 4 THEN 'Operador matemático para calcular o tamanho de uma lista'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 16: Ler CSV Pandas
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 16
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'df = pd.read_csv("arquivo.csv")'
        WHEN 2 THEN 'df = pd.read_excel("arquivo.csv")'
        WHEN 3 THEN 'df = pd.load("arquivo.csv")'
        WHEN 4 THEN 'df = pd.import("arquivo.csv")'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 17: GroupBy Pandas
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 17
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Agrupa dados por uma ou mais colunas e permite aplicar funções de agregação (sum, mean, count, etc)'
        WHEN 2 THEN 'Ordena os dados do DataFrame por uma coluna específica'
        WHEN 3 THEN 'Filtra o DataFrame removendo linhas que não atendem uma condição'
        WHEN 4 THEN 'Mescla dois DataFrames diferentes em um único DataFrame'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 18: Filtrar DataFrame
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 18
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'df_filtrado = df[df["coluna"] > 10]'
        WHEN 2 THEN 'df_filtrado = df.filter("coluna")'
        WHEN 3 THEN 'df_filtrado = df.select("coluna")'
        WHEN 4 THEN 'df_filtrado = df.where("coluna")'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 19: SQL Injection
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 19
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Ataque que injeta código SQL malicioso através de entradas não validadas, podendo acessar ou modificar dados'
        WHEN 2 THEN 'Tipo de banco de dados usado para armazenar informações estruturadas'
        WHEN 3 THEN 'Método de segurança para proteger senhas no banco de dados'
        WHEN 4 THEN 'Protocolo de rede usado para comunicação com servidores de banco de dados'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 20: Autenticação vs Autorização
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 20
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Autenticação verifica quem você é (login), Autorização verifica o que você pode fazer (permissões)'
        WHEN 2 THEN 'São termos sinônimos que significam a mesma coisa'
        WHEN 3 THEN 'Autenticação é a senha, Autorização é o nome de usuário'
        WHEN 4 THEN 'Autenticação são as permissões, Autorização são as credenciais de acesso'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 21: OWASP Top 10
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 21
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Lista das 10 vulnerabilidades de segurança web mais críticas e comuns'
        WHEN 2 THEN 'Top 10 linguagens de programação mais populares para desenvolvimento web'
        WHEN 3 THEN 'Top 10 frameworks JavaScript mais utilizados em projetos web'
        WHEN 4 THEN 'Top 10 bancos de dados relacionais recomendados para aplicações web'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 22: Container Docker
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 22
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Instância executável e isolada de uma imagem Docker, rodando em um ambiente isolado'
        WHEN 2 THEN 'Imagem Docker que ainda não foi executada'
        WHEN 3 THEN 'Servidor físico onde as aplicações são hospedadas'
        WHEN 4 THEN 'Banco de dados usado para armazenar configurações do Docker'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- Pergunta 23: Imagem vs Container
WITH ranked_options AS (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "QuestionOptionEntity"
    WHERE "QuestionId" = 23
)
UPDATE "QuestionOptionEntity" qo
SET
    "Text" = CASE ranked_options.rn
        WHEN 1 THEN 'Imagem é um template/blueprint imutável, Container é uma instância em execução dessa imagem'
        WHEN 2 THEN 'São termos sinônimos que significam exatamente a mesma coisa'
        WHEN 3 THEN 'Container é o template, Imagem é a instância em execução'
        WHEN 4 THEN 'Não há diferença prática entre eles, são apenas nomes diferentes'
    END,
    "IsCorrect" = (ranked_options.rn = 1)
FROM ranked_options
WHERE qo."Id" = ranked_options."Id";

-- ===================================================================
-- VERIFICAÇÃO FINAL
-- ===================================================================
-- Verificar se todas as perguntas foram atualizadas
SELECT
    q."Id",
    q."Statement",
    COUNT(o."Id") as num_opcoes,
    SUM(CASE WHEN o."IsCorrect" = true THEN 1 ELSE 0 END) as respostas_corretas
FROM "QuestionEntity" q
LEFT JOIN "QuestionOptionEntity" o ON q."Id" = o."QuestionId"
GROUP BY q."Id", q."Statement"
ORDER BY q."Id";

SELECT 'Script de atualização concluído com sucesso!' as status;
