#!/usr/bin/env python3
"""
Script para atualizar TODAS as perguntas e opções no banco Supabase
com textos que fazem sentido e são interessantes para testes
"""

import psycopg2
import random

CONNECTION_STRING = "Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ekxsphpaocqpewmufmow;Password=JptEWOJvDTil3ggj;"

def parse_connection_string(conn_str):
    params = {}
    for part in conn_str.split(';'):
        if '=' in part:
            key, value = part.split('=', 1)
            key = key.strip().lower()
            value = value.strip()
            if key == 'host':
                params['host'] = value
            elif key == 'port':
                params['port'] = int(value)
            elif key == 'database':
                params['database'] = value
            elif key == 'username' or key == 'user':
                params['user'] = value
            elif key == 'password':
                params['password'] = value
    return params

# Banco de perguntas interessantes sobre programação
QUESTIONS_DB = [
    # React/JavaScript
    ("Qual é a diferença entre let, const e var em JavaScript?", [
        ("let permite reatribuição, const não permite, var tem escopo de função", True),
        ("Não há diferença, são sinônimos", False),
        ("let é mais rápido que const", False),
        ("var é a forma moderna recomendada", False),
    ]),
    ("O que é o Virtual DOM no React?", [
        ("Representação em memória do DOM real que permite otimizações de renderização", True),
        ("Um banco de dados virtual", False),
        ("Um servidor virtual para desenvolvimento", False),
        ("Uma biblioteca de componentes", False),
    ]),
    ("Qual hook do React é usado para memorizar valores computados?", [
        ("useMemo", True),
        ("useEffect", False),
        ("useState", False),
        ("useCallback", False),
    ]),
    ("O que faz o operador spread (...) em JavaScript?", [
        ("Expande arrays ou objetos em elementos individuais", True),
        ("Multiplica valores", False),
        ("Divide strings", False),
        ("Converte tipos", False),
    ]),
    ("Qual método de array retorna um novo array sem modificar o original?", [
        ("map, filter, slice", True),
        ("push, pop, shift", False),
        ("splice, reverse", False),
        ("sort, unshift", False),
    ]),

    # Python
    ("O que são decoradores em Python?", [
        ("Funções que modificam ou estendem o comportamento de outras funções", True),
        ("Tipos de dados especiais", False),
        ("Operadores matemáticos", False),
        ("Bibliotecas externas", False),
    ]),
    ("Qual é a diferença entre list e tuple em Python?", [
        ("List é mutável, tuple é imutável", True),
        ("Tuple é mutável, list é imutável", False),
        ("Não há diferença", False),
        ("List é mais rápido", False),
    ]),
    ("O que faz o método __init__ em Python?", [
        ("Inicializa uma instância de classe (construtor)", True),
        ("Finaliza uma classe", False),
        ("Importa módulos", False),
        ("Define variáveis globais", False),
    ]),
    ("Como você itera sobre chaves e valores de um dicionário Python?", [
        ("for chave, valor in dicionario.items()", True),
        ("for chave in dicionario.keys()", False),
        ("for valor in dicionario.values()", False),
        ("dicionario.iterate()", False),
    ]),
    ("O que é um gerador (generator) em Python?", [
        ("Função que retorna um iterador usando yield", True),
        ("Tipo de loop", False),
        ("Biblioteca padrão", False),
        ("Método de string", False),
    ]),

    # Banco de Dados
    ("O que é uma chave primária (primary key)?", [
        ("Identificador único e não nulo de uma linha na tabela", True),
        ("Chave estrangeira", False),
        ("Índice secundário", False),
        ("Tipo de join", False),
    ]),
    ("Qual é a diferença entre INNER JOIN e LEFT JOIN?", [
        ("INNER JOIN retorna apenas correspondências, LEFT JOIN retorna todas da esquerda", True),
        ("Não há diferença", False),
        ("LEFT JOIN é mais rápido", False),
        ("INNER JOIN retorna todas as linhas", False),
    ]),
    ("O que é normalização de banco de dados?", [
        ("Processo de organizar dados para reduzir redundância e dependências", True),
        ("Aumentar a velocidade", False),
        ("Adicionar mais tabelas", False),
        ("Criar backups", False),
    ]),
    ("O que é um índice em banco de dados?", [
        ("Estrutura que acelera consultas em colunas específicas", True),
        ("Tipo de tabela", False),
        ("Método de backup", False),
        ("Formato de dados", False),
    ]),
    ("O que é ACID em transações de banco de dados?", [
        ("Atomicity, Consistency, Isolation, Durability - propriedades de transações", True),
        ("Tipo de banco de dados", False),
        ("Linguagem de consulta", False),
        ("Protocolo de rede", False),
    ]),

    # Git/DevOps
    ("O que faz o comando git merge?", [
        ("Combina mudanças de uma branch em outra", True),
        ("Cria uma nova branch", False),
        ("Deleta commits", False),
        ("Faz backup do repositório", False),
    ]),
    ("Qual é a diferença entre git pull e git fetch?", [
        ("git pull baixa e mescla, git fetch apenas baixa", True),
        ("Não há diferença", False),
        ("git fetch é mais rápido", False),
        ("git pull apenas visualiza", False),
    ]),
    ("O que é CI/CD?", [
        ("Integração Contínua e Entrega Contínua - automação de build e deploy", True),
        ("Tipo de banco de dados", False),
        ("Framework JavaScript", False),
        ("Protocolo de rede", False),
    ]),
    ("O que é um Dockerfile?", [
        ("Arquivo de instruções para construir uma imagem Docker", True),
        ("Container em execução", False),
        ("Servidor Docker", False),
        ("Comando Docker", False),
    ]),
    ("O que é Kubernetes?", [
        ("Sistema de orquestração de containers", True),
        ("Tipo de banco de dados", False),
        ("Linguagem de programação", False),
        ("Editor de código", False),
    ]),

    # Segurança
    ("O que é HTTPS?", [
        ("Protocolo HTTP com criptografia SSL/TLS", True),
        ("Versão mais rápida do HTTP", False),
        ("Tipo de banco de dados", False),
        ("Linguagem de programação", False),
    ]),
    ("O que é XSS (Cross-Site Scripting)?", [
        ("Ataque que injeta scripts maliciosos em páginas web", True),
        ("Tipo de banco de dados", False),
        ("Framework JavaScript", False),
        ("Protocolo de rede", False),
    ]),
    ("O que é CORS?", [
        ("Cross-Origin Resource Sharing - política de segurança do navegador", True),
        ("Tipo de banco de dados", False),
        ("Linguagem de programação", False),
        ("Editor de código", False),
    ]),
    ("O que é hashing de senhas?", [
        ("Processo de converter senhas em valores irreversíveis para segurança", True),
        ("Criptografar senhas de forma reversível", False),
        ("Armazenar senhas em texto plano", False),
        ("Compartilhar senhas", False),
    ]),
    ("O que é autenticação de dois fatores (2FA)?", [
        ("Método de segurança que requer duas formas de verificação", True),
        ("Dois usuários fazendo login", False),
        ("Dois servidores", False),
        ("Dois bancos de dados", False),
    ]),

    # Algoritmos/Estruturas de Dados
    ("Qual é a complexidade de tempo do algoritmo de busca binária?", [
        ("O(log n)", True),
        ("O(n)", False),
        ("O(n²)", False),
        ("O(1)", False),
    ]),
    ("O que é uma estrutura de dados FIFO?", [
        ("First In First Out - como uma fila", True),
        ("Last In First Out - como uma pilha", False),
        ("Estrutura ordenada", False),
        ("Estrutura aleatória", False),
    ]),
    ("O que é recursão?", [
        ("Técnica onde uma função chama a si mesma", True),
        ("Tipo de loop", False),
        ("Estrutura de dados", False),
        ("Algoritmo de ordenação", False),
    ]),
    ("Qual é a diferença entre array e linked list?", [
        ("Array tem acesso O(1) por índice, linked list tem acesso O(n)", True),
        ("Não há diferença", False),
        ("Linked list é sempre mais rápido", False),
        ("Array não pode crescer", False),
    ]),
    ("O que é Big O notation?", [
        ("Notação para descrever complexidade de algoritmos", True),
        ("Tipo de algoritmo", False),
        ("Estrutura de dados", False),
        ("Linguagem de programação", False),
    ]),

    # Web Development
    ("O que é REST API?", [
        ("Arquitetura de API que usa métodos HTTP (GET, POST, PUT, DELETE)", True),
        ("Tipo de banco de dados", False),
        ("Framework JavaScript", False),
        ("Protocolo de rede", False),
    ]),
    ("O que é JSON?", [
        ("JavaScript Object Notation - formato de dados leve e legível", True),
        ("Linguagem de programação", False),
        ("Tipo de banco de dados", False),
        ("Framework web", False),
    ]),
    ("O que é AJAX?", [
        ("Asynchronous JavaScript and XML - técnica para requisições assíncronas", True),
        ("Framework JavaScript", False),
        ("Tipo de banco de dados", False),
        ("Linguagem de programação", False),
    ]),
    ("O que é um WebSocket?", [
        ("Protocolo de comunicação bidirecional em tempo real", True),
        ("Tipo de banco de dados", False),
        ("Framework JavaScript", False),
        ("Servidor web", False),
    ]),
    ("O que é CORS?", [
        ("Cross-Origin Resource Sharing - política de segurança do navegador", True),
        ("Tipo de banco de dados", False),
        ("Linguagem de programação", False),
        ("Editor de código", False),
    ]),
]

def update_all_questions():
    """Atualiza todas as perguntas e opções no banco"""
    conn_params = parse_connection_string(CONNECTION_STRING)
    conn = psycopg2.connect(**conn_params)
    cursor = conn.cursor()

    # Busca todas as perguntas
    cursor.execute('SELECT "Id" FROM "QuestionEntity" ORDER BY "Id"')
    question_ids = [row[0] for row in cursor.fetchall()]

    print(f"Encontradas {len(question_ids)} perguntas para atualizar")
    print(f"Banco de perguntas disponível: {len(QUESTIONS_DB)}")

    # Distribui as perguntas do banco entre todas as perguntas existentes
    questions_to_use = []
    for i in range(len(question_ids)):
        questions_to_use.append(QUESTIONS_DB[i % len(QUESTIONS_DB)])

    # Embaralha para variar
    random.seed(42)  # Seed fixo para reprodutibilidade
    questions_to_use = random.sample(QUESTIONS_DB * (len(question_ids) // len(QUESTIONS_DB) + 1), len(question_ids))

    updated = 0
    for q_id, (question_text, options) in zip(question_ids, questions_to_use):
        try:
            # Atualiza a pergunta
            cursor.execute(
                'UPDATE "QuestionEntity" SET "Statement" = %s WHERE "Id" = %s',
                (question_text, q_id)
            )

            # Busca as opções desta pergunta
            cursor.execute(
                'SELECT "Id" FROM "QuestionOptionEntity" WHERE "QuestionId" = %s ORDER BY "Id"',
                (q_id,)
            )
            option_ids = [row[0] for row in cursor.fetchall()]

            # Atualiza as opções
            for opt_id, (opt_text, is_correct) in zip(option_ids, options):
                cursor.execute(
                    'UPDATE "QuestionOptionEntity" SET "Text" = %s, "IsCorrect" = %s WHERE "Id" = %s',
                    (opt_text, is_correct, opt_id)
                )

            updated += 1
            if updated % 50 == 0:
                print(f"Atualizadas {updated}/{len(question_ids)} perguntas...")
                conn.commit()

        except Exception as e:
            print(f"Erro ao atualizar pergunta {q_id}: {e}")
            conn.rollback()
            continue

    conn.commit()
    print(f"\n✅ Total de perguntas atualizadas: {updated}")

    # Verificação final
    cursor.execute("""
        SELECT
            q."Id",
            q."Statement",
            COUNT(o."Id") as num_opcoes,
            SUM(CASE WHEN o."IsCorrect" = true THEN 1 ELSE 0 END) as respostas_corretas
        FROM "QuestionEntity" q
        LEFT JOIN "QuestionOptionEntity" o ON q."Id" = o."QuestionId"
        GROUP BY q."Id", q."Statement"
        ORDER BY q."Id"
        LIMIT 10
    """)

    print("\n=== Verificação (primeiras 10 perguntas) ===")
    for row in cursor.fetchall():
        q_id, statement, num_opts, correct = row
        print(f"ID {q_id}: {num_opts} opções, {correct} correta(s)")
        print(f"  Pergunta: {statement[:70]}...")

    cursor.close()
    conn.close()

if __name__ == "__main__":
    print("=" * 80)
    print("Atualizando TODAS as perguntas e opções no banco Supabase")
    print("=" * 80)
    update_all_questions()
    print("\n✅ Concluído!")


