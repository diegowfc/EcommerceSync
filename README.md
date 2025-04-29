# TCC - E-commerce: Arquitetura Síncrona vs Assíncrona

Este repositório faz parte do Trabalho de Conclusão de Curso (TCC) do MBA em Engenharia de Software da **Universidade de São Paulo (USP)**.

O projeto consiste na criação de uma aplicação de e-commerce simples, desenvolvida com foco em analisar os impactos de diferentes arquiteturas (síncrona e assíncrona) no desempenho e escalabilidade do sistema.

## Objetivo

O principal objetivo do projeto é demonstrar como uma aplicação monolítica baseada em arquitetura **síncrona**, desenvolvida em **.NET** com **PostgreSQL**, se comporta sob alta carga de requisições e como é importante se planejar com foco em escalabilidade, dependendo do tamanho do seu projeto.

## Estrutura do Projeto

Este repositório contém:

1. **E-commerce Síncrono**
   - Arquitetura tradicional monolítica.
   - Comunicação direta entre os módulos.
   - Sem uso de mensageria.

> Obs: Essa versão está em desenvolvimento e será disponibilizada em breve.

## Tecnologias Utilizadas

- **.NET (C#)** para o backend
- **PostgreSQL** como banco de dados
- **Locust** para simulação de carga e testes de desempenho
- **Executada localmente**

## Funcionalidades Implementadas

- Catálogo de produtos
- Gestão de Pedidos
- Itens do Pedido
- 

## Padrões de projeto implementados
-  Arquitetura em Camadas (DDD)
-  Unit of Work
-  Repository Pattern
-  DTO's e AutoMapper


## Testes de Carga

Os testes de carga serão realizados com **Locust**, com o intuito de medir:

- Tempo de resposta
- Quantidade de requisições simultâneas suportadas
- Comportamento da aplicação sob estresse
- Comparativo entre as versões síncrona e assíncrona

## Como Executar

**Pré-requisitos:**

- .NET 6 SDK
- PostgreSQL

### Executando o Projeto

```bash
# Clone o repositório
git clone https://github.com/diegowfc/EcommerceSync
cd EcommerceSync

