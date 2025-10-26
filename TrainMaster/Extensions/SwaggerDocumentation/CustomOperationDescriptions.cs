using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TrainMaster.Extensions.SwaggerDocumentation
{
    public class CustomOperationDescriptions : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context?.ApiDescription?.HttpMethod is null || context.ApiDescription.RelativePath is null)
                return;

            var path = context.ApiDescription.RelativePath.ToLowerInvariant();

            var routeHandlers = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
                { "auth",               () => HandleAuthOperations(operation, context) },
                { "user",               () => HandleUserOperations(operation, context) },
                { "profile",            () => HandlePessoalProfileOperations(operation, context) },
                { "educationlevel",     () => HandleEducationLevelOperations(operation, context) },
                { "professionalprofile",() => HandleProfessionalProfileOperations(operation, context) },
                { "address",            () => HandleAddressOperations(operation, context) },
                { "course-feedback",    () => HandleCourseFeedbackOperations(operation, context) },
                { "course",             () => HandleCourseOperations(operation, context) },
                { "departments",        () => HandleDeparmentOperations(operation, context) },
                { "team",               () => HandleTeamOperations(operation, context) },
                { "notifications",      () => HandleNotificationOperations(operation, context) },
                { "course-activities",  () => HandleCourseActivitiesOperations(operation, context) },
                { "questions",          () => HandleQuestionsOperations(operation, context) },
                { "historypassword",    () => HandleHistoryPasswordOperations(operation, context) },
                { "badges",             () => HandleBadgeOperations(operation, context) }
            };

            foreach (var kv in routeHandlers
                     .OrderByDescending(k => k.Key.Length))
            {
                if (path.Contains(kv.Key))
                {
                    kv.Value.Invoke();
                    return;
                }
            }
        }

        private void HandleBadgeOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            var path = (context.ApiDescription.RelativePath ?? string.Empty).ToLowerInvariant();

            if (method == "GET")
            {
                operation.Summary = "Retornar as badges por ID - usuário.";
                operation.Description = "Esse endpoint é responsável por retornar as badges por ID - usuário.";
                AddResponses(operation, "201", "Retornado com sucesso.");
                return;
            }
        }

        private void HandleQuestionsOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            var path = (context.ApiDescription.RelativePath ?? string.Empty).ToLowerInvariant();

            if (method == "POST")
            {
                operation.Summary = "Criar questões para atividades de curso.";
                operation.Description = "Esse endpoint é responsável por criar questões para atividades de curso.";
                AddResponses(operation, "201", "Questões criadas com sucesso.");
                return;
            }
        }

        private void HandleCourseActivitiesOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            var path = (context.ApiDescription.RelativePath ?? string.Empty).ToLowerInvariant();

            if (method == "POST")
            {
                operation.Summary = "Criar uma nova atividade para o curso";
                operation.Description = "Esse endpoint é responsável por criar uma nova atividade para o curso.";
                AddResponses(operation, "201", "Curso criado com sucesso.");
                return;
            }

            if (method == "PUT")
            {
                operation.Summary = "Atualizar uma nova atividade para o curso";
                operation.Description = "Esse endpoint é responsável por Atualizar uma nova atividade para o curso.";
                AddResponses(operation, "201", "Atualizado criado com sucesso.");
                return;
            }
            else if (method == "GET")
            {
                if (path.Contains("all"))
                {
                    operation.Summary = "Retornar todas as atividades dos cursos.";
                    operation.Description = "Esse endpoint é responsável por retornar todas as atividades dos cursos.";
                    AddResponses(operation, "200", "Todas as atividades dos cursos retornada com sucesso.");
                }
                else
                {
                    operation.Summary = "Retornar todas as atividades dos cursos por id.";
                    operation.Description = "Esse endpoint é responsável por retornar todas as atividades dos cursos por id.";
                    AddResponses(operation, "200", "Todos as atividades do curso buscado por ID retornado com sucesso.");
                }
            }
        }

        private void HandleNotificationOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            var path = (context.ApiDescription.RelativePath ?? string.Empty).ToLowerInvariant();

            if (method == "GET")
            {
                operation.Summary = "Listar todas as notificações do usuário";
                operation.Description = "Retorna todas as notificações.";
                AddResponses(operation, "200", "Notificações retornadas com sucesso.");
                AddResponses(operation, "404", "Nenhuma notificação encontrada.");
            }
        }

        private void HandleCourseFeedbackOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            var path = (context.ApiDescription.RelativePath ?? string.Empty).ToLowerInvariant();

            if (method == "GET")
            {
                if (path.Contains("/course-feedback/by-course"))
                {
                    operation.Summary = "Listar todos os feedbacks pelo ID do curso";
                    operation.Description = "Retorna todos os feedbacks associados ao curso informado pelo ID (courseId).";
                    AddResponses(operation, "200", "Feedbacks do curso retornados com sucesso.");
                    AddResponses(operation, "404", "Nenhum feedback encontrado para o curso informado.");
                }
                else
                {
                    operation.Summary = "Listar todos os feedbacks";
                    operation.Description = "Retorna todos os feedbacks de todos os cursos.";
                    AddResponses(operation, "200", "Feedbacks retornados com sucesso.");
                    AddResponses(operation, "404", "Nenhum feedback encontrado.");
                }
            }
        }

        private void HandleAuthOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                if (path.Contains("login"))
                {
                    operation.Summary = "Realizar o login do usuário.";
                    operation.Description = "Esse endpoint é responsável pelo login do sistema.";
                    AddResponses(operation, "200", "Usuário logado com sucesso.");
                }
                else
                {
                    operation.Summary = "Resetar a senha do usuário.";
                    operation.Description = "Esse endpoint é responsável por resetar a senha.";
                    AddResponses(operation, "200", "Senha resetada com sucesso.");
                }
            }
        }

        private void HandleCourseOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            var path = (context.ApiDescription.RelativePath ?? string.Empty).ToLowerInvariant();

            if (method == "POST")
            {
                operation.Summary = "Criar um novo curso.";
                operation.Description = "Esse endpoint é responsável por criar um novo curso.";
                AddResponses(operation, "201", "Curso criado com sucesso.");
                return;
            }

            if (method == "PUT")
            {
                operation.Summary = "Atualizar um curso.";
                operation.Description = "Esse endpoint é responsável por atualizar um curso pelo ID.";
                AddResponses(operation, "200", "Curso atualizado com sucesso.");
                AddResponses(operation, "404", "Curso não encontrado.");
                return;
            }

            if (method == "DELETE")
            {
                operation.Summary = "Deletar um curso.";
                operation.Description = "Esse endpoint é responsável por deletar um curso por ID.";
                AddResponses(operation, "204", "Curso deletado com sucesso.");
                AddResponses(operation, "404", "Curso não encontrado.");
                return;
            }

            if (method == "GET")
            {
                if (path.Contains("/getbyname"))
                {
                    operation.Summary = "Obter curso pelo nome.";
                    operation.Description = "Esse endpoint retorna o curso filtrando pelo nome.";
                    AddResponses(operation, "200", "Curso retornado com sucesso.");
                }
                else if (path.Contains("/getbyuserid"))
                {
                    operation.Summary = "Listar cursos por usuário.";
                    operation.Description = "Esse endpoint retorna os cursos associados a um usuário (UserId).";
                    AddResponses(operation, "200", "Cursos retornados com sucesso.");
                }
                else if (path.Contains("{id}"))
                {
                    operation.Summary = "Obter curso por ID.";
                    operation.Description = "Esse endpoint retorna um curso específico pelo seu ID.";
                    AddResponses(operation, "200", "Curso retornado com sucesso.");
                    AddResponses(operation, "404", "Curso não encontrado.");
                }
                else
                {
                    operation.Summary = "Listar cursos.";
                    operation.Description = "Esse endpoint retorna a lista de cursos.";
                    AddResponses(operation, "200", "Cursos retornados com sucesso.");
                }
            }
        }

        private void HandleUserOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Criar novo usuário.";
                operation.Description = "Esse endpoint é responsável por criar um usuário.";
                AddResponses(operation, "200", "Usuário criado.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Atualizar o usuário.";
                operation.Description = "Esse endpoint é responsável por atualizar o usuário.";
                AddResponses(operation, "200", "Usuário atualizado com sucesso.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Deletar um usuário.";
                operation.Description = "Esse endpoint é responsável por deletar o usuário por um ID.";
                AddResponses(operation, "200", "Usuário deletado com sucesso.");
                AddResponses(operation, "404", "Usuário não encontrado.");
            }
            else if (method == "GET")
            {
                if (path.Contains("allactives"))
                {
                    operation.Summary = "Retornar todos os usuários.";
                    operation.Description = "Esse endpoint é responsável por retornar todos os usuários ativos.";
                    AddResponses(operation, "200", "Usuários ativos retornados com sucesso.");
                }
                else if (path.Contains("all"))
                {
                    operation.Summary = "Retornar todos os usuários.";
                    operation.Description = "Esse endpoint é responsável por retornar todos os usuários.";
                    AddResponses(operation, "200", "Todos os usuários retornados com sucesso.");
                }
            }
        }

        private void HandlePessoalProfileOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod == "POST")
            {
                operation.Summary = "Adicionar o novo perfil pessoal.";
                operation.Description = "Esse endpoint é responsável por criar um perfil pessoal.";
                AddResponses(operation, "200", "Perfil pessoal foi criado com sucesso.");
            }
            else if (context.ApiDescription.HttpMethod == "PUT")
            {
                operation.Summary = "Atualizar o perfil pessoal.";
                operation.Description = "Esse endpoint é responsável por atualizar um perfil pessoal.";
                AddResponses(operation, "200", "Perfil pessoal foi atualizado com sucesso.");
            }
            else if (context.ApiDescription.HttpMethod == "GET")
            {
                operation.Summary = "Retornar o perfil pessoal.";
                operation.Description = "Esse endpoint é responsável por retornar o perfil pessoal.";
                AddResponses(operation, "200", "Perfil pessoal foi retornado com sucesso.");
            }
        }

        private void HandleEducationLevelOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new Education Level.";
                operation.Description = "This endpoint allows you to create a new Education Level by providing the necessary details.";
                AddResponses(operation, "200", "The Education Level was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing Education Level.";
                operation.Description = "This endpoint allows you to update an existing Education Level by providing the necessary details.";
                AddResponses(operation, "200", "The Education Level was successfully updated.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Delete an existing Education Level.";
                operation.Description = "This endpoint allows you to delete an existing Education Level by providing the ID.";
                AddResponses(operation, "200", "The Education Level was successfully deleted.");
                AddResponses(operation, "404", "Education Level not found. Please verify the ID.");
            }
            else if (method == "GET")
            {
                operation.Summary = "Retrieve all Education Levels.";
                operation.Description = "This endpoint allows you to retrieve details of all existing Education Levels.";
                AddResponses(operation, "200", "All Education Levels details were successfully retrieved.");
            }
        }

        private void HandleProfessionalProfileOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new Professional Profile.";
                operation.Description = "This endpoint allows you to create a new Professional Profile by providing the necessary details.";
                AddResponses(operation, "200", "The Professional Profile was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing Professional Profile.";
                operation.Description = "This endpoint allows you to update an existing Professional Profile by providing the necessary details.";
                AddResponses(operation, "200", "The Professional Profile was successfully updated.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Delete an existing Professional Profile.";
                operation.Description = "This endpoint allows you to delete an existing Professional Profile by providing the ID.";
                AddResponses(operation, "200", "The Professional Profile was successfully deleted.");
                AddResponses(operation, "404", "Professional Profile not found. Please verify the ID.");
            }
            else if (method == "GET")
            {
                operation.Summary = "Retrieve all Professional Profiles.";
                operation.Description = "This endpoint allows you to retrieve details of all existing Professional Profiles.";
                AddResponses(operation, "200", "All Professional Profiles details were successfully retrieved.");
            }
        }

        private void HandleAddressOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new Address.";
                operation.Description = "This endpoint allows you to create a new Address by providing the necessary details.";
                AddResponses(operation, "200", "The Address was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing Address.";
                operation.Description = "This endpoint allows you to update an existing Address by providing the necessary details.";
                AddResponses(operation, "200", "The Address was successfully updated.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Delete an existing Address.";
                operation.Description = "This endpoint allows you to delete an existing Address by providing the ID.";
                AddResponses(operation, "200", "The Address was successfully deleted.");
                AddResponses(operation, "404", "Address not found. Please verify the ID.");
            }
            else if (method == "GET")
            {
                if (path.Contains("postalCode", StringComparison.OrdinalIgnoreCase))
                {
                    operation.Summary = "Retrieve Addresses.";
                    operation.Description = "This endpoint returns Addresses.";
                    AddResponses(operation, "200", "Addresses were successfully retrieved.");
                }
                else if (path.Contains("all"))
                {
                    operation.Summary = "Retrieve all Addresses.";
                    operation.Description = "This endpoint allows you to retrieve details of all existing Addresses.";
                    AddResponses(operation, "200", "All Addresses details were successfully retrieved.");
                }
            }
        }
        private void HandleDeparmentOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Criar um novo departamento.";
                operation.Description = "Esse endpoint é responsável por criar um novo departamento, fornecendo os dados necessários.";
                AddResponses(operation, "201", "Departamento criado com sucesso.");
                return;
            }

            if (method == "PUT")
            {
                operation.Summary = "Atualizar um departamento existente.";
                operation.Description = "Esse endpoint é responsável por atualizar um departamento existente, fornecendo as informações necessárias.";
                AddResponses(operation, "200", "Departamento atualizado com sucesso.");
                AddResponses(operation, "404", "Departamento não encontrado.");
                return;
            }

            if (method == "DELETE")
            {
                operation.Summary = "Excluir um departamento.";
                operation.Description = "Esse endpoint é responsável por excluir um departamento com base no ID informado.";
                AddResponses(operation, "204", "Departamento excluído com sucesso.");
                AddResponses(operation, "404", "Departamento não encontrado.");
                return;
            }

            if (method == "GET")
            {
                if (path.Contains("{id}"))
                {
                    operation.Summary = "Obter departamento por ID.";
                    operation.Description = "Esse endpoint retorna as informações de um departamento específico com base no ID.";
                    AddResponses(operation, "200", "Departamento retornado com sucesso.");
                    AddResponses(operation, "404", "Departamento não encontrado.");
                }
                else
                {
                    operation.Summary = "Listar todos os departamentos.";
                    operation.Description = "Esse endpoint retorna a lista de todos os departamentos cadastrados.";
                    AddResponses(operation, "200", "Departamentos retornados com sucesso.");
                }
            }
        }

        private void HandleTeamOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "POST")
            {
                operation.Summary = "Create a new Team.";
                operation.Description = "This endpoint allows you to create a new Team by providing the necessary details.";
                AddResponses(operation, "200", "The Team was successfully created.");
            }
            else if (method == "PUT")
            {
                operation.Summary = "Update an existing Team.";
                operation.Description = "This endpoint allows you to update an existing Team by providing the necessary details.";
                AddResponses(operation, "200", "The Team was successfully updated.");
            }
            else if (method == "DELETE")
            {
                operation.Summary = "Delete an existing Team.";
                operation.Description = "This endpoint allows you to delete an existing Team by providing the ID.";
                AddResponses(operation, "200", "The Team was successfully deleted.");
                AddResponses(operation, "404", "Team not found. Please verify the ID.");
            }
            else if (method == "GET")
            {
                operation.Summary = "Retrieve all Teams.";
                operation.Description = "This endpoint allows you to retrieve details of all existing Teams.";
                AddResponses(operation, "200", "All Teams details were successfully retrieved.");
            }
        }

        private void HandleHistoryPasswordOperations(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod;
            var path = context.ApiDescription.RelativePath?.ToLower() ?? string.Empty;

            if (method == "PATCH")
            {
                operation.Summary = "Update the password to History Password.";
                operation.Description = "This endpoint allows you to update the password to History Password.";
                AddResponses(operation, "200", "The History Password was successfully updated.");
            }
        }

        private void AddResponses(OpenApiOperation operation, string statusCode, string description)
        {
            if (!operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses.Add(statusCode, new OpenApiResponse { Description = description });
            }
        }
    }
}
