---
layout: default
title: Bellhop
parent: Autoscalers
---

# Bellhop

[Docs](https://azure.github.io/bellhop){: .btn }
[GitHub](https://github.com/Azure/bellhop){: .btn }

Bellhop is an open-source project by Microsoft that allows you to hop between Azure Resource service tiers based on time based on Azure tags.

You can easily deploy it in your Azure subscription and get started.

[![Deploy To Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fbellhop%2Fmain%2Ftemplates%2Fazuredeploy.json/createUIDefinitionUri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fbellhop%2Fmain%2Ftemplates%2FcreateUiDefinition.json)

## Maintainer

Microsoft

## Support

Community-based support is available on GitHub through [GitHub Discussions](https://github.com/Azure/bellhop/discussions).

## Observability

None, but does provide [monitoring for Bellhop itself](https://azure.github.io/bellhop/#/monitoring/README).

## Supported Services

[Full Overview](https://azure.github.io/bellhop/#/README?id=currently-supported-azure-services){: .btn }

You can autoscale the following Azure services:

- Azure App Service
- Azure SQL DB
- Azure SQL DB Elastic Pools
- Azure Virtual Machines

## Limitations

You have to deploy and operate Bellhop in your subscription.
