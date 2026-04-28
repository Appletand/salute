# 🩺 Salute

> Plataforma de gestão clínica multiplataforma para profissionais da saúde.

Salute é um aplicativo desenvolvido com **.NET MAUI 10**, voltado para clínicas e consultórios da área da saúde em geral — medicina, odontologia, fisioterapia, psicologia e demais especialidades. O objetivo é centralizar o gerenciamento de pacientes, anamneses, consultas e prontuários em uma única plataforma moderna, com suporte nativo a Windows, Android, iOS e macOS.

---

## ✨ Funcionalidades

- 📋 **Gestão de Pacientes** — cadastro, consulta e histórico completo
- 📝 **Anamnese Digital** — formulários clínicos estruturados por especialidade
- 📅 **Agendamento de Consultas** — controle de appointments por paciente
- 🗒️ **Prontuário Eletrônico** — notas clínicas vinculadas ao paciente
- 🤖 **Assistente de IA** — geração automática de anamneses via Gemini API *(em desenvolvimento)*
- 🔒 **Segurança** — variáveis sensíveis via `.env`, sem exposição de credenciais

---

## 🛠️ Stack

| Camada | Tecnologia |
|---|---|
| Framework | .NET MAUI 10 |
| Linguagem | C# 13 |
| ORM | Entity Framework Core 8 |
| Banco de Dados | PostgreSQL (Npgsql) |
| IA | Google Gemini API |
| Secrets | DotNetEnv |
| Plataformas | Windows, Android, iOS, macOS |

---

## 📁 Estrutura do Projeto

```
Salute/
├── Models/                  # Entidades do domínio
│   ├── Patient.cs
│   ├── Anamnesis.cs
│   ├── Appointment.cs
│   └── ClinicalNote.cs
├── Interfaces/              # Contratos dos repositórios e serviços
├── Data/
│   ├── AppDbContext.cs
│   └── Repositories/        # Implementações dos repositórios
├── ViewModels/              # Lógica de apresentação (MVVM)
├── Views/                   # Páginas XAML
│   ├── Patients/
│   └── Anamnesis/
├── Features/
│   └── AIAssistant/         # Geração de anamnese via IA
├── Services/                # Serviços externos (Gemini, Mock)
├── Converters/              # Conversores XAML
├── Platforms/               # Código específico por plataforma
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
└── Resources/               # Fontes, ícones, estilos
```

---

## 🚀 Como rodar

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Workload MAUI instalado:
```powershell
dotnet workload install maui
```
- [Windows App SDK Runtime 1.7](https://aka.ms/windowsappsdk/1.7/latest/windowsappruntimeinstall-x64.exe) *(para Windows)*
- PostgreSQL rodando localmente ou em nuvem

### Configuração

1. Clone o repositório:
```powershell
git clone https://github.com/th1eros/appletand_maui.git
cd appletand_maui/Salute
```

2. Crie o arquivo `.env` na raiz do projeto:
```env
DB_CONNECTION_STRING=Host=localhost;Database=SaluteDB;Username=postgres;Password=suasenha
SIGNING_PASSWORD=suasenha
```

3. Rode o projeto no Windows:
```powershell
dotnet run -f net10.0-windows10.0.19041.0
```

4. Rode no Android:
```powershell
dotnet run -f net10.0-android
```

---

## ⚙️ Configuração crítica do `.csproj`

Para rodar corretamente no Windows via `dotnet run`, as seguintes propriedades são **obrigatórias**:

```xml
<!-- Desativa empacotamento MSIX para desenvolvimento local -->
<WindowsPackageType>None</WindowsPackageType>

<!-- Evita crash do auto-inicializador do Windows App SDK -->
<WindowsAppSdkDeploymentManagerInitialize>false</WindowsAppSdkDeploymentManagerInitialize>
```

> Consulte `MAUI_WINDOWS_SETUP.md` para o guia completo de troubleshooting.

---

## 🗺️ Roadmap

- [x] Estrutura base do projeto MAUI multiplataforma
- [x] Modelos de domínio (Patient, Anamnesis, Appointment, ClinicalNote)
- [x] Repositórios e interfaces
- [x] Navegação entre páginas
- [ ] Telas de listagem e cadastro de pacientes
- [ ] Telas de anamnese por especialidade
- [ ] Integração real com Gemini API
- [ ] Suporte a Android
- [ ] Autenticação de usuários
- [ ] Relatórios clínicos

---

## 🔐 Segurança

- Nunca commite o arquivo `.env`
- O certificado `.pfx` não deve estar no repositório
- Todas as credenciais devem ser mantidas fora do código-fonte

---

## 🏢 Desenvolvido por

**Appletand** — `CN=Appletand`  
Projeto em desenvolvimento ativo.