# 🚀 Deploy Azure - TrainMaster API

## ✅ Deploy Realizado com Sucesso!

**Data:** 26 de Novembro de 2025  
**Status:** ✅ Online e Funcionando

## 📋 Informações do Deploy

### App Service
- **Nome:** `trainmaster-api-new-1764190177`
- **URL:** https://trainmaster-api-new-1764190177.azurewebsites.net
- **Swagger UI:** https://trainmaster-api-new-1764190177.azurewebsites.net/swagger
- **Swagger JSON:** https://trainmaster-api-new-1764190177.azurewebsites.net/swagger/v1/swagger.json

### Resource Group
- **Nome:** `trainmaster-new-rg`
- **Região:** Canada Central
- **Subscription:** Azure subscription 1

### App Service Plan
- **Nome:** `trainmaster-new-plan`
- **SKU:** F1 (Free Tier)
- **Sistema Operacional:** Linux

## 🛠️ Scripts de Deploy

Foram criados 3 scripts para deploy:

### 1. `deploy-azure.sh` (Interativo)
Script interativo que permite configurar todos os parâmetros durante a execução.

**Uso:**
```bash
./deploy-azure.sh
```

### 2. `deploy-azure-auto.sh` (Automático)
Script automático com parâmetros padrão, mas aceita argumentos.

**Uso:**
```bash
./deploy-azure-auto.sh [APP_NAME] [RESOURCE_GROUP] [LOCATION] [PLAN_NAME] [SKU]
```

**Exemplo:**
```bash
./deploy-azure-auto.sh trainmaster-api-prod trainmaster-rg eastus trainmaster-plan F1
```

### 3. `deploy-azure-new.sh` (Recomendado)
Script mais completo que detecta planos existentes e oferece opções.

**Uso:**
```bash
./deploy-azure-new.sh [APP_NAME] [RESOURCE_GROUP] [LOCATION] [PLAN_NAME] [SKU]
```

**Exemplo:**
```bash
./deploy-azure-new.sh trainmaster-api-prod trainmaster-new-rg canadacentral trainmaster-new-plan F1
```

## ⚙️ Configurações Aplicadas

### App Settings
- `ASPNETCORE_ENVIRONMENT=Production`
- `ASPNETCORE_URLS=http://+:80`
- `WEBSITES_ENABLE_APP_SERVICE_STORAGE=false`

### Connection Strings
- `WebApiDatabase` (PostgreSQL) - Configurada com a string de conexão do Supabase

## 🧪 Testes Realizados

✅ **Swagger UI:** Funcionando (HTTP 200)  
✅ **Swagger JSON:** Funcionando (retorna OpenAPI 3.0.1)  
✅ **Aplicação:** Online e respondendo  
✅ **Endpoints da API:** Disponíveis e documentados

## 📝 Próximos Passos (Opcional)

### 1. Configurar Domínio Personalizado
```bash
az webapp config hostname add \
  --webapp-name trainmaster-api-new-1764190177 \
  --resource-group trainmaster-new-rg \
  --hostname seu-dominio.com
```

### 2. Habilitar HTTPS (Recomendado)
```bash
az webapp config set \
  --name trainmaster-api-new-1764190177 \
  --resource-group trainmaster-new-rg \
  --https-only true
```

### 3. Configurar Always On (Requer SKU acima de F1)
```bash
az webapp config set \
  --name trainmaster-api-new-1764190177 \
  --resource-group trainmaster-new-rg \
  --always-on true
```

### 4. Configurar Logs
```bash
az webapp log config \
  --name trainmaster-api-new-1764190177 \
  --resource-group trainmaster-new-rg \
  --application-logging filesystem \
  --level information
```

## 🔍 Comandos Úteis

### Ver logs em tempo real
```bash
az webapp log tail --name trainmaster-api-new-1764190177 --resource-group trainmaster-new-rg
```

### Ver status da aplicação
```bash
az webapp show --name trainmaster-api-new-1764190177 --resource-group trainmaster-new-rg
```

### Reiniciar aplicação
```bash
az webapp restart --name trainmaster-api-new-1764190177 --resource-group trainmaster-new-rg
```

### Fazer novo deploy
```bash
cd TrainMaster
dotnet publish -c Release -o ./publish-azure
cd publish-azure
zip -r ../deploy.zip .
cd ..
az webapp deployment source config-zip \
  --resource-group trainmaster-new-rg \
  --name trainmaster-api-new-1764190177 \
  --src ./deploy.zip
rm deploy.zip
```

## 📊 Recursos Criados

- ✅ Resource Group: `trainmaster-new-rg`
- ✅ App Service Plan: `trainmaster-new-plan` (F1 - Free)
- ✅ App Service: `trainmaster-api-new-1764190177`
- ✅ Configurações de ambiente
- ✅ Connection strings
- ✅ Deploy da aplicação

## ⚠️ Notas Importantes

1. **Free Tier (F1):** 
   - A aplicação pode ter cold start (demora para iniciar após inatividade)
   - Sem Always On disponível
   - Limites de CPU e memória

2. **Connection String:**
   - A connection string do banco está configurada nas App Settings
   - Para alterar, use:
   ```bash
   az webapp config connection-string set \
     --name trainmaster-api-new-1764190177 \
     --resource-group trainmaster-new-rg \
     --connection-string-type PostgreSQL \
     --settings "WebApiDatabase=<nova-connection-string>"
   ```

3. **Logs:**
   - Os logs da aplicação podem ser visualizados no Azure Portal ou via CLI
   - Logs de aplicação estão configurados via Serilog

## 🎉 Status Final

**✅ Deploy Completo e Testado!**

A aplicação está online, funcionando e pronta para uso em:
- **URL Principal:** https://trainmaster-api-new-1764190177.azurewebsites.net
- **Swagger:** https://trainmaster-api-new-1764190177.azurewebsites.net/swagger

