# BloodDonation.Api
---

- [*BloodDonation.Api*](#nlooddonationapi)
  - [*Guidelines*](#guidelines)
  - [*Technologies Used*](#technologies-used)
  - [*Controllers*](#controllers)
    - [ADDRESS](#address)
      - [- Get All Addresses ](#--get-all-addresses)
      - [- Get Address By Postal Code ](#--get-address-by-postal-code)
    - [DONATION](#donation)
      - [- Create Donation](#--create-donation)
      - [- Get All Donation](#--get-all-donation)
      - [- Get Donations Report](#--get-donations-report)
    - [DONOR](#--donor)
      - [- Create Donor](#--create-donor)
      - [- Get All Donors](#--get-all-donor)
      - [- Get By Id Donor](#--get-by-id-donor)
      - [- Delete Donor](#--delete-donor)
      - [- Update User](#--update-user)
      - [- Get By Full Name](#--get-by-full-name)
    - [STOCK BLOOD](#--stock-blood)
      - [- Get Stock Blood Report](#--get-stock-blood-report)
      - [- Update Stock Blood](#--update-stock-blood)  
---

## *Guidelines*

Este documento fornece diretrizes e exemplos para a BloodDonation.Api.

A API é responsável por gerenciar doadores e suas doações, além de manter o controle do estoque de sangue, garantindo que todas as informações sejam validadas e atualizadas corretamente.

---

## Objetivo Geral dos Sistemas Operations.Crew.FreePass
O sistema tem como objetivo viabilizar o controle do estoque de sangue.

## *Technologies Used*
- [ASP.NET Core 8](https://github.com/aspnet/Home)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server)

---

## *Controllers*

### Address

### - Get All Addresses

-  [ GET api/Address]

- *Description:*  
Responsável por lidar com a consulta de todos os endereços de forma paginada. Ele consulta os endereços no repositório de endereços, valida a resposta e retorna uma resposta com a lista de endereços, informações de paginação, e o total de páginas.

 - *Request:*

      -Query parameters: 

        "pageNumber" : 1
        "pageSize": 10
     

### - Get Address By Postal Code

-  [ GET api/Address/{postalCode}]

-  *Description:*
Responsável por validar uma solicitação de consulta de código postal e, em seguida, buscar e retornar o endereço correspondente ao código postal fornecido. Se a validação falhar ou se o endereço não for encontrado, ele lança exceções apropriadas para tratar esses cenários.

- *Request:*
    
    -Query parameters: 

       "postalCode": "00000-000"


### Donation

### - Create Donation

-  [ POST api/Donation ]

-  *Description:*
 Processa a criação de uma nova doação de sangue, validando a solicitação, criando a doação no banco de dados e atualizando o estoque de sangue com base no tipo sanguíneo e fator Rh do doador. Ele garante que a doação seja validada corretamente e que o estoque de sangue seja atualizado adequadamente após cada nova doação.

- *Request:*

    -Query parameters: 

       "donorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  
  ```json
    {
      "donationDate": "2024-09-10T17:53:11.332Z",
      "quantityML": 0
    }
  ```

### - Get All Donation

-  [ GET api/Donation ]

-  *Description:*
Processa consultas para obter todas as doações, aplicando paginação conforme os parâmetros fornecidos (número da página e tamanho da página). Ele interage com o caso de uso de doações para buscar as informações do banco de dados, tratando erros caso a busca falhe, e retorna uma resposta contendo a lista paginada de doações e as informações de paginação (como total de páginas e doações).

 - *Request:*

      -Query parameters: 

        "pageNumber" : 1
        "pageSize": 10

### - Get Donations Report

-  [ GET api/Donation/report ]
  
- *Description:*
  Gera um relatório de doações paginado, agregando dados de doadores associados a cada doação. Ele busca as doações e para cada uma, adiciona informações complementares do doador. Caso ocorram erros na recuperação de doações ou doadores, exceções específicas são lançadas. O Response então retorna uma lista paginada de doações junto com os dados de doadores, como nome, tipo sanguíneo e fator RH.

- *Request:*

      -Query parameters: 

        "pageNumber" : 1
        "pageSize": 10

### - Donor

### - Create Donor

-  [ POST api/Donor ]

-  *Description:*
Responsável por receber o comando de criação de doador, validar os dados, verificar se o email já existe, criar o novo doador e retornar os detalhes do doador criado. Se houver erros de validação ou o email já estiver registrado, ele lança exceções para evitar a duplicação de registros.

- *Request:*
  
 ```json
    {
      "fullName": "string",
      "email": "string",
      "birthDate": "2024-09-10T18:36:54.382Z",
      "gender": 0,
      "weight": 0,
      "bloodType": "string",
      "rhFactor": "string",
      "address": {
        "street": "string",
        "number": "string",
      "neighborhood": "string",
      "city": "string",
      "state": "string",
      "zipCode": "string",
      }
    }
  ```

### - Get All Donors

-  [ GET api/Donor ]

-  *Description:*
Responsável por processar a requisição de busca de todos os doadores com paginação. Ele coleta as informações de doadores, calcula a paginação, e retorna uma resposta contendo a lista de doadores e os dados de paginação, garantindo que os dados sejam acessíveis de forma eficiente.

- *Request:*

      -Query parameters: 

        "pageNumber" : 1
        "pageSize": 10

### - Update Donor

-  [ PUT api/Donor ]

-  *Description:*
Permite atualizar as informações de um doador já existente, incluindo seus detalhes pessoais e endereço.

- *Request:*
    
    -Query parameters: 

       "donorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
       "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"

  ```json
    {
      "fullName": "string",
      "email": "string",
      "address": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "street": "string",
        "number": "string",
        "neighborhood": "string",
        "city": "string",
        "state": "string",
        "zipCode": "string",
      }
    }
  ```   

### - Get By Id Donor

-  [ GET api/Donor/{id} ]

-  *Description:*
Responsável por processar uma consulta e retornar as informações detalhadas do doador com base no Id fornecido, garantindo que todos os dados estejam validados e o doador exista no sistema.

- *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"

### - Delete Donor 

-  [ DELETE api/Donor/{id} ]

-  *Description:*
Busca o doador pelo Id, realiza a exclusão e lida com possíveis falhas no processo de exclusão.

- *Request:*
    
    -Query parameters: 

       "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"

### - Get By Full Name

-  [ GET api/Donor/fullname/{fullname}]

-  *Description:*
Reponsavel por buscar um doador pelo nome completo, lançar uma exceção se o doador não for encontrado e retornar uma resposta com os detalhes do doador encontrado, incluindo suas doações e endereço, caso ele exista.

- *Request:*
    
    -Query parameters: 

       "fullname" : "string"


### - Stock Blood

### - Get Stock Blood Report

- [GET api/StockBlood]

-  *Description:*
Consulta o estoque de sangue de forma paginada, lança uma exceção caso a consulta falhe, calcula o número total de páginas e retorna os resultados paginados para o cliente, incluindo detalhes sobre o estoque de sangue.

- *Request:*

      -Query parameters: 

        "pageNumber" : 1
        "pageSize": 10

### - Update Stock Blood

- [UPDATE api/StockBlood]

-  *Description:*
Realiza a seguinte a busca o estoque de sangue existente com base no tipo sanguíneo e fator RH atualiza a quantidade de sangue em estoque com o valor fornecido e salva a alteração e retorna uma resposta contendo os detalhes do estoque atualizado.

- *Request:*

      -Query parameters: 

        "bloodType" : "string"
        "rhFactor": "string"

```json
    {
        "quantityML": 0
    }
  ```  