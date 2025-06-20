# Guia de Configuração e Execução do CommentService

Este documento apresenta um guia completo para configurar e executar o microserviço de comentários (CommentService) em sua máquina local de forma rápida e eficiente.

Este microserviço é responsável pelos comentarios. Todas as rotas requerem autenticação com exceção a rota de login.

## Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas em seu sistema:

| Ferramenta   | Versão Mínima | Link de Download                                 |
|--------------|---------------|--------------------------------------------------|
| **.NET SDK** | 8.0+          | [Download .NET](https://dotnet.microsoft.com/download) |
| **Git**      | Latest        | [Download Git](https://git-scm.com/downloads)    |

> **Dica:** Verifique as versões instaladas executando `dotnet --version` e `git --version`

## Início Rápido

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/MicroEduu/comment-service
cd CommentService
```

### 2️⃣ Configurar a Solução

Se necessário, crie e configure o arquivo de solução:

```bash
# Criar arquivo de solução (se não existir)
dotnet new sln

# Adicionar projeto à solução
dotnet sln CommentService.sln add CommentService.csproj
```

### 3️⃣ Instalar Ferramentas Necessárias

Instale a ferramenta global do Entity Framework:

```bash
# Instalação inicial
dotnet tool install --global dotnet-ef

# Ou atualizar se já estiver instalada
dotnet tool update --global dotnet-ef
```

### 4️⃣ Configurar Banco de Dados

Execute as migrações para preparar o banco SQLite:

```bash
dotnet ef database update
```

### 5️⃣ Executar a Aplicação

```bash
dotnet run
```

🎉 **CommentService iniciado com sucesso!**

A aplicação estará disponível em:
- 🌐 **HTTP:** http://localhost:5213/swagger/index.html

# 📌 Rotas da API

## 🔓 Rotas Públicas (sem token)

| Método | Endpoint           | Descrição                                  |
|--------|--------------------|--------------------------------------------|
| GET    | /api/Auth/token    | Obtém um token de autenticação do usuário. |

> Esta rota serve apenas como um endpoint para acionar a rota de autenticação do microserviço de autenticação.
> 
> É necessário que o microserviço de autenticação esteja em execução para que esta rota funcione corretamente.

### 🔐 Autenticação

| Método | Endpoint              | Descrição                                                   |
|--------|------------------------|------------------------------------------------------------|
| GET    | /api/Auth/user-info    | Retorna informações do usuário com base no token enviado.  |

## 🔒 Rotas Protegidas (com token)

| Método | Endpoint							 | Descrição													  |
|--------|-----------------------------------|----------------------------------------------------------------|
| PUT    | /api/Comment						 | Lista todos os comentários.									  |
| GET    | /api/Comment/{id}				 | Busca um comentário por ID.									  |
| GET    | /api/Comment						 | Cria um novo comentário										  |
| DELETE | /api/Comment/{id}				 | Remove um comentário (se for o autor ou admin).				  |
| PATCH  | /api/Comment/{id}				 | Atualiza parcialmente um comentário (se for o autor ou admin). |
| GET    | /api/Comment/by-course/{courseId} | Retorna informações do usuário com base no token.			  |

### Exemplo de requisição para criar Comentário:

```json
POST /api/Comment
{
  "courseId": 1,
  "text": "Esse curso foi excelente!"
}
```

### Exemplo de resposta da criação:

```json
{
  "id": 12,
  "authorId": 3,
  "courseId": 1,
  "text": "Esse curso foi excelente!",
  "createdAt": "2025-06-20T16:00:00Z"
}
```


### Exemplo de requisição para alterar Comentário:

```json
PATCH /api/Comment/12
{
  "text": "Esse curso foi excelente! update"
}
```

### Exemplo de resposta da alteração:

```json
{
  "id": 12,
  "authorId": 3,
  "courseId": 1,
  "text": "Atualizei meu comentário para adicionar mais detalhes.",
  "createdAt": "2025-06-20T16:00:00Z"
}
```
> Para acessar rotas protegidas, inclua o token no header `Authorization`:
>
> `Authorization: Bearer <token_aqui>`

---
## 🛠️ Observações

- O token acima é um exemplo fixo para fins de teste.
- Para produção, seria utilizado um mecanismo seguro de geração e validação de tokens.


