# ===================================================================
# Script para Iniciar a API, Verificar a Saúde e Rodar os Testes
# ===================================================================

# --- Configuração (AJUSTE ESTES VALORES) ---
# Caminho para o arquivo .csproj da sua API
$apiProjectPath = "API/src/CludeMedSync.Api/CludeMedSync.Api.csproj" 

# Caminho para o arquivo .csproj do seu projeto de testes
$testProjectPath = "API/src/CludeMedSync.Tests/CludeMedSync.Tests.csproj" 

# URL completa do seu endpoint de Health Check. 
# Verifique a porta correta no seu arquivo launchSettings.json!
$healthCheckUrl = "https://localhost:7123/health" 

# Número máximo de tentativas para verificar a saúde da API
$maxRetries = 10 

# Tempo de espera (em segundos) entre cada tentativa
$retryDelaySeconds = 2 

$ColorRed = [System.ConsoleColor]::Red
$ColorGreen = [System.ConsoleColor]::Green
$ColorYellow = [System.ConsoleColor]::Yellow
$ColorWhite = [System.ConsoleColor]::White

# --- Início do Script ---
Write-Host "Iniciando o processo de build e teste..." -ForegroundColor $ColorYellow

# 1. Inicia a API em um processo separado (em background)
Write-Host "Iniciando a API em background..."
# O comando 'dotnet run' inicia a API. '-PassThru' nos dá o objeto do processo.
$apiProcess = Start-Process dotnet -ArgumentList "run --project $apiProjectPath" -PassThru
if (-not $apiProcess) {
    Write-Host "Falha ao iniciar a API." -ForegroundColor $ColorRed
    exit 1
}

# 2. Aguarda a API iniciar e verifica o Health Check
Write-Host "Aguardando a API ficar saudável na URL: $healthCheckUrl"
$isApiHealthy = $false
for ($i = 1; $i -le $maxRetries; $i++) {
    Write-Host "Tentativa $i de $maxRetries..."
    Start-Sleep -Seconds $retryDelaySeconds
    try {
        # Tenta fazer uma requisição web para o endpoint de saúde
        $response = Invoke-RestMethod -Uri $healthCheckUrl -Method Get
        
        # Verifica se o campo 'status' no JSON de resposta é 'Healthy'
        if ($response.status -eq "Healthy") {
            Write-Host "API está saudável!" -ForegroundColor $ColorGreen
            $isApiHealthy = $true
            break # Sai do loop, pois a API está pronta
        }
    }
    catch {
        # Ignora o erro, pois a API pode ainda não estar pronta para responder.
        # O script continuará tentando até o limite de $maxRetries.
    }
}

# 3. Executa os testes ou aborta com base no resultado do Health Check
if ($isApiHealthy) {
    Write-Host "API saudável. Iniciando os testes automatizados..." -ForegroundColor $ColorYellow
    dotnet test $testProjectPath
}
else {
    Write-Host "ERRO: A API não respondeu com status 'Healthy' após $maxRetries tentativas." -ForegroundColor $ColorRed
    Write-Host "Os testes foram abortados." -ForegroundColor $ColorRed
}

# 4. Finaliza o processo da API (passo crucial para não deixar a API rodando)
Write-Host "Finalizando o processo da API..."
Stop-Process -Id $apiProcess.Id -Force
Write-Host "Processo finalizado. Script concluído." -ForegroundColor $ColorYellow
