# AgendaBack

AgendaBack é uma API desenvolvida em C# que funciona como uma agenda de chamados, conectando-se diretamente ao banco de dados do GLPI para autenticação e gerenciamento de tickets. A comunicação com sistemas externos é feita via socket, garantindo rapidez e eficiência no processo de criação de chamados.

## ✨ Funcionalidades

- Conexão direta com o banco de dados do GLPI (sem uso da API REST).
- Autenticação de usuários utilizando o próprio banco do GLPI.
- Criação e gerenciamento de chamados com alto desempenho.
- Comunicação via socket com sistemas externos.

## ⚙️ Pré-requisitos

- .NET Core ou .NET Framework instalado.
- Banco de dados do GLPI acessível (MySQL/MariaDB).
- Dados de conexão com o banco de dados configurados corretamente.

## 🚀 Instalação

1. Clone o repositório:

   ```bash
   git clone https://github.com/misereitor/agendaback.git
   ```

2. Abra o projeto no Visual Studio ou no seu editor de preferência.

3. Configure a string de conexão com o banco do GLPI (geralmente em `appsettings.json` ou diretamente no código, dependendo da versão).

4. Compile e execute a aplicação.

## 🔌 Como funciona

Após iniciado, o sistema fica escutando conexões socket. Sistemas externos (como frontends ou outros sistemas internos) podem se conectar e enviar comandos para criação de chamados.

A autenticação dos usuários é feita diretamente no banco de dados do GLPI, o que permite maior velocidade e integração sem necessidade da API REST.

## 📁 Estrutura do projeto

- `SocketServer/` – Responsável por gerenciar as conexões de entrada.
- `Controllers/` – Lógica de criação e gerenciamento dos chamados.
- `Models/` – Representações das entidades usadas nos chamados e no banco do GLPI.
- `Database/` – Camada de acesso direto ao banco de dados do GLPI.

## 📫 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues, sugerir melhorias ou enviar pull requests.

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
