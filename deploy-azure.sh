#!/bin/bash

# Script de Deploy para Azure App Service
# TrainMaster API - .NET 8.0

set -e  # Para em caso de erro

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configurações
APP_NAME="trainmaster-api-$(date +%s | tail -c 6)"  # Nome único baseado em timestamp
RESOURCE_GROUP="trainmaster-rg"
LOCATION="eastus"  # Pode ser alterado para outra região
PLAN_NAME="trainmaster-plan"
SKU="B1"  # Basic tier - pode ser alterado para S1, P1V2, etc.
RUNTIME="DOTNET:8.0"

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
echo ""

# Perguntar se deseja usar outra subscription
echo -e "${YELLOW}Deseja usar outra subscription? (s/n)${NC}"
read -r USE_OTHER_SUB
if [ "$USE_OTHER_SUB" = "s" ] || [ "$USE_OTHER_SUB" = "S" ]; then
    echo -e "${YELLOW}Subscriptions disponíveis:${NC}"
    az account list --output table
    echo -e "${YELLOW}Digite o Subscription ID ou Name:${NC}"
    read -r SUB_INPUT
    az account set --subscription "$SUB_INPUT"
    echo -e "${GREEN}✓ Subscription alterada${NC}"
fi

# Configurações customizáveis
echo ""
echo -e "${YELLOW}Configurações do Deploy:${NC}"
echo -e "App Name: ${GREEN}$APP_NAME${NC}"
echo -e "Resource Group: ${GREEN}$RESOURCE_GROUP${NC}"
echo -e "Location: ${GREEN}$LOCATION${NC}"
echo -e "Plan: ${GREEN}$PLAN_NAME${NC}"
echo -e "SKU: ${GREEN}$SKU${NC}"
echo ""
echo -e "${YELLOW}Deseja alterar alguma configuração? (s/n)${NC}"
read -r CHANGE_CONFIG
if [ "$CHANGE_CONFIG" = "s" ] || [ "$CHANGE_CONFIG" = "S" ]; then
    echo -e "${YELLOW}Digite o nome do App Service (ou Enter para manter $APP_NAME):${NC}"
    read -r CUSTOM_APP_NAME
    if [ ! -z "$CUSTOM_APP_NAME" ]; then
        APP_NAME="$CUSTOM_APP_NAME"
    fi

    echo -e "${YELLOW}Digite a região (ou Enter para manter $LOCATION):${NC}"
    read -r CUSTOM_LOCATION
    if [ ! -z "$CUSTOM_LOCATION" ]; then
        LOCATION="$CUSTOM_LOCATION"
    fi
fi

echo ""
echo -e "${YELLOW}Iniciando deploy...${NC}"
echo ""

# Criar Resource Group
echo -e "${YELLOW}[1/6] Criando Resource Group...${NC}"
if az group show --name "$RESOURCE_GROUP" &>/dev/null; then
    echo -e "${GREEN}✓ Resource Group já existe${NC}"
else
    az group create --name "$RESOURCE_GROUP" --location "$LOCATION"
    echo -e "${GREEN}✓ Resource Group criado${NC}"
fi

# Criar App Service Plan
echo -e "${YELLOW}[2/6] Criando App Service Plan...${NC}"
if az appservice plan show --name "$PLAN_NAME" --resource-group "$RESOURCE_GROUP" &>/dev/null; then
    echo -e "${GREEN}✓ App Service Plan já existe${NC}"
else
    az appservice plan create \
        --name "$PLAN_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --location "$LOCATION" \
        --sku "$SKU" \
        --is-linux
    echo -e "${GREEN}✓ App Service Plan criado${NC}"
fi

# Criar App Service
echo -e "${YELLOW}[3/6] Criando App Service...${NC}"
if az webapp show --name "$APP_NAME" --resource-group "$RESOURCE_GROUP" &>/dev/null; then
    echo -e "${GREEN}✓ App Service já existe, atualizando...${NC}"
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

# Habilitar sempre on (para evitar cold start)
az webapp config set \
    --name "$APP_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --always-on true

# Configurar porta (Azure App Service usa porta 80 por padrão)
az webapp config appsettings set \
    --name "$APP_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --settings \
        ASPNETCORE_ENVIRONMENT=Production \
        ASPNETCORE_URLS=http://+:80 \
        WEBSITES_ENABLE_APP_SERVICE_STORAGE=false

echo -e "${GREEN}✓ App Service configurado${NC}"

# Publicar aplicação
echo -e "${YELLOW}[5/6] Publicando aplicação...${NC}"
cd TrainMaster

# Limpar builds anteriores
echo "Limpando builds anteriores..."
dotnet clean

# Publicar
echo "Publicando projeto..."
dotnet publish -c Release -o ./publish

# Fazer deploy usando zip deploy
echo "Criando pacote ZIP..."
cd publish
zip -r ../deploy.zip . > /dev/null
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
echo "Aguardando aplicação iniciar (30 segundos)..."
sleep 30

# Testar endpoint
echo "Testando endpoint principal..."
HTTP_CODE=$(curl -s -o /dev/null -w "%{http_code}" "$APP_URL/swagger" || echo "000")

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
    echo -e "${YELLOW}Nota: A string de conexão do banco de dados precisa ser configurada nas App Settings do Azure Portal${NC}"
    echo -e "${YELLOW}ou usando o comando:${NC}"
    echo ""
    echo "az webapp config appsettings set --name $APP_NAME --resource-group $RESOURCE_GROUP --settings \"ConnectionStrings:WebApiDatabase=<sua-connection-string>\""
    echo ""
else
    echo -e "${YELLOW}⚠ Aplicação pode estar iniciando ainda (HTTP $HTTP_CODE)${NC}"
    echo -e "${YELLOW}Verifique os logs:${NC}"
    echo "az webapp log tail --name $APP_NAME --resource-group $RESOURCE_GROUP"
    echo ""
    echo -e "${GREEN}URL da aplicação: $APP_URL${NC}"
fi



