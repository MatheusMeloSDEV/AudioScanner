# 🔊 Audio Scanner

Um utilitário avançado de controle de volume para Windows, capaz de identificar e gerenciar sessões de áudio que o Mixer padrão do Windows muitas vezes oculta ou não identifica corretamente (como processos do *Windows Subsystem for Android™*).

## 📋 Sobre o Projeto

Este projeto nasceu da necessidade de controlar o volume de aplicações modernas e isoladas, especificamente jogos e apps rodando no **Windows Subsystem for Android (WSA)**. O Mixer padrão do Windows muitas vezes exibe esses processos como "Unknown" ou não permite um controle granular.

O **Audio Scanner Pro** utiliza as APIs de baixo nível do Windows (Core Audio APIs / WASAPI) através de interfaces COM (`IMMDeviceEnumerator`, `IAudioSessionManager2`) para:

1. Varrer a memória em busca de sessões de áudio ativas.
2. Identificar o Process ID (PID) real quando o "Display Name" é inválido.
3. Permitir controle de volume e mute em tempo real, mesmo para processos "teimosos".

## ✨ Funcionalidades

* **🔍 Varredura Profunda (Deep Scan):** Identifica processos que não aparecem com nome correto no Mixer do Windows (ex: `WsaClient.exe` identificado corretamente).
* **🎚️ Controle em Tempo Real:** Slider de volume e Mute com resposta instantânea.
* **🎯 Auto-Lock (Busca Automática):** Digite um nome (ex: "Chrome", "Spotify", "Wsa") e o app "gruda" a seleção nesse processo automaticamente, ideal para jogos em tela cheia.
* **👁️ Feedback Visual:** A lista mostra o status `[MUDO]` e a porcentagem de volume atualizada dinamicamente.
* **🛡️ Arquitetura x64 Nativa:** Otimizado para rodar em sistemas 64-bits, essencial para acessar a memória de processos modernos.

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C# (.NET Framework 4.7.2+)
* **Interface:** Windows Forms (WinForms)
* **APIs:**
* `Windows Core Audio APIs` (MMDevice API, WASAPI)
* `System.Runtime.InteropServices` (Interop COM)
* `System.Diagnostics.Process` (Mapeamento de PID para Nome de Processo)



## 🚀 Como Executar

### Pré-requisitos

* Windows 10 ou 11 (64-bits).
* Visual Studio 2019 ou superior.
* Permissões de Administrador (Necessário para ler processos de sistema/WSA).

### Passo a Passo

1. Clone este repositório:
```bash
git clone https://github.com/SEU-USUARIO/AudioScannerPro.git

```


2. Abra a solução (`.sln`) no Visual Studio.
3. **Importante:** Configure o alvo de compilação para **x64** (não use `Any CPU` ou `x86`, pois as interfaces COM de áudio falharão).
4. O projeto já inclui um arquivo de manifesto (`app.manifest`) configurado para solicitar permissão de Administrador (`requireAdministrator`).
5. Compile e execute (F5).

## 🧩 Trecho de Código (Core Logic)

Abaixo, um exemplo de como o app resolve o problema do "Nome Desconhecido" acessando o PID direto da VTable da interface `IAudioSessionControl2`:

```csharp
// Exemplo simplificado da lógica de identificação
ctl.GetProcessId(out pid);
if (pid > 0)
{
    using (Process p = Process.GetProcessById(pid))
    {
        // Se o DisplayName do Windows falhar, usamos o nome real do executável
        processName = p.ProcessName; 
    }
}

```

## 🤝 Contribuição

Contribuições são bem-vindas! Se você tiver ideias para melhorar a interface ou adicionar suporte a dispositivos de áudio secundários (não-padrão), sinta-se à vontade para abrir uma *issue* ou enviar um *pull request*.

## 📄 Licença

Este projeto está sob a licença MIT - sinta-se livre para usar em seus próprios projetos.

---

*Desenvolvido com foco em resolver problemas reais de interoperabilidade no Windows.*
