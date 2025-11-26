#!/bin/bash

# Script de Deploy para Azure App Service - Nova Conta/Resource Group
# TrainMaster API - .NET 8.0

set -e  # Para em caso de erro

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configurações
APP_NAME="${1:-trainmaster-api-new-$(date +%s | tail -c 6)}"
RESOURCE_GROUP="${2:-trainmaster-new-rg}"
LOCATION="${3:-canadacentral}"  # Usando Canada Central onde já há quota
PLAN_NAME="${4:-trainmaster-new-plan}"
SKU="${5:-F1}"
RUNTIME="DOTNETCORE:8.0"

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Deploy TrainMaster API para Azure${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Verificar se Azure CLI está instalado
if ! command -v az &> /dev/null; then
    echo -e "${RED}Azure CLI não está instalado. Por favor, instale primeiro.${NC}"
    exit 1
fi

# Verificar login
echo -e "${YELLOW}Verificando login no Azure...${NC}"
CURRENT_ACCOUNT=$(az account show --query name -o tsv 2>/dev/null || echo "")
if [ -z "$CURRENT_ACCOUNT" ]; then
    echo -e "${RED}Você não está logado no Azure. Execute: az login${NC}"
    exit 1
fi

echo -e "${GREEN}✓ Logado como: $(az account show --query user.name -o tsv)${NC}"
echo -e "${GREEN}✓ Subscription: $(az account show --query name -o tsv)${NC}"
echo -e "${GREEN}✓ Subscription ID: $(az account show --query id -o tsv)${NC}"
echo ""

# Configurações
echo -e "${YELLOW}Configurações do Deploy:${NC}"
echo -e "App Name: ${GREEN}$APP_NAME${NC}"
echo -e "Resource Group: ${GREEN}$RESOURCE_GROUP${NC}"
echo -e "Location: ${GREEN}$LOCATION${NC}"
echo -e "Plan: ${GREEN}$PLAN_NAME${NC}"
echo -e "SKU: ${GREEN}$SKU${NC}"
echo ""

# Verificar se há plano existente que possamos usar
EXISTING_PLAN=$(az appservice plan list --query "[?resourceGroup=='trainmasterapi' && location=='$LOCATION'].[0].name" -o tsv 2>/dev/null || echo "")

if [ ! -z "$EXISTING_PLAN" ]; then
    echo -e "${YELLOW}Plano existente encontrado: $EXISTING_PLAN${NC}"
    echo -e "${YELLOW}Deseja usar o plano existente? (s/n)${NC}"
    read -r USE_EXISTING
    if [ "$USE_EXISTING" = "s" ] || [ "$USE_EXISTING" = "S" ]; then
        PLAN_NAME="$EXISTING_PLAN"
        RESOURCE_GROUP_PLAN="trainmasterapi"
        echo -e "${GREEN}✓ Usando plano existente: $PLAN_NAME${NC}"
    else
        RESOURCE_GROUP_PLAN="$RESOURCE_GROUP"
    fi
else
    RESOURCE_GROUP_PLAN="$RESOURCE_GROUP"
fi

# Criar Resource Group
echo -e "${YELLOW}[1/6] Criando/Verificando Resource Group...${NC}"
if az group show --name "$RESOURCE_GROUP" &>/dev/null; then
    echo -e "${GREEN}✓ Resource Group já existe${NC}"
else
    az group create --name "$RESOURCE_GROUP" --location "$LOCATION"
    echo -e "${GREEN}✓ Resource Group criado${NC}"
fi

# Criar App Service Plan (se não estiver usando existente)
if [ "$RESOURCE_GROUP_PLAN" = "$RESOURCE_GROUP" ]; then
    echo -e "${YELLOW}[2/6] Criando App Service Plan...${NC}"
    if az appservice plan show --name "$PLAN_NAME" --resource-group "$RESOURCE_GROUP" &>/dev/null; then
        echo -e "${GREEN}✓ App Service Plan já existe${NC}"
    else
        # Tentar criar o plano
        if az appservice plan create \
            --name "$PLAN_NAME" \
            --resource-group "$RESOURCE_GROUP" \
            --location "$LOCATION" \
            --sku "$SKU" \
            --is-linux 2>&1; then
            echo -e "${GREEN}✓ App Service Plan criado${NC}"
        else
            echo -e "${YELLOW}⚠ Não foi possível criar o plano. Tentando usar plano existente...${NC}"
            # Tentar encontrar qualquer plano disponível
            AVAILABLE_PLAN=$(az appservice plan list --query "[0].name" -o tsv)
            AVAILABLE_RG=$(az appservice plan list --query "[0].resourceGroup" -o tsv)
            if [ ! -z "$AVAILABLE_PLAN" ]; then
                PLAN_NAME="$AVAILABLE_PLAN"
                RESOURCE_GROUP_PLAN="$AVAILABLE_RG"
                echo -e "${GREEN}✓ Usando plano existente: $PLAN_NAME no resource group $RESOURCE_GROUP_PLAN${NC}"
            else
                echo -e "${RED}✗ Não foi possível criar ou encontrar um plano disponível${NC}"
                exit 1
            fi
        fi
    fi
else
    echo -e "${YELLOW}[2/6] Usando App Service Plan existente...${NC}"
    echo -e "${GREEN}✓ Plano: $PLAN_NAME no resource group $RESOURCE_GROUP_PLAN${NC}"
fi

# Criar App Service
echo -e "${YELLOW}[3/6] Criando App Service...${NC}"
if az webapp show --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" &>/dev/null; then
    echo -e "${GREEN}✓ App Service já existe, atualizando configuração...${NC}"
    # Atualizar runtime se necessário
    az webapp config set \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --linux-fx-version "$RUNTIME"
else
    az webapp create \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --plan "$PLAN_NAME" \
        --runtime "$RUNTIME"
    echo -e "${GREEN}✓ App Service criado${NC}"
fi

# Configurar App Service
echo -e "${YELLOW}[4/6] Configurando App Service...${NC}"

# Habilitar sempre on (se o SKU permitir - F1 não permite)
if [ "$SKU" != "F1" ]; then
    az webapp config set \
        --name "$APP_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --always-on true
fi

# Configurar porta e variáveis de ambiente
az webapp config appsettings set \
    --name "$APP_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --settings \
        ASPNETCORE_ENVIRONMENT=Production \
        ASPNETCORE_URLS=http://+:80 \
        WEBSITES_ENABLE_APP_SERVICE_STORAGE=false

# Configurar connection string do banco
CONNECTION_STRING="Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ekxsphpaocqpewmufmow;Password=JptEWOJvDTil3ggj;"
az webapp config connection-string set \
    --name "$APP_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --connection-string-type PostgreSQL \
    --settings "WebApiDatabase=$CONNECTION_STRING"

echo -e "${GREEN}✓ App Service configurado${NC}"

# Publicar aplicação
echo -e "${YELLOW}[5/6] Publicando aplicação...${NC}"
cd TrainMaster

# Limpar builds anteriores
echo "Limpando builds anteriores..."
dotnet clean --verbosity quiet || true

# Publicar
echo "Publicando projeto..."
dotnet publish -c Release -o ./publish-azure --verbosity minimal

# Verificar se zip está disponível
if ! command -v zip &> /dev/null; then
    echo "zip não está disponível, usando Python..."
    if ! command -v python3 &> /dev/null; then
        echo -e "${RED}zip e python3 não estão disponíveis. Instale um deles.${NC}"
        exit 1
    fi
fi

# Fazer deploy usando zip deploy
echo "Criando pacote ZIP..."
cd publish-azure
if command -v zip &> /dev/null; then
    zip -r ../deploy.zip . > /dev/null 2>&1
else
    python3 -m zipfile -c ../deploy.zip .
fi
cd ..

echo "Fazendo deploy para Azure..."
az webapp deployment source config-zip \
    --resource-group "$RESOURCE_GROUP" \
    --name "$APP_NAME" \
    --src ./deploy.zip

echo -e "${GREEN}✓ Aplicação publicada${NC}"

# Limpar arquivo zip temporário
rm -f deploy.zip
cd ..

# Obter URL da aplicação
APP_URL="https://${APP_NAME}.azurewebsites.net"
echo ""
echo -e "${GREEN}[6/6] Deploy concluído!${NC}"
echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Informações do Deploy${NC}"
echo -e "${GREEN}========================================${NC}"
echo -e "App Name: ${GREEN}$APP_NAME${NC}"
echo -e "URL: ${GREEN}$APP_URL${NC}"
echo -e "Swagger: ${GREEN}$APP_URL/swagger${NC}"
echo -e "Resource Group: ${GREEN}$RESOURCE_GROUP${NC}"
echo ""

# Testar aplicação
echo -e "${YELLOW}Testando aplicação...${NC}"
echo "Aguardando aplicação iniciar (60 segundos para cold start no F1)..."
sleep 60

# Testar endpoint
echo "Testando endpoint principal..."
MAX_RETRIES=8
RETRY_COUNT=0
HTTP_CODE="000"

while [ $RETRY_COUNT -lt $MAX_RETRIES ]; do
    HTTP_CODE=$(curl -s -o /dev/null -w "%{http_code}" --max-time 15 "$APP_URL/swagger" || echo "000")

    if [ "$HTTP_CODE" = "200" ] || [ "$HTTP_CODE" = "301" ] || [ "$HTTP_CODE" = "302" ]; then
        break
    fi

    RETRY_COUNT=$((RETRY_COUNT + 1))
    if [ $RETRY_COUNT -lt $MAX_RETRIES ]; then
        echo "Tentativa $RETRY_COUNT/$MAX_RETRIES falhou (HTTP $HTTP_CODE). Aguardando 20 segundos..."
        sleep 20
    fi
done

if [ "$HTTP_CODE" = "200" ] || [ "$HTTP_CODE" = "301" ] || [ "$HTTP_CODE" = "302" ]; then
    echo -e "${GREEN}✓ Aplicação está respondendo! (HTTP $HTTP_CODE)${NC}"
    echo ""
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}Deploy realizado com sucesso!${NC}"
    echo -e "${GREEN}========================================${NC}"
    echo ""
    echo -e "Acesse: ${GREEN}$APP_URL${NC}"
    echo -e "Swagger: ${GREEN}$APP_URL/swagger${NC}"
    echo ""
    echo -e "${GREEN}✓ Aplicação está online e funcionando!${NC}"

    # Testar um endpoint da API
    echo ""
    echo -e "${YELLOW}Testando endpoint da API...${NC}"
    API_TEST=$(curl -s -o /dev/null -w "%{http_code}" --max-time 10 "$APP_URL/api" || echo "000")
    echo -e "Status API: ${GREEN}$API_TEST${NC}"

    exit 0
else
    echo -e "${YELLOW}⚠ Aplicação pode estar iniciando ainda (HTTP $HTTP_CODE)${NC}"
    echo -e "${YELLOW}Verifique os logs:${NC}"
    echo "az webapp log tail --name $APP_NAME --resource-group $RESOURCE_GROUP"
    echo ""
    echo -e "${GREEN}URL da aplicação: $APP_URL${NC}"
    echo ""
    echo -e "${YELLOW}Verificando logs recentes...${NC}"
    az webapp log tail --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" --timeout 15 2>/dev/null || echo "Não foi possível obter logs. Verifique no portal Azure."
    exit 1
fi

