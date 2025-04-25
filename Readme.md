# 📘 EmpresaCadastroApp

API desenvolvida como parte de um teste técnico com o objetivo de permitir o cadastro de usuários e o cadastro de empresas via CNPJ, integrando com a API pública da ReceitaWS.

## 🚀 Funcionalidades
- ✅ Cadastro e login de usuários com autenticação via JWT
- ✅ Cadastro de empresas por CNPJ com consumo da ReceitaWS
- ✅ Listagem de empresas cadastradas pelo usuário logado
- ✅ Validações de dados e tratamento de erros
- ✅ Arquitetura em camadas (Domain, Application, Infrastructure, Api)

## 🛠️ Tecnologias Utilizadas
- **ASP.NET Core 6**
- **Entity Framework Core**
- **ASP.NET Identity**
- **AutoMapper**
- **FluentValidation**
- **JWT (JSON Web Token)**
- **Swagger (documentação da API)**

## 🔐 Autenticação
A autenticação é feita via **JWT Bearer Token**. 
Após realizar o login, utilize o token retornado no header `Authorization` como `Bearer {seu_token}`.

## 🔄 Endpoints Principais

### Usuário
- **POST** `/api/auth/register` – Cadastro  
- **POST** `/api/auth/login` – Login  

### Empresas
- **POST** `/api/companies` – Cadastro de empresa via CNPJ  
- **GET** `/api/companies` – Listagem das empresas do usuário logado  

## 📦 Como rodar o projeto localmente

1. Clone o repositório:
    ```bash
    git clone https://github.com/seu-usuario/EmpresaCadastroApp.git
    ```

2. Acesse a pasta do projeto:
    ```bash
    cd EmpresaCadastroApp\src\EmpresaCadastroApp.Api
    ```

3. Configure a string de conexão no `appsettings.json` com seu SQL Server.

4. Execute as migrations para criar o banco:
    ```bash
    dotnet ef database update --project ../EmpresaCadastroApp.Infrastructure --startup-project .
    ```

5. Rode a aplicação:
    ```bash
    dotnet run
    ```

6. Acesse o Swagger:
    ```
    https://localhost:{porta}/swagger
    ```

## 📌 Observações
- A aplicação utiliza **FluentValidation** para validar os dados de entrada.
- O retorno das operações é padronizado usando um wrapper `Result<T>`.
- Foi adotado o padrão **Repository e Service** com injeção de dependência para organização do código.

## ✨ Diferenciais Implementados
- ✅ Validação de CNPJ antes da consulta
- ✅ Verificação de empresa duplicada por usuário
- ✅ Padronização de erros
- ✅ Clean architecture em camadas
- ✅ Suporte ao JWT via Swagger  
