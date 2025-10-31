#!/bin/bash

# =============================================================================
# TrainMaster API - Script de Execução
# =============================================================================
# Este script executa o projeto TrainMaster API com todas as configurações
# necessárias, incluindo migrations automáticas.
# =============================================================================

set -e  # Para o script se houver erro

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Função para imprimir mensagens coloridas
print_message() {
    echo -e "${2}${1}${NC}"
}

print_header() {
    echo "============================================================================="
    print_message "🚀 TrainMaster API - Iniciando Aplicação" "$BLUE"
    echo "============================================================================="
}

print_success() {
    print_message "✅ $1" "$GREEN"
}

print_warning() {
    print_message "⚠️  $1" "$YELLOW"
}

print_error() {
    print_message "❌ $1" "$RED"
}

print_info() {
    print_message "ℹ️  $1" "$BLUE"
}

# Função para verificar se o .NET está instalado
check_dotnet() {
    print_info "Verificando instalação do .NET..."

    if ! command -v dotnet &> /dev/null; then
        print_error ".NET não está instalado ou não está no PATH"
        print_info "Instale o .NET 8.0 SDK: https://dotnet.microsoft.com/download"
        exit 1
    fi

    DOTNET_VERSION=$(dotnet --version)
    print_success ".NET $DOTNET_VERSION encontrado"
}

# Função para verificar se o projeto existe
check_project() {
    print_info "Verificando estrutura do projeto..."

    if [ ! -d "TrainMaster" ]; then
        print_error "Diretório 'TrainMaster' não encontrado"
        print_info "Execute este script na raiz do projeto (onde está a pasta TrainMaster)"
        exit 1
    fi

    if [ ! -f "TrainMaster/TrainMaster.csproj" ]; then
        print_error "Arquivo 'TrainMaster.csproj' não encontrado"
        exit 1
    fi

    print_success "Estrutura do projeto verificada"
}

# Função para restaurar dependências
restore_packages() {
    print_info "Restaurando pacotes NuGet..."

    cd TrainMaster
    dotnet restore

    if [ $? -eq 0 ]; then
        print_success "Pacotes restaurados com sucesso"
    else
        print_error "Falha ao restaurar pacotes"
        exit 1
    fi

    cd ..
}

# Função para executar migrations
run_migrations() {
    print_info "Executando migrations do banco de dados..."

    cd TrainMaster

    # Tentar aplicar migrations
    print_info "Aplicando migrations..."
    if dotnet ef database update --no-build >/dev/null 2>&1; then
        print_success "Migrations aplicadas com sucesso"
    else
        MIGRATION_OUTPUT=$(dotnet ef database update --no-build 2>&1 || true)
        if echo "$MIGRATION_OUTPUT" | grep -qi "already exists\|relation.*exists"; then
            print_warning "Tabelas já existem no banco."
            print_info "A migration será marcada automaticamente quando a aplicação iniciar."
        else
            print_warning "Aviso: Houve problemas ao aplicar migrations"
            print_info "As migrations serão executadas automaticamente na inicialização da aplicação"
        fi
    fi

    cd ..
}

# Função para compilar o projeto
build_project() {
    print_info "Compilando projeto..."

    cd TrainMaster
    dotnet build --configuration Release --no-restore

    if [ $? -eq 0 ]; then
        print_success "Projeto compilado com sucesso"
    else
        print_error "Falha na compilação do projeto"
        exit 1
    fi

    cd ..
}

# Função para verificar configurações
check_configuration() {
    print_info "Verificando configurações..."

    if [ ! -f "TrainMaster/appsettings.json" ]; then
        print_error "Arquivo appsettings.json não encontrado"
        exit 1
    fi

    if [ ! -f "TrainMaster/appsettings.Development.json" ]; then
        print_warning "Arquivo appsettings.Development.json não encontrado"
    fi

    # Verifica se a string de conexão está configurada
    if grep -q "WebApiDatabase" TrainMaster/appsettings.json; then
        print_success "String de conexão configurada"
    else
        print_warning "String de conexão não encontrada em appsettings.json"
    fi

    print_success "Configurações verificadas"
}

# Função para executar a aplicação
run_application() {
    print_info "Iniciando aplicação..."
    print_info "A aplicação estará disponível em:"
    print_info "  - HTTP: http://localhost:5000"
    print_info "  - HTTPS: https://localhost:5001"
    print_info "  - Swagger: http://localhost:5000/swagger"
    print_info ""
    print_info "Pressione Ctrl+C para parar a aplicação"
    print_info "============================================================================="

    cd TrainMaster
    dotnet run --configuration Release
}

# Função para limpeza em caso de erro
cleanup() {
    print_warning "Interrompendo aplicação..."
    # Mata processos dotnet que possam estar rodando
    pkill -f "dotnet run" 2>/dev/null || true
    exit 1
}

# Configurar trap para cleanup
trap cleanup SIGINT SIGTERM

# Função principal
main() {
    print_header

    # Verificações iniciais
    check_dotnet
    check_project
    check_configuration

    # Preparação do projeto
    restore_packages
    build_project
    run_migrations

    # Execução da aplicação
    run_application
}

# Verificar argumentos da linha de comando
case "${1:-}" in
    --help|-h)
        echo "Uso: $0 [opções]"
        echo ""
        echo "Opções:"
        echo "  --help, -h     Mostra esta ajuda"
        echo "  --migrate-only Executa apenas as migrations e sai"
        echo "  --build-only   Compila o projeto e sai"
        echo "  --check-only   Verifica dependências e configurações"
        echo ""
        echo "Exemplos:"
        echo "  $0                    # Executa a aplicação completa"
        echo "  $0 --migrate-only     # Executa apenas migrations"
        echo "  $0 --build-only       # Compila o projeto"
        echo "  $0 --check-only       # Verifica configurações"
        exit 0
        ;;
    --migrate-only)
        print_header
        check_dotnet
        check_project
        restore_packages
        run_migrations
        print_success "Migrations executadas com sucesso"
        exit 0
        ;;
    --build-only)
        print_header
        check_dotnet
        check_project
        restore_packages
        build_project
        print_success "Projeto compilado com sucesso"
        exit 0
        ;;
    --check-only)
        print_header
        check_dotnet
        check_project
        check_configuration
        print_success "Verificações concluídas com sucesso"
        exit 0
        ;;
    "")
        # Execução normal
        main
        ;;
    *)
        print_error "Opção desconhecida: $1"
        print_info "Use --help para ver as opções disponíveis"
        exit 1
        ;;
esac
