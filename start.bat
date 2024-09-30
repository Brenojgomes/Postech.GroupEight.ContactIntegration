@echo off

:: Verifica se a rede externa existe
docker network ls | findstr "techchallenge-worker-persistency_custom_network" > nul
if %errorlevel% neq 0 (
    echo Criando a rede externa techchallenge-worker-persistency_custom_network...
    docker network create techchallenge-worker-persistency_custom_network
) else (
    echo Rede techchallenge-worker-persistency_custom_network ja existe.
)

:: Executa o Docker Compose
docker-compose up -d --build