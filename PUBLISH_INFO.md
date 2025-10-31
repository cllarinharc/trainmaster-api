# 📦 TrainMaster API - Informações de Publicação

## ✅ Publicação Concluída

**Data:** $(date)
**Configuração:** Release
**Framework:** .NET 8.0
**Localização:** `TrainMaster/publish/`

## 📊 Estatísticas

- **Tamanho total:** ~59 MB
- **Arquivos principais:**
  - `TrainMaster` - Executável principal (122 KB)
  - `TrainMaster.dll` - Assembly principal (87 KB)
  - `appsettings.json` - Configurações

## 🚀 Como Executar

### Opção 1: Usando o executável

```bash
cd TrainMaster/publish
./TrainMaster
```

### Opção 2: Usando dotnet

```bash
cd TrainMaster/publish
dotnet TrainMaster.dll
```

## ⚙️ Requisitos

- .NET 8.0 Runtime instalado no servidor
- PostgreSQL/Supabase configurado (connection string em appsettings.json)
- Portas 5000 (HTTP) e 5001 (HTTPS) disponíveis

## 🌐 Endpoints Após Inicialização

- **HTTP:** http://localhost:7009
- **HTTPS:** https://localhost:5001
- **Swagger:** http://localhost:7009/swagger

## 📝 Notas

- A aplicação já está configurada para executar migrations automaticamente
- Certifique-se de que a string de conexão no `appsettings.json` está correta
- Os arquivos de configuração (appsettings.json) estão incluídos no publish

## 🔧 Próximos Passos para Deploy

1. Copiar a pasta `publish/` para o servidor
2. Ajustar `appsettings.json` com as configurações de produção
3. Configurar variáveis de ambiente se necessário
4. Executar a aplicação como serviço (systemd, Supervisor, etc.)
