# 🏦 TradeRisk System - ASP.NET Core 8 Classification API

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)
![Database](https://img.shields.io/badge/Database-SQLite-lightgrey)

Sistema de alta performance para classificação automática de risco em operações financeiras (Trades). Este projeto foi estruturado utilizando **Clean Architecture** e o **Strategy Pattern** para garantir que novas regras de risco possam ser adicionadas sem impacto no código existente.

## 🏗️ Arquitetura do Sistema

O design separa as preocupações em camadas, facilitando a testabilidade e a manutenção.

[Image of Clean Architecture layers with Domain, Application, Infrastructure, and API]

### Divisão de Camadas:
* **Domain**: Contém as entidades (`Trade`), Enums e a lógica de negócio central (Interface `IRiskRule`).
* **Application**: Responsável pela orquestração, DTOs e a lógica de agregação estatística para grandes volumes de dados.
* **Infrastructure**: Implementa o acesso a dados utilizando **Entity Framework Core** com **SQLite**.
* **API**: Camada de interface que expõe os endpoints REST e a documentação interativa.

## 🚀 Como Executar Localmente

1.  **Pré-requisitos**:
    * [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.

2.  **Passo a Passo**:
    ```powershell
    # Clone o repositório e entre na pasta
    cd TradeRiskSystem

    # Restaure as dependências
    dotnet restore

    # Execute o projeto da API
    dotnet run --project TradeRisk.Api/TradeRisk.Api.csproj
    ```

3.  **Acesso à Interface**:
    Após o comando acima, a aplicação estará disponível em:
    * **Swagger UI:** [http://localhost:5120/index.html](http://localhost:5120/index.html)
    * **Swagger JSON:** `http://localhost:5120/swagger/v1/swagger.json`


## 📊 Endpoints Disponíveis

* `POST /api/trades/classify`: Recebe uma lista de operações e retorna as categorias (LOWRISK, MEDIUMRISK, HIGHRISK).
* `POST /api/trades/analyze`: Processa até 100.000 registros, retornando a classificação e um resumo estatístico (Total por categoria e cliente com maior exposição).

1.  **Json de Exemplo Testar classyfy**:
	[
	   {
		  "ClientId":10,
		  "value":2000000,
		  "clientSector":"Private"
	   },
	   {
		  "ClientId":20,
		  "value":400000,
		  "clientSector":"Public"
	   },
	   {
		  "ClientId":30,
		  "value":500000,
		  "clientSector":"Public"
	   },
	   {
		  "ClientId":40,
		  "value":3000000,
		  "clientSector":"Public"
	   }
	]

2.  **Json de Exemplo Testar analyze**:
	[
	  {    

		"clientId": "40",
		"value": 1450,
		"clientSector": "Private"    
	  }  
	]

## 🧪 Testes de Performance
O sistema foi validado para processar **100.000 trades** em menos de **500ms**. Para rodar os testes:
```powershell
dotnet test