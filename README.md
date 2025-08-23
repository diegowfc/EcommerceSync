# TCC - E-commerce: Arquitetura Síncrona vs Assíncrona

Este repositório faz parte do Trabalho de Conclusão de Curso (TCC) do MBA em Engenharia de Software da **Universidade de São Paulo (USP)**.

O projeto consiste em uma simulação backend de algumas funcionalidades de um site e-commerce, desenvolvida com foco em analisar os impactos de diferentes arquiteturas (síncrona e assíncrona) no desempenho e escalabilidade do sistema.

## Objetivo

O principal objetivo do projeto é demonstrar como uma aplicação baseada em arquitetura **síncrona** e **assíncrona**, desenvolvida em **.NET** com **PostgreSQL**, se comporta sob alta carga de requisições e qual abordagem levar em consideração durante o desenvolvimento de um projeto.

## Estrutura do Projeto

Este repositório contém:

1. **E-commerce Síncrono** (master)
   - Comunicação direta entre os módulos.
   - Sem uso de mensageria.
  
2. **E-commerce Assíncrono** (RabbitMQ)
   - Publicação de mensagens em filas
   - Consumidores
   - RabbitMQ.

## Tecnologias Utilizadas

- **.NET (C#)** para o backend
- **PostgreSQL** e **SUPABASE** para o banco de dados
- **Locust** para simulação de carga e testes de desempenho
- **RabbitMQ (CLOUDAMQP)** para gerenciamento de filas (broker)
- **Executada tanto localmente quanto com isolamento de camadas, hospedando a aplicação em uma VM Linux**

## Funcionalidades Implementadas

- Catálogo de produtos
- Criação de Pedidos
- Criação de usuários
- Carrinho de compras
- Pagamento

## Padrões de projeto implementados
-  Arquitetura em Camadas (DDD)
-  Unit of Work
-  Repository Pattern
-  DTO's e AutoMapper

## Testes de Carga

Os testes de carga foram realizados com **Locust**, com o intuito de medir:

- Tempo de resposta
- Quantidade de requisições simultâneas suportadas
- Comportamento da aplicação sob estresse
- Tolerância a falhas


## Análise de custos
Foi feito também uma cálculo para se medir gastos em um cenário real de produção. Simulação foi realizada através de calculadores disponibilizadas pela Azure e Amazon.




