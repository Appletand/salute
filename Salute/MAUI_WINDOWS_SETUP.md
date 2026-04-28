# 🛠️ .NET MAUI no Windows — Troubleshooting & Solução Completa

> Documento de referência para configurar e rodar um projeto .NET MAUI 10 no Windows via linha de comando.  
> Baseado em problemas reais encontrados durante o desenvolvimento do projeto **Salute**.

---

## ✅ Pré-requisitos

### 1. .NET SDK 10
Verifica se está instalado:
```powershell
dotnet --version
```

### 2. Workload MAUI
Verifica e instala/atualiza em um único bloco:
```powershell
$installed = dotnet workload list | Select-String "maui"

if ($installed) {
    Write-Host "MAUI encontrado. Atualizando..." -ForegroundColor Cyan
    dotnet workload update
} else {
    Write-Host "MAUI não encontrado. Instalando..." -ForegroundColor Yellow
    dotnet workload install maui
}
```

> ⚠️ **Execute o PowerShell como Administrador**, caso contrário pode falhar.

Após isso, o `dotnet workload list` deve mostrar:
```
android        10.x.x/10.0.100
maui           10.x.x/10.0.100
maui-windows   10.x.x/10.0.100
```

### 3. Windows App SDK Runtime
O MAUI no Windows depende do Windows App SDK Runtime para funcionar:
```powershell
winget install Microsoft.WindowsAppRuntime.1.7 --architecture x64
```

Verifica se foi instalado:
```powershell
Get-AppxPackage | Where-Object { $_.Name -like "*WindowsAppRuntime*" } | Select-Object Name, Version
```

---

## 🗂️ Configuração correta do `.csproj`

### ❌ Erros comuns

**1. Misturar MAUI com WPF/WinForms** — causa erro `MC3074: MauiWinUIApplication não existe`:
```xml
<!-- ERRADO: nunca combine os três -->
<UseMaui>true</UseMaui>
<UseWpf>true</UseWpf>             <!-- ❌ remove -->
<UseWindowsForms>true</UseWindowsForms>  <!-- ❌ remove -->
```

**2. `TargetFramework` singular** — impede multiplataforma:
```xml
<!-- ERRADO -->
<TargetFramework>net10.0-windows10.0.19041.0</TargetFramework>

<!-- CORRETO -->
<TargetFrameworks>net10.0-android;net10.0-ios;net10.0-maccatalyst;net10.0-windows10.0.19041.0</TargetFrameworks>
```

**3. `OutputType` errado para multiplataforma**:
```xml
<!-- ERRADO -->
<OutputType>WinExe</OutputType>

<!-- CORRETO -->
<OutputType>Exe</OutputType>
```

### ✅ `.csproj` correto e completo

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net10.0-android;net10.0-ios;net10.0-maccatalyst;net10.0-windows10.0.19041.0</TargetFrameworks>
    <OutputType>Exe</OutputType>
    <UseMaui>true</UseMaui>
    <SingleProject>true</SingleProject>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <!-- CRÍTICO: desativa empacotamento MSIX para rodar via dotnet run -->
    <WindowsPackageType>None</WindowsPackageType>

    <!-- Evita crash do auto-inicializador do Windows App SDK -->
    <WindowsAppSdkDeploymentManagerInitialize>false</WindowsAppSdkDeploymentManagerInitialize>

    <ApplicationId>com.suaempresa.seuapp</ApplicationId>
    <ApplicationTitle>SeuApp</ApplicationTitle>
    <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
    <ApplicationVersion>1</ApplicationVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="10.0.20" />
    <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="10.0.20" />
    <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="8.0.0" />
    <!-- outras dependências -->
  </ItemGroup>
</Project>
```

---

## ⚠️ Erros encontrados e soluções

### Erro 1 — `MC3074: MauiWinUIApplication não existe`
**Causa:** `<UseWpf>true</UseWpf>` e/ou `<UseWindowsForms>true</UseWindowsForms>` no `.csproj`.  
**Solução:** Remover as duas linhas. MAUI, WPF e WinForms são frameworks de UI mutuamente exclusivos.

---

### Erro 2 — `CS0234: System.Windows.Forms não existe`
**Causa:** Código C# usando `System.Windows.Forms.MessageBox` (resíduo de WinForms).  
**Como encontrar:**
```powershell
Get-ChildItem -Recurse -Filter "*.cs" | Select-String "System.Windows.Forms"
```
**Solução:** Substituir pelo `Console.WriteLine`:
```csharp
// ANTES (❌)
System.Windows.Forms.MessageBox.Show($"{ex.Message}", "ERRO");

// DEPOIS (✅)
Console.WriteLine($"ERRO: {ex.Message}");
Console.WriteLine(ex.StackTrace);
```

---

### Erro 3 — App abre e fecha imediatamente (sem mensagem de erro)
**Causa:** Crash silencioso — o app inicia mas não exibe erro na tela.  
**Como diagnosticar:**
```powershell
Get-EventLog -LogName Application -Newest 5 | Where-Object { $_.EntryType -eq "Error" } | Format-List Message
```

---

### Erro 4 — `REGDB_E_CLASSNOTREG (0x80040154)` no Event Log
**Stack trace:**
```
WindowsAppRuntime.DeploymentInitializeOptions
DeploymentManagerAutoInitializer.cs
```
**Causa:** O Windows App SDK Runtime não estava registrado corretamente via COM.  
**Solução 1:** Re-registrar o runtime (fecha todos os apps como Bloco de Notas antes):
```powershell
Get-Process | Where-Object { $_.Name -like "*Notepad*" } | Stop-Process -Force

Get-AppxPackage "Microsoft.WindowsAppRuntime.1.7" | ForEach-Object {
    Add-AppxPackage -DisableDevelopmentMode -Register "$($_.InstallLocation)\AppxManifest.xml"
}
```

**Solução 2 (definitiva):** Desativar o empacotamento MSIX e o auto-inicializador no `.csproj`:
```xml
<WindowsPackageType>None</WindowsPackageType>
<WindowsAppSdkDeploymentManagerInitialize>false</WindowsAppSdkDeploymentManagerInitialize>
```
> Isso é o correto para desenvolvimento via `dotnet run`. O MSIX é necessário apenas para publicação na Microsoft Store.

---

## 🚀 Como rodar o projeto

### Comando correto para MAUI (sempre especifica o framework):
```powershell
dotnet run -f net10.0-windows10.0.19041.0
```

> ❌ `dotnet run` sem `-f` falha porque o projeto tem múltiplos `TargetFrameworks`.

### Se precisar limpar o cache de build:
```powershell
Remove-Item -Recurse -Force bin, obj
dotnet build -f net10.0-windows10.0.19041.0
dotnet run -f net10.0-windows10.0.19041.0
```

---

## 🔍 Comandos úteis para diagnóstico

```powershell
# Workloads instalados
dotnet workload list

# Runtimes do Windows App SDK instalados
Get-AppxPackage | Where-Object { $_.Name -like "*WindowsAppRuntime*" } | Select-Object Name, Version

# Últimos erros de aplicativo no Event Log
Get-EventLog -LogName Application -Newest 5 | Where-Object { $_.EntryType -eq "Error" } | Format-List Message

# Buscar referências problemáticas no código
Get-ChildItem -Recurse -Filter "*.cs" | Select-String "System.Windows.Forms"
Get-ChildItem -Recurse -Filter "*.cs" | Select-String "System.Windows"

# Verificar se certificado de assinatura está instalado
Get-ChildItem Cert:\CurrentUser\My | Where-Object { $_.Thumbprint -eq "SEU_THUMBPRINT_AQUI" }
```

---

## 📋 Checklist rápido antes de rodar

- [ ] `dotnet workload list` mostra `maui` e `maui-windows`
- [ ] `.csproj` tem `<TargetFrameworks>` (plural) com os 4 targets
- [ ] `.csproj` **não** tem `<UseWpf>` nem `<UseWindowsForms>`
- [ ] `.csproj` tem `<WindowsPackageType>None</WindowsPackageType>`
- [ ] `.csproj` tem `<WindowsAppSdkDeploymentManagerInitialize>false</WindowsAppSdkDeploymentManagerInitialize>`
- [ ] Nenhum arquivo `.cs` usa `System.Windows.Forms`
- [ ] Windows App SDK Runtime 1.7 instalado (`winget list | Select-String "WindowsAppRuntime"`)
- [ ] Comando de run usa `-f net10.0-windows10.0.19041.0`

---

## 📝 Notas adicionais

- O `dotnet run` para MAUI Windows **não** é equivalente ao F5 no Visual Studio. O VS faz um deploy MSIX completo, o `dotnet run` roda sem empacotamento — por isso o `<WindowsPackageType>None</WindowsPackageType>` é essencial.
- O aviso `Failed to remove ...Sdk.iOS.Manifest...` pode ser **ignorado** no Windows — é resíduo de limpeza do SDK do iOS que nunca existiu no sistema.
- Para **publicação** (Store ou sideload), o `<WindowsPackageType>None</WindowsPackageType>` deve ser removido e o processo de assinatura com `.pfx` reativado.