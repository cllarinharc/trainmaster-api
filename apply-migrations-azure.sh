#!/bin/bash

# Connection string do banco de produção
CONNECTION_STRING="Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ekxsphpaocqpewmufmow;Password=JptEWOJvDTil3ggj;"

# Aplicar migrations
cd /Users/larissarabelo/Developer/pessoal/anaclara/trainmaster-api

export ConnectionStrings__WebApiDatabase="$CONNECTION_STRING"

dotnet ef database update --project TrainMaster.Infrastracture --startup-project TrainMaster

