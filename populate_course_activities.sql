-- ===================================================================
-- Script para Popular Atividades de Cursos com Questões
-- ===================================================================
-- Este script adiciona pelo menos 5 atividades para cada curso existente,
-- cada uma com questões e opções de múltipla escolha
-- ===================================================================

-- Variáveis para armazenar IDs
DO $$
DECLARE
    course_record RECORD;
    activity_id INTEGER;
    question_id INTEGER;
    option_id INTEGER;
    activity_counter INTEGER := 1;
    question_counter INTEGER := 1;
    option_counter INTEGER := 1;
    course_start_date TIMESTAMP;
    course_end_date TIMESTAMP;
    activity_start TIMESTAMP;
    activity_due TIMESTAMP;
BEGIN
    -- Loop através de todos os cursos
    FOR course_record IN 
        SELECT "Id", "Name", "StartDate", "EndDate" 
        FROM "CourseEntity" 
        WHERE "IsActive" = true
        ORDER BY "Id"
    LOOP
        RAISE NOTICE 'Processando curso: % (ID: %)', course_record."Name", course_record."Id";
        
        course_start_date := course_record."StartDate";
        course_end_date := course_record."EndDate";
        
        -- Criar 5 atividades para cada curso
        FOR activity_counter IN 1..5 LOOP
            -- Calcular datas da atividade (distribuídas ao longo do curso)
            activity_start := course_start_date + (course_end_date - course_start_date) * (activity_counter - 1) / 5.0;
            activity_due := activity_start + INTERVAL '7 days';
            
            -- Inserir atividade
            INSERT INTO "CourseActivitieEntity" (
                "Title", 
                "Description", 
                "StartDate", 
                "DueDate", 
                "MaxScore", 
                "CourseId",
                "CreateDate",
                "ModificationDate"
            ) VALUES (
                CASE activity_counter
                    WHEN 1 THEN 'Introdução e Conceitos Fundamentais'
                    WHEN 2 THEN 'Prática e Aplicação Básica'
                    WHEN 3 THEN 'Desenvolvimento Intermediário'
                    WHEN 4 THEN 'Avançado e Otimização'
                    WHEN 5 THEN 'Projeto Final e Consolidação'
                END || ' - ' || course_record."Name",
                CASE activity_counter
                    WHEN 1 THEN 'Esta atividade introduz os conceitos fundamentais do curso. Você aprenderá os princípios básicos e a teoria essencial para avançar nos próximos módulos.'
                    WHEN 2 THEN 'Nesta atividade prática, você aplicará os conhecimentos básicos aprendidos. Será uma oportunidade de colocar em prática os conceitos teóricos.'
                    WHEN 3 THEN 'Atividade intermediária que aprofunda os conhecimentos. Você trabalhará com conceitos mais complexos e técnicas avançadas.'
                    WHEN 4 THEN 'Atividade avançada focada em otimização e melhores práticas. Você explorará técnicas profissionais e padrões de mercado.'
                    WHEN 5 THEN 'Projeto final que consolida todo o aprendizado do curso. Esta atividade integra todos os conceitos estudados ao longo do módulo.'
                END,
                activity_start,
                activity_due,
                100,
                course_record."Id",
                NOW(),
                NOW()
            ) RETURNING "Id" INTO activity_id;
            
            RAISE NOTICE '  Atividade criada: ID %', activity_id;
            
            -- Criar 5 questões para cada atividade
            FOR question_counter IN 1..5 LOOP
                -- Inserir questão
                INSERT INTO "QuestionEntity" (
                    "CourseActivitieId",
                    "Statement",
                    "Order",
                    "Points",
                    "CreateDate",
                    "ModificationDate"
                ) VALUES (
                    activity_id,
                    CASE question_counter
                        WHEN 1 THEN 'Qual é o conceito principal abordado nesta atividade?'
                        WHEN 2 THEN 'Qual das seguintes opções representa melhor a aplicação prática deste conteúdo?'
                        WHEN 3 THEN 'Em relação às melhores práticas, qual alternativa está correta?'
                        WHEN 4 THEN 'Qual é a principal vantagem da técnica estudada nesta atividade?'
                        WHEN 5 THEN 'Qual das opções abaixo representa um erro comum a ser evitado?'
                    END,
                    question_counter,
                    20.0,
                    NOW(),
                    NOW()
                ) RETURNING "Id" INTO question_id;
                
                RAISE NOTICE '    Questão criada: ID %', question_id;
                
                -- Criar 4 opções para cada questão (1 correta + 3 incorretas)
                FOR option_counter IN 1..4 LOOP
                    INSERT INTO "QuestionOptionEntity" (
                        "QuestionId",
                        "Text",
                        "IsCorrect",
                        "CreateDate",
                        "ModificationDate"
                    ) VALUES (
                        question_id,
                        CASE option_counter
                            WHEN 1 THEN 'Opção correta: Esta é a resposta adequada baseada nos conceitos estudados.'
                            WHEN 2 THEN 'Opção incorreta: Esta alternativa contém informações parciais mas não está completamente correta.'
                            WHEN 3 THEN 'Opção incorreta: Esta opção apresenta um conceito relacionado mas não é a resposta adequada.'
                            WHEN 4 THEN 'Opção incorreta: Esta alternativa está incorreta e pode levar a confusão se selecionada.'
                        END,
                        CASE WHEN option_counter = 1 THEN true ELSE false END,
                        NOW(),
                        NOW()
                    );
                END LOOP;
            END LOOP;
        END LOOP;
    END LOOP;
    
    RAISE NOTICE 'População de atividades concluída com sucesso!';
END $$;

-- Verificar resultados
SELECT 
    c."Name" as "Curso",
    COUNT(DISTINCT ca."Id") as "Total Atividades",
    COUNT(DISTINCT q."Id") as "Total Questões",
    COUNT(DISTINCT qo."Id") as "Total Opções"
FROM "CourseEntity" c
LEFT JOIN "CourseActivitieEntity" ca ON ca."CourseId" = c."Id"
LEFT JOIN "QuestionEntity" q ON q."CourseActivitieId" = ca."Id"
LEFT JOIN "QuestionOptionEntity" qo ON qo."QuestionId" = q."Id"
WHERE c."IsActive" = true
GROUP BY c."Id", c."Name"
ORDER BY c."Id";


